#if UNITY_2019_4_OR_NEWER

using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class Optimizer_BaseEditor : Editor
    {
        [CanEditMultipleObjects]
        [CustomEditor(typeof(Optimizer2))]
        public class Optimizer2020Editor : Optimizer_BaseEditor
        {
            protected override string TargetName() { return " Optimizer 2 (v2019.4+) BETA "; }
            protected override string TargetTooltip() { return "Optimizer Component which is recommended to use with prefabs especially with nested prefabs.\n\nIt's limited to few optimization components but can be expanded by inheriting but it needs some extra coding and knowledge.\n\nUnity Version 2019.4 gives possibility to handle optimization for custom components like with the 'Scriptable Optimizer' but in much more stable way!"; }
            protected override Texture2D GetSmallIcon() { if (__texOptimizersIcon != null) return __texOptimizersIcon; __texOptimizersIcon = Resources.Load<Texture2D>("FIMSpace/Optimizers 2/Optimizers Essential Icon Small"); return __texOptimizersIcon; }

            const string menuItemunityLOD = "CONTEXT/Optimizer2/Handle Unity LOD Component with Optimizer";

            [MenuItem(menuItemunityLOD, false, 100000)]
            public static void ToggleUnityLODHandling()
            { Optimizer_Base._HandleUnityLODWithReload = !Optimizer_Base._HandleUnityLODWithReload; if (Optimizer_Base._HandleUnityLODWithReload) UnityEngine.Debug.Log("[Optimizers] Now Unity LOD component will be supported"); else UnityEngine.Debug.Log("[Optimizers] Now Unity LOD component will not be detected by Optimizers components"); Menu.SetChecked(menuItemunityLOD, Optimizer_Base._HandleUnityLODWithReload); }

            [MenuItem(menuItemunityLOD, true, 100000)]
            public static bool ToggleUnityLODHandlingValidate()
            { Menu.SetChecked(menuItemunityLOD, Optimizer_Base._HandleUnityLODWithReload); return true; }

            [MenuItem("CONTEXT/Optimizer2/Convert to Essential Optimizer", false, 100000)]
            public static void ConvertOptimizer2ToEssential(MenuCommand menuCommand)
            {
                Optimizer2 opt2 = menuCommand.context as Optimizer2;
                if (opt2)
                {
                    Convert2019ToEssential(opt2);
                }
            }

            public static EssentialOptimizer Convert2019ToEssential(Optimizer2 essOpt)
            {
                EssentialOptimizer nOpt = essOpt.gameObject.AddComponent<EssentialOptimizer>();

                // Copying all base variables from EssentialOptimizer onto Optimizer 2
                System.Reflection.FieldInfo[] fields = typeof(Optimizer2).GetFields();
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
                //for (int i = 0; i < nOpt.ToOptimize.Count; i++)
                //{
                //    if (nOpt.ToOptimize[i].Component != essOpt.ToOptimize[i].Component) continue;
                //    nOpt.ToOptimize[i].SetFromEssential(essOpt.ToOptimize[i]);
                //}

                return nOpt;
            }


            protected override void OnEnable()
            {
                base.OnEnable();

                if (DGet.OptimizationTypes == null)
                {
                    DGet.OptimizationTypes = Resources.Load<Optimizer2020Selector>("Optimizers/Optimizer2020Selector");
                }
            }

            protected override void BelowDragAndDropComponentGUI()
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                base.BelowDragAndDropComponentGUI();
                if (DGet.OptimizationTypes) if (GUILayout.Button("Choose Auto-Get Components")) { Selection.activeObject = DGet.OptimizationTypes; }
                UnityEditor.EditorGUILayout.EndHorizontal();
            }

            public override void OnInspectorGUI()
            {
#if UNITY_2019_4_OR_NEWER
                base.OnInspectorGUI();
#else
   GUILayout.Space(6);
                EditorGUILayout.HelpBox("Optimizer 2019.4+ is available only for Unity 2019.4+ versions!", MessageType.Warning);
                GUILayout.Space(6);
                EditorGUILayout.HelpBox("Optimizer 2019.4+ gives possibility to handle custom components to optimize, like with the 'Scriptable Optimizer' but in much more stable way.", MessageType.Info);
   GUILayout.Space(6);
#endif
            }

            private Optimizer2 DGet { get { if (_dGet == null) _dGet = target as Optimizer2; return _dGet; } }
            private Optimizer2 _dGet;


            protected override void GUI_ToOptimizeHeader()
            {
                GUILayout.Space(3);
                //EditorGUILayout.HelpBox("Essential Optimizer 2020+ supports only optimization for renderers / particle systems / lights / audio sources / nav mesh agents. Use Scriptable Optimizer if you need custom components support.", MessageType.None);
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

#endif