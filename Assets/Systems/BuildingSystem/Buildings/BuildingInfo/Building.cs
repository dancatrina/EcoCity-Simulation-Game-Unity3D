using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    [SerializeField] private EDirection dir;
    [SerializeField] List<Vector2Int> occupiedPositions;

    [SerializeField] private string nameBuilding;
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private ABuilding holdingBuilding;

    public Building() {
        holdingBuilding = null;
    }

    public EDirection Dir { get => dir; set => dir = value; }
    public List<Vector2Int> OccupiedPositions { get => occupiedPositions; set => occupiedPositions = value; }
    public string NameBuilding { get => nameBuilding; set => nameBuilding = value; }
    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public ABuilding HoldingBuilding { get => holdingBuilding; set => holdingBuilding = value; }

    public override string ToString()
    {
        return "OCUPAT";
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }


}
