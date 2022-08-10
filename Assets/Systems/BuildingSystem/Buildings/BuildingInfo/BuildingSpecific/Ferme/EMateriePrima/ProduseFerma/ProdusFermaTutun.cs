
public class ProdusFermaTutun : AProdusFerma
{
    public ProdusFermaTutun(int cantitateProdus) : base(cantitateProdus)
    {
        this.tipMateriaPrima = EMateriePrima.TUTUN;
        this.DenumireProdus = "Tutun";
    }

    public ProdusFermaTutun(ProdusFermaTutun other) : base(other.cantitateProdus)
    {
        this.tipMateriaPrima = other.tipMateriaPrima;
        this.DenumireProdus = other.denumireMateriePrima;
    }

    public override AProdusFerma clone() => new ProdusFermaTutun(this);
}
