using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TehnologieFerma : ATehnologie
{
    public override void cerceteaza()
    {
        if(anterior.cercetat == true && EconomyManager.getInstance().puncteCercetare - costCercetare >= 0)
        {
            EconomyManager.getInstance().coeficientProductieFerme = EconomyManager.getInstance().coeficientProductieFerme = 2;
            EconomyManager.getInstance().containerDate.PuncteCercetare = EconomyManager.getInstance().puncteCercetare -= costCercetare;
            textPret.text = "CERCETAT";
            textPret.fontSize = 28;
            btn.enabled = false;
            cercetat = true;
        }
    }
}
