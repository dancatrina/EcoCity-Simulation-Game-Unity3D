using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerUI : MonoBehaviour
{
    private static ContainerUI instance;

    [Header("PanelsInfo")]
    public IStrategyInfoPanel infoPanelStrategy;

    public StrategyInfoPanelIndustrie strategyInfoIndustrie;
    public StrategyInfoPanelLocuinte strategyInfoLocuinte;
    public StrategyInfoPanelComercial strategyInfoComercial;
    public StrategyInfoDepozit strategyInfoDepozit;
    public StrategyInfoPanelElectricitate strategyInfoElectricitate;
    public StrategyInfoPanelSanatate strategyInfoSanatate;
    public StrategyInfoPanelReligie strategyInfoReligie;
    public StrategyInfoPanelPublic strategyInfoPublic;
    public StrategyInfoPanelFerme strategyInfoFerme;

    public PanelInfoIndustrie panelIndustrie;
    public PanelInfoLocuinte panelLocuinte;
    public PanelInfoComercial panelComercial;
    public PanelInfoDepozit panelDepozit;
    public PanelInfoElectricitate panelElectricitate;
    public PanelInfoSanatate panelSanatate;
    public PanelInfoReligie panelReligie;
    public PanelInfoPublic panelPublic;
    public PanelInfoFerma panelFerma;


    [Header("ImagesContainer")]
    public List<ScriptableMateriiPrime> imagesMateriiPrime;
    public List<ScriptableProduse> imagesProduseList;

    public Sprite getMateriePrimaSprite(EMateriePrima tip)
    {
        foreach (ScriptableMateriiPrime el in imagesMateriiPrime)
        {
            if (el.materiePrima == tip) return el.imagineMateriePrima;
        }
        return null;
    }

    public Sprite getProdusSprite(EProdusIndustrial tip)
    {
        foreach (ScriptableProduse el in imagesProduseList)
        {
            if (el.prodIndustrial == tip) return el.imageProdus;
        }
        return null;
    }

    public static ContainerUI getInstance() => instance;

    //UI
    public delegate void refreshUIPanel();
    public event refreshUIPanel sendSignal;

    private void Start()
    {
        instance = this;
        infoPanelStrategy = null;
        //Manage pannels;

        panelIndustrie = UiManagerSingleton.getView<PanelInfoIndustrie>();
        panelLocuinte = UiManagerSingleton.getView<PanelInfoLocuinte>();
        panelComercial = UiManagerSingleton.getView<PanelInfoComercial>();
        panelDepozit = UiManagerSingleton.getView<PanelInfoDepozit>();
        panelElectricitate = UiManagerSingleton.getView<PanelInfoElectricitate>();
        panelSanatate = UiManagerSingleton.getView<PanelInfoSanatate>();
        panelReligie = UiManagerSingleton.getView<PanelInfoReligie>();
        panelPublic = UiManagerSingleton.getView<PanelInfoPublic>();
        panelFerma = UiManagerSingleton.getView<PanelInfoFerma>();

        //Create Strategys;
        strategyInfoIndustrie = new StrategyInfoPanelIndustrie();
        strategyInfoLocuinte = new StrategyInfoPanelLocuinte();
        strategyInfoComercial = new StrategyInfoPanelComercial();
        strategyInfoDepozit = new StrategyInfoDepozit();
        strategyInfoElectricitate = new StrategyInfoPanelElectricitate();
        strategyInfoSanatate = new StrategyInfoPanelSanatate();
        strategyInfoReligie = new StrategyInfoPanelReligie();
        strategyInfoPublic = new StrategyInfoPanelPublic();
        strategyInfoFerme = new StrategyInfoPanelFerme();

        sendSignal += showPanelInfo;
    }

    public IStrategyInfoPanel giveMeRightStrategy(ABuilding.tipCladire tip, ABuilding building)
    {
        if (tip == ABuilding.tipCladire.INDUSTRIE)
        {
            infoPanelStrategy = strategyInfoIndustrie;
        }else if(tip == ABuilding.tipCladire.LOCUINTA)
        {
            infoPanelStrategy = strategyInfoLocuinte;
        }else if(tip == ABuilding.tipCladire.COMERCIAL)
        {
            infoPanelStrategy = strategyInfoComercial;
        }else if( tip == ABuilding.tipCladire.DEPOZIT)
        {
            infoPanelStrategy = strategyInfoDepozit;
        }else if( tip == ABuilding.tipCladire.ELECTRICITATE)
        {
            infoPanelStrategy = strategyInfoElectricitate;
        }else if( tip == ABuilding.tipCladire.SANATATE)
        {
            infoPanelStrategy = strategyInfoSanatate;
        }else if (tip == ABuilding.tipCladire.RELIGIE)
        {
            infoPanelStrategy = strategyInfoReligie;
        }else if( tip == ABuilding.tipCladire.PUBLIC)
        {
            infoPanelStrategy = strategyInfoPublic;
        }else if (tip == ABuilding.tipCladire.FERMA)
        {
            infoPanelStrategy = strategyInfoFerme;
        }else if(tip == ABuilding.tipCladire.Strada)
        {
            infoPanelStrategy = null;
        }
        if (infoPanelStrategy != null)
        {
            infoPanelStrategy.setBuilding(building);
        }
        return infoPanelStrategy;
    }

    public void showPanelInfo()
    {
        if(infoPanelStrategy != null)
        {
            infoPanelStrategy.showInfoPanel();
        }
    }

    public void invokeEvent()
    {
        sendSignal();
    }
}
