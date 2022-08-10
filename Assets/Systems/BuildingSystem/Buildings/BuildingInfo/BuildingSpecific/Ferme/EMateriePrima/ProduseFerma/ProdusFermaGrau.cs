
public class ProdusFermaGrau : AProdusFerma
{
    public ProdusFermaGrau(int cantitateProdus) : base(cantitateProdus)
    {
        tipMateriaPrima = EMateriePrima.GRAU;
        this.denumireMateriePrima = "GRAU";
    }

    public ProdusFermaGrau(ProdusFermaGrau other) : base(other.cantitateProdus)
    {
        tipMateriaPrima = other.tipMateriaPrima;
        this.denumireMateriePrima = other.denumireMateriePrima;
    }

    public override AProdusFerma clone() => new ProdusFermaGrau(this);
}
