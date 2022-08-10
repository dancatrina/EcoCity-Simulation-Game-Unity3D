using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CodeMonkey.Utils;

public class CategoryStrada : View
{
    [SerializeField] private GameObject turnOff;

    [SerializeField] private List<Button> categories;


    [Header("Detalis")]
    public GameObject detaliFereastra;
    public UiBuildingStrada UiScriptInfo;

    public void OnEnable()
    {
        registerEvents();
    }

    private void registerEvents()
    {
        foreach (var element in categories)
        {
            element.onClick.AddListener(() =>
            {
                UiBuildingStrada current = element.GetComponent<UiBuildingStrada>();
                if (current != null)
                {
                    ABuilding building = new BuildingStrada();
                    BuildingSystem.getInstance().setStrategyJob(new StrategyAddBuilding(current.building, building, current.pret));
                    turnOff.SetActive(false);
                    Hide();
                }
            });
        }
    }

    public void OnDisable()
    {
        foreach (var elem in categories)
        {
            elem.onClick.RemoveAllListeners();
        }
    }

    public override void Initialize()
    {
    }
}
