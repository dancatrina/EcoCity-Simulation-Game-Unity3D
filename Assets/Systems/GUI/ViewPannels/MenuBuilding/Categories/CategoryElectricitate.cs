using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class CategoryElectricitate : View , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;

    [Header("Detalis")]
    public GameObject detaliFereastra;
    public UiBuildingInfoElectricitate UiScriptInfo;
    private DetaliiElectricitate containerFereastra;
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
                UiBuildingInfoElectricitate current = element.GetComponent<UiBuildingInfoElectricitate>();
                if (current != null)
                {
                    ABuilding building = new BuildingElectricitate(
                        current.numarMaximAngajati, 
                        current.numarCurentAngajati,
                        current.numarCataElectricitatePoateProduce, 
                        current.totalElectricitateProdusa,
                        current.taxaCladire);


                   
                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(current.building,building,current.pret));
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
            containerFereastra.taxe.text = UiScriptInfo.taxaCladire + " M";
            containerFereastra.pret.text = UiScriptInfo.pret + " M";
            containerFereastra.catProduce.text = UiScriptInfo.numarCataElectricitatePoateProduce + " MW/pers";
            containerFereastra.descriere.text = UiScriptInfo.descriere;
            containerFereastra.titlu.text = UiScriptInfo.denumireCladire;

            isActiv = true;
            detaliFereastra.SetActive(isActiv);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        containerFereastra = detaliFereastra.GetComponent<DetaliiElectricitate>();
        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        UiScriptInfo = takeCurrentButton.GetComponent<UiBuildingInfoElectricitate>();
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
