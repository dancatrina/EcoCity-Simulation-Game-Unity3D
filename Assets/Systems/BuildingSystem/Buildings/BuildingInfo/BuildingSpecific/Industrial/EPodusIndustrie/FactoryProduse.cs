
public class FactoryProduse
{
    public static AProdusIndustrial creazaProdus(EProdusIndustrial tipProdus,int cantitateProdus)
    {
        if (tipProdus == EProdusIndustrial.PAINE) return new ProdusIndustrialPaine(cantitateProdus);
        if(tipProdus == EProdusIndustrial.CAFEA) return new ProdusIndustrialCafea(cantitateProdus);
        if (tipProdus == EProdusIndustrial.TIGARI) return new ProdusIndustrialTigara(cantitateProdus);
        return null;
        
    }
}
