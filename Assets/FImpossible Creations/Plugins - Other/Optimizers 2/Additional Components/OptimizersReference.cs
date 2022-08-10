using UnityEngine;

namespace FIMSpace.FOptimizing
{
    [AddComponentMenu("FImpossible Creations/Hidden/Optimizers Reference")]
    public class OptimizersReference : MonoBehaviour
    {
        public Optimizer_Base Parent;
        [Tooltip("If Occlusion Culling Rays should stop on this collider, should be untoggled on lights / particle systems cause you can see them throught (transparent). Also untoggle it on models with transparent materials!")]
        public bool IsObstacle = true;
    }

#if UNITY_EDITOR
    [UnityEditor.CanEditMultipleObjects()]
    [UnityEditor.CustomEditor(typeof(OptimizersReference))]
    public class OptimizersReferenceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


#if OPTIMIZERS_DOTS_IMPORTED
            OptimizersReference opt = target as OptimizersReference;
            if (opt.IsObstacle == false)
                if (OptimizersManager.Instance)
                    if (OptimizersManager.Instance.SupportNotObstacles == false)
                        UnityEditor.EditorGUILayout.HelpBox(" Is Obstacle = false - Transparent obstacles will work only when 'SupportNotObstacles' is toggled in OptimizersManager", UnityEditor.MessageType.Warning);
#endif

            GUILayout.Space(4f);
            UnityEditor.EditorGUILayout.HelpBox("   This component is used by Progressive Occlusion Culling ", UnityEditor.MessageType.Info);
        }
    }
#endif

}