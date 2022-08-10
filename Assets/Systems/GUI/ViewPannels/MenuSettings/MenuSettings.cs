using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuSettings : View
{
    [SerializeField] private Button btnExit;
    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => Hide());
    }
}
