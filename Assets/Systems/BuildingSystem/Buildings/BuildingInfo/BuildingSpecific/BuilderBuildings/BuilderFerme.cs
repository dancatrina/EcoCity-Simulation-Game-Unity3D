using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFerme : IBuilder
{
    private BuildingFerme building;

    public BuilderFerme()
    {

        building = new BuildingFerme();
    }

    public BuilderFerme setTaxaCladire(float taxaCladire)
    {
        building.setTaxaCladire(taxaCladire);
        return this;
    }
    public BuilderFerme setConsumElectricitate(float consumElectricitate)
    {
        building.setConsumElectricitate(consumElectricitate);
        return this;
    }

    public BuilderFerme setVenitCladire(float venitCladire)
    {
        building.setVenitCladire(venitCladire);
        return this;
    }

    public BuilderFerme setNumarMaximAngajati(int numarMaximAngajati)
    {
        building.NumarMaximAngajati = numarMaximAngajati;
        return this;
    }

    public BuilderFerme setNumarCurentAngajati(int numarCurentAngajati)
    {
        building.NumarCurentAngajati = numarCurentAngajati;
        return this;
    }

    public BuilderFerme setCantitateProdusaDeFerma(AProdusFerma cantitateProdus)
    {
        building.MateriePrimaCatProduceSiCe = cantitateProdus;
        return this;
    }

    public BuilderFerme setNumarTotalDeMateriiProduse(int numarProduseVandute)
    {
        building.NumarTotalDeMateriePrimaCreata = numarProduseVandute;
        return this;
    }

    public ABuilding build()
    {
        building.tip = ABuilding.tipCladire.FERMA;
        return building;
    }
}
