using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBuildingInfoComercial : UiBuildingInfo
{

    [Header("Specific Cladire")]
    public int numarMaximAngajati;
    public int numarCurentAngajati;
    public int numarTotalDeProduseVandute;

    [Header("Produse oferite de cladire")]
    public EProdusIndustrial tipProdus;
    public int cantitateNecesareMagazinuluiDeAVinde;


}
