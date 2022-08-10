public class StrategyInfoPanelPublic : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoPublic panelPublic;
    private BuildingPublic buildingPublic;

    public StrategyInfoPanelPublic()
    {
        refToContainer = ContainerUI.getInstance();

        panelPublic = refToContainer.panelPublic;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingPublic = (BuildingPublic) aBuilding;
    }

    public void showInfoPanel()
    {
        if(buildingPublic != null)
        {
            panelPublic.angajatiVal.text = buildingPublic.NumarCurentAngajati + "/" + buildingPublic.NumarMaximAngajati;
            panelPublic.consumEnergieVal.text = buildingPublic.getConsumElectricitate() + " MW";
            panelPublic.totalPuncte.text = buildingPublic.PuncteTotalCercetare + "";
            UiManagerSingleton.getInstance().showFast(panelPublic);
        }
    }
}
