#if OPTIMIZERS_DOTS_IMPORTED

using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersManager
    {

        private readonly List<Optimizer_Base> dots_OptimizersList = new List<Optimizer_Base>();
        private readonly Dictionary<Optimizer_Base, int> dots_OptToId = new Dictionary<Optimizer_Base, int>();
        private readonly Dictionary<int, Optimizer_Base> dots_IdToOpt = new Dictionary<int, Optimizer_Base>();
        private readonly List<int> dots_ToRemoveOpt = new List<int>();


        /// <summary>
        /// Adding optimizer to the list
        /// Generating it's collider for culling algorithm raycast detection
        /// </summary>
        public void DOTS_AddOptimizer(Optimizer_Base optimizer)
        {

            #region Checking requirements

            if (optimizer == null) return;
            if (optimizer.UseDOTS == false) return;
            if (Instance.UseDOTSProgressiveCulling == false) return;

            int id = optimizer.GetInstanceID();
            if (dots_IdToOpt.ContainsKey(id)) return;
            if (dots_OptimizersList.Contains(optimizer)) return;

            #endregion

            #region Definingn if using meshes or simple box / sphere shape

            bool simpleMeshes = true;
            if (optimizer.DOTSDetection == EDOTSDetection.Auto)
                if (optimizer.DOTSMeshData != null)
                    if (optimizer.DOTSMeshData.Count != 0)
                        for (int i = 0; i < optimizer.DOTSMeshData.Count; i++)
                        { if (optimizer.DOTSMeshData[i].SharedMesh != null && optimizer.DOTSMeshData[i].SceneTransform != null) { simpleMeshes = false; break; } }

            #endregion


            bool isObstacle = optimizer.DOTSObstacleType == EDOTSObstacle.StopRays || optimizer.DOTSObstacleType == EDOTSObstacle.Auto;

            if (optimizer.DOTSObstacleType == EDOTSObstacle.Auto)
                // Checking if optimizer is optimizing particle systems or light to force not being obstacle
                for (int i = 0; i < optimizer.GetToOptimizeCount(); i++)
                {
                    if (!isObstacle) break;
                    Component c = optimizer.GetOptimizedComponent(i);
                    if (c == null) continue;
                    SkinnedMeshRenderer sk = c as SkinnedMeshRenderer; if (sk) { isObstacle = false; }
                    Light l = c as Light; if (l) { isObstacle = false; }
                    ParticleSystem p = c as ParticleSystem; if (p) { isObstacle = false; }
                }

            if (SupportNotObstacles == false) if (isObstacle == false) return; // If not supporting not obstacles then disabling occlusion on object


            // Custom colliders for culling detection
            if (optimizer.DOTSDetection == EDOTSDetection.Custom)
            {
                List<OptimizersReference> refs = Optimizer_Base.FindComponentsInAllChildren<OptimizersReference>(optimizer.transform);
                foreach (var rf in refs)
                {
                    if (rf.Parent != null) if (rf.Parent != optimizer) continue;
                    rf.Parent = optimizer;
                    rf.gameObject.layer = OptimizersCullingLayer;
                }
            }
            else

            // Using cube or sphere mesh shape
            if (simpleMeshes)
            {
                Transform targetParent = null;
                Mesh targetMesh = null;

                // Generating with simple box or sphere shape
                if (optimizer.DOTSDetection != EDOTSDetection.Auto)
                {
                    targetParent = DOTS_GenerateContainerFor(optimizer.transform, optimizer.DOTSOffset);

                    #region Fit collision shape to object
                    if (optimizer.DOTSDetection == EDOTSDetection.Cube)
                    {
                        targetParent.localScale = optimizer.DOTSSize;
                        targetMesh = _mesh_cube;
                    }
                    else
                    {
                        targetParent.localScale = Vector3.one * optimizer.DOTSRadius * 2f;
                        targetMesh = _mesh_sphere;
                    }
                    #endregion
                }

                if (targetParent == null || targetMesh == null) return;
                DOTS_AttachCollider(targetParent, targetMesh, optimizer, isObstacle);
            }
            else // Including optimized meshes as culling shapes
            {
                if (optimizer.DOTSDetection == EDOTSDetection.Auto)
                {
                    for (int i = 0; i < optimizer.DOTSMeshData.Count; i++)
                    {
                        Optimizer_Base.DOTS_DetectionData data = optimizer.DOTSMeshData[i];

                        if (data == null) continue;
                        if (data.SceneTransform == null) continue;
                        if (data.SharedMesh == null) continue;

                        Transform meshParent = DOTS_GenerateContainerFor(data.SceneTransform, Vector3.zero);
                        DOTS_AttachCollider(meshParent, data.SharedMesh, optimizer, isObstacle);
                    }
                }
            }

            DOTS_AddToCullingLists(optimizer, id);
            optimizer.DOTSObstacleCheck(false);
        }




        /// <summary>
        /// Safely remove optimizer object from culling algorithm
        /// </summary>
        public void DOTS_RemoveOptimizer(Optimizer_Base optimizer)
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;

            if (optimizer.UseDOTS)
                if (dots_OptimizersList.Contains(optimizer))
                    dots_OptimizersList.Remove(optimizer);

            if (!dots_IdToOpt.ContainsValue(optimizer)) return;

            int id = dots_IdToOpt.First(findId => findId.Value == optimizer).Key;
            dots_ToRemoveOpt.Add(id);
        }


        /// <summary>
        /// Clearing scheduled optimizers to remove
        /// </summary>
        private void DOTS_HandleRemoving()
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;
            for (int i = 0; i < dots_ToRemoveOpt.Count; i++) DOTS_InternalRemove(dots_ToRemoveOpt[i]);
            dots_ToRemoveOpt.Clear();
        }


        /// <summary>
        /// Clearing culling data for given optimizer id
        /// </summary>
        private void DOTS_InternalRemove(int id)
        {
            if (Instance.UseDOTSProgressiveCulling == false) return;
            if (!dots_IdToOpt.ContainsKey(id)) return;

            {
                //Optimizer_Base optimizer = dots_IdToOpt[id];
                //if (optimizer != null)
                //{
                //    optimizer.DOTSObstacleCheck(true);
                //    Optimizer_Base opt = dots_OptToId.First(dic => dic.Value == id).Key;
                //    Destroy(opt.gameObject);
                //}
            }

            dots_IdToOpt.Remove(id);

            int optimizerId = dots_VisibleIds.IndexOf(id);
            if (optimizerId < 0) return;

            dots_VisibleIds.RemoveAtSwapBack(optimizerId);
            dots_DelayElapsed.RemoveAtSwapBack(optimizerId);
        }

    }
}


#endif