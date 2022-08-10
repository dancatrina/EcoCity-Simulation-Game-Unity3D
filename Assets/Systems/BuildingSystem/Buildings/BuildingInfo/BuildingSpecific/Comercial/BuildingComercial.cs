using UnityEngine;

public class BuildingComercial : ABuilding
{
    private int numarMaximAngajati;
    private int numarCurentAngajati;

    private AProdusIndustrial cantitateNecesareMagazinuluiDeAVinde;

    private int numarTotalDeProduseVandute;

    public BuildingComercial() : base()
    {
    }

    public BuildingComercial(BuildingComercial other)
    {
        this.numarMaximAngajati = other.numarMaximAngajati;
        this.numarCurentAngajati=other.numarCurentAngajati;
        this.cantitateNecesareMagazinuluiDeAVinde = other.cantitateNecesareMagazinuluiDeAVinde;
        this.numarTotalDeProduseVandute = other.numarTotalDeProduseVandute;

        this.venitCladire = other.venitCladire;
        this.taxaCladire = other.taxaCladire;
        this.consumElectricitate = other.consumElectricitate;
        this.tip = other.tip;
    }

    public int NumarMaximAngajati { get => numarMaximAngajati; set => numarMaximAngajati = value; }
    public int NumarCurentAngajati { get => numarCurentAngajati; set => numarCurentAngajati = value; }
    public AProdusIndustrial CantitateNecesareMagazinuluiDeAVinde { get => cantitateNecesareMagazinuluiDeAVinde; set => cantitateNecesareMagazinuluiDeAVinde = value; }
    public int NumarTotalDeProduseVandute { get => numarTotalDeProduseVandute; set => numarTotalDeProduseVandute = value; }

    public override ABuilding clone() => new BuildingComercial(this);
}
