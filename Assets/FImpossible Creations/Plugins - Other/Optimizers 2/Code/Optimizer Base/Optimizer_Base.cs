using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FM: Base class with methods to support culling groups and other methods useful for optimizer classes.
    /// > Defining how many LODs should be used and on what distances
    /// > Handling detecting distance ranges and object visibility
    /// > Supporting different algorithms for detecting object visibility and distance
    /// > Handling adding new components to be optimized
    /// </summary>
    public abstract partial class Optimizer_Base : MonoBehaviour
    {
        // --- THIS IS PARTIAL CLASS - REFT OF THE CODE IS IN SEPARATED FILES --- \\

        public static void _RefreshHandleUnityLOD() { _HandleUnityLOD = _HandleUnityLODWithReload; }
        public static bool _HandleUnityLOD = true;
        public static bool _HandleUnityLODWithReload
        {
            get { _HandleUnityLOD = PlayerPrefs.GetInt("FOpt_ULOD", 1) == 1; return _HandleUnityLOD; }
            set { _HandleUnityLOD = value; PlayerPrefs.SetInt("FOpt_ULOD", value ? 1 : 0); }
        }

        /// <summary>
        /// Checking correctness and initializing optimizer component
        /// </summary>
        protected virtual void Start()
        {
            StartVariablesRefresh();
            RefreshInitialSettingsForOptimized();

#if OPTIMIZERS_DOTS_IMPORTED
            DOTSInit();
#endif

            // Triggering correct initialization methods
            switch (OptimizingMethod)
            {
                case EOptimizingMethod.Static:
                    InitStaticOptimizer();
                    break;

                case EOptimizingMethod.Dynamic:
                    InitDynamicOptimizer(true);
                    break;

                case EOptimizingMethod.Effective:
                    InitEffectiveOptimizer();
                    break;

                case EOptimizingMethod.TriggerBased:
                    InitTriggerOptimizer();
                    break;
            }

            //if (UseDOTS)
            //    for (int i = 0; i < GetToOptimizeCount(); i++)
            //    {
            //        Collider c = GetOptimizedComponent(i).GetComponent<Collider>();
            //        if (c) c.gameObject.AddComponent<OptimizersReference>().Parent = this;
            //    }

            moveTreshold = (DetectionRadius * transform.lossyScale.x) / 100f;
            if (OptimizersManager.Instance) moveTreshold *= (1f - OptimizersManager.Instance.UpdateBoost * 0.999f);
            //initialized = true;
        }


        /// <summary> Checking if derived type to optimize list is valid </summary>
        public abstract bool OptimizationListExists();


        /// <summary>
        /// Executed every when component added and every change inside inspector window
        /// </summary>
        public virtual void OnValidate()
        {
#if UNITY_EDITOR

            if (Selection.gameObjects.Contains(gameObject))
            {
                if (FadeDuration <= 0f) FadeViewVisibility = false;

                if (!Application.isPlaying)
                {
                    EditorUpdate();

                    if (UseObstacleDetection)
                        if (!wasSearching)
                        {
                            ignoredObstacleColliders = FTransformMethods.FindComponentsInAllChildren<Collider>(transform, true).ToArray();
                            wasSearching = true;
                        }
                }

                OptimizerOnValidate();

                if (UseMultiShape) OnValidateMultiShape();

                CheckAndRemoveRenderersAlreadyInOtherLODGroup();
            }
#endif
        }



        /// <summary>
        /// Method called when component is added to any game object
        /// </summary>
        protected virtual void Reset()
        {
#if UNITY_EDITOR
            OptimizerReset();
#endif
        }


    }
}
