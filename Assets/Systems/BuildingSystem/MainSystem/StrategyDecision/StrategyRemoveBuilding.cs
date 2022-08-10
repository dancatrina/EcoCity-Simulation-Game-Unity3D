using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class StrategyRemoveBuilding : StrategyBuildingJob
{
    public StrategyRemoveBuilding()
    {
    }

    public override void destroyGarbage() {}

    public override void doJob()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                destroyGarbage();
                pointTo.setStrategyJob(new StrategyBuildingInfo());
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Building selectedBuilding = pointTo.getBuildingWithRayCast();
                if (selectedBuilding != null)
                {
                    EconomyManager.getInstance().removeBuilding(selectedBuilding.HoldingBuilding);
                    List<Vector2Int> gridPositionList = selectedBuilding.OccupiedPositions;
                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        pointTo.getGrid().getGridObject(gridPosition.x, gridPosition.y).removeBuilding();
                    }

                    selectedBuilding.destroySelf();
                }
            }
        }
        /* Version 2 - Deprecieted hard to calculate
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Mouse3D.getMouseWorldPosition();
            Debug.Log(mousePosition);
            if (pointTo.getGrid().getGridObject(mousePosition) != null)
            {
                // Valid Grid Position
                Building placedObject = pointTo.getGrid().getGridObject(mousePosition).getBuilding();
                if (placedObject != null)
                {
                    List<Vector2Int> gridPositionList = placedObject.OccupiedPositions;
                    placedObject.destroySelf();


                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        pointTo.getGrid().getGridObject(gridPosition.x, gridPosition.y).removeBuilding();
                    }
                }
            }
        } */
    }
}
