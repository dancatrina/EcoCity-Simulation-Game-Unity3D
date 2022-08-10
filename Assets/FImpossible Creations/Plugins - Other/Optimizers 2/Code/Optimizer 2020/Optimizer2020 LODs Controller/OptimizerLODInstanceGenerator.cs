#if UNITY_2019_4_OR_NEWER

using UnityEngine;
using UnityEngine.AI;

namespace FIMSpace.FOptimizing
{
    public static partial class LODInstanceGenerator
    {
        public enum ESearchMode
        {
            JustUnityComponents, JustCustomComponents, AllComponents
        }

        /// <summary>
        /// Identifying LOD type for target component to optimize
        /// </summary>
        /// <param name="component"> Component which Optimizer will try optimize </param>
        /// <param name="deepSearch"> Searching ALL available component to optimize </param>
        /// <returns></returns>
        public static ILODInstance GenerateInstanceOutOf(Optimizer2 callFrom, Component component, bool deepSearch = true, ESearchMode toIdentify = ESearchMode.AllComponents)
        {
            if (OptimUtils.ShouldBeIgnored(component)) return null;

            if (toIdentify != ESearchMode.JustUnityComponents)
            {
                // Add here lines of custom components to optimize!
                // Custom Components detection START ---------------------



                // Custom Components detection END ---------------------
            }


            if (toIdentify != ESearchMode.JustCustomComponents)
            {
                if (!Optimizer_Base._editor_DragAndDropOptim)
                    if (callFrom)
                        if (callFrom.OptimizationTypes)
                        {
                            if ( callFrom.OptimizationTypes.IsTypeAllowed(component) == false)
                            {
                                return null;
                            }
                        }

                // Default Optimizer handling Unity Components to Optimize
                if (component is MeshRenderer) return GenerateInstanceOutOf(component as MeshRenderer);
                if (component is SkinnedMeshRenderer) return GenerateInstanceOutOf(component as SkinnedMeshRenderer);
                if (component is Light) return GenerateInstanceOutOf(component as Light);
                if (component is ParticleSystem) return GenerateInstanceOutOf(component as ParticleSystem);
                if (Optimizer_Base._HandleUnityLODWithReload) if (component is LODGroup) return GenerateInstanceOutOf(component as LODGroup);
                if (component is NavMeshAgent) return GenerateInstanceOutOf(component as NavMeshAgent);
                if (component is AudioSource) return GenerateInstanceOutOf(component as AudioSource);
                if (deepSearch) if (component is Rigidbody) return GenerateInstanceOutOf(component as Rigidbody);
                //if (component is Terrain) return GenerateInstanceOutOf(component as Terrain);
            }

            // At the end trying to identify mono behaviour
            if (toIdentify != ESearchMode.JustUnityComponents)
            {
                if (component is MonoBehaviour) return GenerateInstanceOutOf(component as MonoBehaviour);
            }

            return null;
        }

        public static ILODInstance GenerateInstanceOutOf(SkinnedMeshRenderer component)
        {
            return new LODI_Renderer();
        }

        public static ILODInstance GenerateInstanceOutOf(MeshRenderer component)
        {
            return new LODI_Renderer();
        }

        public static ILODInstance GenerateInstanceOutOf(Light component)
        {
            return new LODI_Light();
        }

        public static ILODInstance GenerateInstanceOutOf(ParticleSystem component)
        {
            return new LODI_ParticleSystem();
        }

        public static ILODInstance GenerateInstanceOutOf(AudioSource component)
        {
            return new LODI_AudioSource();
        }

        public static ILODInstance GenerateInstanceOutOf(MonoBehaviour component)
        {
            return new LODI_MonoBehaviour();
        }

        public static ILODInstance GenerateInstanceOutOf(Rigidbody component)
        {
            return new LODI_Rigidbody();
        }

        public static ILODInstance GenerateInstanceOutOf(Terrain component)
        {
            return new LODI_Terrain();
        }

        public static ILODInstance GenerateInstanceOutOf(LODGroup component)
        {
            return new LODI_UnityLOD();
        }

        public static ILODInstance GenerateInstanceOutOf(NavMeshAgent component)
        {
            return new LODI_NavMeshAgent();
        }
    }
}

#endif