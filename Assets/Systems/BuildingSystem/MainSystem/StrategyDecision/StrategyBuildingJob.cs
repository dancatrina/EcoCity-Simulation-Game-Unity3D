using UnityEngine;
using System;

public abstract class StrategyBuildingJob
{
    public BuildingType buildingType;
    protected EDirection direction;

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    protected BuildingSystem pointTo = BuildingSystem.getInstance();

    public Vector3 getMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Mouse3D.getMouseWorldPosition();
        pointTo.getGrid().getPositon(mousePosition, out int x, out int z);

        if (buildingType != null)
        {
            Vector2Int rotationOffset = buildingType.getRotationOffset(direction);
            Vector3 placedObjectWorldPosition = pointTo.getGrid().getWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * pointTo.getGrid().CellSize;
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Vector3 getMouseWorldSnappedPositionWithLayermask(LayerMask layerMask)
    {
        Vector3 mousePosition = Mouse3D.getMouseWorldPositionWithLayerMask(layerMask);
        pointTo.getGrid().getPositon(mousePosition, out int x, out int z);

        if (buildingType != null)
        {
            Vector2Int rotationOffset = buildingType.getRotationOffset(direction);

            Vector3 placedObjectWorldPosition = pointTo.getGrid().getWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * pointTo.getGrid().CellSize;
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion getPlacedObjectRotation()
    {
        if (buildingType != null)
        {
            return Quaternion.Euler(0, buildingType.getRotationAngle(direction), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }

    protected void refreshSelectedObjectType() => OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    protected void eventObjectPlaced() => OnObjectPlaced?.Invoke(this, EventArgs.Empty);

    protected void deselectObjectType()
    {
        buildingType = null; refreshSelectedObjectType();
    }

    public void setBuildingType(BuildingType buildingType) { this.buildingType = buildingType; }
    public abstract void doJob();

    public abstract void destroyGarbage();
}
