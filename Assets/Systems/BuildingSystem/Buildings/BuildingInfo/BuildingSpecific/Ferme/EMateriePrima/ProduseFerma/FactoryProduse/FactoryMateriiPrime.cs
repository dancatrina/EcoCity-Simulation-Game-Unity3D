

public class FactoryMateriiPrime
{
    public static AProdusFerma creazaMateriePrima(EMateriePrima tip, int cantitate)
    {
        if (tip == EMateriePrima.BOABE_CAFEA) return new ProdusFermaBoabeCafea(cantitate);
        if (tip == EMateriePrima.TUTUN) return new ProdusFermaTutun(cantitate);
        if (tip == EMateriePrima.GRAU) return new ProdusFermaGrau(cantitate);
        return null;
    }
}
