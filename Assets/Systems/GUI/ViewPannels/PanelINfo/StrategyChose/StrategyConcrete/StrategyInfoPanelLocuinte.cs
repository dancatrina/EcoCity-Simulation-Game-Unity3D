
using UnityEngine;
public class StrategyInfoPanelLocuinte : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoLocuinte panelLocuinta;
    private BuildingLocuinta buildingLocuinta;

    public StrategyInfoPanelLocuinte()
    {
        refToContainer = ContainerUI.getInstance();

        panelLocuinta = refToContainer.panelLocuinte;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingLocuinta = (BuildingLocuinta)aBuilding;
    }

    public void showInfoPanel()
    {
        panelLocuinta.LocuitoriVal.text = buildingLocuinta.getNumarCurentLocuitori() + "/" + buildingLocuinta.getNumarMaximLocuitori();
        panelLocuinta.consumEnergieVal.text = buildingLocuinta.getConsumElectricitate() + " MW";
        panelLocuinta.venitVal.text = buildingLocuinta.getVenitCladire() + " M";
        panelLocuinta.taxeVal.text = buildingLocuinta.getTaxaCladire() + " M";
        panelLocuinta.totalVal.text = buildingLocuinta.VenitTotal + " M";
        UiManagerSingleton.getInstance().showFast(panelLocuinta);
    }
}
