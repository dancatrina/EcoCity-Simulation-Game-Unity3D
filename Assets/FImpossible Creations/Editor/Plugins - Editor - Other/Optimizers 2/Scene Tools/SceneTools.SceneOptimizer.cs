using FIMSpace.FEditor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersPrefabsGrabber : EditorWindow
    {
        public static OptimizersPrefabsGrabber Current;
        Texture2D _Icon;
        Texture _CamIcon;
        Texture _LightIcon;
        Texture _ParticleIcon;
        Texture _LODgIcon;
        Texture _MeshIcon;
        bool dataCollected = false;
        bool fastMode = false;
        bool ignoreSceneSave = false;

        private void OnEnable()
        {
            Current = this;
            if (_Icon == null) _Icon = Resources.Load("FIMSpace/Optimizers 2/OptManagerIconSmall") as Texture2D;
            if (_CamIcon == null) _CamIcon = EditorGUIUtility.IconContent("Camera Icon").image;
            if (_LightIcon == null) _LightIcon = EditorGUIUtility.IconContent("Light Icon").image;
            if (_ParticleIcon == null) _ParticleIcon = EditorGUIUtility.IconContent("Particle Effect").image;
            if (_LODgIcon == null) _LODgIcon = EditorGUIUtility.IconContent("LODGroup Icon").image;
            if (_MeshIcon == null) _MeshIcon = EditorGUIUtility.IconContent("MeshRenderer Icon").image;
            titleContent = new GUIContent(" Scene Optimizer", _Icon);

            minSize = new Vector2(260, 160);
            RefreshLists();

            if (Camera.main)
                furthestCullingDistance = Camera.main.farClipPlane;
        }

        private void OnDestroy()
        {
            OptimizersManager.Instance._editorDrawSphere1 = 0f;
            OptimizersManager.Instance._editorDrawSphere2 = 0f;
            OptimizersManager.Instance._editorDrawSphere3 = 0f;
        }

        private void OnDisable()
        {
            OptimizersManager.Instance._editorDrawSphere1 = 0f;
            OptimizersManager.Instance._editorDrawSphere2 = 0f;
            OptimizersManager.Instance._editorDrawSphere3 = 0f;
        }

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            EditorGUIUtility.labelWidth = 250;
            fastMode = EditorGUILayout.Toggle("Fast Mode (Experimental - Without Saving)", fastMode);
            EditorGUIUtility.labelWidth = 0;

            if (fastMode)
            {
                bool areUnsaved = false;
                for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
                    if (EditorSceneManager.GetSceneAt(i).isDirty) { areUnsaved = true; break; }

                if (areUnsaved)
                {
                    EditorGUILayout.HelpBox("  To use Scene Optimizer, loaded scanes must be unchanged! ", MessageType.Info);
                    if (GUILayout.Button("Save Scene Changes"))
                    {
                        try
                        {
#if UNITY_2019_4_OR_NEWER
                            AssetDatabase.DisallowAutoRefresh();
#endif
                            EditorSceneManager.SaveOpenScenes();
                        }
                        catch (System.Exception) { }
#if UNITY_2019_4_OR_NEWER
                        AssetDatabase.AllowAutoRefresh();
#endif
                    }

                    EditorGUIUtility.labelWidth = 250;
                    ignoreSceneSave = EditorGUILayout.Toggle("Ignore Scene Save (Not Recommended)", ignoreSceneSave);
                    EditorGUIUtility.labelWidth = 0;

                    if (!ignoreSceneSave)
                        return;
                }
                else
                    ignoreSceneSave = false;
            }
            else
                ignoreSceneSave = false;


            if (Application.isPlaying) GUI.enabled = false;
            Current = this;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);

            GUILayout.Space(4);
            EditorGUILayout.HelpBox("In this window you will scan prefabs from whole scene and apply optimizers to them with adjusted automatic settings", MessageType.Info);
            GUILayout.Space(4);
            CheckData();

            if (!dataCollected)
            {
                Color preBg = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.6f, 1f, 0.6f);
                if (GUILayout.Button("Collect Scene Prefabs"))
                {
                    CollectSceneData();

                    if (AllWithoutOptimizers.Count == 0 && AllWithOptimizers.Count == 0)
                        EditorUtility.DisplayDialog("No Prefabs", "Could not find any prefab on the scene", "OK");
                    else
                    {
                        if (AllWithoutOptimizers.Count != 0) view = EView.WithoutOpt;
                    }
                }

                GUI.backgroundColor = preBg;
            }
            else
                if (GUILayout.Button("Clear Collected Data"))
            {
                ClearData();
            }


            if (!dataCollected)
            {
                GUILayout.Space(4);
                EditorGUILayout.HelpBox("  No scene data collected yet!", MessageType.Info);
            }
            else
            {
                DisplayData();
            }

            //if (waitingForSave)
            //{
            //    GUILayout.Space(8);
            //    if (GUILayout.Button(new GUIContent("Save " + latelyChanged + " Assets (Not Required) " + (latelyChanged > 60 ? "It can take big amount of time" : ""), "It can take time by Unity if your scene had many prefabs\nit will save changes to each prefab one by one - it's unavoidable\nSometimes closing project and opening again seems to work faster\ninstead of pressing this button / ctrl+s"), GUILayout.Width(200), GUILayout.Height(40)))
            //    //if (GUILayout.Button("\n[Save Assets to see results on scene]\nit can take time by Unity if your scene had many prefabs\nit will save changes to each prefab one by one - it's unavoidable\nSometimes closing project and opening again seems to work faster\ninstead of pressing this button / ctrl+s\n"))
            //    {
            //        try
            //        {
            //            System.Threading.Thread.Sleep(20);
            //            AssetDatabase.DisallowAutoRefresh();
            //            DisplayProgress("Unity Asset Database is Saving Prefabs...", 0.95f);
            //            System.Threading.Thread.Sleep(20);
            //            AssetDatabase.SaveAssets();
            //            System.Threading.Thread.Sleep(20);
            //        }
            //        catch (System.Exception) { }

            //        waitingForSave = false;
            //        ClearProgress();
            //        AssetDatabase.AllowAutoRefresh();
            //    }
            //}

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        public static void ShowSceneOptimizer()
        {
            GetWindow<OptimizersPrefabsGrabber>(false, " Scene Optimizer", true).OnEnable();
        }

    }
}
