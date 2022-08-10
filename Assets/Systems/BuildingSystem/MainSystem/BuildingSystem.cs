using System;
using UnityEngine;
using System.Collections.Generic;
using CodeMonkey.Utils;

public class BuildingSystem : MonoBehaviour
{
    private static BuildingSystem instance;

    [SerializeField] private int gridHeight;
    [SerializeField] private int gridWidth;
    [SerializeField] private int cellSize;

    private StrategyBuildingJob strategyJob;

    private Grid3D grid;

    public int GridHeight { get => gridHeight; set => gridHeight = value; }
    public int GridWidth { get => gridWidth; set => gridWidth = value; }
    public int CellSize { get => cellSize; set => cellSize = value; }

    private void Awake()
    {
        instance = this;
        grid = new Grid3D(gridWidth,
            gridHeight,
            cellSize,
            new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y, 
                gameObject.transform.position.y));

        strategyJob = new StrategyBuildingInfo();

    }


    private void Update() {
        if (strategyJob != null)
        {
            strategyJob.doJob();
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            strategyJob.destroyGarbage();
            strategyJob = new StrategyBuildingInfo();
        }
    
    }

    public static BuildingSystem getInstance() { return instance; }
    public Grid3D getGrid() { return grid; }

    public void setStrategyJob(StrategyBuildingJob strategyBuild) { strategyJob = strategyBuild; }

    public Vector2Int getGridPosition(Vector3 worldPosition)
    {
        grid.getPositon(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Building getBuildingWithRayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f))
        {
            return raycastHit.transform.GetComponentInParent<Building>();
        }
        return null;
    }

}
