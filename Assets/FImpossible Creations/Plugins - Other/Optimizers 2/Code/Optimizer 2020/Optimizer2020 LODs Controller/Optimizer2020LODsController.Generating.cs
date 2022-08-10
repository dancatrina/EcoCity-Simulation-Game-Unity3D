#if UNITY_2019_4_OR_NEWER

using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace FIMSpace.FOptimizing
{

    public partial class Optimizer2020LODsController
    {
        protected override void GenerateNewLODSettings()
        {
            if (ToOptimize == null) { UnityEngine.Debug.Log("[Optimizers] Unknown to optimize component!"); return; }
            LODInstances = new List<ILODInstance>();
        }

        /// <summary>
        /// Generating initial settings empty instance
        /// </summary>
        private void GenerateInitialSettings()
        {
            LODInitial = GenerateInstance();
        }

        private ILODInstance GenerateInstance()
        {
            return LODInstanceGenerator.GenerateInstanceOutOf(eOptimizer, ToOptimize);
        }


        /// <summary>
        /// Checking if LOD parameters need to be generated for needed LOD levels count.
        /// If count is invalid to needed one, new LOD parameters are generated (empty ones)
        /// </summary>
        protected override void CheckAndGenerateLODParameters()
        {
            // Checking again count in case if it was cleared in previous lines of code
            if (GetLODSettingsCount() != optimizer.LODLevels + 2)
            {
                for (int i = 0; i < optimizer.LODLevels + 2; i++) LODInstances.Add(GenerateInstance());
            }

            RefreshOptimizerLODCount();
        }


        internal override void ApplyLODLevelSettings(ILODInstance currentLOD)
        {
            if (currentLOD == null) return;

            CurrentLODLevel = currentLOD.Index;
            if (IsTransitioningOrOther()) CurrentLODLevel = -1;
            currentLOD.ApplySettingsToTheComponent(Component, InitialSettings);
        }

        public void OnValidate()
        {

#if UNITY_EDITOR
            if (!optimizer) return;
            if (!optimizer.enabled) return;

            if (Application.isPlaying) return;

            if (GetIFLODList() != null)
                if (GetIFLODList().Count > 1)
                    for (int i = 1; i < GetIFLODList().Count; i++)
                    {
                        if (GetIFLODList()[i] == null) continue;
                        GetIFLODList()[i].AssignToggler(GetIFLODList()[0]);
                    }
#endif

        }

        public void SetFromEssential(EssentialLODsController ess)
        {
            List<ILODInstance> essList = ess.GetIFLODsForOptimizer2();
            if (essList.Count != LODInstances.Count) return;
            if (essList.Count <= 0) return;
            if (essList[0].GetType() != LODInstances[0].GetType()) return;

            for (int i = 0; i < LODInstances.Count; i++)
            {
                LODInstances[i] = essList[i].GetCopy();
            }
        }
    }

}

#endif