using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public abstract partial class Optimizer_Base
    {
        [Tooltip("If Progressive Occlusion Culling should be applied with this optimizer")]
        public bool UseDOTS = false;
        [Tooltip("If this object shouldn be or not be sight obstacle. If it lets rays through it means this object is transparent and can't occlude other objects but can be occluded by other objects.\nIf 'Not Obstacles' are disabled under Optimizers Manager then Occlusion will not be triggered on the object if it will be automatically detected that it should be transparent (when particle system or light is attached to the object)")]
        public EDOTSObstacle DOTSObstacleType = EDOTSObstacle.Auto;

        [Tooltip("Detecting object's visibility with meshes or simple shapes")]
        public EDOTSDetection DOTSDetection = EDOTSDetection.Auto;

        [Tooltip("List of meshes to detect visibility on with DOTS")]
        public List<DOTS_DetectionData> DOTSMeshData;

        // Using simple primitive mesh adjustements ----------------
        public Vector3 DOTSOffset = Vector3.zero;
        public Vector3 DOTSSize = Vector3.one;
        public float DOTSRadius = 1f;

        //[Tooltip("If check rays should stop on this object, you should turn it off for lights / particles which are transparent!")]
        //public bool DOTSIsVisibilityObstacle = true;


#if OPTIMIZERS_DOTS_IMPORTED

        public void DOTSInit()
        {

        }

        /// <summary>
        /// For DOTS Implementation
        /// </summary>
        public void DOTSObstacleCheck(bool visible)
        {
            SetHidden(!visible);
        }

#endif

        public void DOTSFindMeshes(bool force = false)
        {
            if (DOTSMeshData == null) DOTSMeshData = new List<DOTS_DetectionData>();
            if (force) DOTSMeshData.Clear();

            if (DOTSMeshData.Count == 0)
            {
                for (int i = 0; i < GetToOptimizeCount(); i++)
                {
                    Component cmp = GetOptimizedComponent(i);

                    #region from LodGroup

                    LODGroup lg = cmp as LODGroup;
                    if (lg)
                    {
                        MeshRenderer target = null;

                        LOD[] lll = lg.GetLODs();
                        if (lll != null)
                            for (int lI = 0; lI < lll.Length; lI++)
                            {
                                if (lll[lI].renderers == null) continue;
                                for (int lrI = 0; lrI < lll[lI].renderers.Length; lrI++)
                                {
                                    Renderer r = lll[lI].renderers[lrI];
                                    if (r != null)
                                    {
                                        MeshRenderer potentialTarget = r as MeshRenderer;
                                        if (potentialTarget != null)
                                        {
                                            MeshFilter mf = r.GetComponent<MeshFilter>();
                                            if (mf) if (mf.sharedMesh != null)
                                                {
                                                    if (!DOTSAlreadyContains(mf.transform))
                                                    {
                                                        DOTSMeshData.Add(new DOTS_DetectionData().Set(mf.transform, mf.sharedMesh));
                                                        target = potentialTarget;
                                                    }
                                                    else

                                                    break;
                                                }
                                        }
                                    }
                                }

                                if (target != null) break;

                            }

                        continue;
                    }

                    #endregion

                    if (cmp is MeshRenderer == false) continue;

                    MeshFilter f = cmp.GetComponent<MeshFilter>();
                    if (f) if (f.sharedMesh)
                        {
                            bool notLOD = true;

                            if (!force)
                                if (f.name.ToLower().Contains("_lod"))
                                    if (!f.name.ToLower().Contains("_lod0")) notLOD = false;

                            if (notLOD)
                                if (!DOTSAlreadyContains(cmp.transform)) DOTSMeshData.Add(new DOTS_DetectionData().Set(f.transform, f.sharedMesh));
                        }
                }
            }
        }

        public bool DOTSAlreadyContains(Transform t)
        {
            return DOTSMeshData.FirstOrDefault(x => x.SceneTransform == t) != null;
        }

        [System.Serializable]
        public class DOTS_DetectionData
        {
            public DOTS_DetectionData Set(Transform t, Mesh m)
            {
                SharedMesh = m;
                SceneTransform = t;
                return this;
            }

            public Mesh SharedMesh;
            public Transform SceneTransform;
        }

    }
}
