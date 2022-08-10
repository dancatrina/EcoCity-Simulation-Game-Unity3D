#if OPTIMIZERS_DOTS_IMPORTED

using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersManager
    {

        #region Generating primitive mesh helpers

        private static Mesh _mesh_cube;
        private static Mesh _mesh_sphere;

        private static Mesh CreatePrimitiveMesh(PrimitiveType type)
        {
            GameObject gameObject = GameObject.CreatePrimitive(type);
            Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
            Destroy(gameObject);
            return mesh;
        }

        #endregion



        /// <summary>
        /// Using Halton Sequence to choose propabilistic coords for raycasts
        /// </summary>
        private float DOTS_PointDisperse(int index, int baseV)
        {
            float sum = 0f; float functionV = 1f / baseV; int i = index;
            while (i > 0) { sum += functionV * (i % baseV); i = Mathf.FloorToInt(i / baseV); functionV /= baseV; }
            return sum;
        }


        /// <summary>
        /// Generating directions for screen point raycasts inside camera's frustum view
        /// </summary>
        private void DOTS_RefreshRaycastFrustum()
        {
            float scaleRatio = (Screen.width * Screen.height) / 4;
            int dirsCount = Mathf.RoundToInt(scaleRatio / ProgressiveResponseQuality) * ProgressiveResponseQuality;

            if (dots_Rays.IsCreated) dots_Rays.Dispose();
            dots_Rays = new NativeArray<float3>(dirsCount, Allocator.Persistent);

            Vector3 prePos = MainCamera.transform.position;
            quaternion preRot = MainCamera.transform.rotation;
            float preFov = MainCamera.fieldOfView;

            MainCamera.fieldOfView += 1;
            MainCamera.transform.position = Vector3.zero;
            MainCamera.transform.rotation = Quaternion.identity;

            for (int i = 0; i < dots_Rays.Length; i++)
            {
                Vector2 screenPoint = new Vector2(DOTS_PointDisperse(i, 2), DOTS_PointDisperse(i, 3));
                Ray ray = MainCamera.ViewportPointToRay(new Vector3(screenPoint.x, screenPoint.y, 0));
                dots_Rays[i] = ray.direction;
            }

            MainCamera.transform.position = prePos;
            MainCamera.transform.rotation = preRot;
            MainCamera.fieldOfView = preFov;
        }


        /// <summary>
        /// Drawing raycasts for debugging
        /// </summary>
        private void DOTS_DebugDrawRaycasts()
        {
            NativeArray<RaycastHit> hits = dots_HitsResults;
            NativeArray<RaycastHit> thits = dots_TrHitsResults;

            Color rayCol = new Color(0.9f, .25f, 0.4f, 0.02f * DebugProgrAllAlpha);

            if (DebugProgrAllAlpha > 0f)
            {
                if (DebugProgrFreq < 0.3f) DebugProgrFreq = 0.3f;
                for (int i = 0; i < dots_Rays.Length; i++)
                    Debug.DrawRay(MainCamera.transform.position, MainCamera.transform.rotation * dots_Rays[i] * MainCamera.farClipPlane, rayCol, UnityEngine.Random.Range(0.35f, 0.9f) * DebugProgrFreq);
            }

            Color hitOptCol = new Color(0f, 1f, 0f, 0.75f);
            Color hitNoOptCol = new Color(0f, 0f, 0f, 0.5f);
            Color hitNoObstacleOptCol = new Color(.8f, .8f, .8f, 0.1f);

            for (int i = 0; i < hits.Length; i++)
                if (hits[i].collider)
                    if (hits[i].collider.gameObject.GetComponent<OptimizersReference>())
                    {
                        if (hits[i].collider.gameObject.GetComponent<OptimizersReference>().IsObstacle)
                            Debug.DrawLine(MainCamera.transform.position, hits[i].point, hitOptCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);
                        else
                            Debug.DrawLine(MainCamera.transform.position, hits[i].point, hitNoObstacleOptCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);
                    }
                    else
                        Debug.DrawLine(MainCamera.transform.position, hits[i].point, hitNoOptCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);


            if (SupportNotObstacles)
            {
                if (dots_TransparentHitPoints.Length > 0)
                {
                    Color trCol = new Color(.8f, .8f, .8f, 0.01f);
                    for (int i = 0; i < dots_TransparentHitPoints.Length; i++)
                    {
                        Vector3 dir = (Vector3)dots_TransparentHitPoints[i] - MainCamera.transform.position;
                        Debug.DrawRay(dots_TransparentHitPoints[i], dir.normalized * (MainCamera.farClipPlane - dir.magnitude), trCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);
                    }
                }

                hitOptCol = new Color(.0f, 1f, 1f, 1f);
                for (int i = 0; i < thits.Length; i++)
                    if (thits[i].collider)
                        if (thits[i].collider.gameObject.GetComponent<OptimizersReference>())
                        {
                            if (thits[i].collider.gameObject.GetComponent<OptimizersReference>().IsObstacle)
                                Debug.DrawLine(MainCamera.transform.position, thits[i].point, hitOptCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);
                            else
                                Debug.DrawLine(MainCamera.transform.position, thits[i].point, hitNoObstacleOptCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);
                        }
                        else
                            Debug.DrawLine(MainCamera.transform.position, thits[i].point, hitNoOptCol, UnityEngine.Random.Range(0.8f, 1f) * DebugProgrFreq);
            }

        }


        /// <summary>
        /// Clearing progressive culling system
        /// </summary>
        private void DOTS_Dispose()
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;

            if (dots_JobHandles.IsCreated && dots_JobHandles.Length > 0)
            {
                JobHandle.CompleteAll(dots_JobHandles);
                dots_JobHandles.Dispose();
            }

            if (dots_Rays.IsCreated) dots_Rays.Dispose();

            if (dots_VisibleIds.IsCreated) dots_VisibleIds.Dispose();
            if (dots_HittedId.IsCreated) dots_HittedId.Dispose();
            if (dots_DelayElapsed.IsCreated) dots_DelayElapsed.Dispose();

            dots_RayCommands.Dispose();
            dots_HitsResults.Dispose();

            dots_TransparentHitPoints.Dispose();

            if (SupportNotObstacles)
            {
                dots_TransparentRayCommands.Dispose();
                dots_TrHitsResults.Dispose();
            }
        }


        /// <summary>
        /// Adjust duration of being visible for raycasted objects
        /// </summary>
        public void DOTS_AutomaticallySetCullDelayDuration()
        {
            if (ProgAutoDelay)
            {
                float mul = Mathf.Lerp(1f, 0.65f, UpdateBoost);
                float mul2 = Mathf.Lerp(1f, 0.75f, UpdateBoost);

                if (ProgressiveResponseQuality < 1000)
                    ProgCullDelay = Mathf.Lerp(3.5f * mul, 2f * mul, Mathf.InverseLerp(100, 1000, ProgressiveResponseQuality));
                else
                    ProgCullDelay = Mathf.Lerp(2f * mul, 0.5f * mul2, Mathf.InverseLerp(1000, 6000, ProgressiveResponseQuality));
            }
        }

    }
}


#endif