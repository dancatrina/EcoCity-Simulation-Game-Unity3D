#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FM: Class to solve many troubles with different unity versions to use not shared (unique) settings
    /// in all cases (isolated scene prefab mode / creating prefabs from scene etc.)
    /// Class with methods supporting versions after unity 2018.3
    /// </summary>
    public static partial class Optimizers_LODTransport
    {

#if UNITY_2018_3_OR_NEWER ///////////////////////////////////////////////

        public static Object GetPrefab(GameObject target)
        {
            Object prefab = null;


            if (target.gameObject)
            {
                if (PrefabUtility.IsPartOfAnyPrefab(target.gameObject)) // If it's related to any prefab
                    if (!PrefabUtility.IsPartOfPrefabInstance(target.gameObject)) // If it is project asset prefab
                    {
                        if (PrefabUtility.GetNearestPrefabInstanceRoot(target.gameObject)) // Nesting support
                        {
                            prefab = PrefabUtility.GetNearestPrefabInstanceRoot(target.gameObject);
                        }
                        else
                        {
                            prefab = target.gameObject;
                        }
                    }
                    else // If it is instance of prefab
                    {
                        if (PrefabUtility.GetNearestPrefabInstanceRoot(target.gameObject))  // Nesting support
                        {
                            prefab = PrefabUtility.GetNearestPrefabInstanceRoot(target.gameObject);
                        }
                        else
                            prefab = PrefabUtility.GetCorrespondingObjectFromSource(target.gameObject);
                    }
            }
            else
                Debug.LogError("[OPTIMIZERS EDITOR] No Game Object inside lods controller!");

            if (prefab)
                if (!AssetDatabase.Contains(prefab))
                    return null; // It's not in assets database?

            return prefab;
        }

        public static GameObject GetProjectPrefabSimple(Object reference)
        {
            GameObject prefab = null;

            if (reference)
            {
                string path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(reference);
                //if (path.Length > 1)
                //{
                //prefab = PrefabUtility.GetNearestPrefabInstanceRoot(reference);
                prefab = (GameObject)AssetDatabase.LoadMainAssetAtPath(path);

                //prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                //EditorUtility.ClearDirty(prefab);
                if (PrefabUtility.GetPrefabAssetType(reference) == PrefabAssetType.Model) prefab = null;
                if (PrefabUtility.GetPrefabAssetType(reference) == PrefabAssetType.Variant) prefab = null;
                //}
                //else prefab = null;
            }

            return prefab;
        }

        public static string GetProjectPrefabPath(Object reference)
        {
            string path = string.Empty;
            if (reference) path = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(reference);
            return path;
        }

        public static GameObject GetPrefabRootObject(Object reference)
        {
            GameObject prefab = null;

            if (reference)
            {
                prefab = PrefabUtility.GetOutermostPrefabInstanceRoot(reference);
            }

            return prefab;
        }


#endif ///////////////////////////////////////////////

    }
}
#endif
