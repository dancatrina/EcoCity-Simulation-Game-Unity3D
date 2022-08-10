using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{

    [CustomEditor(typeof(TerrainOptimizer))]
    [CanEditMultipleObjects]
    public class OptimizerEditorTerrain : ScriptableOptimizerEditor
    {
        private SerializedProperty sp_Terrain;
        private SerializedProperty sp_TerrainC;
        private SerializedProperty sp_SafeBorders;
        private SerializedProperty sp_ToOpt;

        protected override string TargetName() { return " Multi-Terrains Optimizer"; }
        protected override string TargetTooltip() { return "Optimizer Component to use with maps which are using multiple terrain components. It is deriving from ScriptableOptimizer."; }
        protected override Texture2D GetSmallIcon() { if (__texOptimizersIcon != null) return __texOptimizersIcon; __texOptimizersIcon = Resources.Load<Texture2D>("FIMSpace/Optimizers 2/OptTerrIconSmall"); return __texOptimizersIcon; }

        private TerrainOptimizer TGet { get { if (_tGet == null) _tGet = target as TerrainOptimizer; return _tGet; } }
        private TerrainOptimizer _tGet;

        protected override void OnEnable()
        {
            base.OnEnable();
            drawHiddenRange = false;
            drawDetectionSphereHandle = false;
            drawNothingToOptimizeWarning = false;
            sp_Terrain = serializedObject.FindProperty("Terrain");
            sp_TerrainC = serializedObject.FindProperty("TerrainCollider");
            sp_SafeBorders = serializedObject.FindProperty("SafeBorders");
            if (TGet.ToOptimize == null) TGet.ToOptimize = new System.Collections.Generic.List<ScriptableLODsController>();
            sp_ToOpt = serializedObject.FindProperty("ToOptimize");

            TGet.UseMultiShape = false;
            TGet.UseObstacleDetection = false;

            visibleOptMethod = false;
            visibleCamRelation = false;
            visibleToOptList = false;
            visibleAddFeatures = false;
        }

        protected override void El_ContainerSwitch()
        {
            EditorGUILayout.PropertyField(sp_SafeBorders);
        }

        protected override void GUI_PreLODLevelsSetup()
        {
            GUI.color = new Color(1f, 1f, 0.3f, .75f);
            GUILayout.BeginVertical(FGUI_Resources.BGInBoxLightStyle);
            GUI.color = c;
            EditorGUILayout.PropertyField(sp_Terrain, true);
            EditorGUILayout.PropertyField(sp_TerrainC, true);

            FGUI_Inspector.DrawUILine(0.4f, 0.15f, 2, 7);

            EditorGUILayout.PropertyField(sp_DetRad);
            EditorGUILayout.PropertyField(sp_SafeBorders);
            EditorGUILayout.PropertyField(sp_GizmosAlpha);

            GUILayout.Space(3);

            EditorGUILayout.EndVertical();
        }

        protected override void OnSceneGUI() { }


        protected override void GUI_CategorySelectorAndDraw(RectOffset zeroOff, float bgAlpha, int headerHeight)
        {
            GUILayout.Space(4f);
            GUI_PreLODLevelsSetup();

            GUILayout.Space(4f);
            string title = Lang("LOD Levels Setup");
            GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.6f, .95f, .55f, bgAlpha * 0.375f), Vector4.one * 3, 3));
            Tab_DrawLODSetup();
            GUILayout.EndVertical();
        }

        protected override void DefaultInspectorStack(Optimizer_Base targetScript, bool endVert = true)
        {
            EditorGUI.BeginChangeCheck();

            TerrainOptimizer targetTerr = targetScript as TerrainOptimizer;

            if (Application.isPlaying) GUI.enabled = false;
            EditorGUILayout.BeginVertical(FEditor.FGUI_Inspector.Style(new Color(0.975f, 0.975f, 0.975f, .325f)));

            EditorGUI.indentLevel++;
            DrawSetup = EditorGUILayout.Foldout(DrawSetup, Lang("Optimizer Setup"), true, new GUIStyle(EditorStyles.foldout) { fontStyle = FontStyle.Bold });
            EditorGUI.indentLevel--;

            if (DrawSetup)
            {
                FGUI_Inspector.DrawUILine(new Color(0.5f, 0.5f, 0.5f, 0.5f), 2, 4);
                GUILayout.Space(1f);

                EditorGUILayout.PropertyField(sp_MaxDist);
                targetScript.DetectionRadius = EditorGUILayout.FloatField(new GUIContent("Detection Radius", "Radius for controll spheres placed on terrain, they will define visibility triggering when camera lookin on or away"), targetScript.DetectionRadius);
                targetScript.DetectionRadius = targetTerr.LimitRadius(targetScript.DetectionRadius);
                EditorGUILayout.PropertyField(sp_SafeBorders);
                EditorGUILayout.PropertyField(sp_GizmosAlpha);

                //EditorGUILayout.PropertyField(serializedObject.FindProperty("ToOptimize"), true);

                GUILayout.Space(3f);
            }

            EditorGUILayout.EndVertical();
            if (Application.isPlaying) GUI.enabled = true;
            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(target);
        }

        protected override void PreLODGUI()
        {
            base.PreLODGUI();

            if (!TGet.SaveSetFilesInPrefab)
                if (TGet.ToOptimize.Count > 0)
                    if (TGet.ToOptimize[0] != null)
                    {
                        SerializedProperty sp_sett = null;
                        sp_sett = sp_ToOpt.GetArrayElementAtIndex(0).FindPropertyRelative("sharedLODSet");
                        if (sp_sett != null)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.PropertyField(sp_sett);

                            if (GUILayout.Button(new GUIContent("New", "Generate new LOD set file basing on current settings in optimizer component."), new GUILayoutOption[2] { GUILayout.Width(40), GUILayout.Height(15) }))
                            {
                                TGet.ToOptimize[0].SaveLODSet();
                                AssetDatabase.SaveAssets();
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }

            GUILayout.Space(3f);
        }

        protected override void DrawFadeDurationSlider(Optimizer_Base targetScript) { }

    }

}
