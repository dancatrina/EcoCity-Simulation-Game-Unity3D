using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTutorial : View
{
    [SerializeField] Button btnExit;
    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => Hide());
    }
}
