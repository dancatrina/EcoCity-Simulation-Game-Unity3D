using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBuildingInfoIndustrie : UiBuildingInfo
{
    [Header("Specific industrial")]
    public int numarMaximAngajati;
    public int numarCurentAngajati;

    public int numarTotalDeProduseFabricate;

    [Header("Materile Constructie")]
    public EMateriePrima tipMateriePrimaIn;
    public EProdusIndustrial tipProdusIndustrial;
    public int cantitateMateriePrimaNecesaraProductie;
    public int cantitateProdusaInUrmaProcesariiMaterialelor;



}
