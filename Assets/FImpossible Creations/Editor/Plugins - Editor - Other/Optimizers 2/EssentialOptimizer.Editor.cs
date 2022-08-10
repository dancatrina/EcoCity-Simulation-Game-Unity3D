using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_BaseEditor : Editor
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(EssentialOptimizer))]
        public class EssentialOptimizerEditor : Optimizer_BaseEditor
        {
            protected override string TargetName() { return " Essential Optimizer"; }
            protected override string TargetTooltip() { return "Optimizer Component which is recommended to use with prefabs especially with nested prefabs.\n\nIt's limited to few optimization components but can be expanded by inheriting but it needs some extra coding and knowledge."; }
            protected override Texture2D GetSmallIcon() { if (__texOptimizersIcon != null) return __texOptimizersIcon; __texOptimizersIcon = Resources.Load<Texture2D>("FIMSpace/Optimizers 2/Optimizers Essential Icon Small"); return __texOptimizersIcon; }

#if UNITY_2019_4_OR_NEWER
            [MenuItem("CONTEXT/EssentialOptimizer/Switch displaying Optimizer 2019.4+ Info")]
            private static void Switch2019OptimizerInfo(MenuCommand menuCommand)
            {
                int current = EditorPrefs.GetInt("FOpt2019", 1);
                if (current == 1) current = 0; else current = 1;
                EditorPrefs.SetInt("FOpt2019", current);
            }
#endif


            const string menuItemunityLOD = "CONTEXT/EssentialOptimizer/Handle Unity LOD Component with Optimizer";

            [MenuItem(menuItemunityLOD, false, 100000)]
            public static void ToggleUnityLODHandling()
            { Optimizer_Base._HandleUnityLODWithReload = !Optimizer_Base._HandleUnityLODWithReload; if (Optimizer_Base._HandleUnityLODWithReload) UnityEngine.Debug.Log("[Optimizers] Now Unity LOD component will be supported"); else UnityEngine.Debug.Log("[Optimizers] Now Unity LOD component will not be detected by Optimizers components"); Menu.SetChecked(menuItemunityLOD, Optimizer_Base._HandleUnityLODWithReload); }

            [MenuItem(menuItemunityLOD, true, 100000)]
            public static bool ToggleUnityLODHandlingValidate()
            { Menu.SetChecked(menuItemunityLOD, Optimizer_Base._HandleUnityLODWithReload); return true; }

#if UNITY_2019_4_OR_NEWER
            [MenuItem("CONTEXT/EssentialOptimizer/Convert to Optimizer 2019.4+", false, 100000)]
            public static void ConvertEssentialToOp2MenuItem(MenuCommand menuCommand)
            {
                EssentialOptimizer eopt = menuCommand.context as EssentialOptimizer;
                if (eopt)
                {
                    ConvertEssentialTo2019(eopt);
                }
            }

            public static Optimizer2 ConvertEssentialTo2019(EssentialOptimizer essOpt)
            {
                Optimizer2 nOpt = essOpt.gameObject.AddComponent<Optimizer2>();

                // Copying all base variables from EssentialOptimizer onto Optimizer 2
                System.Reflection.FieldInfo[] fields = typeof(EssentialOptimizer).GetFields();
                foreach (System.Reflection.FieldInfo field in fields)
                {
                    try { field.SetValue(nOpt, field.GetValue(essOpt)); } catch (System.Exception) { }
                }

                // Mimicing LOD Settings
                nOpt.ToOptimize.Clear();
                nOpt.LODLevels = essOpt.LODLevels;

                for (int i = 0; i < essOpt.ToOptimize.Count; i++)
                {
                    nOpt.AddToOptimizeIfCan(essOpt.ToOptimize[i].Component, null);
                }

                nOpt.RefreshToOptimizeList();
                nOpt.RefreshLODSettings();

                // Copying LOD instances settings from Essential Optimizer to Optimizer 2
                for (int i = 0; i < nOpt.ToOptimize.Count; i++)
                {
                    if (nOpt.ToOptimize[i].Component != essOpt.ToOptimize[i].Component) continue;
                    nOpt.ToOptimize[i].SetFromEssential(essOpt.ToOptimize[i]);
                }

                return nOpt;
            }
#endif

            protected override void OnEnable()
            {
                base.OnEnable();

                if ( DGet.OptimizationTypes == null)
                {
                    DGet.OptimizationTypes = Resources.Load<EssentialOptimizerSelector>("Optimizers/EssentialSelector");
                }
            }

            protected override void BelowDragAndDropComponentGUI()
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                base.BelowDragAndDropComponentGUI();
                if (DGet.OptimizationTypes) if (GUILayout.Button("Choose Auto-Get Components")) { Selection.activeObject = DGet.OptimizationTypes; }
                UnityEditor.EditorGUILayout.EndHorizontal();
            }

            private EssentialOptimizer DGet { get { if (_dGet == null) _dGet = target as EssentialOptimizer; return _dGet; } }
            private EssentialOptimizer _dGet;

            public override void OnInspectorGUI()
            {

#if UNITY_2019_4_OR_NEWER


                if (EditorPrefs.GetInt("FOpt2019", 1) == 1)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Custom components optimization info:", MessageType.None);

                    if (GUILayout.Button(new GUIContent("Info"), GUILayout.Height(20)))
                    {
                        EditorUtility.DisplayDialog("Optimizers 2019.4+", "Other component: Optimizers 2019.4+ (not Essential Optimizer) gives possibility to implement fully custom optimization like scriptable optimizer, but in more stable way for handling nested prefabs.\nIf you don't need this feature, then it's better to use Essential Optimizer! Optimizers 2 is in BETA stage!\n\nHit right mouse button on the Essential Optimizer header and hit 'Convert to Optimizers 2019.4+' to try it.", "Ok");
                    }

                    EditorGUILayout.EndHorizontal();
                }

