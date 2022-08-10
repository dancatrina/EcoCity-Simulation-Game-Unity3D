using UnityEngine;

namespace FIMSpace.FOptimizing
{
    [CreateAssetMenu(menuName = "Optimizer 2019_4+ Types Setup")]
    public class Optimizer2020Selector : ScriptableObject
    {
        public bool Light = true;
        public bool Particle = true;
        public bool Renderer = true;
        public bool SkinnedRenderer = true;
        public bool MonoBehaviour = true;
        public bool AudioSource = false;
        public bool NavMeshAgent = false;
        public bool Rigidbody = false;
        //public bool LODGroup = false;

        //private void OnValidate()
        //{
        //    Optimizer_Base._HandleUnityLODWithReload = LODGroup;
        //}

        public bool IsTypeAllowed(Component type)
        {
            if (type is ParticleSystem) return Particle;
            else if (type is Light) return Light;
            else if (type is SkinnedMeshRenderer) return SkinnedRenderer;
            else if (type is Renderer) return Renderer;
            else if (type is MonoBehaviour) return MonoBehaviour;
            else if (type is AudioSource) return AudioSource;
            else if (type is UnityEngine.AI.NavMeshAgent) return NavMeshAgent;
            else if (type is Rigidbody) return Rigidbody;

            return false;
        }

    }


#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(Optimizer2020Selector))]
    public class Optimizer2020SelectorEditor : UnityEditor.Editor
    {
        public Optimizer2020Selector Get { get { if (_get == null) _get = (Optimizer2020Selector)target; return _get; } }
        private Optimizer2020Selector _get;

        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("Choose Components to be automatically detected by 'Optimizer 2019.4+'", UnityEditor.MessageType.Info);
            GUILayout.Space(4f);
            DrawDefaultInspector();
            GUILayout.Space(4f);
        }
    }
#endif

}
