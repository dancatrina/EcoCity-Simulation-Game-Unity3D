using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersPrefabsGrabber
    {
        void DisplayProgress(string info, float progress)
        {
            EditorUtility.DisplayProgressBar("Scene Optimizer is working...", info, progress);
        }

        void DisplayProgress(string title, string info, float progress)
        {
            EditorUtility.DisplayProgressBar(title, info, progress);
        }

        void ClearProgress()
        {
            EditorUtility.ClearProgressBar();
        }

        void RefreshLists()
        {
            if (AllPrefabs == null || AllWithOptimizers == null || AllWithoutOptimizers == null || OptimizeGroups == null)
            {
                AllPrefabs = new List<GameObject>();
                OptimizeGroups = new List<OptimizerGroupSettings>();
                AllWithOptimizers = new List<OptimizerSceneData>();
                AllWithoutOptimizers = new List<OptimizerSceneData>();
            }
        }

        void CheckData()
        {
            if (AllPrefabs.Count == 0 && AllWithOptimizers.Count == 0 && AllWithoutOptimizers.Count == 0)
            {
                dataCollected = false;
            }
        }

        float GetCullDistance(float min, float max, float step, float i)
        {
            return Mathf.Round(Mathf.Lerp(min, max, step * i));
        }

        int GetDistanceRange(float objectSize, int groups)
        {
            float factor = Mathf.InverseLerp(nearestCullingDistance / 8f, furthestCullingDistance / 6f, objectSize);
            int gr = Mathf.RoundToInt(factor * groups);
            if (gr < 0) gr = 0; if (gr > groups - 1) gr = groups - 1;
            return gr;
        }


        void FrameManager(float distance)
        {
            FrameObject(OptimizersManager.Instance.transform, distance);
        }

        void FrameObject(Transform target, float distance)
        {
            SceneView view = SceneView.lastActiveSceneView;
            if (view == null) return;

            if (distance < 2f) distance = 2f;
            Vector3 focPoint = target.position;
            Vector3 toOpt = focPoint - view.camera.transform.position;

            var tgt = view.camera;
            tgt.transform.position = focPoint - toOpt.normalized * distance;
            tgt.transform.rotation = Quaternion.LookRotation(toOpt);
            view.AlignViewToObject(tgt.transform);
        }

        List<GameObject> GetAllPrefabsFromScene()
        {
            List<GameObject> prefabs = new List<GameObject>();

            AllWithOptimizers.Clear();
            AllWithoutOptimizers.Clear();

            try
            {
                Transform[] allSceneObjs = FindObjectsOfType<Transform>();
                int total = allSceneObjs.Length;
                int iter = 0;
                int interval;
                interval = total / 250;
                if (interval < 10) interval = 10;

                DisplayProgress("Collecting scene prefab data", 0f);

                //AssetDatabase.DisallowAutoRefresh();

                foreach (var t in allSceneObjs)
                {
                    GameObject pr = Optimizers_LODTransport.GetProjectPrefabSimple(t.gameObject);
                    GameObject scenep = Optimizers_LODTransport.GetPrefabRootObject(t.gameObject);

                    if (pr != null)
                    {
                        if (!prefabs.Contains(pr))
                        {
                            prefabs.Add(pr);
                            Optimizer_Base optimizer = pr.GetComponentInChildren<Optimizer_Base>();
                            if (optimizer)
                            {
                                OptimizerSceneData f = AllWithOptimizers.FirstOrDefault(ff => ff.prefabObject.GetInstanceID() == pr.GetInstanceID());
                                if (f == null) AllWithOptimizers.Add(new OptimizerSceneData(optimizer, pr, scenep));
                            }
                            else
                            {
                                OptimizerSceneData f = AllWithoutOptimizers.FirstOrDefault(ff => ff.prefabObject.GetInstanceID() == pr.GetInstanceID());
                                if (f == null) AllWithoutOptimizers.Add(new OptimizerSceneData(pr, scenep));
                            }
                        }
                    }

                    iter++;
                    if (iter % interval == 0)
                        DisplayProgress(t.name, "(" + iter + " / " + total + ") Collecting prefabs data from scene... (" + prefabs.Count + ")", (float)iter / (float)total);
                }

            }
            catch (System.Exception e)
            {
                Debug.Log("[Scene Optimizer] Error occured during collecting prefabs from scene: " + e);
            }

            //AssetDatabase.AllowAutoRefresh();

            //foreach (var item in AllWithOptimizers) if (item.prefabObject) EditorUtility.ClearDirty(item.prefabObject);
            //foreach (var item in AllWithoutOptimizers) if (item.prefabObject) EditorUtility.ClearDirty(item.prefabObject);

            //UpdateDatabase();

            return prefabs;
        }

        private void GenerateGroupsSettings()
        {
            OptimizeGroups = new List<OptimizerGroupSettings>();
            for (int i = 0; i < groupsCount; i++)
            {
                OptimizeGroups.Add(new OptimizerGroupSettings());

                if (i > 0)
                {
                    float step = (float)i / (float)groupsCount;
                    OptimizeGroups[i].LODCount += Mathf.FloorToInt(step * 4f);
                }
            }
        }

        static float MeasureObject(GameObject o)
        {
            Vector3[] finalMeasure = Optimizer_Base.MeasureBiggest(o.transform);

            foreach (Transform t in o.GetComponentInChildren<Transform>(true))
            {
                Vector3[] measure = Optimizer_Base.MeasureBiggest(t);
                if (measure[0].x > finalMeasure[0].x) finalMeasure = measure;
            }

            //if (biggest == 0f) biggest = OptimizersManager.Instance.WorldScale * 2f;


            return (float)System.Math.Round(finalMeasure[0].x, 2);
        }


        int ChangePage(int current, int pages, int changeVal)
        {
            int p = current;
            p += changeVal;
            if (p < 0) p = pages;
            if (p > pages) p = 0;
            return p;
        }


        [System.Serializable]
        public class OptimizerSceneData
        {
            public GameObject prefabObject;
            public string prefabPath;
            public GameObject sceneObject;
            public Optimizer_Base optimizer;
            public float scale;



            public OptimizerSceneData(GameObject o, GameObject sceneObj)
            {
                prefabObject = o;
                prefabPath = Optimizers_LODTransport.GetProjectPrefabPath(o);
                sceneObject = sceneObj;
                scale = MeasureObject(o);
            }

            public OptimizerSceneData(Optimizer_Base opt, GameObject o, GameObject sceneObj) : this(o, sceneObj)
            {
                optimizer = opt;
            }
        }

        [System.Serializable]
        public class OptimizerGroupSettings
        {
            public bool Enabled = true;

            public int LODCount = 2;
            public float TransitionTime = 0f;

            public enum EOGCullingMode : int
            {
                NoCulling = 0, ProgressiveCulling = 1, RaycastsPerObject = 2
            }

            public EOGCullingMode CullingMode = EOGCullingMode.NoCulling;

            public bool Lights = true;
            public bool Meshes = true;
            public bool Particles = true;
            public bool UnityLOD = true;
            public bool All = false;

            public int CullAt = 0;
        }


    }
}
