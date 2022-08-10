#if UNITY_2019_4_OR_NEWER

using System.Collections.Generic;
using UnityEngine;


namespace FIMSpace.FOptimizing
{
    public partial class Optimizer2
    {

        protected override void OptimizerReset()
        {
#if UNITY_EDITOR
            SetAutoDistance(0.23f);

            //AssignComponentsToOptimizeFrom(gameObject.transform);
            if (ToOptimize == null) ToOptimize = new List<Optimizer2020LODsController>();

            if (ToOptimize.Count == 0) AssignComponentsToBeOptimizedFromAllChildren(gameObject);
            //if (ToOptimize.Count == 0) AssignComponentsToBeOptimizedFromAllChildren(gameObject, true);

            DrawDeactivateToggle = true;
#endif
        }

        public override void SyncWithReferences()
        {
            #region Prefab Syncing

            if (ToOptimize.Count > 0)
                if (ToOptimize[0].Component != null)
                    if (ToOptimize[0].GetLODSettingsCount() - 2 != LODLevels)
                    {
                        LODLevels = ToOptimize[0].GetLODSettingsCount() - 2;
                        preLODLevels = LODLevels;
                    }

            #endregion
        }

        protected override void OnValidateRefreshComponents()
        {
            if (ToOptimize != null)
                RefreshToOptimizeList();
            else
                AssignComponentsToOptimizeFrom(gameObject.transform);
        }

        protected override void OnValidateUpdateToOptimize(bool hard = false)
        {
            //for (int i = 0; i < ToOptimize.Count; i++) if (ToOptimize[i].Component) ToOptimize[i].StoreLODLockedSettings();

            if (preLODLevels != LODLevels || hard)
            {
                ResetLODs(hard);
            }

#if UNITY_EDITOR
            if (ToOptimize != null) for (int i = 0; i < ToOptimize.Count; i++) if (ToOptimize[i].Component) ToOptimize[i].OnValidate();
#endif

            //for (int i = 0; i < ToOptimize.Count; i++) if (ToOptimize[i].Component) ToOptimize[i].RestoreLODLockedSettings();
        }

        public override void CheckForNullsToOptimize()
        {
            if (ToOptimize == null) return;
            for (int i = ToOptimize.Count - 1; i >= 0; i--)
            {
                if (ToOptimize[i] == null)
                    ToOptimize.RemoveAt(i);
                //else
                //{
                //    if (ToOptimize[i].GetLODSetting(0).TargetComponent == null)
                //        ToOptimize.RemoveAt(i);
                //}
            }
        }

        protected override void ResetLODs(bool hard = false)
        {
#if UNITY_EDITOR
            for (int i = 0; i < ToOptimize.Count; i++)
            {
                ToOptimize[i].GenerateLODParameters(hard);
            }

            if (ToOptimize.Count > 0)
                if (ToOptimize[0].GetLODSetting(0) != null)
                    if (LODLevels != ToOptimize[0].GetLODSettingsCount() - 2) HiddenCullAt = LODLevels;

            preLODLevels = LODLevels;
#endif

        }

        protected override void RefreshInitialSettingsForOptimized()
        {
            RefreshDistances();

            for (int i = ToOptimize.Count - 1; i >= 0; i--)
            {
                if (ToOptimize == null) { ToOptimize.RemoveAt(i); continue; }
                ToOptimize[i].OnStart();
            }
        }


