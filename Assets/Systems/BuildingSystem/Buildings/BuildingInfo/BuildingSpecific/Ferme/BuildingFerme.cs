using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFerme : ABuilding
{
    private int numarMaximAngajati;
    private int numarCurentAngajati;

    private AProdusFerma materiePrimaCatProduceSiCe;

    private int numarTotalDeMateriePrimaCreata;

    public BuildingFerme() : base() { }

    public BuildingFerme(BuildingFerme other)
    {
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati=other.numarCurentAngajati;
        this.materiePrimaCatProduceSiCe=other.materiePrimaCatProduceSiCe.clone();
        this.numarTotalDeMateriePrimaCreata=other.numarTotalDeMateriePrimaCreata;

        this.venitCladire = other.venitCladire;
        this.taxaCladire = other.taxaCladire;
        this.consumElectricitate = other.consumElectricitate;
        this.tip = other.tip;
    }
    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }
    public AProdusFerma MateriePrimaCatProduceSiCe { get => materiePrimaCatProduceSiCe; set => materiePrimaCatProduceSiCe = value; }
    public int NumarTotalDeMateriePrimaCreata { get => numarTotalDeMateriePrimaCreata; set => numarTotalDeMateriePrimaCreata = value; }

    public override ABuilding clone() => new BuildingFerme(this);
}
