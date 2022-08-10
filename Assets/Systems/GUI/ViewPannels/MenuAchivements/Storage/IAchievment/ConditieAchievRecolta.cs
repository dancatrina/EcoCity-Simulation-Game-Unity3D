using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditieAchievRecolta : AConditieAchiev
{
    public override void actiune(int conditie)
    {
        if (EconomyManager.getInstance().productieGrau >= conditie)
        {
            indeplinit = true;
        }
    }
}
