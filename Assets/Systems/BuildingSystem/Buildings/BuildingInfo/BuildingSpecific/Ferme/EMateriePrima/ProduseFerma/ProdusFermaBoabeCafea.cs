public class ProdusFermaBoabeCafea : AProdusFerma
{
    public ProdusFermaBoabeCafea(int cantitateProdus) : base(cantitateProdus)
    {
        this.tipMateriaPrima = EMateriePrima.BOABE_CAFEA;
        this.DenumireProdus = "Boabe de cafea";
    }

    public ProdusFermaBoabeCafea(ProdusFermaBoabeCafea other) : base(other.cantitateProdus)
    {
        this.tipMateriaPrima = other.tipMateriaPrima;
        this.DenumireProdus = other.DenumireProdus;
    }

    public override AProdusFerma clone() => new ProdusFermaBoabeCafea(this);
}
