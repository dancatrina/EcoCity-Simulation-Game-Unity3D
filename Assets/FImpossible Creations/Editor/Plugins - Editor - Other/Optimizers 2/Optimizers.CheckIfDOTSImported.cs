using FIMSpace.FEditor;
using UnityEngine;

[UnityEditor.InitializeOnLoad]
static class FOptimizers_CheckDOTSDefine
{
#if UNITY_2019_1_OR_NEWER
    static string thedef = "OPTIMIZERS_DOTS_IMPORTED";
    static FOptimizers_CheckDOTSDefine()
    {
        //Debug.Log("Unity.Burst " + FDefinesCompilation.GetTypesInNamespace("Unity.Burst", "").Count);
        //Debug.Log("Unity.Jobs " + FDefinesCompilation.GetTypesInNamespace("Unity.Jobs", "").Count);
        //Debug.Log("Unity.Mathematics " + FDefinesCompilation.GetTypesInNamespace("Unity.Mathematics", "").Count);
        //Debug.Log("Unity.Collections " + FDefinesCompilation.GetTypesInNamespace("Unity.Collections", "").Count);

        if (FDefinesCompilation.GetTypesInNamespace("Unity.Burst", "").Count > 100 &&
            FDefinesCompilation.GetTypesInNamespace("Unity.Jobs", "").Count > 20 &&
            FDefinesCompilation.GetTypesInNamespace("Unity.Mathematics", "").Count > 80 &&
            FDefinesCompilation.GetTypesInNamespace("Unity.Collections", "").Count > 160)
        {
            FDefinesCompilation.SetDefine(thedef);
        }
        else FDefinesCompilation.RemoveDefine(thedef);
    }
#endif
}
