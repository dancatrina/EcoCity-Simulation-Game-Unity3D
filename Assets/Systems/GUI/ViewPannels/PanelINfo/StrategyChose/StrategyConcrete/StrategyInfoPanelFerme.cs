using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyInfoPanelFerme : IStrategyInfoPanel
{
    private ContainerUI refToContainer;
    private PanelInfoFerma panelFerma;
    private BuildingFerme buildingFerma;

    public StrategyInfoPanelFerme()
    {
        refToContainer = ContainerUI.getInstance();

        panelFerma = refToContainer.panelFerma;
    }

    public void setBuilding(ABuilding aBuilding)
    {
        buildingFerma = (BuildingFerme)aBuilding;
    }

    public void showInfoPanel()
    {
        if(buildingFerma != null)
        {
            panelFerma.AngajatiVal.text = buildingFerma.NumarCurentAngajati + "/" + buildingFerma.NumarMaximAngajati;
            panelFerma.venitVal.text = buildingFerma.getVenitCladire() + " M";
            panelFerma.taxeVal.text = buildingFerma.getTaxaCladire() + " M";
            panelFerma.consumEnergieVal.text = buildingFerma.getConsumElectricitate() + " MW";
            panelFerma.imgMateriePrima.sprite = refToContainer.getMateriePrimaSprite(buildingFerma.MateriePrimaCatProduceSiCe.TipMateriaPrima);
            panelFerma.outMateriePrima.text = buildingFerma.MateriePrimaCatProduceSiCe.CantitateProdus + "";
            panelFerma.totalVal.text = buildingFerma.NumarTotalDeMateriePrimaCreata + "";
            UiManagerSingleton.getInstance().showFast(panelFerma);
        }
    }
}
