

public class ProdusIndustrialCafea : AProdusIndustrial
{
    public ProdusIndustrialCafea(int cantitateProdus) : base(cantitateProdus)
    {
        tipProdus = EProdusIndustrial.CAFEA;
        denumireProdus = "Cafea";
    }

    public ProdusIndustrialCafea(ProdusIndustrialCafea other) : base(other.cantitateProdus)
    {
        tipProdus = other.tipProdus;
        denumireProdus = other.denumireProdus;
    }

    public override AProdusIndustrial clone() => new ProdusIndustrialCafea(this);
}
