using System;
using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public abstract partial class Optimizer_Base
    {

        #region Compute Scale


        public float GetDetectionRadiusRaw()
        {
            if (CullIfNotSee == false)
                return 0f;
            else
                return DetectionRadius;
        }


        public void TryAutoComputeDetectionShape(float scaleUp = 1f)
        {
            float biggest = 0f;
            Vector3[] finalMeasure = null;
            Component biggestC = null;
            List<Vector3[]> allMeasures = new List<Vector3[]>();

            for (int i = 0; i < GetToOptimizeCount(); i++)
            {
                Component cmp = GetOptimizedComponent(i);
                if (cmp == null) continue;

                Vector3[] measure = MeasureBiggest(cmp.transform, false, 0f, cmp.GetType());
                float scale = measure[0].x;

                allMeasures.Add(measure);
                if (scale > biggest) { biggest = scale; finalMeasure = measure; biggestC = cmp; }
            }


            // Trying to find shape which will contain all objects
            if (GetToOptimizeCount() > 1)
            {
                Vector3 positive = Vector3.zero;
                Vector3 negative = Vector3.zero;
                Bounds centerBound = new Bounds(allMeasures[0][2], Vector3.zero);
                //Bounds centerBound = new Bounds(transform.position, Vector3.zero);
                Bounds scaleBound = new Bounds(allMeasures[0][2] + Vector3.up * allMeasures[0][0].x, Vector3.zero);

                for (int i = 0; i < allMeasures.Count; i++)
                {
                    if (i > 0)
                        centerBound.Encapsulate(allMeasures[i][2]);

                    Vector3 itsCenter = transform.InverseTransformPoint(allMeasures[i][2]);
                    Vector3 boundsDir = transform.InverseTransformVector(Vector3.one * allMeasures[i][0].x);

                    if (itsCenter.x + boundsDir.x > positive.x) positive.x = itsCenter.x + boundsDir.x;
                    else
                    if (itsCenter.x - boundsDir.x < negative.x) negative.x = itsCenter.x - boundsDir.x;

                    if (itsCenter.y + boundsDir.y > positive.y) positive.y = itsCenter.y + boundsDir.y;
                    else
                    if (itsCenter.y - boundsDir.y < negative.y) negative.y = itsCenter.y - boundsDir.y;

                    if (itsCenter.z + boundsDir.z > positive.z) positive.z = itsCenter.z + boundsDir.z;
                    else
                    if (itsCenter.z - boundsDir.z < negative.z) negative.z = itsCenter.z - boundsDir.z;

                    //Vector3 fromCenterTo = allMeasures[i][2] - transform.position;

                    if (i > 0)
                        scaleBound.Encapsulate(allMeasures[i][2] + Vector3.up * allMeasures[i][0].x);

                    scaleBound.Encapsulate(allMeasures[i][2] + Vector3.down * allMeasures[i][0].x);
                    scaleBound.Encapsulate(allMeasures[i][2] - Vector3.right * allMeasures[i][0].x);
                    scaleBound.Encapsulate(allMeasures[i][2] + Vector3.right * allMeasures[i][0].x);
                    scaleBound.Encapsulate(allMeasures[i][2] - Vector3.forward * allMeasures[i][0].x);
                    scaleBound.Encapsulate(allMeasures[i][2] + Vector3.forward * allMeasures[i][0].x);
                }

                DetectionRadius = GetBiggest(positive);

                if (GetBiggest(negative, true) > DetectionRadius)
                    DetectionRadius = GetBiggest(negative, true);

                DetectionBounds = Vector3.zero;

                DetectionOffset = transform.InverseTransformPoint(centerBound.center);
                DetectionOffset = transform.InverseTransformPoint(scaleBound.center);
                DetectionRadius = scaleBound.extents.magnitude;
                DetectionBounds = scaleBound.extents * 2f;
            }
            else
            {
                if (finalMeasure != null)
                {
                    DetectionRadius = finalMeasure[0].x;
                    DetectionBounds = finalMeasure[1] * 2f;

                    if (biggestC != null) DetectionOffset = transform.InverseTransformPoint(finalMeasure[2]);
                }
            }

            DetectionRadius /= transform.lossyScale.x;
            DetectionRadius *= scaleUp;
            DetectionBounds.x /= transform.lossyScale.x;
            DetectionBounds.y /= transform.lossyScale.y;
            DetectionBounds.z /= transform.lossyScale.z;
            DetectionBounds *= scaleUp;

        }

        public static float GetBiggest(Vector3 from, bool abs = false)
        {
            if (abs)
            {
                from.x = Mathf.Abs(from.x);
                from.y = Mathf.Abs(from.y);
                from.z = Mathf.Abs(from.z);
            }

            if (from.x > from.y)
            {
                if (from.z > from.x) return from.z;
                else return from.x;
            }
            else
            {
                if (from.z > from.y) return from.z;
                else return from.y;
            }
        }

        /// <summary>
        /// Returns 3 elements array : [0].x is radius [1] is bounds extends dimensions (half size)  [2] bounds center in world space
        /// </summary>
        public static Vector3[] MeasureBiggest(Transform t, bool withLodGroups = true, float limitTo = 0, Type checkOnly = null)
        {
            Vector3[] result = new Vector3[3];
            result[0] = new Vector3();
            result[1] = new Vector3();
            result[2] = new Vector3();

            float size = 0f;
            float biggest = 0f;
            float biggestBounds = 0f;
            Vector3 biggestBoundsV = Vector3.zero;
            Vector3 biggestBoundsC = Vector3.zero;


            bool canCol = true;
            if (checkOnly != null) if (typeof(Collider) == checkOnly) canCol = true;

            if (canCol)
            {
                Collider col = t.gameObject.GetComponent<Collider>();
                if (col)
                {
                    size = col.bounds.extents.magnitude;

                    if (size > biggest)
                    {
                        biggest = size;
                        if (size > biggestBounds)
                        {
                            biggestBounds = size;
                        }
                    }

                    if (col.bounds.extents.magnitude > biggestBoundsV.magnitude)
                    {
                        biggestBoundsV = col.bounds.extents;
                        biggestBoundsC = col.bounds.center;
                    }
                }
            }

            bool canRend = true;
            if (checkOnly != null)
            {
                if (typeof(Renderer) == checkOnly) canRend = true;
                if (typeof(MeshRenderer) == checkOnly) canRend = true;
                if (typeof(SkinnedMeshRenderer) == checkOnly) canRend = true;
            }

            if (canRend)
            {
                Renderer r = t.gameObject.GetComponent<Renderer>();
                if (r)
                {
                    size = r.bounds.extents.magnitude;
                    if (size > biggest)
                    {
                        biggest = size;
                        if (size > biggestBounds) biggestBounds = size;
                    }

                    if (r.bounds.extents.magnitude > biggestBoundsV.magnitude)
                    {
                        biggestBoundsV = r.bounds.extents;
                        biggestBoundsC = r.bounds.center;
                    }
                }
            }

            bool canLight = true;
            if (checkOnly != null) if (typeof(Light) == checkOnly) canLight = true;
            if (canLight)
            {
                Light l = t.gameObject.GetComponent<Light>();
                if (l) size = l.range * 1.2f;
                if (size > biggest) biggest = size;
            }

            bool canParticle = true;
            if (checkOnly != null) if (typeof(ParticleSystem) == checkOnly) canParticle = true;

            if (canParticle)
            {
                ParticleSystem ps = t.gameObject.GetComponent<ParticleSystem>();
                if (ps)
                {
                    //size = ps.main.startSize.constantMax * ps.main.startSpeed.constantMax * ps.transform.lossyScale.x * 1.8f;
                    //if (size > biggest) biggest = size;

                    ParticleSystemRenderer psr = ps.GetComponent<ParticleSystemRenderer>();
                    if (psr)
                    {
                        size = Vector3.Scale(psr.bounds.extents, t.lossyScale).magnitude * 2f;
                        if (size > biggest)
                        {
                            biggest = size;
                            if (size > biggestBounds) biggestBounds = size;
                        }

                        if (psr.bounds.extents.magnitude > biggestBoundsV.magnitude)
                        {
                            biggestBoundsV = Vector3.Scale(psr.bounds.extents, t.lossyScale);
                            biggestBoundsC = psr.bounds.center;
                        }
                    }
                }
            }


            if (withLodGroups)
            {
                LODGroup lod = t.gameObject.GetComponent<LODGroup>();
                if (lod)
                {
                    lod.RecalculateBounds();

                    float cullPerc = lod.GetLODs()[lod.GetLODs().Length - 1].screenRelativeTransitionHeight;
                    Camera cam = OptimizersManager.MainCamera;
                    if (cam == null) cam = Camera.main;
                    float fov = 60f; if (cam) fov = cam.fieldOfView;
                    float div = 100f / fov;

                    size = 0.3f * lod.size * div * lod.transform.lossyScale.x / cullPerc;
                    if (lod.GetLODs().Length == 1) size *= 0.75f;
                    if (size > biggest) biggest = size;
                }
            }

            if (limitTo > 0)
                if (biggest > limitTo) biggest = limitTo;

            if (biggestBoundsC == Vector3.zero) biggestBoundsC = t.position;

            if (t.lossyScale.x == 0 || t.lossyScale.y == 0 || t.lossyScale.z == 0)
            {
                Debug.Log("[Optimizers] Object can't have zero scale in any axis! (" + t.name + ")");
                result[0].x = biggest;
                if (biggestBoundsV.magnitude == 0f) biggestBoundsV = Vector3.one * biggest;
                result[1] = biggestBoundsV;
                result[2] = biggestBoundsV;
                result[3] = biggestBoundsC;
                return result;
            }

            result[0].x = biggest;
            if (biggestBoundsV.magnitude == 0f) biggestBoundsV = Vector3.one * biggest;
            result[1] = biggestBoundsV;
            result[2] = biggestBoundsC;

            return result;
        }

        #endregion


        /// <summary>
        /// Removing renderer optimizers if using LODGroup with this renderers inside
        /// </summary>
        public void CheckAndRemoveRenderersAlreadyInOtherLODGroup()
        {
            for (int i = 0; i < GetToOptimizeCount(); i++)
            {
                LODGroup lg = GetOptimizedComponent(i) as LODGroup;

                if (lg != null)
                {
                    for (int m = GetToOptimizeCount(); m >= 0; m--)
                    {
                        Renderer r = GetOptimizedComponent(m) as Renderer;
                        if (r)
                        {
                            if (IsRendererInLODGroup(r, lg)) { RemoveFromToOptimizeAt(m); }
                        }
                    }

                    if (lg.lodCount > LODLevels) LODLevels = lg.lodCount;
                }
            }

            EditorUpdate();
        }


        /// <summary>
        /// Checking if some renderer is included inside Unity LOD Group
        /// </summary>
        public static bool IsRendererInLODGroup(Renderer r, LODGroup lod)
        {
            for (int lodsI = 0; lodsI < lod.lodCount; lodsI++)
            {
                for (int rendsI = 0; rendsI < lod.GetLODs()[lodsI].renderers.Length; rendsI++)
                {
                    if (lod.GetLODs()[lodsI].renderers[rendsI] == r) return true;
                }
            }

            return false;
        }
    }
}
