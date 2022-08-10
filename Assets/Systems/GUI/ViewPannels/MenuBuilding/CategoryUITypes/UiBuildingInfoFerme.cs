using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBuildingInfoFerme : UiBuildingInfo
{
    [Header("Specific cladire")]
    public int numarMaximAngajati;
    public int numarCurentAngajati;

    public EMateriePrima tipMaterie;
    public int catProduceMateriePrima;

    public int numarTotalPodus;
}
