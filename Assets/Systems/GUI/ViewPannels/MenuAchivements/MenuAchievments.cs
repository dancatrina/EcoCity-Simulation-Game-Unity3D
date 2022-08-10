
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuAchievments : View {
    [SerializeField] Button btnExit;
    [SerializeField] Button btnCompleted;
    [SerializeField] Button btnInProgress;

    [SerializeField] List<View> viewList;

    [SerializeField] List<Achievement> achievList;

    private View currentView;

    public GameObject refIconRightPanel;
    public GameObject refTitleTextRightPanel;
    public GameObject refDescriptionRightPanel;
    public GameObject refToParentObject;

    public override void Initialize()
    {
        if (btnExit == null) throw new System.NullReferenceException("btnExit = NULL");
        if (btnCompleted == null) throw new System.NullReferenceException("btnComplete = NULL");
        if (btnInProgress == null) throw new System.NullReferenceException("btnInProgress = NULL");

        btnExit.onClick.AddListener(() => Hide());

        if (viewList == null) throw new System.NullReferenceException("viewList is NULL");

        btnCompleted.onClick.AddListener(() => Show<MenuAchievCompleted>());
        btnInProgress.onClick.AddListener(() => Show<MenuAchievLocked>());

        if( achievList == null) throw new System.NullReferenceException("AchievList is NULL");


        if( refTitleTextRightPanel == null) throw new System.NullReferenceException();
        if( refDescriptionRightPanel == null) throw new System.NullReferenceException();

        foreach(Achievement element in achievList)
        {
            if (element != null)
                element.init(refIconRightPanel.GetComponent<Image>(),
                    refTitleTextRightPanel.GetComponent<TMP_Text>(),
                    refDescriptionRightPanel.GetComponent<TMP_Text>(),
                    refToParentObject);
        }
    }

    private void Show<T>() where T : View
    {
        for (int i = 0; i < viewList.Count; i++)
        {
            if (viewList[i] is T)
            {
                if ( currentView != null)
                {
                    currentView.Hide();
                }

                viewList[i].Show();

                currentView = viewList[i];
            }
        }
    }

    [System.Obsolete]
    private void OnDisable()
    {
        Show<MenuAchievCompleted>();
        refToParentObject.SetActive(false);
    }

}
