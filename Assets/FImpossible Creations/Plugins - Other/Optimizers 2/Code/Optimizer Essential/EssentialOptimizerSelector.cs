using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    [CreateAssetMenu(menuName = "Essential Types Setup")]
    public class EssentialOptimizerSelector : ScriptableObject
    {
        public bool Light = true;
        public bool Particle = true;
        public bool Renderer = true;
        public bool MonoBehaviour = true;
        public bool AudioSource = false;
        public bool NavMeshAgent = false;
        public bool Rigidbody = false;
        //public bool LODGroup = false;

        //private void OnValidate()
        //{
        //    Optimizer_Base._HandleUnityLODWithReload = LODGroup;
        //}

        public bool IsTypeAllowed(EssentialLODsController.EEssType type)
        {
            switch (type)
            {
                case EssentialLODsController.EEssType.Particle: if (Particle) return true; break;
                case EssentialLODsController.EEssType.Light: if (Light) return true; break;
                case EssentialLODsController.EEssType.MonoBehaviour: if (MonoBehaviour) return true; break;
                case EssentialLODsController.EEssType.Renderer: if (Renderer) return true; break;
                case EssentialLODsController.EEssType.NavMeshAgent: if (NavMeshAgent) return true; break;
                case EssentialLODsController.EEssType.AudioSource: if (AudioSource) return true; break;
                case EssentialLODsController.EEssType.Rigidbody: if (Rigidbody) return true; break;
                //case EEssType.LODGroup: if (t.lo) return true; break;
            }

            return false;
        }

    }


#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(EssentialOptimizerSelector))]
    public class EssentialOptimizerSelectorEditor : UnityEditor.Editor
    {
        public EssentialOptimizerSelector Get { get { if (_get == null) _get = (EssentialOptimizerSelector)target; return _get; } }
        private EssentialOptimizerSelector _get;

        //private void OnEnable()
        //{
        //    Get.LODGroup = Optimizer_Base._HandleUnityLODWithReload;
        //}

        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("Choose Components to be automatically detected by Essential Optimizer", UnityEditor.MessageType.Info);
            GUILayout.Space(4f);
            DrawDefaultInspector();
            GUILayout.Space(4f);
        }
    }
#endif

}