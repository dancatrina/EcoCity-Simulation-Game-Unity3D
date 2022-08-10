using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_BaseEditor : Editor
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(Optimizer2D))]
        public class Optimizer2DEditor : EssentialOptimizerEditor
        {
            protected override string TargetName() { return " 2D Optimizer"; }
            protected override string TargetTooltip() { return "Optimizer Component which is recommended to use with prefabs especially with nested prefabs.\n\nIt's limited to few optimization components but can be expanded by inheriting but it needs some extra coding and knowledge."; }
            protected override Texture2D GetSmallIcon() { if (__texOptimizersIcon != null) return __texOptimizersIcon; __texOptimizersIcon = Resources.Load<Texture2D>("FIMSpace/Optimizers 2/Optimizers Essential Icon Small"); return __texOptimizersIcon; }

            protected override void OnEnable()
            {
                Get._editor_horizontal = false;
                drawSetup = false;
                visibleAddFeatures = false;
                drawSuggestButton = false;
                base.OnEnable();
            }

            private EssentialOptimizer tGet { get { if (_tGet == null) _tGet = target as Optimizer2D; return _tGet; } }
            private EssentialOptimizer _tGet;

            protected override void LODSetupTop()
            {
                Get._editor_horizontal = false;
                El_DrawMaxDistance();

                bool pre = Get.CullIfNotSee;
                Get.CullIfNotSee = EditorGUILayout.IntPopup(new GUIContent("Camera Relation", "IF you want use detections shape to cull object when camera is looking in different direction than this object position"), Get.CullIfNotSee ? 0 : 1, setp_cullIfSeeNames, setp_cullIfSee) == 0;
                if (pre != Get.CullIfNotSee) { serializedObject.ApplyModifiedProperties(); }
                GUILayout.Space(4f);

                if (Get.CullIfNotSee)
                {
                    EditorGUILayout.BeginHorizontal();
                    Vector2 bounds = EditorGUILayout.Vector2Field(new GUIContent(sp_DetBounds.displayName, sp_DetBounds.tooltip), new Vector2(sp_DetBounds.vector3Value.x, sp_DetBounds.vector3Value.y));
                    sp_DetBounds.vector3Value = new Vector3(bounds.x, bounds.y, 1f);
                    //EditorGUILayout.PropertyField(sp_DetBounds);
                    El_AutoDetectionShapeButton();
                    EditorGUILayout.EndHorizontal();

                    //EditorGUILayout.PropertyField(sp_DetOffs);
                    bounds = EditorGUILayout.Vector2Field(new GUIContent(sp_DetOffs.displayName, sp_DetOffs.tooltip), new Vector2(sp_DetOffs.vector3Value.x, sp_DetOffs.vector3Value.y));
                    sp_DetOffs.vector3Value = new Vector3(bounds.x, bounds.y, 0f);
                }

            }



        }

    }
}

