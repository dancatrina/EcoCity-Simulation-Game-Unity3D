using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditieAchievBani : AConditieAchiev
{
    public override void actiune(int conditie)
    {
        if(EconomyManager.getInstance().BaniOras >= conditie)
        {
            indeplinit = true;
        }
    }
}
