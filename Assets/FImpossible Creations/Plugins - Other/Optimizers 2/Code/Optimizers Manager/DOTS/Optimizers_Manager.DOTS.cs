#if OPTIMIZERS_DOTS_IMPORTED

using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersManager
    {
        // INSPECTOR VARIABLES ----------------------

        public bool UseDOTSProgressiveCulling = false;

        [Tooltip("Which Layers should be treated as obstacles in sight.\nIf it's the same like 'OptimizersCullingLayer' then only objects with optimizers will be able to cover other objects with optimizers.\nUse another layers to make them cover optimized objects (without optimizers will not be hidden - they will be just sight obstacles).")]
        public LayerMask ProgressiveCullingMask = 1 << 4;
        [Tooltip("Layer for optimizers detection colliders, should be unique for better performance. (this layer will be applied for culling detection colliders generated when game starts)")]
        //[Tooltip("Layer for culling detection, best would be creating unique layer for this purpose (it will be automatically assigned for culling detector colliders)")]
        [FPD_Layers] public int OptimizersCullingLayer = 4;
        [Tooltip("Allowing raycasts going through objects with 'Is Obstacle = false' under OptimizerReference component. Means it can be used for example on lights -> can be occluded but can't occlude others")]
        public bool SupportNotObstacles = true;

        [Tooltip("Higher value = shorter time for disappearing objects outside camera frustum and higher precision but slightly bigger load on performance.\nIf your scene have a lot of small detail objects for culling you should put response quality higher but if your scenes have just many medium/sized objects with optimizers on then you can lower it.\nIf some objects starts to disappear and appear every few frames that means response quality is too low.")]
        [SerializeField] [Range(250, 3000)] private int ProgressiveResponseQuality = 1000;
        [Tooltip("Auto Set progress delay to hide objects")]
        public bool ProgAutoDelay = true;
        [Tooltip("Target progress time delay in seconds to hide objects")]
        [SerializeField] private float ProgCullDelay = 1.5f;

        private float dots_debug_drawRaysTimer = 0f;

        [Tooltip("Enabling drawing raycasts done for progressive culling")] public bool DebugProgressiveCasting = false;
        [Tooltip("Interval for drawing raycasts done by algorithm")] [Range(0.01f, 2f)] public float DebugProgrFreq = 1f;
        [Tooltip("Drawing all done raycasts, not only ones which hit optimizers")] [Range(0.0f, 1f)] public float DebugProgrAllAlpha = 0f;

        [Tooltip("Automatically refresh progressive culling range when screen size changes or camera's FOV")]
        public bool AutoDetectFOVAndScreenChange = false;

        // DOTS CULLING IMPLEMENTATION --------------------------------------------------->

        private void DOTS_Initialize()
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;

            dots_VisibleIds = new NativeList<int>(Allocator.Persistent);
            dots_HittedId = new NativeList<int>(Allocator.Persistent);
            dots_DelayElapsed = new NativeList<float>(Allocator.Persistent);
            dots_JobHandles = new NativeList<JobHandle>(Allocator.Persistent);
            dots_JobHandlesTr = new NativeList<JobHandle>(Allocator.Persistent);

            Instance.dots_TransparentHitPoints = new NativeList<float3>(Allocator.Persistent);

            _mesh_cube = CreatePrimitiveMesh(PrimitiveType.Cube);
            _mesh_sphere = CreatePrimitiveMesh(PrimitiveType.Sphere);

            DOTS_RefreshCamera();
            DOTS_AutomaticallySetCullDelayDuration();
        }


        /// <summary>
        /// Refreshing raycasting setup for progressive culling
        /// </summary>
        public static void DOTS_RefreshCamera()
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;

            if (Instance.dots_RayCommands.IsCreated) Instance.dots_RayCommands.Dispose();
            Instance.dots_RayCommands = new NativeArray<RaycastCommand>(Instance.ProgressiveResponseQuality, Allocator.Persistent);
            Instance.dots_HitsResults = new NativeArray<RaycastHit>(Instance.ProgressiveResponseQuality, Allocator.Persistent);

            Instance.DOTS_RefreshRaycastFrustum();

            Instance.dots_preScreen = new Vector2(Screen.width, Screen.height);
            if (Instance.TargetCamera != null) Instance.dots_preFOV = Instance.TargetCamera.fieldOfView;
        }



        /// <summary>
        /// Preparing jobs for raycasting -------------------------------------------------------------->
        /// </summary>
        private void DOTS_PreUpdate()
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;

            if ( AutoDetectFOVAndScreenChange ) DOTS_CheckScreenAndFOVChange();

            DOTS_ScheduleRaycasting();
            if (SupportNotObstacles) DOTS_ScheduleTrasnparentRaycasting();
        }

        /// <summary>
        /// Processing done jobs -------------------------------------------------------------->
        /// </summary>
        private void DOTS_PostUpdate()
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;

            JobHandle.CompleteAll(dots_JobHandles);

            if (SupportNotObstacles)
            {
                JobHandle.CompleteAll(dots_JobHandlesTr);
                dots_TransparentHitPoints.Clear();
            }

            // Filling data with results ------------------------------
            ApplyHitsResults(dots_HitsResults);

            if (SupportNotObstacles) ApplyHitsResults(dots_TrHitsResults);


            // Scheduling final job ------------------------------
            GetResultsJob resultsJob = new GetResultsJob()
            {
                visible = dots_VisibleIds,
                hitted = dots_HittedId,
                elapsed = dots_DelayElapsed
            };

            resultsJob.Schedule().Complete();


            // Applying Optimizers state to objects ---------------------------------------->
            int optimizerId = 0;
            while (optimizerId < dots_VisibleIds.Length)
            {
                int id = dots_VisibleIds[optimizerId];

                try
                {
                    // Hiding after invisibility progress
                    if (dots_DelayElapsed[optimizerId] > ProgCullDelay)
                    {
                        dots_IdToOpt[id].DOTSObstacleCheck(false);
                        dots_VisibleIds.RemoveAtSwapBack(optimizerId);
                        dots_DelayElapsed.RemoveAtSwapBack(optimizerId);
                    }
                    else // Making visible when detected
                    {
                        dots_IdToOpt[id].DOTSObstacleCheck(true);
                        optimizerId++;
                    }
                }
                catch (MissingReferenceException) // Safety check
                {
                    dots_ToRemoveOpt.Add(id);
                    optimizerId++;
                }
            }


            DOTS_HandleRemoving();


            #region Debug Raycasts ---------------------

            if (DebugProgressiveCasting)
            {
                dots_debug_drawRaysTimer -= Time.deltaTime;
                if (dots_debug_drawRaysTimer < 0f)
                {
                    dots_debug_drawRaysTimer = DebugProgrFreq;
                    DOTS_DebugDrawRaycasts();
                }
            }

            #endregion


        }


        private void ApplyHitsResults(NativeArray<RaycastHit> results)
        {
            for (int j = 0; j < results.Length; j++)
            {
                if (results[j].collider != null)
                {
                    OptimizersReference opt = results[j].collider.GetComponent<OptimizersReference>();
                    if (opt)
                    {
                        if (opt.Parent == null) Destroy(opt);
                        else
                        {
                            dots_HittedId.Add(dots_OptToId[opt.Parent]);

                            if (SupportNotObstacles)
                                if (opt.IsObstacle == false) // Letting raycast go through this object -> Adding new ray to be casted
                                    dots_TransparentHitPoints.Add(results[j].point - results[j].normal * results[j].distance * 0.01f);
                        }
                    }
                }
            }
        }



        private Vector2 dots_preScreen;
        private float dots_preFOV = 60;
        private bool dots_triggerRefresh = false;
        private void DOTS_CheckScreenAndFOVChange()
        {
            Vector2 currentScreen = new Vector2(Screen.width, Screen.height);

            if (dots_preScreen != currentScreen) dots_triggerRefresh = true;
            else
            {
                if (TargetCamera != null) if (dots_preFOV != TargetCamera.fieldOfView) dots_triggerRefresh = true;
            }

            if (dots_paused == false)
                if (Application.isFocused && Application.isPlaying)
                    if (dots_triggerRefresh)
                    {
                        DOTS_RefreshCamera();
                        dots_triggerRefresh = false;
                    }

            if (TargetCamera != null) dots_preFOV = TargetCamera.fieldOfView;
            dots_preScreen = currentScreen;
        }

        private bool dots_paused = false;
        private void OnApplicationPause(bool pause)
        {
            dots_paused = pause;
        }

    }
}


#endif