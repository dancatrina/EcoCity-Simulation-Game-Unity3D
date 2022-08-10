using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditieIndustrial : AConditieAchiev
{
    public override void actiune(int conditie)
    {
        if(EconomyManager.getInstance().listIndustri.Count >= conditie)
        {
            indeplinit = true;
        }
    }
}
