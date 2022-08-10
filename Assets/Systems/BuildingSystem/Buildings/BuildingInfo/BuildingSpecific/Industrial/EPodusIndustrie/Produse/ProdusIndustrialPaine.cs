

public class ProdusIndustrialPaine : AProdusIndustrial
{
    public ProdusIndustrialPaine(int cantitateProdus) : base(cantitateProdus)
    {
        this.tipProdus = EProdusIndustrial.PAINE;
        this.denumireProdus = "PAINE";
    }

    public ProdusIndustrialPaine(ProdusIndustrialPaine other) : base(other.cantitateProdus)
    {
        this.tipProdus = other.tipProdus;
        this.denumireProdus = other.denumireProdus;
    }

    public override AProdusIndustrial clone() => new ProdusIndustrialPaine(this);
}