#endif

                base.OnInspectorGUI();
            }

            protected override bool DrawOptInfoButton { get { return true; } }

            protected override void GUI_ToOptimizeHeader()
            {
                if (drawOptInfo)
                {
                    GUILayout.Space(3);
                    EditorGUILayout.HelpBox("Essential Optimizer supports only optimization for renderers / particle systems / lights / audio sources / nav mesh agents. Use Scriptable Optimizer if you need custom components support.", MessageType.None);
                }
            }


            protected override void FillToOptimizeList()
            {
                for (int i = DGet.ToOptimize.Count - 1; i >= 0; i--)
                {
                    if (DGet.ToOptimize[i].Component == null) DGet.ToOptimize.RemoveAt(i);
                }

                if (DGet.ToOptimize.Count == 1)
                {
                    if (DGet.ToOptimize[0].Component is Renderer)
                    {
                        EditorGUILayout.HelpBox("Using optimizer on just one mesh renderer is not recommended, try using it on more complex objects.", MessageType.Warning);
                        GUI.color = new Color(1f, 0.9f, 0.5f);
                        GUILayout.Space(4);
                    }
                }

                if (DGet.ToOptimize.Count > 0) GUILayout.Space(3f);

                base.FillToOptimizeList();
            }


            protected override void DrawHideProperties()
            {
                for (int i = 0; i < DGet.GetToOptimizeCount(); i++)
                {
                    DGet.ToOptimize[i].GUI_HideProperties(true);
                }
            }


            protected override void OnStartGenerateProperties()
            {
                if (DGet.ToOptimize != null)
                {
                    bool generated = false;
                    for (int i = 0; i < DGet.GetToOptimizeCount(); i++)
                    {
                        if (DGet.ToOptimize[i].GetLODSettingsCount() == 0)
                        {
                            DGet.ToOptimize[i].GenerateLODParameters();
                            generated = true;
                        }
                    }

                    if (generated)
                    {
                        Debug.LogWarning("[OPTIMIZERS EDITOR] LOD Settings generated from scratch for " + DGet.name + ". Did you copy and paste objects through scenes? Unity is not able to remember LOD settings for not prefabed objects and to objects without shared settings between scenes like that :/");
                    }
                }
            }


            protected override void DrawLODOptionsFor(int lodID, int from = 0, int to = 0)
            {
                if (from > DGet.ToOptimize.Count) from = 0;
                if (to > DGet.ToOptimize.Count) to = DGet.ToOptimize.Count;
                if (to <= 0) to = DGet.ToOptimize.Count;

                for (int i = from; i < to; i++)
                {
                    if (DGet.ToOptimize[i] == null)
                    {
                        DGet.ToOptimize.RemoveAt(i);
                        return;
                    }
                    else if (DGet.ToOptimize[i].Component == null)
                    {
                        DGet.ToOptimize.RemoveAt(i);
                        return;
                    }

                    DGet.ToOptimize[i].Editor_DrawValues(lodID, DGet.ToOptimize[i].ToOptimizeIndex);
                }
            }


        }

    }
}

