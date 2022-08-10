using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BuildingTypes")]
public class BuildingType : ScriptableObject
{
    EDirection dir;
    EBuildingType typeBuilding;

    public string nameBuilding;
    public int width;
    public int height;

    public Transform prefab;
    public Transform visual;


    public static EDirection GetNextDir(EDirection dir)
    {
        switch (dir)
        {
            default:
            case EDirection.Down: return EDirection.Left;
            case EDirection.Left: return EDirection.Up;
            case EDirection.Up: return EDirection.Right;
            case EDirection.Right: return EDirection.Down;
        }
    }


    public int getRotationAngle(EDirection dir)
    {
        switch (dir)
        {
            default:
            case EDirection.Down: return 0;
            case EDirection.Left: return 90;
            case EDirection.Up: return 180;
            case EDirection.Right: return 270;
        }
    }
    public Vector2Int getRotationOffset(EDirection dir)
    {
        switch (dir)
        {
            default:
            case EDirection.Down: return new Vector2Int(0, 0);
            case EDirection.Left: return new Vector2Int(0, width);
            case EDirection.Up: return new Vector2Int(width, height);
            case EDirection.Right: return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> getGridPositionList(Vector2Int offset, EDirection dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();
        switch (dir)
        {
            default:
            case EDirection.Down:
            case EDirection.Up:
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case EDirection.Left:
            case EDirection.Right:
                for (int x = 0; x < height; x++)
                {
                    for (int y = 0; y < width; y++)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }
        return gridPositionList;
    }

    public  Building Create(Vector3 worldPosition, Vector2Int origin, EDirection dir)
    {
        Transform placedObjectTransform = Instantiate(prefab, worldPosition, Quaternion.Euler(0, getRotationAngle(dir), 0));
        Building placedObject = placedObjectTransform.GetComponent<Building>();

        placedObject.NameBuilding = nameBuilding;
        placedObject.Width = width;
        placedObject.Height = height;
        placedObject.OccupiedPositions = getGridPositionList(origin, dir);
        placedObject.Dir = dir;

        return placedObject;
    }

    public Transform createAnimation()
    {
        return Instantiate(visual,Vector3.zero,Quaternion.identity);

    }

}
