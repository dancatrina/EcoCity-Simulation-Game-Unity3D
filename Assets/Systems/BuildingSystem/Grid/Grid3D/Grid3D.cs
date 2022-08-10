using UnityEngine;
using System;
using CodeMonkey.Utils;


public class Grid3D
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 gridPosition;

    private GridObject[,] gridObjects;

    public event EventHandler<onChangeActionGrid> onGridObjectChanged;

    //De sters
    TextMesh[,] debugTextArray;

    public class onChangeActionGrid : EventArgs
    {
        public int x;
        public int z;
    }

    public Grid3D(int width, int height, int cellSize, Vector3 gridPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.gridPosition = gridPosition;

        gridObjects = new GridObject[width,height];

        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for(int z =0; z < gridObjects.GetLength(1); z++)
            {
                gridObjects[x, z] = new GridObject(x, z);
            }
        }

      // showDebug();
    }

    public void showDebug()
    {

        TextMesh[,] debugTextArray = new TextMesh[width, height];
        for (int x = 0; x < gridObjects.GetLength(0); x++)
        {
            for (int z = 0; z < gridObjects.GetLength(1); z++)
            {
                debugTextArray[x, z] = UtilsClass.CreateWorldText(gridObjects[x, z]?.ToString(), null, getWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 8, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                Debug.DrawLine(getWorldPosition(x, z), getWorldPosition(x, z + 1), Color.white, 20f);
                Debug.DrawLine(getWorldPosition(x, z), getWorldPosition(x + 1, z), Color.white, 20f);
            }
        }
        Debug.DrawLine(getWorldPosition(0, height), getWorldPosition(width, height), Color.white, 100f);
        Debug.DrawLine(getWorldPosition(width, 0), getWorldPosition(width, height), Color.white, 100f);

        onGridObjectChanged += (object sender, onChangeActionGrid eventArgs) => {
            debugTextArray[eventArgs.x, eventArgs.z].text = gridObjects[eventArgs.x, eventArgs.z]?.ToString();
        };

    }

    public int Width { get => width; set => width = value; }
    public int Height { get => height; set => height = value; }
    public float CellSize { get => cellSize; set => cellSize = value; }
    public Vector3 GridPosition { get => gridPosition; set => gridPosition = value; }
    public GridObject[,] GridObjects { get => gridObjects; set => gridObjects = value; }


    public void triggerGridObjectChanged(int x, int z) => onGridObjectChanged?.Invoke(this, new onChangeActionGrid { x = x, z = z });
    public Vector3 getWorldPosition(int x, int z) => new Vector3(x, 0, z) * cellSize + gridPosition;

    public void getPositon(Vector3 worldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - gridPosition).x / cellSize);
        z = Mathf.FloorToInt((worldPosition - gridPosition).z / cellSize);
    }

    public GridObject getGridObject(int x, int z) => (x >= 0 && z >= 0 && x < width && z < height) ? gridObjects[x, z] : default(GridObject);
    public GridObject getGridObject(Vector3 worldPosition)
    {
        int x, z;
        getPositon(worldPosition, out x, out z);

        return getGridObject(x, z);
    }

    public void setGridObject(int x, int z, GridObject obj)
    {
        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            gridObjects[x, z] = obj;
            triggerGridObjectChanged(x, z);
        }
    }

    public Vector2Int validateGridPosition(Vector2Int gridPosition) => new Vector2Int(Mathf.Clamp(gridPosition.x, 0, width - 1), Mathf.Clamp(gridPosition.y, 0, height - 1));


}
