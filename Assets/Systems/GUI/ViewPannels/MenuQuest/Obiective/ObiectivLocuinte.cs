using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiectivLocuinte : AObiectiv
{
    public override void verificaObiectiv()
    {
        if (revendicat == false)
        {
            if (EconomyManager.getInstance().listLocuinte.Count >= obiectiv)
            {
                EconomyManager.getInstance().BaniOras = castig;
                revendicat = true;
            }
        }
    }

    private void OnEnable()
    {
        verificaObiectiv();
        defaultPanel.SetActive(true);
    }
}
