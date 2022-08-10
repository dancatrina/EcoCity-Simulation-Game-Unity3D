using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManagerSingleton : MonoBehaviour
{
    private static UiManagerSingleton instanceManager;

    //Bottom buttons
    [SerializeField] private Button btnBuild;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnAchievments;
    [SerializeField] private Button btnCommerce;
    [SerializeField] private Button btnQuest;
    [SerializeField] private Button btnRewards;
    [SerializeField] private Button btnResearch;
    [SerializeField] private Button btnStatistics;
    [SerializeField] private Button btnTutorial;

    //Views
    [SerializeField] private List<View> views;
    private View currentView;



    public static UiManagerSingleton getInstance() => instanceManager;
    public void Start()
    {

        btnBuild.onClick.AddListener(() => Show<MenuBuilding>());
        btnSettings.onClick.AddListener(() => Show<MenuSettings>());
        btnAchievments.onClick.AddListener(() => Show<MenuAchievments>());
        btnCommerce.onClick.AddListener(() => Show<MenuCommerce>());
        btnQuest.onClick.AddListener(() => Show<MenuQuest>());
        btnRewards.onClick.AddListener(() => Show<MenuRewards>());
        btnResearch.onClick.AddListener(() => Show<MenuResearch>());
        btnStatistics.onClick.AddListener(() => Show<MenuStatistics>());
        btnTutorial.onClick.AddListener(() => Show<MenuTutorial>());




        for(int i = 0; i< views.Count; i++)
        {
            views[i].Initialize();
            views[i].Hide();
        }
    }

    public void Awake()
    {
        instanceManager = this;
    }


    //Methods
    public static T getView<T>() where T : View
    {
        foreach(View elem in instanceManager.views)
        {
            if(elem is T tview)
            {
                return tview;
            }
        }
        return null;
    }

    public static void Show<T>() where T : View
    {
        for (int i = 0; i < instanceManager.views.Count; i++)
        {
            if (instanceManager.views[i] is T)
            {
                if (instanceManager.currentView != null)
                {

                    instanceManager.currentView.Hide();
                }

                instanceManager.views[i].Show();

                instanceManager.currentView = instanceManager.views[i];
            }
        }

        if(ContainerUI.getInstance().infoPanelStrategy != null)
        {
            ContainerUI.getInstance().infoPanelStrategy = null;
        }
    }

    public void showFast(View view)
    {
        if(currentView != null)
        {
            currentView.Hide();
        }
        currentView = view;
        currentView.Show();
    }

}


