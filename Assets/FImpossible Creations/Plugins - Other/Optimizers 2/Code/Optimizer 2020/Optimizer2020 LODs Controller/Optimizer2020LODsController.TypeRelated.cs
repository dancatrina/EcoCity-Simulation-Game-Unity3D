#if UNITY_2019_4_OR_NEWER

namespace FIMSpace.FOptimizing
{

    public partial class Optimizer2020LODsController
    {
        public override ILODInstance GetLODSetting(int lod)
        {
            return LODInstances[lod];
        }

        public override int GetLODSettingsCount()
        {
            return LODInstances.Count;
        }

        public override ILODInstance InitialSettings
        {
            get
            {
                return LODInitial;
            }

            protected set
            {
                LODInitial = value;
            }
        }
    }
}

#endif