        public override void AssignComponentsToOptimizeFrom(Component target, bool includeAdvanced = false)
        {
#if UNITY_EDITOR
            if (ToOptimize == null) ToOptimize = new List<Optimizer2020LODsController>();

            // Checking if there is no other optimizer using this components for optimization
            List<Optimizer_Base> childOptimizers = FindComponentsInAllChildren<Optimizer_Base>(transform);

            manager = OptimizersManager.Instance; manager = null; // Casting Get() to generate optimizers manager

            Component[] allOnTarget = target.GetComponents<Component>();

            for (int i = 0; i < allOnTarget.Length; i++)
            {
                //Optimizer2020LODsController.EEssType type;

                //if (includeAdvanced) 
                //    type = Optimizer2020LODsController.GetEssentialTypeAll(allOnTarget[i]);
                //else
                //    type = Optimizer2020LODsController.GetEssentialType(allOnTarget[i]);

                //if (type != Optimizer2020LODsController.EEssType.Unknown && type != Optimizer2020LODsController.EEssType.MonoBehaviour)

                var instance = LODInstanceGenerator.GenerateInstanceOutOf(this, allOnTarget[i], true, includeAdvanced ? LODInstanceGenerator.ESearchMode.AllComponents : LODInstanceGenerator.ESearchMode.JustUnityComponents);
                if (instance == null) continue;
                AddToOptimizeIfCan(allOnTarget[i], childOptimizers);
            }
#endif
        }



        public override void AssignCustomComponentToOptimize(MonoBehaviour target)
        {

            if (ToOptimize == null) ToOptimize = new List<Optimizer2020LODsController>();

            // Checking if there is no other optimizer using this components for optimization
            List<Optimizer_Base> childOptimizers = FindComponentsInAllChildren<Optimizer_Base>(transform);

            manager = OptimizersManager.Instance; manager = null; // Casting Get() to generate optimizers manager

            Component[] allOnTarget = target.GetComponents<Component>();

            for (int i = 0; i < allOnTarget.Length; i++)
            {
                var instance = LODInstanceGenerator.GenerateInstanceOutOf(this, allOnTarget[i], true, LODInstanceGenerator.ESearchMode.JustCustomComponents);
                if (instance == null) continue;
                //if (Optimizer2020LODsController.GetEssentialTypeAll(allOnTarget[i]) != Optimizer2020LODsController.EEssType.Unknown)
                AddToOptimizeIfCan(allOnTarget[i], childOptimizers);
            }

        }



        public void AddToOptimizeIfCan(Component target, List<Optimizer_Base> childOptimizers)
        {
#if UNITY_EDITOR
            if (target == null) return;
            if (target is Optimizer_Base) return;
            if (target is OptimizersReference) return;
            if (target is Optimizers_TriggerHelper) return;
            if (target is OptimizersScriptablesCleaner) return;
            if (target is Camera) return;
            if (target is OptimizersManager) return;
            //if (target is OptimizersRuntimeGenerator) return;
            if (target.GetType().IsSubclassOf(typeof(Optimizer_Base))) return;


            Optimizer2020LODsController controller = new Optimizer2020LODsController(this, target, ToOptimize.Count, "Optim");

            bool inUse = false;

            for (int i = 0; i < ToOptimize.Count; i++)
                if (ToOptimize[i].Component == target) { inUse = true; break; }

            if (childOptimizers != null)
                if (!inUse)
                    for (int i = 0; i < ToOptimize.Count; i++)
                        if (CheckIfAlreadyInUse(ToOptimize[i], childOptimizers)) { inUse = true; break; }

            if (!inUse) AddToOptimize(controller);
#endif
        }


        public override void RemoveFromToOptimizeAt(int i)
        {
#if UNITY_EDITOR
            if (i < ToOptimize.Count)
                ToOptimize.RemoveAt(i);
#endif
        }


        public override bool ContainsComponent(Component component)
        {
            for (int i = ToOptimize.Count - 1; i >= 0; i--)
            {
                if (ToOptimize == null) { ToOptimize.RemoveAt(i); continue; }
                if (ToOptimize[i].Component == component) return true;
                //if (ToOptimize[i].GetLODSetting(0).TargetComponent == component) return true;
            }

            return false;
        }


        public override void RefreshToOptimizeList()
        {
            for (int i = ToOptimize.Count - 1; i >= 0; i--)
            {
                if (ToOptimize[i] == null) ToOptimize.RemoveAt(i);
            }
        }


    }
}

#endif