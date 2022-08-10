#if UNITY_2019_4_OR_NEWER

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FM: Class which is helping holding settings and references for one optimized component.
    /// > Containing reference to target optimized component from scene/prefab
    /// > Handling applying changes to target optimized component in playmode
    /// > Handling drawing editor windows elements for optimization settings etc. 
    /// </summary>
    [System.Serializable]
    public partial class Optimizer2020LODsController : LODsControllerBase
    {
        [SerializeReference] public List<Component> ToOptimizes = new List<Component>();
        [SerializeReference] public Component ToOptimize;
        [SerializeReference] public ILODInstance LODInitial;
        [SerializeReference] public List<ILODInstance> LODInstances = new List<ILODInstance>();
        protected override List<ILODInstance> GetIFLODList() { return LODInstances; }
        [UnityEngine.SerializeField] private Optimizer2 eOptimizer;

        public Optimizer2020LODsController(Optimizer_Base sourceOptimizer, Component toOptimize, int index, string header = "") : base(sourceOptimizer, toOptimize, index, header)
        {
            eOptimizer = sourceOptimizer as Optimizer2;
            ToOptimize = toOptimize;
        }

        public override void OnStart()
        {
            if (InitialSettings == null) GenerateInitialSettings();
            InitialSettings.SetSameValuesAsComponent(Component);
        }

        protected override void RefreshToOptimizeIndex()
        {
            for (int i = 0; i < eOptimizer.ToOptimize.Count; i++)
            {
                if (eOptimizer.ToOptimize[i] == this)
                {
                    ToOptimizeIndex = i;
                    return;
                }
            }
        }


#if UNITY_EDITOR
        protected override SerializedObject GetSerializedObject()
        {
            return new SerializedObject(eOptimizer);
        }

        public override SerializedProperty GetSerializedLODPropertyFor(int lod)
        {
            if (lod < 0) return null;
            if (lod >= GetLODSettingsCount()) return null;

            string serializedPropList = "LODInstances";

            RefreshToOptimizeIndex();

            SerializedObject s = new SerializedObject(eOptimizer);
            SerializedProperty lodContr = s.FindProperty("ToOptimize").GetArrayElementAtIndex(ToOptimizeIndex);

            return lodContr.FindPropertyRelative(serializedPropList).GetArrayElementAtIndex(lod);
        }
#endif

        internal override ILODInstance GetCurrentLOD()
        {
            return GetIFLODList()[CurrentLODLevel];
        }

        internal override ILODInstance GetCullingLOD()
        {
            return GetIFLODList()[GetIFLODList().Count - 2];
        }

        internal override ILODInstance GetHiddenLOD()
        {
            return GetIFLODList()[GetIFLODList().Count - 1];
        }
    }
}

#endif