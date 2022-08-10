
public class BuilderIndustrie : IBuilder
{
   private BuildingIndustrie building;

    public BuilderIndustrie() {
     
        building = new BuildingIndustrie();
    }

    public BuilderIndustrie setTaxaCladire(float taxaCladire) { 
        building.setTaxaCladire(taxaCladire);
        return this;
    }
    public BuilderIndustrie setConsumElectricitate(float consumElectricitate) {
        building.setConsumElectricitate(consumElectricitate);
        return this;
    }
    
    public BuilderIndustrie setVenitCladire(float venitCladire) { 
        building.setVenitCladire(venitCladire);
        return this;
    }

    public BuilderIndustrie setNumarMaximAngajati(int numarMaximAngajati){
        building.NumarMaximAngajati = numarMaximAngajati;
        return this;
    }

    public BuilderIndustrie setNumarCurentAngajati(int numarCurentAngajati){
        building.NumarCurentAngajati = numarCurentAngajati;
        return this;
    }

    public BuilderIndustrie setCantitateNecesaraPentruProducere(AProdusFerma numarMateriiPrimeIn)
    {
        building.CantitateNecesaraPentruProducere = numarMateriiPrimeIn;
        return this;
    }

    public BuilderIndustrie setCantitateaProdusaDeIndustrie(AProdusIndustrial numarProdusCurentCreat)
    {
        building.CantitateaProdusaDeIndustrie = numarProdusCurentCreat;
        return this;
    }

    public BuilderIndustrie setNumarTotalDeProduseFabricate(int numarProductieDeBaza)
    {
        building.NumarTotalDeProduseFabricate = numarProductieDeBaza;
        return this;
    }

    public BuilderIndustrie setDescriereCladire(string denumireCladire)
    {
        building.DescriereCladire = denumireCladire;
        return this;
    }


    public ABuilding build()
    {
        building.tip = ABuilding.tipCladire.INDUSTRIE;
        return building;
    }
}
