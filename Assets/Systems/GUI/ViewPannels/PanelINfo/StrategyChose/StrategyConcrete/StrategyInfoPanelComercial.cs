using UnityEngine;

public class StrategyInfoPanelComercial : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoComercial panelComert;

    private BuildingComercial buildingComert;

    private Sprite inSprite;

    public StrategyInfoPanelComercial()
    {
        refToContainer = ContainerUI.getInstance();
        panelComert = refToContainer.panelComercial;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingComert = (BuildingComercial)aBuilding;
    }

    public void showInfoPanel()
    {
         if(buildingComert != null)
        {
            inSprite = refToContainer.getProdusSprite(buildingComert.CantitateNecesareMagazinuluiDeAVinde.getTipProdus());
            panelComert.imgProdus.sprite = inSprite;
            panelComert.AngajatiVal.text = buildingComert.NumarCurentAngajati + "/" + buildingComert.NumarMaximAngajati;
            panelComert.consumEnergieVal.text = buildingComert.getConsumElectricitate() + " MW";
            panelComert.venitVal.text = buildingComert.getVenitCladire() + " M";
            panelComert.taxeVal.text = buildingComert.getTaxaCladire() + " M";
            panelComert.inProdus.text = buildingComert.CantitateNecesareMagazinuluiDeAVinde.getCantitateProdus() + " buc";
       
            panelComert.totalVal.text = buildingComert.NumarTotalDeProduseVandute + "";
            UiManagerSingleton.getInstance().showFast(panelComert);
        }
    }
}
