using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObiectivImport : AObiectiv
{
    public override void verificaObiectiv()
    {
        if (revendicat == false)
        {
            if (EconomyManager.getInstance().importTotal>= obiectiv)
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
