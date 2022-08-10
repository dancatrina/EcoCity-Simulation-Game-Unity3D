using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class CategoryIndustrial : View , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;

    [Header("Detalis")]
    public GameObject detaliFereastra;
    public GameObject detaliFereastraDepozit;

    public UiBuildingInfoIndustrie UiScriptInfo;
    public UiBuildingDepozit UiScriptInfoDepozit;

    private DetaliiIndustrie containerFereastra;
    private DetaliiDepozit containerFereastraDepozit;
    public int distantaFataDeButon;

    bool isActiv = false;
    bool isActivDepozit = false;
    public override void Initialize()
    {
    }

    public void OnEnable()
    {
        registerEvents();
    }

    public void OnDisable()
    {
        foreach (var elem in categories)
        {
            elem.onClick.RemoveAllListeners();
        }
    }



    private void registerEvents()
    {
        foreach (var element in categories)
        {
            element.onClick.AddListener(() =>
            {
                UiBuildingInfoIndustrie current = element.GetComponent<UiBuildingInfoIndustrie>();
                if (current != null)
                {
                    AProdusFerma materiePrima = FactoryMateriiPrime.creazaMateriePrima(current.tipMateriePrimaIn, current.cantitateMateriePrimaNecesaraProductie);
                    AProdusIndustrial produsIndustrialDeCreat = FactoryProduse.creazaProdus(current.tipProdusIndustrial, current.cantitateProdusaInUrmaProcesariiMaterialelor);

                    IBuilder builder = new BuilderIndustrie()
                    .setTaxaCladire(current.taxaCladire)
                    .setConsumElectricitate(current.consumElectricitate)
                    .setVenitCladire(current.venitCladire)
                    .setNumarMaximAngajati(current.numarMaximAngajati)
                    .setNumarCurentAngajati(current.numarCurentAngajati)
                    .setNumarTotalDeProduseFabricate(current.numarTotalDeProduseFabricate)
                    .setCantitateNecesaraPentruProducere(materiePrima)
                    .setCantitateaProdusaDeIndustrie(produsIndustrialDeCreat)
                    .setDescriereCladire(current.descriere);

                    ABuilding createdInfoBuilding = builder.build();
                   

                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(current.building, createdInfoBuilding, current.pret));
                    turnOff.SetActive(false);
                    Destroy();
                    Hide();
                }
                else
                {
                    UiBuildingDepozit depozit = element.GetComponent<UiBuildingDepozit>();
                    if (depozit != null)
                    {
                        ABuilding building = new BuildingDepozit(depozit.numarMaximDeResurse, depozit.numarCurentDeResurse
                            , depozit.consumElectricitate, depozit.taxaCladire);

                      

                        BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(depozit.building, building,depozit.pret));
                        turnOff.SetActive(false);
                        Destroy();
                        Hide();
                    }
                }
            });
        }
    }

    public void seteazaDetaliiCladire()
    {
        if (UiScriptInfo != null)
        {
            containerFereastra.angajati.text = UiScriptInfo.numarMaximAngajati + "";
            containerFereastra.consumEnergie.text = UiScriptInfo.consumElectricitate + " MW";
            containerFereastra.taxe.text = UiScriptInfo.taxaCladire + " M";
            containerFereastra.pret.text = UiScriptInfo.pret + " M";
            containerFereastra.materiePrimaNecesara.text = UiScriptInfo.cantitateMateriePrimaNecesaraProductie + "";
            containerFereastra.imagineMateriePrima.sprite = ContainerUI.getInstance().getMateriePrimaSprite(UiScriptInfo.tipMateriePrimaIn);
            containerFereastra.descriere.text = UiScriptInfo.descriere;
            containerFereastra.titlu.text = UiScriptInfo.denumireCladire;
            containerFereastra.produsCreat.text = UiScriptInfo.cantitateProdusaInUrmaProcesariiMaterialelor + "/pers";
            containerFereastra.imagineProdusCreat.sprite = ContainerUI.getInstance().getProdusSprite(UiScriptInfo.tipProdusIndustrial);

            isActiv = true;
            detaliFereastra.SetActive(isActiv);

        }
    }

    public void seteazaDetaliiCladireDepozit()
    {
        containerFereastraDepozit.consumEnergie.text = UiScriptInfoDepozit.consumElectricitate + " ";
        containerFereastraDepozit.taxe.text = UiScriptInfoDepozit.taxaCladire + " M";
        containerFereastraDepozit.pret.text = UiScriptInfoDepozit.pret + " M";
        containerFereastraDepozit.capacitate.text = UiScriptInfoDepozit.numarMaximDeResurse + "";
        containerFereastraDepozit.descriere.text = UiScriptInfoDepozit.descriere;
        containerFereastraDepozit.titlu.text = UiScriptInfoDepozit.denumireCladire;
        isActivDepozit = true;
        detaliFereastraDepozit.SetActive(isActivDepozit);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        containerFereastra = detaliFereastra.GetComponent<DetaliiIndustrie>();
        containerFereastraDepozit = detaliFereastraDepozit.GetComponent<DetaliiDepozit>();

        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        UiScriptInfo = takeCurrentButton.GetComponent<UiBuildingInfoIndustrie>();
        if(UiScriptInfo != null)
        {
            FunctionTimer.Create(seteazaDetaliiCladire, 1, "df");
            containerFereastra.transform.position = takeCurrentButton.transform.position;
            containerFereastra.transform.position = new Vector2(containerFereastra.transform.position.x + distantaFataDeButon, containerFereastra.transform.position.y);
        }

        UiScriptInfoDepozit = takeCurrentButton.GetComponent<UiBuildingDepozit>();
        if(UiScriptInfoDepozit != null)
        {
            FunctionTimer.Create(seteazaDetaliiCladireDepozit, 1, "df");
            containerFereastraDepozit.transform.position = takeCurrentButton.transform.position;
            containerFereastraDepozit.transform.position = new Vector2(containerFereastraDepozit.transform.position.x + distantaFataDeButon, containerFereastraDepozit.transform.position.y);
        }

        

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy();
    }

    void Destroy()
    {
        FunctionTimer.StopAllTimersWithName("df");
        if (isActiv == true)
        {
            isActiv = false;
            detaliFereastra.SetActive(isActiv);
        }

        if( isActivDepozit == true)
        {
            isActivDepozit = false;
            detaliFereastraDepozit.SetActive(isActiv);
        }
    }
}
