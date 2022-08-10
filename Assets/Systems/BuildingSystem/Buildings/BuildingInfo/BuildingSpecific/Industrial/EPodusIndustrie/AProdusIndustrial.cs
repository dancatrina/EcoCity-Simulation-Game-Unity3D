using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AProdusIndustrial
{
    protected string denumireProdus;
    protected int cantitateProdus;
    protected EProdusIndustrial tipProdus;

    public AProdusIndustrial(int cantitateProdus)
    {
        this.cantitateProdus=cantitateProdus;
    }

    public string getDenumireProdus() => denumireProdus;
    public int getCantitateProdus() => cantitateProdus;
    public EProdusIndustrial getTipProdus() => tipProdus;

    public void setDenumireProdus(string denumireProdus) { this.denumireProdus = denumireProdus; }
    public void setCantitateProdus(int cantitateProdus) { this.cantitateProdus = cantitateProdus; }

    public abstract AProdusIndustrial clone();

    //De adaugat operatorii de overloading
}
