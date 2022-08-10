using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public class IgnoreOptimizer : MonoBehaviour { }


#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(IgnoreOptimizer))]
    public class IgnoreOptimizerEditor : UnityEditor.Editor
    {
        public IgnoreOptimizer Get { get { if (_get == null) _get = (IgnoreOptimizer)target; return _get; } }
        private IgnoreOptimizer _get;

        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("This component will prevent optimizer to gather components to optimize from this object", UnityEditor.MessageType.Info);
        }
    }
#endif

}