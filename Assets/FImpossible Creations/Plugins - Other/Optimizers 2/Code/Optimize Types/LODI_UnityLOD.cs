using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FM: Helper class for single LOD level settings on Renderer
    /// </summary>
    [System.Serializable]
    public sealed class LODI_UnityLOD : ILODInstance
    {
        #region Main Settings : Interface Properties

        public int Index { get { return index; } set { index = value; } }
        internal int index = -1;
        public string Name { get { return LODName; } set { LODName = value; } }
        internal string LODName = "";
        public bool CustomEditor { get { return true; } }
        public bool Disable { get { return SetDisabled; } set { SetDisabled = value; } }
        [HideInInspector] public bool SetDisabled = false;
        public bool DrawDisableOption { get { return true; } }

        public bool SupportingTransitions { get { return DrawingVersion == 2; } }
        public bool DrawLowererSlider { get { return false; } }
        public float QualityLowerer { get { return 1f; } set { new System.NotImplementedException(); } }
        public string HeaderText { get { return "Unity LODGroup Settings"; } }
        public float ToCullDelay { get { return 0f; } }
        public bool SupportVersions { get { return true; } }
        public int DrawingVersion { get { return _version; } set { _version = value; } }
        [HideInInspector] [SerializeField] private int _version = 1;
        public Texture Icon
        {
            get
            {
                return
#if UNITY_EDITOR
            EditorGUIUtility.IconContent("LODGroup Icon").image;
#else
        null;
#endif
            }
        }

        public Component TargetComponent { get { return cmp; } }

        public bool LockSettings { get { return _Locked; } set { _Locked = value; } }
        [HideInInspector] [SerializeField] private bool _Locked = false;

        [SerializeField] [HideInInspector] private LODGroup cmp;

        #endregion

        [Tooltip("Which LOD level from LOD Group should be applied on this Optimizer LOD Level")]
        public int UnityLODLevel = 0;
        public string shaderParam = "_Transparency";
        public bool ColorParameter = false;
        public float shaderVisibleValue = 1f;
        public float shaderInvisibleValue = 0f;
        public float crossfadeHelper = 0f;

        public void SetSameValuesAsComponent(Component component)
        {
            if (component == null) Debug.LogError("[OPTIMIZERS] Given component is null instead of Unity LODGroup!");
            else
                cmp = component as LODGroup;

            UnityLODLevel = 0;
        }

        public void ApplySettingsToTheComponent(Component component, ILODInstance initialSettingsRef)
        {
            LODI_UnityLOD initialSettings = initialSettingsRef as LODI_UnityLOD;

            #region Security

            if (component == null) { Debug.Log("[OPTIMIZERS] Target component is null"); return; }
            if (initialSettings == null) { Debug.Log("[OPTIMIZERS] Target LOD is not UnityLODGroup or is null"); return; }

            #endregion

            LODGroup comp = component as LODGroup;
            LOD[] lods = comp.GetLODs();

            comp.enabled = false;

            if (Disable)
            {
                for (int i = 0; i < lods.Length; i++) for (int j = 0; j < lods[i].renderers.Length; j++) if (lods[i].renderers[j] != null) lods[i].renderers[j].enabled = false;
            }
            else
            {
                if (crossfadeHelper <= 0f || crossfadeHelper >= 1f)
                {
                    if (crossfadeHelper <= 0f)
                        for (int i = 0; i < lods.Length; i++)
                        {
                            if (i == UnityLODLevel) continue;

                            for (int j = 0; j < lods[i].renderers.Length; j++)
                                if (lods[i].renderers[j] != null) lods[i].renderers[j].enabled = false;
                        }

                    if (UnityLODLevel < lods.Length)
                    {
                        for (int j = 0; j < lods[UnityLODLevel].renderers.Length; j++)
                            if (lods[UnityLODLevel].renderers[j] != null) lods[UnityLODLevel].renderers[j].enabled = true;
                    }
                }
            }

        }


        public void AssignAutoSettingsAsForLODLevel(int lodIndex, int lodCount, Component component)
        {
            LODGroup comp = component as LODGroup;
            if (comp == null) Debug.LogError("[OPTIMIZERS] Given component for reference values is null or is not LODGroup Component!");

            UnityLODLevel = lodIndex + 1;
            if (UnityLODLevel > comp.lodCount - 1) UnityLODLevel = comp.lodCount - 1;
            cmp = comp;
        }

        public void AssignSettingsAsForCulled(Component component)
        {
            FLOD.AssignDefaultCulledParams(this);

            LODGroup comp = component as LODGroup;
            if (comp != null)
            {
                cmp = comp;
                UnityLODLevel = comp.lodCount;
            }
        }

        public void AssignSettingsAsForNearest(Component component)
        {
            FLOD.AssignDefaultNearestParams(this);
            UnityLODLevel = 0;
            if (component != null) cmp = component as LODGroup;
        }

        public void AssignSettingsAsForHidden(Component component)
        {
            FLOD.AssignDefaultHiddenParams(this);
            LODGroup comp = component as LODGroup;
            if (comp != null)
            {
                UnityLODLevel = comp.lodCount;
                cmp = comp;
            }
        }


        public ILODInstance GetCopy()
        {
            LODI_UnityLOD r = MemberwiseClone() as LODI_UnityLOD;
            r.cmp = cmp;
            r.UnityLODLevel = UnityLODLevel;
            r.shaderVisibleValue = shaderVisibleValue;
            r.shaderInvisibleValue = shaderInvisibleValue;
            r.ColorParameter = ColorParameter;
            r.shaderParam = shaderParam;
            r.DrawingVersion = DrawingVersion;
            return r as ILODInstance;
        }

        public void InterpolateBetween(ILODInstance a, ILODInstance b, float transitionToB)
        {
            FLOD.DoBaseInterpolation(this, a, b, transitionToB);

            LODI_UnityLOD aa = a as LODI_UnityLOD;
            LODI_UnityLOD bb = b as LODI_UnityLOD;

            DrawingVersion = b.DrawingVersion;

            crossfadeHelper = transitionToB;
            aa.crossfadeHelper = 1f - transitionToB;
            bb.crossfadeHelper = 1f - transitionToB;

            ApplyCrossfade(aa, bb, transitionToB);
        }

        private static void ApplyCrossfade(LODI_UnityLOD pre, LODI_UnityLOD next, float toNew)
        {
            if (pre.UnityLODLevel != next.UnityLODLevel)
                if (pre.cmp != null)
                    if (pre.UnityLODLevel < pre.cmp.lodCount)
                    {
                        Renderer[] rends = pre.cmp.GetLODs()[pre.UnityLODLevel].renderers;
                        for (int i = 0; i < rends.Length; i++)
                        {
                            Renderer r = rends[i];
                            if (toNew < 1f) r.enabled = true;

                            if (r.materials.Length > 0)
                                foreach (var item in r.materials)
                                    if (item.HasProperty(pre.shaderParam))
                                    {
                                        if (pre.ColorParameter)
                                        {
                                            Color targetColor = item.GetColor(pre.shaderParam);
                                            targetColor.a = pre.GetFadeMaterialValueToVisible(1f - toNew);
                                            item.SetColor(pre.shaderParam, targetColor);
                                        }
                                        else
                                        {
                                            item.SetFloat(pre.shaderParam, pre.GetFadeMaterialValueToVisible(1f - toNew));
                                        }
                                    }
                        }
                    }

            if (next.cmp != null)
                if (next.UnityLODLevel < next.cmp.lodCount)
                {
                    Renderer[] rends = next.cmp.GetLODs()[next.UnityLODLevel].renderers;

                    for (int i = 0; i < rends.Length; i++)
                    {
                        Renderer r = rends[i];
                        r.enabled = true;
                        if (r.materials.Length > 0)
                        {
                            foreach (var item in r.materials)
                            {
                                if (item.HasProperty(next.shaderParam))
                                {
                                    if (next.ColorParameter)
                                    {
                                        Color targetColor = item.GetColor(next.shaderParam);
                                        targetColor.a = next.GetFadeMaterialValueToVisible(toNew);
                                        item.SetColor(next.shaderParam, targetColor);
                                    }
                                    else
                                    {
                                        item.SetFloat(next.shaderParam, next.GetFadeMaterialValueToVisible(toNew));
                                    }
                                }
                            }
                        }
                    }
                }
        }

        private float GetFadeMaterialValueToVisible(float progress)
        {
            return Mathf.LerpUnclamped(shaderInvisibleValue, shaderVisibleValue, progress);
        }


#if UNITY_EDITOR
        public void AssignToggler(ILODInstance reference)
        { }

        public void DrawTogglers(SerializedProperty iflodProp)
        { }

        public void CustomEditorWindow(SerializedProperty prop, LODsControllerBase contr)
        {
            bool pre = GUI.enabled;

            EditorGUI.BeginChangeCheck();

            if (prop != null)
            {
                EditorGUILayout.HelpBox("Using Optimizer instead of LODGroup (LODGroup will be disabled in playmode)", MessageType.None);

                prop.NextVisible(true);
                GUI.enabled = pre;
                LODGroup cmp = contr.Component as LODGroup;

                if (cmp == null)
                {
                    EditorGUILayout.HelpBox("No LODGroup Component found!", MessageType.Error);
                    return;
                }

                EditorGUILayout.IntSlider(prop, 0, cmp.lodCount);
                prop.NextVisible(true);

                if (contr.GetLODSetting(0) == this) // Selected LOD 0
                {
                    bool pree = GUI.enabled;
                    GUI.enabled = true;

                    if (DrawingVersion != 2)
                    {
                        if (GUILayout.Button("Add Shader Settings for Crossfade"))
                        {
                            DrawingVersion = 2;
                        }
                        else
                        {
                            contr.InitialSettings.DrawingVersion = DrawingVersion;
                            for (int i = 1; i < contr.GetLODSettingsCount(); i++)
                            {
                                LODI_UnityLOD rr = contr.GetLODSetting(i) as LODI_UnityLOD;
                                if (rr != null) rr.DrawingVersion = DrawingVersion;
                            }
                        }

                    }
                    else
                    {
                        if (GUILayout.Button("Remove Shader Settings")) { DrawingVersion = 1; prop.serializedObject.ApplyModifiedProperties(); prop.serializedObject.Update(); }
                        EditorGUILayout.HelpBox("Use transitioning for crossfade mesh transparency", MessageType.None);

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(prop, true); prop.NextVisible(true);
                        EditorGUIUtility.labelWidth = 50;
                        EditorGUILayout.PropertyField(prop, new GUIContent("Color"), true, GUILayout.Width(70)); prop.NextVisible(true); EditorGUIUtility.labelWidth = 0;
                        EditorGUILayout.EndHorizontal();

                        if (ColorParameter) EditorGUILayout.PropertyField(prop, new GUIContent("Shader Color Alpha"), true);
                        else EditorGUILayout.PropertyField(prop, true);

                        prop.NextVisible(true); EditorGUILayout.PropertyField(prop, true);

                        contr.InitialSettings.DrawingVersion = DrawingVersion;
                        for (int i = 1; i < contr.GetLODSettingsCount(); i++)
                        {
                            LODI_UnityLOD rr = contr.GetLODSetting(i) as LODI_UnityLOD;
                            if (rr != null)
                            {
                                rr.DrawingVersion = DrawingVersion;
                                rr.shaderParam = shaderParam;
                                rr.ColorParameter = ColorParameter;
                                rr.shaderVisibleValue = shaderVisibleValue;
                                rr.shaderInvisibleValue = shaderInvisibleValue;
                                rr.cmp = cmp;
                            }
                        }

                    }

                    GUI.enabled = pree;
                }
                else // Other LODS
                {
                    ILODInstance baseLod = contr.GetLODSetting(0);
                    if (baseLod != null)
                    {
                        LODI_UnityLOD baseLodR = baseLod as LODI_UnityLOD;
                        if (baseLodR != null) if (baseLodR.DrawingVersion == 2)
                            {
                                prop.NextVisible(true);
                                prop.NextVisible(true);

                                if (baseLodR.ColorParameter)
                                    EditorGUILayout.PropertyField(prop, new GUIContent("Shader Color Alpha"), true);
                                else
                                    EditorGUILayout.PropertyField(prop, new GUIContent("Target Shader Value"), true);

                                prop.NextVisible(true);
                                EditorGUILayout.PropertyField(prop, true);
                            }
                    }
                }

                if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(contr.Optimizer);

                prop.serializedObject.ApplyModifiedProperties();
                prop.serializedObject.Update();

                if (contr.Optimizer.LODLevels < cmp.lodCount)
                {
                    EditorGUILayout.HelpBox("Optimizer LOD Levels count is lower than unity LOD Group count (" + cmp.lodCount + ") You probably should adjust Optimizers LOD Level Count!", MessageType.Warning);
                }
            }

        }

        public void DrawVersionSwitch(SerializedProperty iflodProp, LODsControllerBase lODsControllerBase)
        {
            EditorGUILayout.EndHorizontal();
        }
#endif

    }
}
