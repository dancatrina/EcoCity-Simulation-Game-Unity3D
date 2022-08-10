using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => functionTest());   
    }

    void functionTest()
    {
       // EconomyManager.getInstance().asigneazaPopulatie();
        //EconomyManager.getInstance().asigeanzaPopulatieLocMunca();

        Debug.Log(EconomyManager.getInstance().containerResurse.ToString());

    }

}
