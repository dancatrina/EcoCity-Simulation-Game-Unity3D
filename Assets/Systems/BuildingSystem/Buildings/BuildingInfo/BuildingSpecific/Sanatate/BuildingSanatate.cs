

public class BuildingSanatate : ABuilding
{
    private int numarMaximAngajati;
    private int numarCurentAngajati;
    private int nouNascutiTotal;

    public BuildingSanatate() { }
    public BuildingSanatate(int numarMaximAngajati, int numarCurentAngajati, int nouNascutiTotal, float taxaCladire, float consumCladire, float venitCladire)
    {
        this.numarMaximAngajati = numarMaximAngajati;
        this.numarCurentAngajati = numarCurentAngajati;
        this.nouNascutiTotal = nouNascutiTotal;
        this.taxaCladire = taxaCladire;
        this.venitCladire = venitCladire;
        this.consumElectricitate = consumCladire;
        this.tip = tipCladire.SANATATE;
    }

    public BuildingSanatate(BuildingSanatate other)
    {
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati = other.numarCurentAngajati;
        this.nouNascutiTotal = other.nouNascutiTotal;
        this.taxaCladire = other.taxaCladire;
        this.venitCladire = other.venitCladire;
        this.consumElectricitate = other.consumElectricitate;
        this.tip = other.tip;
    }

    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }
    public int NouNascutiTotal { get => nouNascutiTotal; set => nouNascutiTotal = value; }

    public override ABuilding clone()
    {
        return new BuildingSanatate(this);
    }
}
