using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBiserica : ABuilding
{
    private int numarMaximAngajati;
    private int numarCurentAngajati;

    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }

    public BuildingBiserica(int numarMaximAngajati,int numarCurentAngajati,float taxeCladire, float consumElectricitate, float venitCladire)
    {
        this.numarMaximAngajati = numarMaximAngajati;
        this.numarCurentAngajati = numarCurentAngajati;
        this.taxaCladire = taxeCladire;
        this.consumElectricitate = consumElectricitate;
        this.venitCladire = venitCladire;
        tip = tipCladire.RELIGIE;
    }

    public BuildingBiserica(BuildingBiserica other)
    {
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati = other.numarCurentAngajati;
        this.taxaCladire = other.taxaCladire;
        this.consumElectricitate = other.consumElectricitate;
        this.venitCladire = other.venitCladire;
        tip = other.tip;
    }

    public override ABuilding clone()
    {
        return new BuildingBiserica(this);
    }
}
