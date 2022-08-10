using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuStatistics : View
{
    [SerializeField] Button btnExit;

    [Header("Butoane Panel")]
    public Button btnSumar;
    public Button btnBalanta;
    public Button btnProductie;

    [SerializeField] List<View> viewList;
    private View currentView;


    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => Hide());
        btnSumar.onClick.AddListener(() => Show<PanelStatisticsSumar>());
        btnBalanta.onClick.AddListener(() => Show<PanelStatisticsBalanta>());
        btnProductie.onClick.AddListener(() => Show<PanelStatisticProductie>());

        foreach( var view in viewList)
        {
            view.Initialize();
            view.Hide();
        }

        Show<MenuDefaultView>();
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


    private void OnDisable()
    {
        currentView = viewList[3];
        currentView.Show();
    }
}
