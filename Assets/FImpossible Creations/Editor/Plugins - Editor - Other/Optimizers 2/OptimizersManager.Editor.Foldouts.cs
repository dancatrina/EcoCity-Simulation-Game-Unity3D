using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_EditorManager
    {
        void Foldout_DrawSetup()
        {
            FGUI_Inspector.FoldHeaderStart(ref Get._editor_drawSetup, " Main Setup", FGUI_Resources.BGInBoxStyle, FGUI_Resources.Tex_GearSetup, 22);

            if (!loadTry)
            {
                loadTry = true;
                if (Resources.Load("Optimizers/FLOD_Terrain Reference") == null)
                {
                    EditorGUILayout.HelpBox("There are no components reference types. Hit right mouse button on this component and then 'Reset'", MessageType.Error);
                }
            }

            if (Get._editor_drawSetup)
            {
                GUILayout.Space(7f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("TargetCamera"));
                if (!Application.isPlaying)
                {
                    if (Get.GetCameraAfter <= 0) GUI.color = new Color(1f, 1f, 1f, 0.7f);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("GetCameraAfter"));
                    if (Get.GetCameraAfter <= 0) GUI.color = c;
                }

                GUILayout.Space(3f);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("SingleContainerCapacity"));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty("GlobalMaxDistanceMultiplier"));
                GUILayout.Space(3f);

                if (!Get.ExistThroughScenes)
                    GUI.color = new Color(1f, 0.9f, 0.7f, 0.9f);
                SerializedProperty sp_exthrsc = serializedObject.FindProperty("ExistThroughScenes");
                EditorGUILayout.PropertyField(sp_exthrsc); sp_exthrsc.NextVisible(false);
                GUI.color = c;

                //bool preE = GUI.enabled;
                //if (Application.isPlaying) GUI.enabled = false;
                //EditorGUIUtility.labelWidth = 192;
                //EditorGUILayout.BeginHorizontal();
                //EditorGUILayout.PropertyField(sp_exthrsc);
                //if (Get.GlobalMaxDistanceMultiplier != 1f) EditorGUILayout.LabelField(new GUIContent(FGUI_Resources.Tex_Warning, "Be sure you know what you doing with this parameter! Objects might disappear from view too soon if you set it too low!"), GUILayout.Width(21), GUILayout.Height(21));
                //EditorGUILayout.EndHorizontal();
                //EditorGUIUtility.labelWidth = 0;
                //if (Application.isPlaying) GUI.enabled = preE;

                GUILayout.Space(4f);
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(6f);

        }

        void Foldout_DrawDynamicOpt()
        {
            GUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("  " + FGUI_Resources.GetFoldSimbol(Get._editor_drawDynamicSetup, 10, "►") + "  Dynamic Optimizing Setup", FGUI_Resources.TexMotionIcon), FGUI_Resources.FoldStyle, GUILayout.Height(22))) Get._editor_drawDynamicSetup = !Get._editor_drawDynamicSetup;
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 70;
            Get.Advanced = EditorGUILayout.Toggle("Advanced", Get.Advanced);
            EditorGUIUtility.labelWidth = 0;
            GUILayout.EndHorizontal();


            if (Get._editor_drawDynamicSetup)
            {
                GUILayout.Space(5f);

                if (!Get.Advanced)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("WorldScale"));

                    GUILayout.FlexibleSpace();
                    GUILayout.Label(new GUIContent("(End Distance: " + Mathf.Round(Get.Distances[Get.Distances.Length - 1]) + ")"));

                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateBoost"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("DetectCameraFreeze"));

                    GUILayout.Space(3f);
                    FEditor.FGUI_Inspector.DrawUILine(new Color(0.5f, 0.5f, 0.5f, 0.3f), 1, 6);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("GizmosAlpha"));
                    GUILayout.Space(2f);
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("UpdateBoost"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("DetectCameraFreeze"));
                    if (Get.DetectCameraFreeze)
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("MoveTreshold"));

                    GUILayout.Space(3f);
                    FEditor.FGUI_Inspector.DrawUILine(new Color(0.5f, 0.5f, 0.5f, 0.5f), 1, 6);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("Debugging"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("GizmosAlpha"));
                    GUILayout.Space(3f);
                    FEditor.FGUI_Inspector.DrawUILine(new Color(0.5f, 0.5f, 0.5f, 0.5f), 1, 6);

                    EditorGUILayout.LabelField(new GUIContent("Clocks Distance Ranges", "Check Gizmos in scene view for more (At Main Camera position)"), new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter });
                    GUILayout.Space(4f);

                    GUIStyle smallStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 9 };
                    GUI.color = new Color(1f, 1f, 1f, 0.7f);
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label(new GUIContent("Highest Priority", "[Check scene view in camera position]\n" + OptimizersManager.RangeInfos[0]), smallStyle);
                    GUI.color = new Color(1f, 1f, 0.7f, 0.7f);
                    GUILayout.Label(new GUIContent("High Priority", "[Check scene view in camera position]\n" + OptimizersManager.RangeInfos[1]), smallStyle);
                    GUI.color = new Color(0.55f, 1f, .8f, 0.7f);
                    GUILayout.Label(new GUIContent("Medium Priority", "[Check scene view in camera position]\n" + OptimizersManager.RangeInfos[2]), smallStyle);
                    GUI.color = new Color(0.6f, .82f, 1f, 0.7f);
                    GUILayout.Label(new GUIContent("Low Priority", "[Check scene view in camera position]\n" + OptimizersManager.RangeInfos[3]), smallStyle);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(-1f);

                    EditorGUILayout.BeginHorizontal();
                    EditorGUIUtility.labelWidth = 5;
                    GUI.color = new Color(0.7f, 1f, 0.7f, 0.95f);
                    Get.Distances[0] = EditorGUILayout.FloatField(" ", Get.Distances[0]);
                    GUI.color = new Color(1f, 1f, 0.7f, 0.95f);
                    Get.Distances[1] = EditorGUILayout.FloatField(" ", Get.Distances[1]);
                    GUI.color = new Color(0.55f, 1f, .8f, 0.95f);
                    Get.Distances[2] = EditorGUILayout.FloatField(" ", Get.Distances[2]);
                    GUI.color = new Color(0.6f, .82f, 1f, 0.95f);
                    Get.Distances[3] = EditorGUILayout.FloatField(" ", Get.Distances[3]);
                    EditorGUIUtility.labelWidth = 0;
                    GUI.color = c;

                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(4f);
                }

                EditorGUIUtility.labelWidth = 180;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawHumanSizeRefIcon"));
                EditorGUIUtility.labelWidth = 0;

                GUILayout.Space(8);
                Foldout_DynamicOptDebug();

            }

            EditorGUILayout.EndVertical();

        }


        void Foldout_DrawTools()
        {
            //if (GetSceneToolsCount() == 0) return;

            GUILayout.Space(3f);

            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.6f, .95f, .55f, bgAlpha * 0.375f), Vector4.one * 1, 1));

            FGUI_Inspector.FoldHeaderStart(ref Get._editor_drawTools, " Scene Tools", FGUI_Resources.BGInBoxStyle, FGUI_Resources.Tex_Tweaks, 22);

            if (Get._editor_drawTools)
            {

                float finDistance = GetFinDistance();

                GUILayout.Space(4f);

                Color preBg = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
                if (GUILayout.Button(new GUIContent("Open Scene Optimizer Window (Experimental)")))
                {
                    OptimizersPrefabsGrabber.ShowSceneOptimizer();
                }
                GUI.backgroundColor = preBg;

                GUILayout.Space(2f);

                GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
                if (GUILayout.Button(new GUIContent("Open Optimizer Prefab Utility Tools")))
                {
                    OptimizersPrefabUtilities.ShowPrefabTools();
                }
                GUI.backgroundColor = preBg;

                GUILayout.Space(2f);


                if (Get.TargetCamera != null)
                {
                    if (Get.TargetCamera.farClipPlane != finDistance)
                    {
                        if (GUILayout.Button(new GUIContent("Sync Camera Clipping Plane with 'World Scale' (" + Mathf.Round(finDistance) + ")", "Make sure range will not be too small for your game needs")))
                        {
                            Get.TargetCamera.farClipPlane = finDistance;
                        }

                        GUILayout.Space(2f);

                    }

                    if (RenderSettings.fog)
                    {
                        bool showFogButton = false;

                        float targetFogDensity = CalculateTargetFogDensity(finDistance, RenderSettings.fogMode);

                        if (RenderSettings.fogMode == FogMode.Linear)
                        {
                            if (Mathf.Round(RenderSettings.fogEndDistance) != Mathf.Round(finDistance * 0.965f)) showFogButton = true;
                        }
                        else
                        {
                            if (RenderSettings.fogDensity != targetFogDensity) showFogButton = true;
                        }

                        if (showFogButton)
                        {
                            if (GUILayout.Button(new GUIContent("Sync Scene Fog with Camera's 'Far Clipping Plane'", "Applying fog density/distance value in scene settings to end on Camera's 'Far Clipping Plane' distance")))
                            {
                                SyncFog(finDistance);
                            }

                            GUILayout.Space(2f);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button(new GUIContent("Enable scene fog and sync range", "Enabling fog in scene settings and syncing it's range with Camera's 'Far Clipping Plane' distance")))
                        {
                            SyncFog(finDistance);
                            RenderSettings.fog = true;
                        }

                        GUILayout.Space(2f);
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("There is no camera in the scene!", MessageType.Warning);
                }

                bool preEe = GUI.enabled;
                if (Application.isPlaying) GUI.enabled = false;

                if (GUILayout.Button(new GUIContent(Get.UpdateOptimizersSystem ? "Disable Optimizers" : "Enable Optimizers", "Just for debugging purpose")))
                {
                    Get.UpdateOptimizersSystem = !Get.UpdateOptimizersSystem;
                }
                GUI.enabled = preEe;

                GUILayout.Space(2f);

            }

            EditorGUILayout.EndVertical();
            GUILayout.EndVertical();
            GUILayout.Space(3f);
        }


        static bool drawDebug = false;
        void Foldout_DrawDebug()
        {

            FGUI_Inspector.FoldHeaderStart(ref drawDebug, " Containers Debug", FGUI_Resources.BGInBoxStyle, FGUI_Resources.Tex_Module, 22);

            if (drawDebug)
            {
                GUILayout.Space(4f);

                if (!Application.isPlaying)
                    EditorGUILayout.HelpBox("Data will be available only during playmode", MessageType.Info);
                else
                    DrawContainersBrowser();

                GUILayout.Space(4f);

                DrawUncontained();
            }

            EditorGUILayout.EndVertical();
        }

        void Foldout_DynamicOptDebug()
        {
            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);

            if (!Application.isPlaying)
            {
                EditorGUILayout.LabelField("During playmode there will be dynamic optimization debug log");
            }
            else
            {
                OptimizersManager mg = Get;
                EditorGUILayout.LabelField("Dynamic Optimizers: " + Get.notContainedDynamicOptimizers.Count);
                for (int i = 0; i < mg.GetClocks.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("[" + i + "] Clock optimizers: " + mg.GetClocks[i].Optimizers.Count, GUILayout.Width(130));
                    EditorGUILayout.LabelField("Adapted delay: " + mg.GetClocks[i].AdaptedDelay, GUILayout.Width(160));
                    EditorGUILayout.LabelField("Last Upd Frame: " + mg.GetClocks[i].LastTickFrame, GUILayout.Width(160));
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

    }
}
