public class StrategyInfoPanelReligie : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoReligie panelReligie;
    private BuildingBiserica buildingReligie;

    public StrategyInfoPanelReligie()
    {
        refToContainer = ContainerUI.getInstance();

        panelReligie = refToContainer.panelReligie;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingReligie = (BuildingBiserica)aBuilding;
    }

    public void showInfoPanel()
    {
        if(buildingReligie != null)
        {
            panelReligie.AngajatiVal.text = buildingReligie.NumarCurentAngajati + "/" + buildingReligie.NumarMaximAngajati;
            panelReligie.consumEnergieVal.text = buildingReligie.getConsumElectricitate() + " MW";
            panelReligie.venitVal.text = buildingReligie.getVenitCladire() + " M";
            panelReligie.taxeVal.text = buildingReligie.getTaxaCladire() + " M";
            UiManagerSingleton.getInstance().showFast(panelReligie);
        }
    }


}
