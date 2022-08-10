using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TehnologieComercial : ATehnologie
{
    public override void cerceteaza()
    {
        if (anterior.cercetat == true && EconomyManager.getInstance().puncteCercetare - costCercetare > 0)
        {
            EconomyManager.getInstance().coeficientComercialDeVenit = EconomyManager.getInstance().coeficientComercialDeVenit = 2;
            EconomyManager.getInstance().containerDate.PuncteCercetare = EconomyManager.getInstance().puncteCercetare -= costCercetare;
            textPret.text = "CERCETAT";
            btn.enabled = false;
            textPret.fontSize = 28;
            cercetat = true;
        }
    }
}
