using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif

namespace FIMSpace.FOptimizing
{

    public partial class EssentialLODsController
    {
        private List<ILODInstance> _iflod;
        public List<ILODInstance> GetIFLODsForOptimizer2() { return GetIFLODList(); }

        /// <summary>
        /// Getting list of interface type for quicker assign stuff in loops
        /// List of interface can't be serialized by unity so we temporarily creating IFLOD list for active use
        /// </summary>
        protected override List<ILODInstance> GetIFLODList()
        {
            if (_iflod != null)
            {
                if (_iflod.Count == eOptimizer.LODLevels + 2) return _iflod;
            }

            _iflod = new List<ILODInstance>();

            switch (ControlerType)
            {
                case EEssType.Particle:
                    for (int i = 0; i < LODs_Particle.Count; i++) _iflod.Add(LODs_Particle[i]);
                    break;
                case EEssType.Light:
                    for (int i = 0; i < LODs_Light.Count; i++) _iflod.Add(LODs_Light[i]);
                    break;
                case EEssType.MonoBehaviour:
                    for (int i = 0; i < LODs_Mono.Count; i++) _iflod.Add(LODs_Mono[i]);
                    break;
                case EEssType.Renderer:
                    for (int i = 0; i < LODs_Renderer.Count; i++) _iflod.Add(LODs_Renderer[i]);
                    break;
                case EEssType.NavMeshAgent:
                    for (int i = 0; i < LODs_NavMesh.Count; i++) _iflod.Add(LODs_NavMesh[i]);
                    break;
                case EEssType.AudioSource:
                    for (int i = 0; i < LODs_Audio.Count; i++) _iflod.Add(LODs_Audio[i]);
                    break;
                case EEssType.Rigidbody:
                    for (int i = 0; i < LODs_Rigidbody.Count; i++) _iflod.Add(LODs_Rigidbody[i]);
                    break;
                case EEssType.LODGroup:
                    for (int i = 0; i < LODs_LODGroup.Count; i++) _iflod.Add(LODs_LODGroup[i]);
                    break;

                    //case EEssType.Particle:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_Particle[i]);
                    //    break;
                    //case EEssType.Light:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_Light[i]);
                    //    break;
                    //case EEssType.MonoBehaviour:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_Mono[i]);
                    //    break;
                    //case EEssType.Renderer:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_Renderer[i]);
                    //    break;
                    //case EEssType.NavMeshAgent:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_NavMesh[i]);
                    //    break;
                    //case EEssType.AudioSource:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_Audio[i]);
                    //    break;
                    //case EEssType.Rigidbody:
                    //    for (int i = 0; i < optimizer.LODLevels + 2; i++) _iflod.Add(LODs_Rigidbody[i]);
                    //    break;
            }

            return _iflod;
        }


        protected override void GenerateNewLODSettings()
        {
            if (ControlerType == EEssType.Unknown) { UnityEngine.Debug.Log("[Optimizers] Unknown to optimize type!"); return; }

            switch (ControlerType)
            {
                case EEssType.Particle: LODs_Particle = new List<LODI_ParticleSystem>(); break;
                case EEssType.Light: LODs_Light = new List<LODI_Light>(); break;
                case EEssType.MonoBehaviour: LODs_Mono = new List<LODI_MonoBehaviour>(); break;
                case EEssType.Renderer: LODs_Renderer = new List<LODI_Renderer>(); break;
                case EEssType.NavMeshAgent: LODs_NavMesh = new List<LODI_NavMeshAgent>(); break;
                case EEssType.AudioSource: LODs_Audio = new List<LODI_AudioSource>(); break;
                case EEssType.Rigidbody: LODs_Rigidbody = new List<LODI_Rigidbody>(); break;
                case EEssType.LODGroup: LODs_LODGroup = new List<LODI_UnityLOD>(); break;
            }
        }


        /// <summary>
        /// Generating initial settings empty instance
        /// </summary>
        private void GenerateInitialSettings()
        {
            switch (ControlerType)
            {
                case EEssType.Particle: Ini_Particle = new LODI_ParticleSystem(); break;
                case EEssType.Light: Ini_Light = new LODI_Light(); break;
                case EEssType.MonoBehaviour: Ini_Mono = new LODI_MonoBehaviour(); break;
                case EEssType.Renderer: Ini_Rend = new LODI_Renderer(); break;
                case EEssType.NavMeshAgent: Ini_Nav = new LODI_NavMeshAgent(); break;
                case EEssType.AudioSource: Ini_Audio = new LODI_AudioSource(); break;
                case EEssType.Rigidbody: Ini_Rigidbody = new LODI_Rigidbody(); break;
                case EEssType.LODGroup: Ini_LODGroup = new LODI_UnityLOD(); break;
            }
        }


        private ILODInstance GenerateInstance()
        {
            switch (ControlerType)
            {
                case EEssType.Particle: return new LODI_ParticleSystem();
                case EEssType.Light: return new LODI_Light();
                case EEssType.MonoBehaviour: return new LODI_MonoBehaviour();
                case EEssType.Renderer: return new LODI_Renderer();
                case EEssType.NavMeshAgent: return new LODI_NavMeshAgent();
                case EEssType.AudioSource: return new LODI_AudioSource();
                case EEssType.Rigidbody: return new LODI_Rigidbody();
                case EEssType.LODGroup: return new LODI_UnityLOD();
            }

            return null;
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
                switch (ControlerType)
                {
                    case EEssType.Particle:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_Particle.Add(new LODI_ParticleSystem());
                        break;
                    case EEssType.Light:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_Light.Add(new LODI_Light());
                        break;
                    case EEssType.MonoBehaviour:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_Mono.Add(new LODI_MonoBehaviour());
                        break;
                    case EEssType.Renderer:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_Renderer.Add(new LODI_Renderer());
                        break;
                    case EEssType.NavMeshAgent:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_NavMesh.Add(new LODI_NavMeshAgent());
                        break;
                    case EEssType.AudioSource:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_Audio.Add(new LODI_AudioSource());
                        break;
                    case EEssType.Rigidbody:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_Rigidbody.Add(new LODI_Rigidbody());
                        break;
                    case EEssType.LODGroup:
                        for (int i = 0; i < optimizer.LODLevels + 2; i++) LODs_LODGroup.Add(new LODI_UnityLOD());
                        break;
                }
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

    }

}
