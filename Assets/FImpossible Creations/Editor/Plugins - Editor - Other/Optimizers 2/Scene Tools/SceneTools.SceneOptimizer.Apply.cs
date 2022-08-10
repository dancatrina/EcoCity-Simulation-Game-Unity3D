using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FIMSpace.FOptimizing
{
    public partial class OptimizersPrefabsGrabber
    {
        //int latelyChanged = 0;
        public enum EOptType
        {
            UseEssentialOptimizer,
            UseOptimizer2_2019_4_OR_NEWER
        }

        public EOptType OptimizerType = EOptType.UseOptimizer2_2019_4_OR_NEWER;

        void AddOptimizersToPrefabs()
        {
            if (AllWithoutOptimizers.Count == 0)
            {
                EditorUtility.DisplayDialog("No prefabs to add Optimizers!", "No prefabs to add Optimizers!", "OK");
                return;
            }
            else
            {
#if UNITY_2018_4_OR_NEWER
                if (fastMode == false)
                    if (AllWithoutOptimizers.Count > 30)
                        EditorUtility.DisplayDialog("It can take time!", "New unity prefab pipeline is very slow.\nMost of the time for next process will be reimporting changed prefabs which is unavoidable.\nPlease be patient.", "OK");
#endif
            }

            try
            {
                //latelyChanged = AllWithoutOptimizers.Count;
                System.Threading.Thread.Sleep(20);

                for (int i = 0; i < AllWithoutOptimizers.Count; i++)
                {
                    OptimizerSceneData o = AllWithoutOptimizers[i];
                    float progress = (float)i / (float)AllWithoutOptimizers.Count;

                    if (o != null)
                        if (o.prefabObject)
                        {
                            DisplayProgress("Waiting for Unity Asset Database (" + i + " / " + AllWithoutOptimizers.Count + ")", "Adding Optimizer to " + o.prefabObject.name, progress);
                            StarChangingAsset();

                            EssentialOptimizer eopt = o.prefabObject.GetComponent<EssentialOptimizer>();
                            if (eopt == null)
                            {
                                int groupIndex = GetDistanceRange(o.scale * scaleBias, groupsCount);
                                if (groupIndex < OptimizeGroups.Count)
                                {
                                    OptimizerGroupSettings g = OptimizeGroups[groupIndex];
                                    if (g != null)
                                    {
                                        if (g.Enabled)
                                        {
                                            eopt = AllWithoutOptimizers[i].prefabObject.AddComponent<EssentialOptimizer>();
                                            ApplyGroupSettingsTo(o, eopt, g, groupIndex);
                                            if (o.prefabObject != null) EditorUtility.SetDirty(o.prefabObject);
                                        }
                                    }
                                }
                            }

                            EndChangingAsset(o);
                        }
                }

            }
            catch (System.Exception e)
            {
                Debug.Log("[Optimizers Scene Tool Error] " + e);
            }

            ClearProgress();
            ClearData();

            UpdateDatabase(false);
        }

        //bool waitingForSave = false;


        void StarChangingAsset()
        {
            if (!fastMode)
            {
#if UNITY_2019_4_OR_NEWER
                AssetDatabase.DisallowAutoRefresh();
#endif
                AssetDatabase.StartAssetEditing();
            }
        }

        void EndChangingAsset(OptimizerSceneData o)
        {
            if (!fastMode)
            {
                //EditorUtility.SetDirty(o.prefabObject);
                AssetDatabase.StopAssetEditing();
                //AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();

#if UNITY_2019_4_OR_NEWER
                AssetDatabase.AllowAutoRefresh();
#endif
            }

            System.Threading.Thread.Sleep(20);
        }


        void UpdateDatabase(bool reloadIfNeed = true)
        {
            try
            {
                DisplayProgress("Updating Asset Database by Unity", 0.9f);
                bool check = OptimizersManager.Exists;

                if (!fastMode)
                {
                    //waitingForSave = true;
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                else if (reloadIfNeed)
                {
                    Scene cScene = EditorSceneManager.GetActiveScene();
                    if (!ignoreSceneSave) if (cScene.isDirty == true) EditorSceneManager.SaveScene(cScene);
                    EditorSceneManager.OpenScene(cScene.path);
                }

            }
            catch (System.Exception)
            { }

            ClearProgress();
        }


        void RefreshOptimizersOnPrefabs()
        {
            if (AllWithOptimizers.Count == 0)
            {
                EditorUtility.DisplayDialog("No prefabs to refresh Optimizers on!", "No prefabs to refresh Optimizers on!", "OK");
                return;
            }
            else
            {
#if UNITY_2018_4_OR_NEWER
                if (fastMode == false)
                    if (AllWithOptimizers.Count > 30)
                        EditorUtility.DisplayDialog("It can take time!", "New unity prefab pipeline is very slow.\nMost of the time for next process will be reimporting changed prefabs which is unavoidable.\nPlease be patient.", "OK");
#endif
            }

            try
            {
                //latelyChanged = AllWithOptimizers.Count;
                System.Threading.Thread.Sleep(20);

                for (int i = 0; i < AllWithOptimizers.Count; i++)
                {
                    float progress = (float)i / (float)AllWithOptimizers.Count;
                    OptimizerSceneData o = AllWithOptimizers[i];

                    if (AllWithOptimizers[i].prefabObject)
                    {
                        DisplayProgress("Waiting for Unity Asset Database (" + i + " / " + AllWithOptimizers.Count + ")", "Refreshing Optimizer on " + AllWithOptimizers[i].prefabObject.name, progress);

                        StarChangingAsset();
                        EssentialOptimizer eopt = o.prefabObject.GetComponent<EssentialOptimizer>();

                        if (eopt != null)
                        {
                            int groupIndex = GetDistanceRange(o.scale * scaleBias, groupsCount);

                            if (groupIndex < OptimizeGroups.Count)
                            {
                                OptimizerGroupSettings g = OptimizeGroups[groupIndex];
                                if (g != null)
                                {
                                    if (g.Enabled)
                                    {
                                        ApplyGroupSettingsTo(o, eopt, g, groupIndex);
                                        EditorUtility.SetDirty(o.prefabObject);
                                    }
                                }
                            }
                        }

                        EndChangingAsset(o);
                        System.Threading.Thread.Sleep(20);
                    }
                }
            }
            catch (System.Exception e)
            {
                AssetDatabase.StopAssetEditing();
                Debug.Log("[Optimizers Scene Tool Error] " + e);
            }

            ClearProgress();
            ClearData();

            UpdateDatabase();
        }


        void DestroyOptimizersOnPrefabs()
        {
            if (AllWithOptimizers.Count == 0)
            {
                EditorUtility.DisplayDialog("No Optimizers to remove!", "No Optimizers to remove!", "OK");
                return;
            }
            else
            {
#if UNITY_2018_4_OR_NEWER
                if (fastMode == false)
                    if (AllWithOptimizers.Count > 30)
                        EditorUtility.DisplayDialog("It can take time!", "New unity prefab pipeline is very slow.\nMost of the time for next process will be reimporting changed prefabs which is unavoidable.\nPlease be patient.", "OK");
#endif
            }

            try
            {
                //latelyChanged = AllWithOptimizers.Count;
                System.Threading.Thread.Sleep(20);

                for (int i = 0; i < AllWithOptimizers.Count; i++)
                {
                    OptimizerSceneData o = AllWithOptimizers[i];
                    DisplayProgress("Waiting for Unity Asset Database (" + i + " / " + AllWithOptimizers.Count + ")", "Removing Optimizer from " + o.prefabObject.name, (float)i / (float)AllWithOptimizers.Count);
                    StarChangingAsset();
                    Optimizer_Base opt = o.optimizer;

                    if (opt) GameObject.DestroyImmediate(opt, true);
                    EndChangingAsset(o);
                }

                System.Threading.Thread.Sleep(20);

            }
            catch (System.Exception e)
            {
                Debug.Log("[Optimizers Scene Tool Error] " + e);
            }

            ClearProgress();
            ClearData();

            UpdateDatabase();
        }


        void ApplyGroupSettingsTo(OptimizerSceneData data, EssentialOptimizer optimizer, OptimizerGroupSettings g, int groupIndex)
        {
            if (optimizer == null) return;
            if (optimizer.gameObject.isStatic) optimizer.OptimizingMethod = EOptimizingMethod.Static;
            float cullDist = GetCullDistance(nearestCullingDistance, furthestCullingDistance, (1f / (float)(groupsCount - 1)), groupIndex);

            optimizer.MaxDistance = cullDist;

            if (g.All)
            {
                optimizer.AssignComponentsToBeOptimizedFromAllChildren(data.prefabObject, true);
            }
            else
            {
#region Removing not wanted components for optimization

                if (g.Lights == false)
                {
                    for (int i = optimizer.GetToOptimizeCount() - 1; i >= 0; i--)
                    {
                        Component c = optimizer.GetOptimizedComponent(i);
                        if (c == null) continue;
                        Light l = c as Light;
                        if (l == null) continue;
                        optimizer.RemoveFromToOptimizeAt(i);
                    }
                }

                if (g.Meshes == false)
                {
                    for (int i = optimizer.GetToOptimizeCount() - 1; i >= 0; i--)
                    {
                        Component c = optimizer.GetOptimizedComponent(i);
                        if (c == null) continue;
                        Renderer r = c as Renderer;
                        if (r == null) continue;
                        optimizer.RemoveFromToOptimizeAt(i);
                    }
                }

                if (g.Particles == false)
                {
                    for (int i = optimizer.GetToOptimizeCount() - 1; i >= 0; i--)
                    {
                        Component c = optimizer.GetOptimizedComponent(i);
                        if (c == null) continue;
                        ParticleSystem p = c as ParticleSystem;
                        if (p == null) continue;
                        optimizer.RemoveFromToOptimizeAt(i);
                    }
                }

                if (g.UnityLOD == false)
                {
                    for (int i = optimizer.GetToOptimizeCount() - 1; i >= 0; i--)
                    {
                        Component c = optimizer.GetOptimizedComponent(i);
                        if (c == null) continue;
                        LODGroup p = c as LODGroup;
                        if (p == null) continue;
                        optimizer.RemoveFromToOptimizeAt(i);
                    }
                }

#endregion
            }

            optimizer.LODLevels = g.LODCount;
            optimizer.CheckAndRemoveRenderersAlreadyInOtherLODGroup();

            optimizer.HiddenCullAt = g.CullAt;
            optimizer.FadeDuration = g.TransitionTime;

            EssentialOptimizer eOpt = optimizer as EssentialOptimizer;
            if (eOpt)
                for (int i = 0; i < eOpt.ToOptimize.Count; i++)
                {
                    eOpt.ToOptimize[i].GenerateLODParameters();
                }

            optimizer.EditorUpdate();

            //optimizer.DetectionRadius = data.scale * scaleBias;

            if (g.CullingMode == OptimizerGroupSettings.EOGCullingMode.RaycastsPerObject)
            {
                //optimizer.DOTSFindMeshes();
                optimizer.UseDOTS = false;
                optimizer.UseObstacleDetection = true;
                optimizer.CullIfNotSee = true;
            }
            else if (g.CullingMode == OptimizerGroupSettings.EOGCullingMode.ProgressiveCulling)
            {
                optimizer.UseDOTS = true;
                optimizer.DOTSFindMeshes();
                optimizer.UseObstacleDetection = false;
                //optimizer.CullIfNotSee = true;
            }

            optimizer.TryAutoComputeDetectionShape();

            if ( optimizer.GetToOptimizeCount() <= 0)
            {
                DestroyImmediate(optimizer, true);
            }

        }
    }
}
