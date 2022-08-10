#if OPTIMIZERS_DOTS_IMPORTED

using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersManager
    {

        private Transform DOTS_GenerateContainerFor(Transform parent, Vector3 localOffset)
        {
            Transform g = new GameObject("Optimizers-DOTS Culling Helper").transform;

            g.SetParent(parent.transform);
            g.localPosition = localOffset;
            g.localRotation = Quaternion.identity;
            g.localScale = Vector3.one;

            return g;
        }


        private void DOTS_AttachCollider(Transform parent, Mesh collision, Optimizer_Base optimizer, bool isObstacle)
        {
            MeshCollider collider = parent.gameObject.AddComponent<MeshCollider>();

            collider.gameObject.layer = OptimizersCullingLayer;
            collider.sharedMesh = collision;

            OptimizersReference optRef = parent.gameObject.AddComponent<OptimizersReference>();
            optRef.Parent = optimizer;
            optRef.IsObstacle = isObstacle;
        }


        private void DOTS_AddToCullingLists(Optimizer_Base optimizer, int id)
        {
            dots_IdToOpt.Add(id, optimizer);
            dots_OptToId.Add(optimizer, id);
            dots_OptimizersList.Add(optimizer);
        }


    }
}


#endif