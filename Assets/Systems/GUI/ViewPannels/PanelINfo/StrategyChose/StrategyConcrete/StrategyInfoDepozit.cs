using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyInfoDepozit : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoDepozit panelDepozit;
    private BuildingDepozit buildingDepozit;

    public StrategyInfoDepozit()
    {
        refToContainer = ContainerUI.getInstance();

        panelDepozit = refToContainer.panelDepozit;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingDepozit = (BuildingDepozit)aBuilding;
    }

    public void showInfoPanel()
    {
        panelDepozit.totalResurse.text = buildingDepozit.NumarCurentDeResurse + "/" + buildingDepozit.NumarMaximDeResurse;
        panelDepozit.consumEnergie.text = buildingDepozit.getConsumElectricitate() + " MW";
        panelDepozit.taxe.text = buildingDepozit.getTaxaCladire() + "M";
        UiManagerSingleton.getInstance().showFast(panelDepozit);
    }
}
