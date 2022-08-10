using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class CategoryComercial : View , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;

    [Header("Detalis")]
    public GameObject detaliFereastra;
    public UiBuildingInfoComercial UiScriptInfo;
    private DetaliiComercial containerFereastra;
    public int distantaFataDeButon;

    bool isActiv = false;
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
                UiBuildingInfoComercial current = element.GetComponent<UiBuildingInfoComercial>();

                if (current != null)
                {
                    BuildingType currentBuilding = current.Building;

                    AProdusIndustrial produs = FactoryProduse.creazaProdus(current.tipProdus, current.cantitateNecesareMagazinuluiDeAVinde);
                    IBuilder builder = new BuilderComert()
                    .setVenitCladire(current.venitCladire)
                    .setTaxaCladire(current.taxaCladire)
                    .setConsumElectricitate(current.consumElectricitate)
                    .setCantitateNecesaraMagazinuluiDeVanzare(produs)
                    .setNumarMaximAngajati(current.numarMaximAngajati)
                    .setNumarCurentAngajati(current.numarCurentAngajati)
                    .setNumarTotalDeProduseVandute(current.numarTotalDeProduseVandute);

                    ABuilding createdBuilding = builder.build();



                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(currentBuilding, createdBuilding,current.pret));
                    turnOff.SetActive(false);
                    Destroy();
                    Hide();
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
            containerFereastra.ceProdusConsuma.text = UiScriptInfo.cantitateNecesareMagazinuluiDeAVinde + "";
            containerFereastra.imagineConsumProdus.sprite = ContainerUI.getInstance().getProdusSprite(UiScriptInfo.tipProdus);
            containerFereastra.descriere.text = UiScriptInfo.descriere;
            containerFereastra.titlu.text = UiScriptInfo.denumireCladire;

            isActiv = true;
            detaliFereastra.SetActive(isActiv);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        containerFereastra = detaliFereastra.GetComponent<DetaliiComercial>();
        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        UiScriptInfo = takeCurrentButton.GetComponent<UiBuildingInfoComercial>();
        containerFereastra.transform.position = takeCurrentButton.transform.position;
        containerFereastra.transform.position = new Vector2(containerFereastra.transform.position.x + distantaFataDeButon, containerFereastra.transform.position.y);
        FunctionTimer.Create(seteazaDetaliiCladire, 1, "df");
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
    }
}
