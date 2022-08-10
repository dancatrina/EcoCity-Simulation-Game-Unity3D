using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class CategoryReligie : View , IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;

    [Header("Detalis")]
    public GameObject detaliFereastra;
    public UiBuildingInfoReligie UiScriptInfo;
    private DetaliiReligie containerFereastra;
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
                UiBuildingInfoReligie current = element.GetComponent<UiBuildingInfoReligie>();
                if (current != null)
                {
                    ABuilding building = new BuildingBiserica(current.numarMaximAngajati,
                        current.numarCurentAngajati, current.taxaCladire, current.consumElectricitate, current.venitCladire);

                  

                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(current.Building,building, current.pret));
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
            containerFereastra.titlu.text = UiScriptInfo.denumireCladire;
            containerFereastra.pret.text = UiScriptInfo.pret + " M";
            containerFereastra.descriere.text = UiScriptInfo.descriere;

            isActiv = true;
            detaliFereastra.SetActive(isActiv);

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        containerFereastra = detaliFereastra.GetComponent<DetaliiReligie>();
        GameObject takeCurrentButton = eventData.pointerCurrentRaycast.gameObject;
        UiScriptInfo = takeCurrentButton.GetComponent<UiBuildingInfoReligie>();
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
