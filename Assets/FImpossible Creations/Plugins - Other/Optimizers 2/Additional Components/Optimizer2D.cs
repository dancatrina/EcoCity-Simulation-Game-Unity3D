using System.Collections.Generic;
using UnityEngine;

namespace FIMSpace.FOptimizing
{
    /// <summary>
    /// FC: Essential Optimizer is holding optimization data within serialized MonoBehaviour 
    /// instead of ScriptableObjects giving better flexibility but not allowing for custom
    /// optimization types implementation, unless it can be done through inheriting new monobehaviour from this class
    /// </summary>
    [AddComponentMenu("FImpossible Creations/2D Optimizer", 2)]
    public partial class Optimizer2D : EssentialOptimizer, UnityEngine.EventSystems.IDropHandler, IFHierarchyIcon
    {
        #region Hierarchy Icon

        new public string EditorIconPath { get { if (PlayerPrefs.GetInt("OptH", 1) == 0) return ""; else return "FIMSpace/Optimizers 2/OptEsIconSmall"; } }
        new public void OnDrop(UnityEngine.EventSystems.PointerEventData data) { }

        #endregion

        protected override void Reset()
        {
            LODLevels = 1;
            base.Reset();
            MaxDistance = 25;
        }

        protected override void Start()
        {
            DetectionBounds.z = 1f;
            DetectionOffset.z = 0f;
            base.Start();
        }

        public override float GetReferenceDistance()
        {
            return Vector2.Distance(PreviousPosition, LastDynamicCheckCameraPosition);
        }

        public override Vector3 GetReferencePosition()
        {
            Vector3 refPos = transform.position; refPos.z = OptimizersManager.MainCamera.transform.position.z;
            return refPos;
        }

        public override void OnValidate()
        {
            OptimizingMethod = EOptimizingMethod.Dynamic;
            base.OnValidate();
        }
    }
}