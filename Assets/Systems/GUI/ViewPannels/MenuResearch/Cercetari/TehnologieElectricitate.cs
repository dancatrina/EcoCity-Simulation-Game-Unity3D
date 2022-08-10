using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TehnologieElectricitate : ATehnologie
{
    public override void cerceteaza()
    {
        if (EconomyManager.getInstance().puncteCercetare - costCercetare > 0)
        {
            EconomyManager.getInstance().coeficientProductieEnergie = EconomyManager.getInstance().coeficientProductieEnergie = 3;
            EconomyManager.getInstance().containerDate.PuncteCercetare = EconomyManager.getInstance().puncteCercetare -= costCercetare;
            textPret.text = "CERCETAT";
            btn.enabled = false;
            textPret.fontSize = 28;
            cercetat = true;
        }
        
    }
}
