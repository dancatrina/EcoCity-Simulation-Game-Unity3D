using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public static class OptimUtils
    {
#if UNITY_2019_4_OR_NEWER
            private static IgnoreOptimizer ign;
#endif
        public static bool ShouldBeIgnored(Component comp)
        {
#if UNITY_2019_4_OR_NEWER
            return comp.gameObject.TryGetComponent(out ign);
#else
            return comp.gameObject.GetComponent<IgnoreOptimizer>() != null;
#endif
        }
    }
}
