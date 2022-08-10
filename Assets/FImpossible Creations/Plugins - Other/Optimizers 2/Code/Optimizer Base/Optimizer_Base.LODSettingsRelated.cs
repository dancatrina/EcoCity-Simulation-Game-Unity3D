using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public abstract partial class Optimizer_Base
    {
        public virtual int GetToOptimizeCount() { return 0; }

        /// <summary> Reassigning auto settings </summary>
        public abstract void RefreshLODSettings();

    }
}
