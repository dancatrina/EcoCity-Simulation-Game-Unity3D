

public abstract class AProdusFerma
{
    protected string denumireMateriePrima;
    protected int cantitateProdus;
    protected EMateriePrima tipMateriaPrima;

    public string DenumireProdus { get => denumireMateriePrima; set => denumireMateriePrima = value; }
    public int CantitateProdus { get => cantitateProdus; set => cantitateProdus = value; }
    public EMateriePrima TipMateriaPrima { get => tipMateriaPrima; set => tipMateriaPrima = value; }

    public AProdusFerma(int cantitateProdus)
    {
        this.cantitateProdus = cantitateProdus;
    }

    public override string ToString()
    {
        return denumireMateriePrima + " " + cantitateProdus + " " + tipMateriaPrima.ToString();
    }

    public abstract AProdusFerma clone();

}
