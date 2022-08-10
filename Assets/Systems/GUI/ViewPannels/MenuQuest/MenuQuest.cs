using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuQuest : View
{
    [SerializeField] private Button btnExit;
    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => Hide());

    }
}
