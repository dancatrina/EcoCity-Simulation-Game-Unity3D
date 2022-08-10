using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCommerce : View
{
    [SerializeField] private Button btnExit;

    [SerializeField] Button btnImport;
    [SerializeField] Button btnExport;

    [SerializeField] List<View> viewList;

    private View currentView;

    public override void Initialize()
    {
        if (btnExit == null) throw new System.NullReferenceException("btnExit = NULL");
        if (btnImport == null) throw new System.NullReferenceException("btnComplete = NULL");
        if (btnExport == null) throw new System.NullReferenceException("btnInProgress = NULL");

        btnExit.onClick.AddListener(() => Hide());

        if (viewList == null) throw new System.NullReferenceException("NULL");

        btnImport.onClick.AddListener(() => Show<MenuCommerceImport>());
        btnExport.onClick.AddListener(() => Show<MenuCommerceExport>());

        foreach( View view in viewList)
        {
            view.Hide();
        }
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

    private void OnEnable()
    {
        currentView = viewList[2];
        Show<MenuDefaultView>();
    }
    private void OnDisable()
    {
        currentView.Hide();
        currentView = viewList[2];
    }
}
