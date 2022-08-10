using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class CategoryLocuinte : View , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;

    [Header("Detalis")]
    public GameObject detaliFereastra;
    public UiBuildingInfoLocuinta UiScriptInfo;
    private DetallLocuinta containerFereastra;
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
        foreach(var elem in categories)
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
                UiBuildingInfoLocuinta current = element.GetComponent<UiBuildingInfoLocuinta>();
                if (current != null)
                {
                    ABuilding currentBuilding = new BuildingLocuinta(current.numarMaximLocatari,
                        current.numarCurentLocatari, 0, current.taxaCladire, current.consumElectricitate, current.venitCladire,ABuilding.tipCladire.LOCUINTA);

                  

                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(current.building,currentBuilding, current.pret));
                    turnOff.SetActive(false);
                    Destroy();
                    Hide();
                }
            });
        }
    }

    public void seteazaDetaliiCladire()
    {
        if(UiScriptInfo != null)
        {
            containerFereastra.locuitori.text = UiScriptInfo.numarMaximLocatari + "";
            containerFereastra.consumEnergie.text = UiScriptInfo.consumElectricitate + " MW";
            containerFereastra.taxe.text = UiScriptInfo.taxaCladire + " M";
            containerFereastra.titlu.text = UiScriptInfo.denumireCladire;
            containerFereastra.pret.text = UiScriptInfo.pret + " M";
            containerFereastra.descriere.text = UiScriptInfo.descriere;

            isActiv = true;
            detaliFereastra.SetActive(isActiv);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        containerFereastra = detaliFereastra.GetComponent<DetallLocuinta>();
        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        UiScriptInfo = takeCurrentButton.GetComponent<UiBuildingInfoLocuinta>();
        containerFereastra.transform.position = takeCurrentButton.transform.position;
        containerFereastra.transform.position = new Vector2(containerFereastra.transform.position.x + distantaFataDeButon,containerFereastra.transform.position.y);
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
