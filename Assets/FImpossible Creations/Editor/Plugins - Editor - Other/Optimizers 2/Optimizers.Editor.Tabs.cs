using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_BaseEditor
    {

        #region GUI Helpers

        int[] setp_cullIfSee = new int[2] { 0, 1 };
        GUIContent[] setp_cullIfSeeNames = new GUIContent[2] { new GUIContent("Cull if camera don't see", "Culling object if camera is not seeing detection shape in view frustum"), new GUIContent("Not cull if invisible in camera view", "Not culling object if is not visible in camera view frustum") };

        #endregion

        #region Visiblity switches

        protected bool visibleOptMethod = true;
        protected bool visibleCamRelation = true;
        protected bool visibleToOptList = true;
        protected bool visibleAddFeatures = true;

        #endregion

        private void Tab_DrawOptimSetup()
        {
            FGUI_Inspector.VSpace(-3, -4);
            GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);

            if (Application.isPlaying) GUI.enabled = false;

            GUILayout.Space(9);
            El_DrawMaxDistance();
            GUILayout.Space(6);

            if (visibleOptMethod)
            {
                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle);
                //GUILayout.Space(-2f);
                EditorGUILayout.PropertyField(sp_OptMeth);
                //GUILayout.Space(-2f);

                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);
                GUILayout.Space(2f);
            }

            if (visibleCamRelation)
            {
                bool pre = Get.CullIfNotSee;
                Get.CullIfNotSee = EditorGUILayout.IntPopup(new GUIContent("Camera Relation", "IF you want use detections shape to cull object when camera is looking in different direction than this object position"), Get.CullIfNotSee ? 0 : 1, setp_cullIfSeeNames, setp_cullIfSee) == 0;
                if (pre != Get.CullIfNotSee) { serializedObject.ApplyModifiedProperties(); }

                GUILayout.Space(4f);
            }

            if (Get.CullIfNotSee)
            {

                if (Get.OptimizingMethod == EOptimizingMethod.Static || Get.OptimizingMethod == EOptimizingMethod.Effective)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(sp_DetectionRadius);
                    El_AutoDetectionShapeButton();
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(sp_DetBounds);
                    El_AutoDetectionShapeButton();
                    EditorGUILayout.EndHorizontal();
                }

                if (visibleCamRelation)
                    EditorGUILayout.PropertyField(sp_DetOffs);

                GUILayout.Space(4f);
            }

            if (visibleOptMethod) EditorGUILayout.EndVertical();

            //GUILayout.Space(9f);

            GUILayout.EndVertical();
            GUILayout.Space(afterSetupFooter);

            if (Application.isPlaying) GUI.enabled = true;
        }

        private void El_AutoDetectionShapeButton(int width = 50)
        {
            if (GUILayout.Button("Auto", GUILayout.Width(width))) Get.TryAutoComputeDetectionShape(1f);
        }

        protected int afterSetupFooter = 0;
        protected virtual void El_ContainerSwitch()
        {
            if (Get.OptimizingMethod == EOptimizingMethod.Static || Get.OptimizingMethod == EOptimizingMethod.Effective)
            {
                //EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle);
                EditorGUIUtility.labelWidth = 210;
                EditorGUILayout.PropertyField(sp_AddToCont, new GUIContent("Add To Container (Recommended)", sp_AddToCont.tooltip));
                EditorGUIUtility.labelWidth = 0;
                //EditorGUILayout.EndVertical();
                afterSetupFooter = -4;
            }
        }

        protected virtual void GUI_ToOptimizeHeader() { }

        private int drawedPage = 0;
        protected virtual void FillToOptimizeList()
        {
            int perPage = 10;
            int allPages = (Get.GetToOptimizeCount() - 1) / perPage;

            if (allPages > 0)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("<", FGUI_Resources.ButtonStyle, GUILayout.Width(48))) { drawedPage--; }
                EditorGUILayout.LabelField((drawedPage + 1) + " / " + (allPages + 1), FGUI_Resources.HeaderStyle);
                if (GUILayout.Button(">", FGUI_Resources.ButtonStyle, GUILayout.Width(48))) { drawedPage++; }
                if (drawedPage < 0) drawedPage = allPages; if (drawedPage > allPages) drawedPage = 0;
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(6);
            }

            int start = drawedPage * perPage;
            int end = start + perPage; if (end > Get.GetToOptimizeCount()) end = Get.GetToOptimizeCount();

            for (int i = start; i < end; i++)
            {
                EditorGUILayout.BeginHorizontal();

                GUI.color = new Color(1f, 1f, 1f, 0.6f);
                EditorGUIUtility.labelWidth = 35;
                string objTitle = " [" + i + "]";

                if (Get.GetOptimizedComponent(i) != null)
                    EditorGUILayout.ObjectField(objTitle, Get.GetOptimizedComponent(i), typeof(Component), true);

                GUI.color = c;

                EditorGUIUtility.labelWidth = 0;

                if (!Application.isPlaying)
                    if (GUILayout.Button(new GUIContent("X", "Remove component to be optimized from the list"), FGUI_Resources.ButtonStyle, new GUILayoutOption[2] { GUILayout.Width(20), GUILayout.Height(16) }))
                    {
                        Undo.RecordObject(serializedObject.targetObject, "Removing component to optimize");
                        Get.RemoveFromToOptimizeAt(i);
                        EditorUtility.SetDirty(Get);
                        serializedObject.Update();
                        serializedObject.ApplyModifiedProperties();
                        return;
                    }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(2f);
            }
        }

        protected bool drawCustomCompFindButton = true;
        private void Tab_DrawToOptList()
        {
            FGUI_Inspector.VSpace(-3, -4);
            GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);

            if (Get.GetToOptimizeCount() == 0)
            {
                if (Get.transform.childCount > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Not found any object to optimize, maybe try searching for them deeper?", MessageType.Info);
                    if (GUILayout.Button(new GUIContent(Lang("Find Custom Comps"), "Checking components added to this game object and all child game objects for components to be optimized"), FGUI_Resources.ButtonStyle, GUILayout.ExpandHeight(true))) { Undo.RecordObject(serializedObject.targetObject, "Finding components to optimize in children"); Get.AssignComponentsToBeOptimizedFromAllChildren(Get.gameObject, true); }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                bool isMono = false;
                for (int i = 0; i < Get.GetToOptimizeCount(); i++)
                    if ( Get.GetOptimizedComponent(i) is MonoBehaviour) { isMono = true; break; }
                
                if ( isMono)
                {
                    EditorGUILayout.HelpBox("Optimizer included some MonoBehaviours to optimize! Please make sure if this scripts are capable for optimizing!", MessageType.Warning);
                    GUILayout.Space(-5);
                }
            }

            GUILayout.Space(8);
            GUI.color = new Color(1f, 1f, 1f, 0.7f);
            EditorGUILayout.LabelField(Lang("LangManageInfo"), FGUI_Resources.HeaderStyle);
            GUI_ToOptimizeHeader();

            GUI.color = c;
            GUILayout.Space(4);

            FillToOptimizeList();

            if (Get.GetToOptimizeCount() > 0) GUILayout.Space(8f);

            GUI.color = preCol;
            if (Application.isPlaying) GUI.enabled = false;

            GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle);

            EditorGUI.indentLevel++;

            EditorGUILayout.BeginHorizontal();
            DrawAddCompOptions = EditorGUILayout.Foldout(DrawAddCompOptions, Lang("Assigning"), true);

            if (DrawOptInfoButton)
            {
                if (drawOptInfo) GUI.backgroundColor = Color.gray;
                if (GUILayout.Button(new GUIContent(FGUI_Resources.Tex_Info), FGUI_Resources.ButtonStyle, GUILayout.Width(18), GUILayout.Height(17)))
                {
                    drawOptInfo = !drawOptInfo;
                }
                GUI.backgroundColor = preCol;
            }

            EditorGUILayout.EndHorizontal();

            if (DrawAddCompOptions)
            {
                //FEditor_Styles.DrawUILine(new Color(0.5f, 0.5f, 0.5f, 0.45f), 2, 4);
                GUILayout.Space(5f);

                EditorGUILayout.BeginVertical(/*FEditor_Styles.LGrayBackground*/);
                if (GUILayout.Button(new GUIContent(Lang("Detect UNITY C"), "Checking UNITY components added to this game object and adding them to be optimized if they're supported by Optimizers"), FGUI_Resources.ButtonStyle)) { Undo.RecordObject(serializedObject.targetObject, "Finding components to optimize"); Get.AssignComponentsToOptimizeFrom(Get); }
                GUILayout.Space(3f);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(new GUIContent(Lang("Find Comps in c"), "Checking UNITY components added to this game object and all child game objects for components to be optimized"), FGUI_Resources.ButtonStyle)) { Undo.RecordObject(serializedObject.targetObject, "Finding components to optimize in children"); Get.AssignComponentsToBeOptimizedFromAllChildren(Get.gameObject); }

                if (drawCustomCompFindButton)
                    if (GUILayout.Button(new GUIContent(Lang("Find Custom Comps"), "Checking components added to this game object and all child game objects for components to be optimized"), FGUI_Resources.ButtonStyle)) { Undo.RecordObject(serializedObject.targetObject, "Finding components to optimize in children"); Get.AssignComponentsToBeOptimizedFromAllChildren(Get.gameObject, true); }
                GUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                if (Application.isPlaying) GUI.enabled = true;
                GUILayout.Space(8f);
                //FEditor_Styles.DrawUILine(new Color(0.5f, 0.5f, 0.5f, 0.45f), 2, 4);
                DrawDragAndDropSquare(Get);
                GUILayout.Space(2f);
            }

            EditorGUI.indentLevel--;
            GUILayout.EndVertical();


            GUILayout.EndVertical();
            GUILayout.Space(-4f);
        }


        protected void Tab_DrawLODSetup()
        {
            FGUI_Inspector.VSpace(-3, -4);
            GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);

            DrawLODRangesGUI();

            GUILayout.EndVertical();
            GUILayout.Space(-4f);
        }

        protected virtual void LODSetupTop()
        {
            El_DrawMaxDistance();
        }


        private void Tab_DrawAdditionalModules()
        {
            FGUI_Inspector.VSpace(-3, -4);
            GUILayout.BeginVertical(FGUI_Resources.ViewBoxStyle);

            GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle); GUILayout.Space(5f);
            Fold_DrawDOTS();
            GUILayout.Space(3f);
            GUILayout.EndVertical();

            // Obstacles
            GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle); GUILayout.Space(5f);
            Fold_DrawModuleObstacles();

            GUILayout.Space(3f);
            GUILayout.EndVertical();

            // Multi Shape
            GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle); GUILayout.Space(2f);
            Fold_DrawModuleMultiShape();

            GUILayout.Space(2f);
            GUILayout.EndVertical();

            //// Partial Blend
            //GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle); GUILayout.Space(2f);
            //Fold_DrawModulePartialBlend();

            //GUILayout.Space(2f);
            //GUILayout.EndVertical();

            //// IK
            //GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle); GUILayout.Space(2f);
            //Fold_DrawModuleIK();

            //GUILayout.Space(2f);
            //GUILayout.EndVertical();

            //GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle); GUILayout.Space(5f);
            Fold_DrawExtra();
            //GUILayout.Space(3f);
            //GUILayout.EndVertical();


            GUILayout.EndVertical();
            GUILayout.Space(-4f);
        }
    }

}

