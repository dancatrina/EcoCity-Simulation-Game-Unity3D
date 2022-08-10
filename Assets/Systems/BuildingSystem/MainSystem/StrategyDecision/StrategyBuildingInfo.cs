using UnityEngine.EventSystems;
using UnityEngine;

public class StrategyBuildingInfo : StrategyBuildingJob
{
    ContainerUI containerUi;

    public StrategyBuildingInfo()
    {
        containerUi = ContainerUI.getInstance();
    }
    public override void destroyGarbage()
    {
    }

    public override void doJob()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                destroyGarbage();
                pointTo.setStrategyJob(new StrategyBuildingInfo());
            }
        }else if (Input.GetMouseButtonDown(0))
        {
            Building selectedBuilding = pointTo.getBuildingWithRayCast();
            if(selectedBuilding != null)
            {
                containerUi.infoPanelStrategy = containerUi.giveMeRightStrategy(selectedBuilding.HoldingBuilding.getTip(), selectedBuilding.HoldingBuilding);
                containerUi.showPanelInfo();
            }
        }


        /* Version 2 - Deprecieted
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Mouse3D.getMouseWorldPosition();
            if (pointTo.getGrid().getGridObject(mousePosition) != null)
            {
                // Valid Grid Position
                Building placedObject = pointTo.getGrid().getGridObject(mousePosition).getBuilding();
                if (placedObject != null)
                {
                }
            }
        } */
    }
}
