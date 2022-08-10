using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButtons : MonoBehaviour
{
    public List<TabButton> listButtons;

    public Color tabIdle;
    public Color tabHover;
    public Color tabActive;

    private TabButton selectedTab;

    public void Subscribe(TabButton button)
    {
        if (button == null) throw new System.NullReferenceException("TabButton is NUll");

        if (listButtons == null)
        {
            listButtons = new List<TabButton>();
        }
        listButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.ImageObject.color = tabHover;
        }
    }

    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }

    public void onTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.ImageObject.color = tabActive;
    }

    public void ResetTabs()
    {
        foreach(TabButton button in listButtons)
        {
            if(button == selectedTab) { continue; }
            button.ImageObject.color = tabIdle;
        }
    }

    private void OnDisable()
    {
        foreach (TabButton button in listButtons)
        {
            button.ImageObject.color = tabIdle;
            selectedTab = null;
        }
    }
}
