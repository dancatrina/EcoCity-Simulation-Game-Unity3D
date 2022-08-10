public class BuilderComert : IBuilder
{
    private BuildingComercial building;

    public BuilderComert()
    {

        building = new BuildingComercial();
    }

    public BuilderComert setTaxaCladire(float taxaCladire)
    {
        building.setTaxaCladire(taxaCladire);
        return this;
    }
    public BuilderComert setConsumElectricitate(float consumElectricitate)
    {
        building.setConsumElectricitate(consumElectricitate);
        return this;
    }

    public BuilderComert setVenitCladire(float venitCladire)
    {
        building.setVenitCladire(venitCladire);
        return this;
    }

    public BuilderComert setNumarMaximAngajati(int numarMaximAngajati)
    {
        building.NumarMaximAngajati = numarMaximAngajati;
        return this;
    }

    public BuilderComert setNumarCurentAngajati(int numarCurentAngajati)
    {
        building.NumarCurentAngajati = numarCurentAngajati;
        return this;
    }

    public BuilderComert setCantitateNecesaraMagazinuluiDeVanzare(AProdusIndustrial cantitateProdus)
    {
        building.CantitateNecesareMagazinuluiDeAVinde = cantitateProdus;
        return this;
    }

    public BuilderComert setNumarTotalDeProduseVandute(int numarProduseVandute)
    {
        building.NumarTotalDeProduseVandute = numarProduseVandute;
        return this;
    }

    public ABuilding build()
    {
        building.tip = ABuilding.tipCladire.COMERCIAL;
        return building;
    }
}
