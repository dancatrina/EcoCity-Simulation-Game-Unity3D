using FIMSpace.FEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersPrefabsGrabber
    {
        List<GameObject> AllPrefabs;
        List<OptimizerSceneData> AllWithOptimizers;
        List<OptimizerSceneData> AllWithoutOptimizers;
        List<OptimizerGroupSettings> OptimizeGroups;

        private void CollectSceneData()
        {
            System.Exception exc = null;
            DisplayProgress("Preparing scene data...", 0.05f);

            try
            {
                AllPrefabs = GetAllPrefabsFromScene();
                dataCollected = true;
            }
            catch (System.Exception e) { exc = e; }

            ClearProgress();
            if (exc != null) Debug.LogError("[Scene Optimizer] " + exc);
        }

        private void ClearData()
        {
            RefreshLists();

            dataCollected = false;
        }


        int ItemsPerPage = 10;
        int Page = 0;
        int Pages = 1;

        bool drawOptList = true;
        bool drawGrouping = false;
        bool drawGroupingCustomize = false;
        int groupsCount = 10;
        float nearestCullingDistance = 30;
        float furthestCullingDistance = 1000;
        OptimizerSceneData selectedObj;
        float scaleBias = .7f;

        enum EView { WithoutOpt, WithOpt, RemoveOpt }
        EView view = EView.WithoutOpt;

        private void DisplayData()
        {
            bool preE = GUI.enabled;
            Color preBg = GUI.backgroundColor;
            GUILayout.Space(4);
            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);


            #region Horizontal buttons tab

            EditorGUILayout.BeginHorizontal();
            if (view == EView.WithoutOpt) GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Objects Without Optimizers", EditorStyles.miniButtonLeft)) view = EView.WithoutOpt;
            GUI.backgroundColor = preBg;

            if (view == EView.WithOpt) GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Adjust Optimizers", EditorStyles.miniButton)) view = EView.WithOpt;
            GUI.backgroundColor = preBg;

            if (view == EView.RemoveOpt) GUI.backgroundColor = Color.gray;
            if (GUILayout.Button("Remove Optimizers", EditorStyles.miniButtonRight)) view = EView.RemoveOpt;
            GUI.backgroundColor = preBg;

            #endregion


            if (groupsCount != OptimizeGroups.Count)
            {
                GenerateGroupsSettings();
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(9);

            if (view != EView.RemoveOpt)
            {
                GUILayout.Space(4);

                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);

                FGUI_Inspector.HeaderBox(ref drawGrouping, "1. Grouping Tools", true, FGUI_Resources.Tex_Gear);


                #region Grouping

                if (drawGrouping)
                {
                    GUILayout.Space(5);
                    EditorGUILayout.HelpBox("Helping Culling Containers with grouped parameters (Check Manual for more info) ", MessageType.Info);
                    GUILayout.Space(4);

                    EditorGUIUtility.labelWidth = 180;

                    GUILayout.Space(4);
                    EditorGUI.BeginChangeCheck();
                    nearestCullingDistance = EditorGUILayout.FloatField("Nearest Culling Distance", nearestCullingDistance);
                    if (EditorGUI.EndChangeCheck()) FrameManager(nearestCullingDistance * 1.05f);

                    OptimizersManager.Instance._editorDrawSphere1 = nearestCullingDistance;
                    OptimizersManager.Instance._editorDrawSphere2 = furthestCullingDistance;

                    if (nearestCullingDistance < 0f) nearestCullingDistance = 0f;
                    EditorGUI.BeginChangeCheck();
                    furthestCullingDistance = EditorGUILayout.FloatField("Furthest Culling Distance", furthestCullingDistance);
                    if (EditorGUI.EndChangeCheck()) FrameManager(furthestCullingDistance * 1.05f);
                    EditorGUIUtility.labelWidth = 0;

                    groupsCount = EditorGUILayout.IntSlider("Groups Count", groupsCount, 5, 32);
                    GUILayout.Space(4);

                    float step = 1f / (float)(groupsCount - 1);

                    GUILayout.Space(6);
                    FGUI_Inspector.HeaderBox(ref drawGroupingCustomize, "2. Customize Group Prameters", true, FGUI_Resources.Tex_Default);

                    if (drawGroupingCustomize)
                    {
                        GUILayout.Space(3);
                        EditorGUILayout.HelpBox("Here you can select which stuff should be used on optimizers components", MessageType.None);
                        GUILayout.Space(5);

                        for (int i = 0; i < OptimizeGroups.Count; i++)
                        {
                            Color preC = GUI.color;
                            GUI.color = i % 2 == 0 ? owhite : olgray;
                            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyleH);
                            GUI.color = preC;
                            EditorGUILayout.BeginHorizontal();
                            OptimizerGroupSettings s = OptimizeGroups[i];

                            EditorGUIUtility.labelWidth = 1;
                            s.Enabled = EditorGUILayout.Toggle(GUIContent.none, s.Enabled, GUILayout.Width(28));

                            if (s.Enabled == false) GUI.enabled = false;

                            bool preee = GUI.enabled;
                            EditorGUIUtility.labelWidth = 49;
                            EditorGUILayout.LabelField("Group " + i, GUILayout.Width(50));
                            GUILayout.Space(8);
                            EditorGUIUtility.labelWidth = 39;
                            s.LODCount = EditorGUILayout.IntField(new GUIContent("LODs", "Target number of LOD levels for this culling range"), s.LODCount, GUILayout.Width(58));
                            if (s.LODCount < 2) s.LODCount = 2; if (s.LODCount > 5) s.LODCount = 5;
                            GUILayout.Space(8);
                            //s.TransitionTime = EditorGUILayout.FloatField(new GUIContent("Fade", "Transition Duration - 0 to disable"), s.TransitionTime, GUILayout.Width(58));
                            //GUILayout.Space(8);
                            s.CullingMode = (OptimizerGroupSettings.EOGCullingMode)
                                EditorGUILayout.EnumPopup(GUIContent.none, s.CullingMode, GUILayout.Width(86));
                            //s.CullingMode = EditorGUILayout.Toggle(new GUIContent("DOTS", "If you wish to use progressive culling (needed packages if not imported in this project)"), s.CullingMode, GUILayout.Width(58));

                            if (s.All) GUI.enabled = false;
                            EditorGUIUtility.labelWidth = 18;
                            GUILayout.Space(9);
                            s.Lights = EditorGUILayout.Toggle(new GUIContent(_LightIcon, "If you want to optimize light components in target objects"), s.Lights, GUILayout.Width(34));
                            GUILayout.Space(9);
                            s.Meshes = EditorGUILayout.Toggle(new GUIContent(_MeshIcon, "If you want to optimize light components in target objects"), s.Meshes, GUILayout.Width(34));
                            GUILayout.Space(9);
                            s.Particles = EditorGUILayout.Toggle(new GUIContent(_ParticleIcon, "If you want to optimize particle effects in target objects"), s.Particles, GUILayout.Width(34));
                            GUILayout.Space(9);
                            //s.UnityLOD = EditorGUILayout.Toggle(new GUIContent(_LODgIcon, "If you want let optimizer control Unity LOD Groups"), s.UnityLOD, GUILayout.Width(34));
                            //GUILayout.Space(9);
                            EditorGUIUtility.labelWidth = 29;

                            GUI.enabled = preee;
                            s.All = EditorGUILayout.Toggle(new GUIContent("All", "If you wish to optimize everything available for Optimizers"), s.All, GUILayout.Width(58));
                            EditorGUIUtility.labelWidth = 0;

                            GUILayout.FlexibleSpace();
                            EditorGUILayout.LabelField("Cull At " + GetCullDistance(nearestCullingDistance, furthestCullingDistance, step, i), GUILayout.Width(70));

                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.EndVertical();
                            GUI.enabled = preE;
                        }

                        GUILayout.Space(4);
                        if (GUILayout.Button("Switch 'All' for each group")) for (int i = 0; i < OptimizeGroups.Count; i++) OptimizeGroups[i].All = !OptimizeGroups[i].All;
                        if (GUILayout.Button("Switch Culling Mode for each group")) for (int i = 0; i < OptimizeGroups.Count; i++) { OptimizeGroups[i].CullingMode += 1; if ((int)OptimizeGroups[i].CullingMode > 2) OptimizeGroups[i].CullingMode = 0; }
                        //if (GUILayout.Button("Switch Transition Duration for each group")) for (int i = 0; i < OptimizeGroups.Count; i++) if (OptimizeGroups[i].TransitionTime == 0f) OptimizeGroups[i].TransitionTime = 1f; else OptimizeGroups[i].TransitionTime = 0f;
                    }
                    else
                    {
                        string gInfo = "Depending on objects sizes there will be assigned different culling distances:\n";
                        for (int i = 0; i < groupsCount; i++)
                        {
                            if (i > 0) gInfo += "   ";
                            gInfo += "[" + i + "] Cull At " + GetCullDistance(nearestCullingDistance, furthestCullingDistance, step, i);
                        }

                        EditorGUILayout.HelpBox(gInfo, MessageType.None);
                    }

                }
                else
                {
                    OptimizersManager.Instance._editorDrawSphere1 = 0f;
                    OptimizersManager.Instance._editorDrawSphere2 = 0f;
                }

                #endregion


                EditorGUILayout.EndVertical();
            }


            GUILayout.Space(9);


            if (view == EView.WithoutOpt)
            {
                GUI.backgroundColor = new Color(0.3f, 1f, 0.4f, 1f);
                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyleH);
                if (GUILayout.Button("Add Optimizers to prefabs! " + (AllWithoutOptimizers.Count > 500 ? " (it can take time)" : ""), GUILayout.Height(35)))
                {
                    AddOptimizersToPrefabs();
                }
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = preBg;
            }
            else if (view == EView.WithOpt)
            {
                GUI.backgroundColor = new Color(0.25f, 1f, 0.75f, 1f);
                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyleH);
                if (GUILayout.Button("Refresh optimizers settings! " + (AllWithOptimizers.Count > 500 ? " (it can take time)" : ""), GUILayout.Height(35)))
                {
                    RefreshOptimizersOnPrefabs();
                }
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = preBg;
            }
            else
            {
                GUI.backgroundColor = new Color(0.7f, 1f, 0.1f, 1f);
                EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyleH);
                if (GUILayout.Button("Remove optimizers from prefabs below", GUILayout.Height(32)))
                {
                    DestroyOptimizersOnPrefabs();
                }
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = preBg;
            }


            GUILayout.Space(14);
            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyle);

            bool dataAvailable = false;

            if (view == EView.WithoutOpt)
            {
                if (AllWithoutOptimizers.Count != 0) dataAvailable = true;
            }
            else
                if (AllWithOptimizers.Count != 0) dataAvailable = true;


            if (dataAvailable)
            {

                string title = "";
                if (view == EView.WithoutOpt) title = ("3. Prepare Optimizers (" + AllWithoutOptimizers.Count + ")");
                else if (view == EView.WithOpt) title = ("3. Re-Adjust Optimizers (" + AllWithOptimizers.Count + ")");
                else if (view == EView.RemoveOpt) title = ("Removing Optimizers (" + AllWithOptimizers.Count + ")");
                FGUI_Inspector.HeaderBox(ref drawOptList, title, true, _Icon, 24, 24);


                #region Optimization Customize

                if (drawOptList)
                {
                    FGUI_Inspector.DrawUILine(0.5f, 0.4f, 1, 4, 1f);


                    GUILayout.Space(4);
                    ItemsPerPage = EditorGUILayout.IntSlider("Items Per Page", ItemsPerPage, 8, 50);
                    GUILayout.Space(4);

                    if (view != EView.RemoveOpt)
                    {

                        #region Main Header Tabs

                        toRemoveWith = new List<OptimizerSceneData>();
                        toRemoveWithout = new List<OptimizerSceneData>();

                        List<OptimizerSceneData> targetList;
                        if (view != EView.WithOpt) targetList = AllWithoutOptimizers; else targetList = AllWithOptimizers;
                        Pages = (targetList.Count - 1) / ItemsPerPage;

                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("<<<")) Page = 0;
                        if (GUILayout.Button("<")) Page = ChangePage(Page, Pages, -1);
                        EditorGUILayout.LabelField(Page + " / " + Pages, FGUI_Resources.HeaderStyle);
                        if (GUILayout.Button(">")) Page = ChangePage(Page, Pages, 1);
                        if (GUILayout.Button(">>>")) Page = Pages;
                        EditorGUILayout.EndHorizontal();
                        FGUI_Inspector.DrawUILine(0.5f, 0.4f, 1, 4, 1f);

                        #endregion


                        if (view == EView.WithoutOpt)
                        {
                            EditorGUILayout.HelpBox("Adjust settings for generating optimizers components on selected prefabs", MessageType.None);
                            GUILayout.Space(3);

                            scaleBias = EditorGUILayout.Slider("Scale Bias:", scaleBias, 0.1f, 1.25f);

                            int start = Page * ItemsPerPage;
                            for (int i = start; i < start + ItemsPerPage; i++)
                                if (i < targetList.Count)
                                    DrawOptimizerSceneDataGUI(i, targetList[i]);
                        }
                        else if (view == EView.WithOpt)
                        {

                            EditorGUILayout.HelpBox("There you can re-adjust optimizers settings on prefabs which already have optimizers component", MessageType.None);
                            GUILayout.Space(3);

                            scaleBias = EditorGUILayout.Slider("Scale Bias:", scaleBias, 0.1f, 1.25f);

                            int start = Page * ItemsPerPage;
                            for (int i = start; i < start + ItemsPerPage; i++)
                                if (i < targetList.Count)
                                    DrawOptimizerSceneDataGUI(i, targetList[i]);
                        }


                        if (selectedObj != null)
                        {
                            if (selectedObj.prefabObject != null)
                            {
                                OptimizersManager.Instance._editorDrawSphere3Origin = selectedObj.prefabObject.transform.position;
                                OptimizersManager.Instance._editorDrawSphere3 = selectedObj.scale * scaleBias;

                                OptimizersManager.Instance._editorDrawSphere4Origin = selectedObj.prefabObject.transform.position;
                                int gr = GetDistanceRange(selectedObj.scale * scaleBias, groupsCount);
                                OptimizersManager.Instance._editorDrawSphere4 = GetCullDistance(nearestCullingDistance, furthestCullingDistance, (1f / (float)(groupsCount - 1)), gr);
                            }
                        }

                        for (int i = 0; i < toRemoveWith.Count; i++) AllWithOptimizers.Remove(toRemoveWith[i]);
                        for (int i = 0; i < toRemoveWithout.Count; i++) AllWithoutOptimizers.Remove(toRemoveWithout[i]);
                    }
                    else
                    {
                        EditorGUILayout.HelpBox("There you can remove Optimizers components from found prefabs (For debugging or cleaning)", MessageType.Info);

                        List<OptimizerSceneData> targetList = AllWithOptimizers;

                        int start = Page * ItemsPerPage;
                        for (int i = start; i < start + ItemsPerPage; i++)
                            if (i < targetList.Count)
                                DrawOptimizerSceneDataGUI(i, targetList[i]);
                    }
                }

                #endregion

            }
            else
            {
                EditorGUILayout.HelpBox(" Not found data to work on here", MessageType.Info);
            }


            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
        }


        readonly Color olgray = new Color(0.72f, 0.72f, 0.72f, .8f);
        readonly Color owhite = new Color(0.95f, 0.95f, 0.95f, .9f);
        void DrawOptimizerSceneDataGUI(int index, OptimizerSceneData o)
        {
            if (o == null) return;

            Color preC = GUI.color;
            GUI.color = index % 2 == 0 ? owhite : olgray;
            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxStyleH);
            GUI.color = preC;


            EditorGUILayout.BeginHorizontal();

            int group = GetDistanceRange(o.scale * scaleBias, groupsCount);
            float cullDist = GetCullDistance(nearestCullingDistance, furthestCullingDistance, (1f / (float)(groupsCount - 1)), group);

            EditorGUIUtility.labelWidth = 34;
            EditorGUILayout.ObjectField("[" + index + "]", o.prefabObject, typeof(GameObject), true, GUILayout.Width(140));
            EditorGUIUtility.labelWidth = 50;
            GUILayout.Space(6);
            o.scale = EditorGUILayout.FloatField(" Scale: ", o.scale, GUILayout.Width(98));
            GUILayout.Space(8);
            EditorGUILayout.LabelField(" Group: " + group + " : Cull At " + Mathf.Round(cullDist), GUILayout.Width(130));
            EditorGUIUtility.labelWidth = 0;

            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent(_CamIcon), GUILayout.Height(20), GUILayout.Width(24)))
            {
                selectedObj = o;

                if (o.sceneObject)
                {
                    Selection.activeObject = o.sceneObject;
                    FrameObject(o.sceneObject.transform, cullDist);
                }
            }
            GUILayout.Space(4);

            EditorGUIUtility.labelWidth = 46;
            EditorGUILayout.ObjectField("(Scene)", o.sceneObject, typeof(GameObject), true, GUILayout.Width(120));

            if (GUILayout.Button("X", GUILayout.Height(20), GUILayout.Width(24)))
                if (o.optimizer) toRemoveWith.Add(o); else toRemoveWithout.Add(o);

            EditorGUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();
        }

        List<OptimizerSceneData> toRemoveWithout;
        List<OptimizerSceneData> toRemoveWith;

    }
}
