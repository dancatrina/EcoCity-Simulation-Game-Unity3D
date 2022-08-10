using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{
    public static Mouse3D Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public static Vector3 getMouseWorldPosition() => Instance.getMouseWorldPosition_Instance();
    public static Vector3 getMouseWorldPositionWithLayerMask(LayerMask layerMask) => Instance.getMouseWorldPositionWithLayerMask_Instance(layerMask);


    private Vector3 getMouseWorldPosition_Instance()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
    private Vector3 getMouseWorldPositionWithLayerMask_Instance(LayerMask layerMask)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f,layerMask))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

}

