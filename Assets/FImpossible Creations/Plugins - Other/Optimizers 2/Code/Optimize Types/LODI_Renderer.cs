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
    public sealed class LODI_Renderer : ILODInstance
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
        public string HeaderText { get { return "Renderer LOD Settings"; } }
        public float ToCullDelay { get { return 0f; } }
        public bool SupportVersions { get { return true; } }
        public int DrawingVersion { get { return _version; } set { _version = value; } }
        [HideInInspector] [SerializeField] private int _version = 1;
        public bool LockSettings { get { return _Locked; } set { _Locked = value; } }
        [HideInInspector] [SerializeField] private bool _Locked = false;
        public Texture Icon
        {
            get
            {
                return
#if UNITY_EDITOR
            EditorGUIUtility.IconContent("SkinnedMeshRenderer Icon").image;
#else
        null;
#endif
            }
        }

        public Component TargetComponent { get { return cmp; } }
        [SerializeField] [HideInInspector] private Renderer cmp;

        #endregion

        [Space(4)]
        [Tooltip("If model should cast and receive shadows (receive will be always false if renderer have it marked as false by default)")]
        public bool UseShadows = true;
        internal UnityEngine.Rendering.ShadowCastingMode ShadowsCast = UnityEngine.Rendering.ShadowCastingMode.On;
        internal bool ShadowsReceive;

        public MotionVectorGenerationMode MotionVectors = MotionVectorGenerationMode.Object;

        [Tooltip("If it is skinned mesh renderer we can switch bones weights spread quality")]
        public SkinQuality SkinnedQuality = SkinQuality.Auto;


        public string shaderParam = "_Transparency";
        public bool ColorParameter = false;
        public float targetParamValue = 1f;

        public void SetSameValuesAsComponent(Component component)
        {
            if (component == null) Debug.LogError("[OPTIMIZERS] Given component is null instead of Renderer!");

            Renderer comp = component as Renderer;

            if (comp != null)
            {
                cmp = comp;

                UseShadows = true;
                if (comp.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.Off) UseShadows = false;

                ShadowsCast = comp.shadowCastingMode;
                ShadowsReceive = comp.receiveShadows;
                MotionVectors = comp.motionVectorGenerationMode;

                SkinnedMeshRenderer skin = component as SkinnedMeshRenderer;
                if (skin) SkinnedQuality = skin.quality;
            }
        }


        public void ApplySettingsToTheComponent(Component component, ILODInstance initialSettingsRef)
        {
            LODI_Renderer initialSettings = initialSettingsRef as LODI_Renderer;

            #region Security

            if (component == null) { Debug.Log("[OPTIMIZERS] Target component is null"); return; }
            if (initialSettings == null) { Debug.Log("[OPTIMIZERS] Target LOD is not Renderer LOD or is null"); return; }

            #endregion

            Renderer comp = component as Renderer;

            if (UseShadows)
            {
                comp.shadowCastingMode = initialSettings.ShadowsCast;
                comp.receiveShadows = initialSettings.ShadowsReceive;
            }
            else
            {
                comp.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                comp.receiveShadows = false;
            }

            comp.motionVectorGenerationMode = MotionVectors;

            #region Material fade

            if (DrawingVersion == 2)
            {
                if (comp.materials.Length > 0)
                {
                    foreach (var item in comp.materials)
                    {
                        if (item.HasProperty(shaderParam))
                        {
                            if (ColorParameter)
                            {
                                Color targetColor = item.GetColor(shaderParam);
                                targetColor.a = targetParamValue;
                                item.SetColor(shaderParam, targetColor);
                            }
                            else
                                item.SetFloat(shaderParam, targetParamValue);
                        }
                    }
                }
            }

            #endregion

#if UNITY_2019_1_OR_NEWER
            if (QualitySettings.skinWeights != SkinWeights.OneBone)
                if (comp is SkinnedMeshRenderer)
                {
                    if (QualitySettings.skinWeights == SkinWeights.TwoBones)
                    {
                        if (SkinnedQuality == SkinQuality.Bone4) SkinnedQuality = SkinQuality.Bone2;
                    }

                    SkinnedMeshRenderer skin = comp as SkinnedMeshRenderer;
                    skin.quality = SkinnedQuality;
                }
#else
            if (QualitySettings.blendWeights != BlendWeights.OneBone)
                if (comp is SkinnedMeshRenderer)
                {
                    if (QualitySettings.blendWeights == BlendWeights.TwoBones)
                    {
                        if (SkinnedQuality == SkinQuality.Bone4) SkinnedQuality = SkinQuality.Bone2;
                    }

                    SkinnedMeshRenderer skin = comp as SkinnedMeshRenderer;
                    skin.quality = SkinnedQuality;
                }
#endif


            if (Disable) comp.enabled = false; else comp.enabled = true;
        }


        public void AssignAutoSettingsAsForLODLevel(int lodIndex, int lodCount, Component component)
        {
            Renderer comp = component as Renderer;
            if (comp == null) Debug.LogError("[OPTIMIZERS] Given component for reference values is null or is not Renderer Component!");

            float mul = FLOD.GetValueForLODLevel(1f, 0f, lodIndex, lodCount);
            UseShadows = !(comp.shadowCastingMode == UnityEngine.Rendering.ShadowCastingMode.Off);

            if (lodIndex >= 0)
            {
                if (comp.motionVectorGenerationMode != MotionVectorGenerationMode.ForceNoMotion)
                    MotionVectors = MotionVectorGenerationMode.Camera;
            }

            if (lodCount == 2) if (comp.motionVectorGenerationMode == MotionVectorGenerationMode.Object) MotionVectors = MotionVectorGenerationMode.Camera;

            //if (mul > 0.43f) SkinnedQuality = SkinQuality.Bone2;
            //if (lodCount > 4)
            //    if (mul > 0.6f) SkinnedQuality = SkinQuality.Bone1;

            SkinnedMeshRenderer sk = comp as SkinnedMeshRenderer;
            SkinQuality defQ = SkinQuality.Auto; if (sk) defQ = sk.quality;

            if (mul < 0.6f) SkinnedQuality = defQ == SkinQuality.Bone4 ? SkinQuality.Bone2 : SkinQuality.Auto;
            if (mul < 0.4f) SkinnedQuality = SkinQuality.Bone1;
            if (mul < 0.55f) UseShadows = false;

            if (lodIndex == lodCount - 2)
            {
                UseShadows = false;
                if (lodCount != 2) MotionVectors = MotionVectorGenerationMode.ForceNoMotion;
                SkinnedQuality = SkinQuality.Bone1;
            }

            //name = "LOD" + (lodIndex + 2); // + 2 to view it in more responsive way for user inside inspector window
        }


        public void AssignSettingsAsForCulled(Component component)
        {
            FLOD.AssignDefaultCulledParams(this);
            UseShadows = false;
            MotionVectors = MotionVectorGenerationMode.ForceNoMotion;
            SkinnedQuality = SkinQuality.Bone1;

            targetParamValue = 0f;
        }

        public void AssignSettingsAsForNearest(Component component)
        {
            UnityEngine.Debug.Log("nrst");
            FLOD.AssignDefaultNearestParams(this);
        }

        public void AssignSettingsAsForHidden(Component component)
        {
            FLOD.AssignDefaultHiddenParams(this);
            UseShadows = false;
            MotionVectors = MotionVectorGenerationMode.ForceNoMotion;
            SkinnedQuality = SkinQuality.Bone1;

            targetParamValue = 0f;
        }


        public ILODInstance GetCopy()
        {
            LODI_Renderer r = MemberwiseClone() as LODI_Renderer;
            r.targetParamValue = targetParamValue;
            r.ColorParameter = ColorParameter;
            r.shaderParam = shaderParam;
            r.DrawingVersion = DrawingVersion;
            return r as ILODInstance;
        }

        public void InterpolateBetween(ILODInstance a, ILODInstance b, float transitionToB)
        {
            FLOD.DoBaseInterpolation(this, a, b, transitionToB);

            LODI_Renderer aa = a as LODI_Renderer;
            LODI_Renderer bb = b as LODI_Renderer;

            DrawingVersion = b.DrawingVersion;
            targetParamValue = Mathf.Lerp(aa.targetParamValue, bb.targetParamValue, transitionToB);
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
                GUI.enabled = pre;

                prop.NextVisible(true);

                EditorGUILayout.PropertyField(prop, true); prop.NextVisible(true);
                EditorGUILayout.PropertyField(prop, true); prop.NextVisible(true);
                EditorGUILayout.PropertyField(prop, true); prop.NextVisible(true);

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
                            if (contr != null)
                            {
                                if ( contr.InitialSettings != null) contr.InitialSettings.DrawingVersion = DrawingVersion;
                                for (int i = 1; i < contr.GetLODSettingsCount(); i++)
                                {
                                    LODI_Renderer rr = contr.GetLODSetting(i) as LODI_Renderer;
                                    if (rr != null) rr.DrawingVersion = DrawingVersion;
                                }
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

                        contr.InitialSettings.DrawingVersion = DrawingVersion;
                        for (int i = 1; i < contr.GetLODSettingsCount(); i++)
                        {
                            LODI_Renderer rr = contr.GetLODSetting(i) as LODI_Renderer;
                            if (rr != null)
                            {
                                rr.DrawingVersion = DrawingVersion;
                                rr.shaderParam = shaderParam;
                                rr.ColorParameter = ColorParameter;
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
                        LODI_Renderer baseLodR = baseLod as LODI_Renderer;
                        if (baseLodR != null) if (baseLodR.DrawingVersion == 2)
                            {
                                prop.NextVisible(true);
                                prop.NextVisible(true);

                                if (baseLodR.ColorParameter)
                                    EditorGUILayout.PropertyField(prop, new GUIContent("Shader Color Alpha"), true);
                                else
                                    EditorGUILayout.PropertyField(prop, true);
                            }
                    }
                }

                if (EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(contr.Optimizer);
            }

        }

        public void DrawVersionSwitch(SerializedProperty iflodProp, LODsControllerBase lODsControllerBase)
        {
            EditorGUILayout.EndHorizontal();
        }
#endif

    }
}
