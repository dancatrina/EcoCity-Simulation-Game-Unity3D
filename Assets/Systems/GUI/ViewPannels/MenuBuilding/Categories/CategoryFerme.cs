using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;


public class CategoryFerme : View , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;

    [Header("Detalis")]
    public GameObject detaliFereastra;
    public UiBuildingInfoFerme UiScriptInfo;
    private DetaliiFerme containerFereastra;
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

    public void registerEvents() {
        foreach (var element in categories)
        {
            element.onClick.AddListener(() =>
            {
                UiBuildingInfoFerme current = element.GetComponent<UiBuildingInfoFerme>();
                if (current != null)
                {
                    AProdusFerma produs = FactoryMateriiPrime.creazaMateriePrima(current.tipMaterie, current.catProduceMateriePrima);

                    IBuilder builderFerma = new BuilderFerme()
                     .setNumarMaximAngajati(current.numarMaximAngajati)
                     .setNumarCurentAngajati(current.numarCurentAngajati)
                     .setCantitateProdusaDeFerma(produs)
                     .setVenitCladire(current.venitCladire)
                     .setConsumElectricitate(current.consumElectricitate)
                     .setTaxaCladire(current.taxaCladire)
                     .setNumarTotalDeMateriiProduse(current.numarTotalPodus);
                   
                    ABuilding building = builderFerma.build();

                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(current.building,building, current.pret));
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
            containerFereastra.catProduce.text = UiScriptInfo.catProduceMateriePrima + "/pers";
            containerFereastra.imagineMateriePrima.sprite = ContainerUI.getInstance().getMateriePrimaSprite(UiScriptInfo.tipMaterie);
            containerFereastra.descriere.text = UiScriptInfo.descriere;
            containerFereastra.titlu.text = UiScriptInfo.denumireCladire;

            isActiv = true;
            detaliFereastra.SetActive(isActiv);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        containerFereastra = detaliFereastra.GetComponent<DetaliiFerme>();
        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        UiScriptInfo = takeCurrentButton.GetComponent<UiBuildingInfoFerme>();
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
