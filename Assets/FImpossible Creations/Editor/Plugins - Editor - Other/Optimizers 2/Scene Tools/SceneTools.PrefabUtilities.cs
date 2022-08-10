using FIMSpace.FEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersPrefabUtilities : EditorWindow
    {
        public static OptimizersPrefabUtilities Current;
        Texture2D _Icon;
        private float ScaleUp = 1f;

        private void OnEnable()
        {
            Current = this;
            if (_Icon == null) _Icon = Resources.Load("FIMSpace/Optimizers 2/Optimizers Essential Icon Small") as Texture2D;
            titleContent = new GUIContent(" Prefab Utilities", _Icon);

            minSize = new Vector2(260, 160);
        }

        Vector2 scrollPos = Vector2.zero;

        void OnGUI()
        {
            if (Application.isPlaying) GUI.enabled = false;

            Current = this;
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);
            EditorGUILayout.BeginVertical(FGUI_Resources.BGInBoxBlankStyle);

            GUILayout.Space(4);
            EditorGUILayout.HelpBox("In this window you will find handy tools for selected prefab with optimizer", MessageType.None);
            FGUI_Inspector.DrawUILine(0.3f, 0.35f, 1, 6);
            GUILayout.Space(4);

            Optimizer_Base opt = null;
            if (Selection.activeGameObject) opt = Selection.activeGameObject.GetComponent<Optimizer_Base>();

            if (opt != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Try auto find detection shape scale (all selected)", GUILayout.Height(22)))
                {
                    for (int i = 0; i < Selection.gameObjects.Length; i++)
                    {
                        Optimizer_Base opti = Selection.gameObjects[i].GetComponent<Optimizer_Base>();
                        if (opti) opti.TryAutoComputeDetectionShape(ScaleUp);

                    }
                    opt.TryAutoComputeDetectionShape(ScaleUp);
                }

                EditorGUIUtility.labelWidth = 60;
                ScaleUp = EditorGUILayout.FloatField("Scale Up", ScaleUp);
                EditorGUIUtility.labelWidth = 0;
                EditorGUILayout.EndHorizontal();

                GameObject prefabed = Optimizers_LODTransport.GetProjectPrefabSimple(opt.gameObject);

                {
                    //if (GUILayout.Button("Convert unity LOD Group to use Only Optimizers (Remove LOD Group Component)", GUILayout.Height(22)))
                    //{
                    //    List<LODGroup> lodGroups = Optimizer_Base.FindComponentsInAllChildren<LODGroup>(prefabed.transform);
                    //    if (lodGroups.Count > 0)
                    //    {
                    //        for (int i = 0; i < lodGroups.Count; i++)
                    //        {
                    //            TransferLODGroupToOptimizer(lodGroups[i], opt);
                    //            DestroyImmediate(lodGroups[i], true);
                    //        }
                    //    }
                    //    else
                    //        EditorUtility.DisplayDialog("No LOD Group components found", "No LOD Group components found", "OK");
                    //}
                }

                if (GUILayout.Button("Set unity LOD Group to be used by Optimizers (LOD Group will remain)", GUILayout.Height(22)))
                {
                    List<LODGroup> lodGroups = Optimizer_Base.FindComponentsInAllChildren<LODGroup>(opt.transform);
                    if (lodGroups.Count > 0)
                    {
                        for (int i = 0; i < lodGroups.Count; i++)
                            TransferLODGroupToOptimizer(lodGroups[i], opt);
                    }
                    else
                        EditorUtility.DisplayDialog("No LOD Group components found", "No LOD Group components found", "OK");
                }


                if (prefabed)
                {
                    GUI.enabled = false;
                    if (GUILayout.Button("Search for fade parameters in mesh renderer components and set crossfade settings", GUILayout.Height(22)))
                    {
                        EssentialOptimizer eopt = opt as EssentialOptimizer;
                        if (eopt)
                        {
                            List<Renderer> rends = new List<Renderer>();
                            for (int i = 0; i < opt.GetToOptimizeCount(); i++)
                            {
                                Component c = opt.GetOptimizedComponent(i);
                                if (c == null) continue;
                                Renderer r = c as Renderer;
                                if (r == null) continue;
                                rends.Add(r);
                            }

                            if (rends.Count == 0)
                                EditorUtility.DisplayDialog("Not Found Renderers", "Not found renderers in optimization list", "OK");
                            else
                            {
                                for (int i = 0; i < rends.Count; i++)
                                {
                                    Renderer r = rends[i];
                                    if (r.sharedMaterial)
                                    {
                                        int kI = -1;

                                        for (int k = 0; k < crossfadeKewords.Length; k++)
                                            if (r.sharedMaterial.HasProperty(crossfadeKewords[k]))
                                            {
                                                kI = k;
                                                bool isFloat = false;
                                                try { r.sharedMaterial.GetFloat(crossfadeKewords[k]); isFloat = true; } catch (System.Exception) { }

                                                //bool isColor = false;
                                                if (!isFloat)
                                                    try { r.sharedMaterial.GetColor(crossfadeKewords[k]); /*isColor = true;*/ } catch (System.Exception) { }



                                                break;
                                            }

                                        if (kI != -1)
                                        {

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Scriptable Optimizer not yet supported", "Scriptable Optimizer not yet supported", "OK");
                        }
                    }
                    GUI.enabled = true;

                    Optimizer_Base opb = prefabed.GetComponentInChildren<Optimizer_Base>();

                    if (opb)
                    {
                        GUI.backgroundColor = new Color(0.8f, 0.4f, 0.3f, 1f);
                        if (GUILayout.Button("Remove optimizer from the prefab", GUILayout.Height(22)))
                        {
                            DestroyImmediate(opb, true);
                            AssetDatabase.SaveAssets();
                        }
                        GUI.backgroundColor = Color.white;
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox(" Selected object is not detected as prefab!", MessageType.Warning);
                }
            }
            else
            {
                GameObject prefabed = null;
                if (Selection.activeGameObject)
                {
                    prefabed = Optimizers_LODTransport.GetProjectPrefabSimple(Selection.activeGameObject);
                    if (prefabed)
                    {
                        if (GUILayout.Button("Add optimizer to the prefab", GUILayout.Height(30)))
                        {
                            EssentialOptimizer eopt = prefabed.AddComponent<EssentialOptimizer>();
                            AssetDatabase.SaveAssets();

                            prefabed = Optimizers_LODTransport.GetProjectPrefabSimple(Selection.activeGameObject);
                            if (prefabed)
                            {
                                eopt = prefabed.GetComponent<EssentialOptimizer>();

                                eopt.AssignComponentsToBeOptimizedFromAllChildren(eopt.gameObject);
                                eopt.HiddenCullAt = 0;
                                //if (eopt)
                                //    for (int i = 0; i < eopt.ToOptimize.Count; i++)
                                //        eopt.ToOptimize[i].GenerateLODParameters();

                                //eopt.EditorUpdate();
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                            else
                                Debug.Log("Not Prefabed after save");

                        }
                    }
                }

                if (!prefabed)
                    EditorGUILayout.HelpBox(" Select game object with optimizer for more options", MessageType.Info);
            }

            GUILayout.Space(4);
            FGUI_Inspector.DrawUILine(0.3f, 0.35f, 1, 6);
            EditorGUILayout.HelpBox("  ! In next updates there will me more !  ", MessageType.Info);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        static readonly string[] crossfadeKewords = new string[] { "_Cutoff", "_Dither", "_Crossfade", "_Opacity", "_Fade", "_Transparency" };

        public static void ShowPrefabTools()
        {
            GetWindow<OptimizersPrefabUtilities>(false, " Prefab Utilities", true).OnEnable();
        }

        public static void TransferLODGroupToOptimizer(LODGroup g, Optimizer_Base opt)
        {
            opt.AssignComponentsToOptimizeFrom(g, true);
            opt.CheckAndRemoveRenderersAlreadyInOtherLODGroup();
        }

    }
}
