using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBuilding : View
{
    [SerializeField] private Button btnExit;

    [SerializeField] private Button btnDestroy;
    [SerializeField] private Button btnStrada;
    [SerializeField] private Button btnLocuinte;
    [SerializeField] private Button btnFerma;
    [SerializeField] private Button btnIndustrie;
    [SerializeField] private Button btnElectricitate;
    [SerializeField] private Button btnComercial;

    [SerializeField] private Button btnSanatate;
    [SerializeField] private Button btnReligie;
    [SerializeField] private Button btnPublic;

    [SerializeField] List<View> viewList;

    private View currentView;

    public override void Initialize()
    {


        
        foreach(var elem in viewList)
        {
            elem.Initialize();
            elem.Hide();
        }
        
    }

    private void OnEnable()
    {
        btnExit.onClick.AddListener(() => Hide());


        btnDestroy.onClick.AddListener(() => demoleazaCladire());
        btnLocuinte.onClick.AddListener(() => Show<CategoryLocuinte>());
        btnStrada.onClick.AddListener(() => Show<CategoryStrada>());
        btnIndustrie.onClick.AddListener(() => Show<CategoryIndustrial>());
        btnElectricitate.onClick.AddListener(() => Show<CategoryElectricitate>());
        btnComercial.onClick.AddListener(() => Show<CategoryComercial>());
        btnSanatate.onClick.AddListener(() => Show<CategorySanatate>());
        btnReligie.onClick.AddListener(() => Show<CategoryReligie>());
        btnPublic.onClick.AddListener(() => Show<CategoryPublic>());
        btnFerma.onClick.AddListener(() => Show<CategoryFerme>());

        Show<MenuDefaultView>();
    }

    private void OnDisable()
    {
        btnExit.onClick.RemoveAllListeners();
        btnDestroy.onClick.RemoveAllListeners();
        btnLocuinte.onClick.RemoveAllListeners();
        btnStrada.onClick.RemoveAllListeners();
        btnIndustrie.onClick.RemoveAllListeners();
        btnElectricitate.onClick.RemoveAllListeners();
        btnComercial.onClick.RemoveAllListeners();
        btnSanatate.onClick.RemoveAllListeners();
        btnReligie.onClick.RemoveAllListeners();
        btnPublic.onClick.RemoveAllListeners();
        btnFerma.onClick.RemoveAllListeners();

        foreach(View el in viewList)
        {
            el.Hide();
        }

        
    }


    public void demoleazaCladire()
    {
        BuildingSystem.getInstance().setStrategyJob(new StrategyRemoveBuilding());
        Hide();
    }

    private void Show<T>() where T : View
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            if (viewList[i] is T)
            {
                if (currentView != null)
                {
                    currentView.Hide();
                }

                viewList[i].Show();

                currentView = viewList[i];
            }
        }
    }
}
