using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyInfoPanelElectricitate : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoElectricitate panelElectricitate;
    private BuildingElectricitate buildingElectricitate;

    public StrategyInfoPanelElectricitate()
    {
        refToContainer = ContainerUI.getInstance();

        panelElectricitate = refToContainer.panelElectricitate;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingElectricitate = (BuildingElectricitate)aBuilding;
    }

    public void showInfoPanel()
    {
        if(buildingElectricitate != null)
        {
            panelElectricitate.AngajatiVal.text = buildingElectricitate.NumarCurentAngajati + "/" + buildingElectricitate.NumarMaximAngajati;
            panelElectricitate.taxeVal.text = buildingElectricitate.getTaxaCladire() + " M";
            panelElectricitate.outElectricitate.text = buildingElectricitate.NumarCataElectricitatePoateProduce + " MW";
            panelElectricitate.totalVal.text = buildingElectricitate.TotalElectricitateProdusa + " MW";
            UiManagerSingleton.getInstance().showFast(panelElectricitate);
        }
    }
}
