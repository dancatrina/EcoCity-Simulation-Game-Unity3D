
public class BuildingLocuinta : ABuilding
{
    private int numarMaximLocatari;
    private int numarCurentLocatari;
    private float venitTotal;

    public BuildingLocuinta(int numarMaximLocatari, int numarCurentLocatari, float venitTotal, float taxaCladire, float consumCladire, float venitCladire,tipCladire tip)
    {
        this.numarMaximLocatari = numarMaximLocatari;
        this.numarCurentLocatari = numarCurentLocatari;
        this.venitTotal = venitTotal;
        this.taxaCladire = taxaCladire;
        this.venitCladire = venitCladire;
        this.consumElectricitate = consumCladire;
        this.tip = tip;

    }

    public BuildingLocuinta(BuildingLocuinta other)
    {
        this.numarMaximLocatari = other.numarMaximLocatari;
        this.numarCurentLocatari = other.numarCurentLocatari;
        this.venitTotal = other.venitTotal;
        this.taxaCladire = other.taxaCladire;
        this.venitCladire = other.venitCladire;
        this.consumElectricitate = other.consumElectricitate;
        this.tip = other.tip;

    }




    public float VenitTotal { get => venitTotal; set => venitTotal = value; }



    public int getNumarMaximLocuitori() => numarMaximLocatari;
    public int getNumarCurentLocuitori() => numarCurentLocatari;

    public void setNumarMaximLocuitori(int numarMaximLocuitori) { this.numarMaximLocatari = numarMaximLocuitori; }
    public void setNumarCurentLocuitori(int numarCurentLocuitori) { this.numarCurentLocatari = numarCurentLocuitori; }

    public override ABuilding clone()
    {
        return new BuildingLocuinta(this);
    }
}
