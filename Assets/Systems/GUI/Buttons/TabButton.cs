using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    
    public GroupButtons groupButtons;

    private Image imageObject;

    public Image ImageObject { get => imageObject; set => imageObject = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        groupButtons.onTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        groupButtons.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        groupButtons.OnTabExit(this);
    }

    private void Start()
    {
        imageObject = GetComponent<Image>();
        groupButtons.Subscribe(this);

    }
}
