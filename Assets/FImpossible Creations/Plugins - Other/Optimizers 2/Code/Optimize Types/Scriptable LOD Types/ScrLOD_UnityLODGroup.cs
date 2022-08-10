using UnityEngine;

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FC: Scriptable container for IFLOD
    /// </summary>
    //[CreateAssetMenu(menuName = "Custom Optimizers/FLOD_UnityLODGroup Reference")]
    public sealed class ScrLOD_UnityLODGroup : ScrLOD_Base
    {
        [SerializeField]
        private LODI_UnityLOD settings;
        public override ILODInstance GetLODInstance() { return settings; }
        public ScrLOD_UnityLODGroup() { settings = new LODI_UnityLOD(); }

        public override ScrLOD_Base GetScrLODInstance()
        { return CreateInstance<ScrLOD_UnityLODGroup>(); }

        public override ScrLOD_Base CreateNewScrCopy()
        {
            ScrLOD_UnityLODGroup newR = CreateInstance<ScrLOD_UnityLODGroup>();
            newR.settings = settings.GetCopy() as LODI_UnityLOD;
            return newR;
        }


        /// <summary>
        /// Assign this LOD type to FOptimizers_Manager
        /// </summary>
        public override ScriptableLODsController GenerateLODController(Component target, ScriptableOptimizer optimizer)
        {
            LODGroup uLOD = target as LODGroup;
            if (!uLOD) uLOD = target.GetComponent<LODGroup>();
            if (uLOD) if (!optimizer.ContainsComponent(uLOD))
                {
                    return new ScriptableLODsController(optimizer, uLOD, -1, "UnityLODGroup", this);
                }

            return null;
        }

    }
}

