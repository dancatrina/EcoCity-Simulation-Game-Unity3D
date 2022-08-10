using FIMSpace.FEditor;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    [CustomEditor(typeof(Optimizer_Base))]
    [CanEditMultipleObjects]
    public partial class Optimizer_BaseEditor : Editor
    {

        #region GUI Helpers

        private Optimizer_Base Get { get { if (_get == null) _get = target as Optimizer_Base; return _get; } }
        private Optimizer_Base _get;
        protected Color c;
        protected Color bc;
        protected bool drawSetup = true;
        protected bool hardNotShowHidden = false;
        protected bool drawOptInfo = false;
        protected virtual bool DrawOptInfoButton { get { return false; } }

        protected virtual string TargetName() { return "Optimizer"; }
        protected virtual string TargetTooltip() { return ""; }

        #endregion

        [MenuItem("CONTEXT/Optimizer_Base/Switch displaying header bar")]
        private static void HideFImpossibleHeader(MenuCommand menuCommand)
        {
            int current = EditorPrefs.GetInt("FOptHeader", 1);
            if (current == 1) current = 0; else current = 1;
            EditorPrefs.SetInt("FOptHeader", current);
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

        public override void OnInspectorGUI()
        {
            //EditorGUILayout.PropertyField(sp_MaxDist);
            //serializedObject.ApplyModifiedProperties();
            //return;

            Undo.RecordObject(target, "Optimizers Inspector");

            if (OptimizersManager.Instance.UpdateOptimizersSystem == false)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Optimizers System is disabled under 'Scene Tools' of Optimizers Manager!", MessageType.Warning);
                if (GUILayout.Button("Select Manager", GUILayout.ExpandHeight(true))) Selection.activeGameObject = OptimizersManager.Instance.gameObject;
                EditorGUILayout.EndHorizontal();
            }

            if (Get.UseObstacleDetection)
                drawCullIfNotSee = false;
            else
                drawCullIfNotSee = true;

            if (serializedObject != null) serializedObject.Update();

            #region Unity Version Specifics

#if UNITY_2020_1_OR_NEWER
            if (target == null) return;
            if (Get == null) return;
#endif

            #endregion

            #region Corrections Checking

            if (Get.OptimizingMethod == EOptimizingMethod.TriggerBased) DrawAddRigidbodyToCamera();

            #endregion


            LODFrame(Get); // Drawing coloured LOD frame to identify LOD state in playmode


            if (EditorPrefs.GetInt("FOptHeader", 1) == 1)
            {
                HeaderBoxMain(new GUIContent(TargetName(), TargetTooltip()), ref Get.DrawGizmos, ref drawDefaultInspector, GetSmallIcon(), Get, 27);
            }
            else
            {
                GUILayout.Space(2f);
            }


            if (drawDefaultInspector)
                DrawDefaultInspector();
            else
            {
                DrawNewGUI();

                //if (target != null) // Unity 2020 have problems with 'target'
                //    if (Application.isPlaying) EditorUtility.SetDirty(target);

                if (serializedObject != null) // Unity 2020 prevention
                {
                    serializedObject.ApplyModifiedProperties();
                }
            }

            if (Application.isPlaying) EditorGUILayout.EndVertical(); // LODFrame End
        }


        void DrawCategoryButton(Optimizer_Base.EFOptEditorCategory target, Texture icon, string lang)
        {
            if (Get._editor_category == target) GUI.backgroundColor = new Color(0.1f, 1f, 0.2f, 1f);

            int height = 28;
            int lim = 330;
            if (choosedLang == ELangs.русский) lim = 360;

            if (EditorGUIUtility.currentViewWidth > lim)
            {
                if (GUILayout.Button(new GUIContent("  " + Lang(lang), icon), FGUI_Resources.ButtonStyle, GUILayout.Height(height))) Get._editor_category = target;
            }
            else
                if (GUILayout.Button(new GUIContent(icon, Lang(lang)), FGUI_Resources.ButtonStyle, GUILayout.Height(height))) Get._editor_category = target;

            GUI.backgroundColor = bc;
        }


        void DrawNewGUI()
        {

            #region Preparations for unity versions and skin

            c = GUI.color;
            bc = GUI.color;

            RectOffset zeroOff = new RectOffset(0, 0, 0, 0);

#if UNITY_2019_3_OR_NEWER
            float bgAlpha = 0.05f; if (EditorGUIUtility.isProSkin) bgAlpha = 0.1f;
            int headerHeight = 22;
#else
            float bgAlpha = 0.05f; if (EditorGUIUtility.isProSkin) bgAlpha = 0.2f;
            int headerHeight = 22;
#endif


            #endregion


            GUILayout.BeginVertical(FGUI_Resources.BGBoxStyle); GUILayout.Space(1f);


            if (Get._editor_horizontal)
            {

                if (!Application.isPlaying)
                {
                    GUI_CategorySelectorAndDraw(zeroOff, bgAlpha, headerHeight);
                }
                else
                {
                    //GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.7f, .7f, 0.7f, bgAlpha), Vector4.one * 3, 3));
                    //FGUI_Inspector.HeaderBox(ref Get._editor_DrawSetup, Lang("Optimizer Setup"), true, FGUI_Resources.Tex_GearSetup, headerHeight, headerHeight - 1, LangBig());
                    //if (Get._editor_DrawSetup) Tab_DrawOptimSetup();
                    //GUILayout.EndVertical();


                    string title = "";
                    if (Application.isPlaying) title = Lang("Realtime Preview"); else title = Lang("LOD Levels Setup");

                    GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.6f, .95f, .55f, bgAlpha * 0.375f), Vector4.one * 3, 3));
                    FGUI_Inspector.HeaderBox(ref Get._editor_DrawLODLevelsSetup, title, true, _TexLODIcon, headerHeight, headerHeight - 1, LangBig());
                    if (Get._editor_DrawLODLevelsSetup) Tab_DrawLODSetup();
                    GUILayout.EndVertical();
                }

                GUILayout.Space(2f);
                GUILayout.EndVertical();

                if (drawNothingToOptimizeWarning)
                    if (Get.GetToOptimizeCount() == 0)
                    {
                        string childrenInfo = "";
                        if (Get.gameObject.transform.childCount > 0) childrenInfo = " Maybe there are components to optimize in child game objects? Please check buttons inside 'To Optimize' tab.";
                        EditorGUILayout.HelpBox("Nothing to optimize! You can only cull game object with the component." + childrenInfo, MessageType.Info);
                    }
            }
            else
            {
                if (drawSetup)
                {
                    GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.7f, .7f, 0.7f, bgAlpha), Vector4.one * 3, 3));

                    FGUI_Inspector.HeaderBox(ref Get._editor_DrawSetup, Lang("Optimizer Setup"), true, FGUI_Resources.Tex_GearSetup, headerHeight, headerHeight - 1, LangBig());
                    if (Get._editor_DrawSetup) Tab_DrawOptimSetup();

                    GUILayout.EndVertical();
                }

                if (!Application.isPlaying)
                {
                    // ------------------------------------------------------------------------

                    if (visibleToOptList)
                    {
                        GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(1f, 1f, .4f, bgAlpha * 0.8f), Vector4.one * 3, 3));
                        FGUI_Inspector.HeaderBox(ref Get._editor_DrawOptimizeList, Lang("To Optimize List") + " (" + Get.GetToOptimizeCount() + ")", true, FGUI_Resources.Tex_Sliders, headerHeight, headerHeight - 1, LangBig());
                        if (Get._editor_DrawOptimizeList) Tab_DrawToOptList();

                        GUILayout.EndVertical();
                    }


                    // ------------------------------------------------------------------------

                    if (visibleAddFeatures)
                    {
                        GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.5f, 1f, .83f, bgAlpha * 0.6f), Vector4.one * 3, 3));
                        FGUI_Inspector.HeaderBox(ref Get._editor_DrawAddFeatures, Lang("Additional Features"), true, FGUI_Resources.Tex_Module, headerHeight, headerHeight - 1, LangBig());

                        if (Get._editor_DrawAddFeatures) Tab_DrawAdditionalModules();

                        GUILayout.EndVertical();
                    }

                    GUI_PreLODLevelsSetup();
                }

                // ------------------------------------------------------------------------


                //if ( serializedObject != null ) serializedObject.ApplyModifiedProperties();
                //Get.EditorUpdate();

                string title = "";
                if (Application.isPlaying) title = Lang("Realtime Preview"); else title = Lang("LOD Levels Setup");

                GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.6f, .95f, .55f, bgAlpha * 0.375f), Vector4.one * 3, 3));
                FGUI_Inspector.HeaderBox(ref Get._editor_DrawLODLevelsSetup, title, true, _TexLODIcon, headerHeight, headerHeight - 1, LangBig());

                if (Get._editor_DrawLODLevelsSetup) Tab_DrawLODSetup();

                GUILayout.EndVertical();

                GUILayout.Space(2f);
                GUILayout.EndVertical();

                // ------------------------------------------------------------------------

                if (drawNothingToOptimizeWarning)
                    if (Get.GetToOptimizeCount() == 0)
                    {
                        string childrenInfo = "";
                        if (Get.gameObject.transform.childCount > 0) childrenInfo = " Maybe there are components to optimize in child game objects? Please check buttons inside 'To Optimize' tab.";
                        EditorGUILayout.HelpBox("Nothing to optimize! You can only cull game object with the component." + childrenInfo, MessageType.Info);
                    }
            }
        }

        protected virtual void GUI_CategorySelectorAndDraw(RectOffset zeroOff, float bgAlpha, int headerHeight)
        {
            GUILayout.Space(2);
            EditorGUILayout.BeginHorizontal();
            DrawCategoryButton(Optimizer_Base.EFOptEditorCategory.Setup, FGUI_Resources.Tex_GearSetup, "Setup");
            //if (visibleToOptList) DrawCategoryButton(Optimizer_Base.EFOptEditorCategory.List, FGUI_Resources.Tex_Sliders, "Choose");
            DrawCategoryButton(Optimizer_Base.EFOptEditorCategory.Features, FGUI_Resources.Tex_Module, "Features");
            DrawCategoryButton(Optimizer_Base.EFOptEditorCategory.LODs, _TexLODIcon, "LODs");
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(4);


            switch (Get._editor_category)
            {
                case Optimizer_Base.EFOptEditorCategory.Setup:
                case Optimizer_Base.EFOptEditorCategory.List:

                    GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.7f, .7f, 0.7f, bgAlpha), Vector4.one * 3, 3));
                    FGUI_Inspector.HeaderBox(ref Get._editor_DrawSetup, Lang("Optimizer Setup"), true, FGUI_Resources.Tex_GearSetup, headerHeight, headerHeight - 1, LangBig());
                    if (Get._editor_DrawSetup) Tab_DrawOptimSetup();
                    GUILayout.EndVertical();

                    if (visibleToOptList)
                    {
                        GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(1f, 1f, .4f, bgAlpha * 0.8f), Vector4.one * 3, 3));
                        FGUI_Inspector.HeaderBox(ref Get._editor_DrawOptimizeList, Lang("To Optimize List") + " (" + Get.GetToOptimizeCount() + ")", true, FGUI_Resources.Tex_Sliders, headerHeight, headerHeight - 1, LangBig());
                        if (Get._editor_DrawOptimizeList) Tab_DrawToOptList();
                        GUILayout.EndVertical();
                    }

                    break;

                case Optimizer_Base.EFOptEditorCategory.Features:
                    Get._editor_DrawAddFeatures = true;
                    GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.5f, 1f, .83f, bgAlpha * 0.6f), Vector4.one * 3, 3));
                    FGUI_Inspector.HeaderBox(Lang("Additional Features"), true, FGUI_Resources.Tex_Module, headerHeight, headerHeight - 1, LangBig());
                    Tab_DrawAdditionalModules();
                    GUILayout.EndVertical();
                    break;

                case Optimizer_Base.EFOptEditorCategory.LODs:
                    Get._editor_DrawLODLevelsSetup = true;
                    GUI_PreLODLevelsSetup();
                    string title = Lang("LOD Levels Setup");
                    GUILayout.BeginVertical(FGUI_Inspector.Style(zeroOff, zeroOff, new Color(.6f, .95f, .55f, bgAlpha * 0.375f), Vector4.one * 3, 3));
                    FGUI_Inspector.HeaderBox(title, true, _TexLODIcon, headerHeight, headerHeight - 1, LangBig());
                    if (Get._editor_DrawLODLevelsSetup) Tab_DrawLODSetup();
                    GUILayout.EndVertical();
                    break;
            }
        }

        protected virtual void GUI_PreLODLevelsSetup()
        {

        }
    }



}

