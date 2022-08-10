

public class BuildingPublic : ABuilding
{
    private int numarMaximAngajati;
    private int numarCurentAngajati;

    private int puncteTotalCercetare;

    public BuildingPublic(int numarMaximAngajati, int numarCurentAngajati, int puncteTotalCercetare, float consumElectricitate)
    {
        this.numarMaximAngajati = numarMaximAngajati;
        this.numarCurentAngajati = numarCurentAngajati;
        this.puncteTotalCercetare = puncteTotalCercetare;
        this.consumElectricitate = consumElectricitate;
        tip = tipCladire.PUBLIC;
    }

    public BuildingPublic(BuildingPublic other)
    {
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati = other.numarCurentAngajati;
        this.puncteTotalCercetare = other.puncteTotalCercetare;
        this.consumElectricitate = other.consumElectricitate;
        tip = other.tip;
    }

    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }
    public int PuncteTotalCercetare { get => puncteTotalCercetare; set => puncteTotalCercetare = value; }

    public override ABuilding clone() => new BuildingPublic(this);
}
