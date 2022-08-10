
public class BuildingIndustrie : ABuilding
{
    private string descriereCladire;

    private int numarMaximAngajati;
    private int numarCurentAngajati;

    private AProdusFerma cantitateNecesaraPentruProducere;
    private AProdusIndustrial cantitateaProdusaDeIndustrie;

    private int numarTotalDeProduseFabricate;

    public BuildingIndustrie() : base() {}

    public BuildingIndustrie(BuildingIndustrie other) : base() { 
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati=other.numarCurentAngajati;
        this.cantitateNecesaraPentruProducere = other.cantitateNecesaraPentruProducere.clone();
        this.cantitateaProdusaDeIndustrie = other.cantitateaProdusaDeIndustrie.clone();
        this.numarTotalDeProduseFabricate = other.numarTotalDeProduseFabricate;

        this.consumElectricitate = other.consumElectricitate;
        this.venitCladire = other.venitCladire;
        this.taxaCladire = other.taxaCladire;
        this.tip = other.tip;
    }

    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }
    public AProdusFerma CantitateNecesaraPentruProducere { get => cantitateNecesaraPentruProducere; set => cantitateNecesaraPentruProducere = value; }
    public AProdusIndustrial CantitateaProdusaDeIndustrie { get => cantitateaProdusaDeIndustrie; set => cantitateaProdusaDeIndustrie = value; }
    public int NumarTotalDeProduseFabricate { get => numarTotalDeProduseFabricate; set => numarTotalDeProduseFabricate = value; }
    public string DescriereCladire { get => descriereCladire; set => descriereCladire = value; }

    public override ABuilding clone() => new BuildingIndustrie(this);

    public override string ToString()
    {
        return "[numar mAxim angajati:" + numarMaximAngajati + " numar curent de angajati " + numarCurentAngajati + " cantitateNecesaraProducere " + cantitateNecesaraPentruProducere.CantitateProdus
            + " cantitateProdusDeIndustrie:" + cantitateaProdusaDeIndustrie.getCantitateProdus() + " numartotalDeProduseFabricate:" + numarTotalDeProduseFabricate + " " + descriereCladire + "]" + "[Taxa:" + getTaxaCladire() +  
            " Venit" + getVenitCladire() + " Consum energie" + getConsumElectricitate() + "]";
    }
}
