public class BuildingDepozit : ABuilding
{
    private int numarMaximDeResurse;
    private int numarCurentDeResurse;

    public BuildingDepozit(int numarMaximDeResurse, int numarCurentDeResurse, float consumEnergie, float taxaCladire)
    {
        this.numarMaximDeResurse = numarMaximDeResurse;
        this.numarCurentDeResurse = numarCurentDeResurse;
        this.taxaCladire = taxaCladire;
        this.consumElectricitate = consumEnergie;
        this.tip = tipCladire.DEPOZIT;

    }

    public BuildingDepozit(BuildingDepozit other)
    {
        this.numarMaximDeResurse = other.numarMaximDeResurse;
        this.numarCurentDeResurse = other.numarCurentDeResurse;
        this.taxaCladire = other.taxaCladire;
        this.consumElectricitate = other.consumElectricitate;
        this.tip = other.tip;

    }

    public int NumarMaximDeResurse { get => numarMaximDeResurse; set => numarMaximDeResurse = value; }
    public int NumarCurentDeResurse { get => numarCurentDeResurse; set => numarCurentDeResurse = value; }

    public override ABuilding clone() => new BuildingDepozit(this);
}
