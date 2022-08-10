using FIMSpace.FEditor;
#if OPTIMIZERS_DOTS_IMPORTED
using Unity.Burst;
#endif
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_BaseEditor
    {
        bool drawObstaclesDetection = true;
        void Fold_DrawModuleObstacles()
        {
            FGUI_Inspector.FoldSwitchableHeaderStart(ref Get.UseObstacleDetection, ref drawObstaclesDetection, Lang("Obstacles Detection"), null, _TexDetectionIcon, 22, "Tooltip", LangBig());

            if (drawObstaclesDetection && Get.UseObstacleDetection)
            {
                GUILayout.Space(3f);

                Obstacles_DrawObstaclesSettings();

                GUILayout.Space(5f);
            }
        }

        bool drawMultiShape = true;
        void Fold_DrawModuleMultiShape()
        {
            FGUI_Inspector.FoldSwitchableHeaderStart(ref Get.UseMultiShape, ref drawMultiShape, Lang("Complex Detection Shape"), null, _TexShapeIcon, 22, "Tooltip", LangBig());

            if (Get.UseMultiShape) if (Get.UseObstacleDetection) Get.UseObstacleDetection = false;
            if (Get.UseDOTS) Get.UseObstacleDetection = false;

            if (drawMultiShape && Get.UseMultiShape)
            {
                if (Get.OptimizingMethod == EOptimizingMethod.Dynamic || Get.OptimizingMethod == EOptimizingMethod.TriggerBased) Get.OptimizingMethod = EOptimizingMethod.Effective;

                GUILayout.Space(3f);

                DefaultInspectorStackMultiShape();

                GUILayout.Space(5f);
            }
        }

        bool drawDOTS = true;
        void Fold_DrawDOTS()
        {
            bool e = GUI.enabled;
            FGUI_Inspector.FoldSwitchableHeaderStart(ref Get.UseDOTS, ref drawDOTS, "Progressive Culling (Experimental)", null, FGUI_Resources.Tex_Optimization, 22, "Tooltip", LangBig());

            if (drawDOTS && Get.UseDOTS)
            {
                if (Application.isPlaying) GUI.enabled = false;

                GUILayout.Space(3f);

#if OPTIMIZERS_DOTS_IMPORTED

                #region Hleper buttons

                if (Get.UseDOTS)
                {
                    if (BurstCompiler.Options.EnableBurstCompilation == false)
                    {
                        GUILayout.Space(3f);
                        EditorGUILayout.HelpBox("Enable 'Burst Compilation' for performance boost!", MessageType.Warning);
                        if (GUILayout.Button("Enable Burst Compilation")) BurstCompiler.Options.EnableBurstCompilation = true;
                    }
                }

                if (Get.UseObstacleDetection)
                {
                    GUILayout.Space(3f);
                    EditorGUILayout.HelpBox("You shouldn't use Progressive Culling in the same time with Obstacle Detection!", MessageType.Warning);
                }

                if (OptimizersManager.Instance.UseDOTSProgressiveCulling == false)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("You must enable 'Progressive Culling' in optimizers manager to make it work!", MessageType.Info);
                    if (GUILayout.Button("Switch", GUILayout.ExpandHeight(true))) OptimizersManager.Instance.UseDOTSProgressiveCulling = true;
                    EditorGUILayout.EndHorizontal();
                }

                #endregion


                // Header ----------------
                SerializedProperty prp = serializedObject.FindProperty("UseDOTS"); prp.NextVisible(false);
                GUILayout.Space(1);
                EditorGUILayout.HelpBox("With DOTS you can use meshes for detection or other custom shapes", MessageType.None);
                GUILayout.Space(3);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prp); prp.NextVisible(false);
                if (Get.DOTSObstacleType == EDOTSObstacle.SeeThrough) if (!OptimizersManager.Instance.SupportNotObstacles) EditorGUILayout.HelpBox("'Not obstacles' are disabled in Optimizers Manager!", MessageType.None);
                EditorGUILayout.EndHorizontal();

                EditorGUIUtility.labelWidth = 130;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(prp); prp.NextVisible(false);

                if (GUILayout.Button(FGUI_Resources.Tex_Refresh, GUILayout.Width(24), GUILayout.Height(20)))
                {
                    if (Get.DOTSDetection == EDOTSDetection.Auto)
                        Get.DOTSFindMeshes(true);
                    else
                    {
                        Get.DOTSRadius = Get.DetectionRadius;
                        Get.DOTSOffset = Get.DetectionOffset;
                        Get.DOTSSize = Get.DetectionBounds;
                    }
                }

                EditorGUILayout.EndHorizontal();

                if (Get.DOTSMeshData == null) Get.DOTSMeshData = new System.Collections.Generic.List<Optimizer_Base.DOTS_DetectionData>();

                if (Get.DOTSDetection == EDOTSDetection.Auto)
                {
                #region Drawing mesh fields

                    Get.DOTSFindMeshes();

                    if (Get.DOTSMeshData.Count == 0) { Get.DOTSMeshData.Add(new Optimizer_Base.DOTS_DetectionData()); serializedObject.Update(); serializedObject.ApplyModifiedProperties(); return; }
                    GUILayout.Space(3);

                    for (int i = 0; i < prp.arraySize; i++)
                    {
                        if (Get.DOTSMeshData.Count != prp.arraySize) break;
                        if (Get.DOTSMeshData[i] == null) Get.DOTSMeshData[i] = new Optimizer_Base.DOTS_DetectionData();

                        EditorGUILayout.BeginHorizontal(FGUI_Resources.BGInBoxStyle);
                        SerializedProperty dotsEl = prp.GetArrayElementAtIndex(i).FindPropertyRelative("SharedMesh");
                        EditorGUIUtility.labelWidth = 90;

                        if (dotsEl != null)
                        {
                            EditorGUILayout.PropertyField(dotsEl); dotsEl.NextVisible(true);
                            EditorGUIUtility.labelWidth = 45;
                            EditorGUILayout.PropertyField(dotsEl, new GUIContent("Scene", "Scene owner of the mesh"), GUILayout.Width(130));
                            EditorGUIUtility.labelWidth = 130;
                        }

                        if (i == 0)
                        {
                            if (GUILayout.Button("C", GUILayout.Width(24)))
                            {
                                Optimizer_Base.DOTS_DetectionData d0 = null; if (Get.DOTSMeshData.Count != 0 ) d0 = Get.DOTSMeshData[0];
                                Get.DOTSMeshData.Clear(); Get.DOTSMeshData.Add(d0);
                                serializedObject.Update(); serializedObject.ApplyModifiedProperties();
                                return;
                            }

                            if (GUILayout.Button("+", GUILayout.Width(24)))
                            {
                                Get.DOTSMeshData.Add(new Optimizer_Base.DOTS_DetectionData()); serializedObject.Update(); serializedObject.ApplyModifiedProperties();
                                return;
                            }
                        }
                        else if (GUILayout.Button("X", GUILayout.Width(24))) { Get.DOTSMeshData.RemoveAt(i); serializedObject.Update(); return; }

                        EditorGUILayout.EndHorizontal();

                    }

                    Optimizer_Base.DOTS_DetectionData data = Get.DOTSMeshData[0];
                    if (data.SharedMesh == null)
                    {
                        GUILayout.Space(3);
                        EditorGUILayout.HelpBox("No mesh so detection shape will be used", MessageType.Info);
                    }

                    Get.DOTSRadius = Get.DetectionRadius;
                    Get.DOTSOffset = Get.DetectionOffset;
                    Get.DOTSSize = Get.DetectionBounds;

                #endregion
                }
                else if (Get.DOTSDetection == EDOTSDetection.Custom)
                {
                    EditorGUILayout.HelpBox("With 'Custom' you can add colliders manually (as this optimizer children) and add to them component 'OptimizersReference'", MessageType.Info);
                }
                else
                {
                #region Drawing simple mesh fields

                    GUILayout.Space(3);
                    if (Get.DOTSDetection == EDOTSDetection.Sphere)
                    {
                        prp.NextVisible(false);
                        EditorGUILayout.PropertyField(prp); prp.NextVisible(false); prp.NextVisible(false);
                        EditorGUILayout.PropertyField(prp);// prp.NextVisible(false);
                        //EditorGUILayout.PropertyField(prp);

                    }
                    else
                    {
                        prp.NextVisible(false);
                        EditorGUILayout.PropertyField(prp); prp.NextVisible(false);
                        EditorGUILayout.PropertyField(prp); prp.NextVisible(false); //prp.NextVisible(false);
                        //EditorGUILayout.PropertyField(prp);
                    }

                #endregion
                }

#else

#if UNITY_2019_1_OR_NEWER
                EditorGUILayout.HelpBox("To use Progressive Culling you have to import packages like\nBurst, Jobs, Collections and Mathematics.\nAfter importing you will unlock new parameters here.", MessageType.Info);
                GUILayout.Space(3);
                EditorGUILayout.HelpBox("First import 'Jobs' it will give you automatically 'mathematics' and 'collections' and then import 'Burst'", MessageType.None);
                GUILayout.Space(3);
                EditorGUILayout.HelpBox("Search at Unity Registry and enable 'Show Preview Packages' under 'Advanced'", MessageType.None);
#if UNITY_2019_4_OR_NEWER
                GUILayout.Space(3);
                if (GUILayout.Button("Open Package Manager", GUILayout.Height(30))) UnityEditor.PackageManager.UI.Window.Open("");
#endif

#else
                EditorGUILayout.HelpBox("Progressive Culling is supported since Unity version 2019.1.", MessageType.Info);
                GUILayout.Space(3);
#endif
#endif

                GUILayout.Space(5f);
                GUI.enabled = e;
                EditorGUIUtility.labelWidth = 0;

            }

        }

        void Fold_DrawExtra()
        {
            bool e = GUI.enabled;
            FGUI_Inspector.FoldHeaderStart(ref Get._editor_DrawExtra, "Extra Settings", FGUI_Resources.BGInBoxStyle, FGUI_Resources.Tex_Add, 22);

            if (Get._editor_DrawExtra)
            {
                if (Application.isPlaying) GUI.enabled = false;
                GUILayout.Space(5f);

                El_ContainerSwitch();
                GUILayout.Space(3f);
                if (OptimizersManager.Exists) if (GUILayout.Button("Select Optimizers Manager")) Selection.activeGameObject = OptimizersManager.Instance.gameObject;

                GUILayout.Space(5f);
                GUI.enabled = e;
            }

            EditorGUILayout.EndVertical();

        }


    }

}

