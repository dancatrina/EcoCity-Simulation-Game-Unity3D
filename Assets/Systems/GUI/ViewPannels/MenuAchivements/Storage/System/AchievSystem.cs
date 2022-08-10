using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class AchievSystem : MonoBehaviour
{
    private static AchievSystem instance;

    [Header("Lista achiev")]
    public List<Achievement> listAchievs;

    [Header("Destinatie")]
    public GameObject destinatieAmplasare;

    public static AchievSystem Instance { get => instance; set => instance = value; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        FunctionTimer.Create(() => {
            EconomyManager.getInstance().containerDate.onDataContainerChange += mutaAchiev;
        },3f);
        
    }

    void mutaAchiev()
    {
        if (listAchievs.Count > 0)
        {
            foreach (Achievement item in listAchievs)
            {
                if (item.conditieAchiev != null)
                {
                    item.conditieAchiev.actiune(item.conditie);

                    if (item.conditieAchiev.indeplinit == true)
                    {
                        item.gameObject.transform.SetParent(destinatieAmplasare.transform, false);
                    }
                }
            }
        }
    }


}
