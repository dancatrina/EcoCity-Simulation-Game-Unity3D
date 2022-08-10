
public class StrategyInfoPanelSanatate : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoSanatate panelSanatate;
    private BuildingSanatate buildingSanatate;

    public StrategyInfoPanelSanatate()
    {
        refToContainer = ContainerUI.getInstance();

        panelSanatate = refToContainer.panelSanatate;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingSanatate = (BuildingSanatate)aBuilding;

    }

    public void showInfoPanel()
    {
        if(buildingSanatate != null)
        {
            panelSanatate.AngajatiVal.text = buildingSanatate.NumarCurentAngajati + "/" + buildingSanatate.NumarMaximAngajati;
            panelSanatate.venitVal.text = buildingSanatate.getVenitCladire() + " M";
            panelSanatate.taxeVal.text = buildingSanatate.getTaxaCladire() + " M";
            panelSanatate.consumEnergieVal.text = buildingSanatate.getConsumElectricitate() + " MW";
            panelSanatate.totalNounascutiVal.text = buildingSanatate.NouNascutiTotal + "";
            UiManagerSingleton.getInstance().showFast(panelSanatate);
        }
    }
}
