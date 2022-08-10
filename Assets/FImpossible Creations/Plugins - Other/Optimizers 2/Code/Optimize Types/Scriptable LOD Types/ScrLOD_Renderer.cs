using UnityEngine;

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FC: Scriptable container for IFLOD
    /// </summary>
    //[CreateAssetMenu(menuName = "Custom Optimizers/FLOD_Renderer Reference")]
    public sealed class ScrLOD_Renderer : ScrLOD_Base
    {
        [SerializeField]
        private LODI_Renderer settings;
        public override ILODInstance GetLODInstance() { return settings; }
        public ScrLOD_Renderer() { settings = new LODI_Renderer(); }

        public override ScrLOD_Base GetScrLODInstance()
        { return CreateInstance<ScrLOD_Renderer>(); }

        public override ScrLOD_Base CreateNewScrCopy()
        {
            ScrLOD_Renderer newR = CreateInstance<ScrLOD_Renderer>();
            newR.settings = settings.GetCopy() as LODI_Renderer;
            return newR;
        }


        /// <summary>
        /// Assign this LOD type to FOptimizers_Manager
        /// </summary>
        public override ScriptableLODsController GenerateLODController(Component target, ScriptableOptimizer optimizer)
        {
            Renderer rend = target as Renderer;
            if (!rend) rend = target.GetComponent<Renderer>();
            if (rend) if (!optimizer.ContainsComponent(rend))
                {
                    return new ScriptableLODsController(optimizer, rend, -1, "MeshRenderer", this);
                }

            return null;
        }

    }
}
