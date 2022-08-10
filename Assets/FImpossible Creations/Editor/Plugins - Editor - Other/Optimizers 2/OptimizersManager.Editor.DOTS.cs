#if OPTIMIZERS_DOTS_IMPORTED
using FIMSpace.FEditor;
using Unity.Burst;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_EditorManager
    {
        private bool drawDotsDebug = false;

        void DrawDOTSGUI()
        {
            GUILayout.Space(4);
            SerializedProperty sp_dots = serializedObject.FindProperty("UseDOTSProgressiveCulling");
            bool e = GUI.enabled;

            //FGUI_Inspector.FoldHeaderStart(ref drawSetup, " Progressive Culling (DOTS)", FGUI_Resources.BGInBoxStyle, FGUI_Resources.Tex_Optimization, 22);

            GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("  " + FGUI_Resources.GetFoldSimbol(Get._editor_drawDOTS, 10, "►") + "  Progressive Occlusion Culling (Experimental)", FGUI_Resources.Tex_Optimization), FGUI_Resources.FoldStyle, GUILayout.Height(22))) Get._editor_drawDOTS = !Get._editor_drawDOTS;
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 40;
            if (Application.isPlaying) GUI.enabled = false;
            Get.UseDOTSProgressiveCulling = EditorGUILayout.Toggle("Use", Get.UseDOTSProgressiveCulling, GUILayout.Width(66)); sp_dots.NextVisible(false);
            GUI.enabled = e;
            EditorGUIUtility.labelWidth = 0;
            GUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = 180;


            if (Get._editor_drawDOTS)
            {
                if (Get.UseDOTSProgressiveCulling)
                {
                    GUILayout.Space(3);
                    if (Get.AutoDetectFOVAndScreenChange == false)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox("Beware! Every change in camera FOV / Screen size must be updated with OptimizersManager.DOTS_RefreshCamera()", MessageType.Warning);
                        if (GUILayout.Button("Do it\nautomatically", GUILayout.Width(110))) Get.AutoDetectFOVAndScreenChange = true;
                        EditorGUILayout.EndHorizontal();
                    }
                    else
                    {
                        EditorGUIUtility.labelWidth = 240;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("AutoDetectFOVAndScreenChange"));
                        EditorGUIUtility.labelWidth = 0;
                    }


                    if (BurstCompiler.Options.EnableBurstCompilation == false)
                    {
                        GUILayout.Space(3f);
                        EditorGUILayout.HelpBox("Enable 'Burst Compilation' for performance boost!", MessageType.Warning);
                        if (GUILayout.Button("Enable Burst Compilation")) BurstCompiler.Options.EnableBurstCompilation = true;
                    }
                }

                GUILayout.Space(3);
                if (!Get.UseDOTSProgressiveCulling) GUI.enabled = false;
                if (Application.isPlaying) GUI.enabled = false;
                EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);

                EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);
                GUILayout.Space(2);
                EditorGUILayout.HelpBox("Progressive Culling Layer should be unique for better performance", MessageType.Info);
                GUILayout.Space(4);

                EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);
                EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);

                EditorGUILayout.BeginHorizontal(); EditorGUIUtility.labelWidth = 110;
                EditorGUILayout.PropertyField(sp_dots, GUILayout.MaxWidth(140)); sp_dots.NextVisible(false); EditorGUIUtility.labelWidth = 20;
                bool preprogE = GUI.enabled; if (Get.ProgAutoDelay) GUI.enabled = false; else if (Get.UseDOTSProgressiveCulling) GUI.enabled = true;
                EditorGUILayout.PropertyField(sp_dots, new GUIContent(" ", sp_dots.tooltip)); EditorGUIUtility.labelWidth = 0;
                GUI.enabled = preprogE;
                EditorGUILayout.EndHorizontal();

                if (Get.ProgAutoDelay)
                    EditorGUILayout.HelpBox("Response Quality determinates how quickly and responsive objects will disappear behind obstacles (you can boost it with 'Update Boost')", MessageType.None);

                // Auto bool option
                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.PropertyField(sp_dots, GUIContent.none, GUILayout.Width(24)); sp_dots.NextVisible(false);
                //EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);
                //Get.DOTS_AutomaticallySetCullDelayDuration();
                //EditorGUILayout.EndHorizontal(); 

                GUI.enabled = e;
                if (!Get.UseDOTSProgressiveCulling) GUI.enabled = false;

                GUILayout.Space(4);
                FGUI_Inspector.FoldHeaderStart(ref drawDotsDebug, " Debugging Tools", FGUI_Resources.BGInBoxStyle, FGUI_Resources.Tex_Default, 19);
                if (drawDotsDebug)
                {
                    if (!Application.isPlaying)
                    {
                        GUILayout.Space(2);
                        EditorGUILayout.HelpBox("Visible in playmode in scene view (camera)", MessageType.None);
                    }

                    GUILayout.Space(2);
                    EditorGUILayout.HelpBox("Beware, this will trigger drawing a lot of raycast rays", MessageType.None);

                    EditorGUIUtility.labelWidth = 160; sp_dots.NextVisible(false);
                    EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);
                    EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);
                    EditorGUILayout.PropertyField(sp_dots); sp_dots.NextVisible(false);
                }
                GUILayout.EndVertical();
                GUI.enabled = e;
            }

            EditorGUIUtility.labelWidth = 0;
            GUILayout.EndVertical();
        }

    }
}
#endif
