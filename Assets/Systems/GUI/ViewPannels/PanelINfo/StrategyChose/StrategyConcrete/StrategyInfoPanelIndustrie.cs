using UnityEngine;


public class StrategyInfoPanelIndustrie : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoIndustrie panelIndustrie;
    
    private BuildingIndustrie buildingIndustrie;

    private Sprite inSprite;
    private Sprite outSprite;

    public StrategyInfoPanelIndustrie()
    {
        refToContainer = ContainerUI.getInstance();

        panelIndustrie = refToContainer.panelIndustrie;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingIndustrie = (BuildingIndustrie)aBuilding;
    }

    public void showInfoPanel()
    {
        if(buildingIndustrie != null)
        {
            inSprite = refToContainer.getMateriePrimaSprite(buildingIndustrie.CantitateNecesaraPentruProducere.TipMateriaPrima);
            outSprite = refToContainer.getProdusSprite(buildingIndustrie.CantitateaProdusaDeIndustrie.getTipProdus());
            panelIndustrie.iconIn.sprite = inSprite;
            panelIndustrie.iconOut.sprite = outSprite;
            panelIndustrie.muncitoriVal.text = buildingIndustrie.NumarCurentAngajati + "/" + buildingIndustrie.NumarMaximAngajati;
            panelIndustrie.consumEnergieVal.text = buildingIndustrie.getConsumElectricitate() + " MW";
            panelIndustrie.venitVal.text = buildingIndustrie.getVenitCladire() + " M";
            panelIndustrie.taxeVal.text = buildingIndustrie.getTaxaCladire() + " M";
            panelIndustrie.inVal.text = buildingIndustrie.CantitateNecesaraPentruProducere.CantitateProdus + " buc";
            panelIndustrie.outVal.text = buildingIndustrie.CantitateaProdusaDeIndustrie.getCantitateProdus() + " buc";
            panelIndustrie.totalVal.text = buildingIndustrie.NumarTotalDeProduseFabricate + "";
            UiManagerSingleton.getInstance().showFast(panelIndustrie);
        }
    }
}
