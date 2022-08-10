using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingElectricitate : ABuilding
{
    private int numarMaximAngajati;
    private int numarCurentAngajati;

    private int numarCataElectricitatePoateProduce;
    private int totalElectricitateProdusa;

    public BuildingElectricitate(int numarMaximAngajati, int numarCurentAngajati, int numarCataElectricitatePoateProduce, int TotalElectricitateProdusa, float taxeCladire)
    {
        this.numarMaximAngajati = numarMaximAngajati;
        this.numarCurentAngajati= numarCurentAngajati;
        this.numarCataElectricitatePoateProduce = numarCataElectricitatePoateProduce;
        this.taxaCladire = taxeCladire;
        tip = tipCladire.ELECTRICITATE;

    }

    public BuildingElectricitate(BuildingElectricitate other)
    {
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati = other.numarCurentAngajati;
        this.numarCataElectricitatePoateProduce = other.numarCataElectricitatePoateProduce;
        this.taxaCladire = other.taxaCladire;
        tip = other.tip;

    }

    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }
    public int NumarCataElectricitatePoateProduce { get => numarCataElectricitatePoateProduce; set => numarCataElectricitatePoateProduce = value; }
    public int TotalElectricitateProdusa { get => totalElectricitateProdusa; set => totalElectricitateProdusa = value; }

    public override ABuilding clone() => new BuildingElectricitate(this);
}
