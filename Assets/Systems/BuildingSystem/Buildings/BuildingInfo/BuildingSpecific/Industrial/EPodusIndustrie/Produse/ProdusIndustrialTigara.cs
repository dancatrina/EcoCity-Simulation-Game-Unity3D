using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProdusIndustrialTigara : AProdusIndustrial
{
    public ProdusIndustrialTigara(int cantitateProdus) : base(cantitateProdus)
    {
        this.tipProdus = EProdusIndustrial.TIGARI;
        this.denumireProdus = "Tigari";
    }

    public ProdusIndustrialTigara(ProdusIndustrialTigara other) : base(other.cantitateProdus)
    {
        this.tipProdus = other.tipProdus;
        this.denumireProdus = other.denumireProdus;
    }

    public override AProdusIndustrial clone() => new ProdusIndustrialTigara(this);
}
