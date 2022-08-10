public abstract class ABuilding
{
    protected float taxaCladire;
    protected float consumElectricitate;
    protected float venitCladire;

    public enum tipCladire
    {
        LOCUINTA,
        INDUSTRIE,
        COMERCIAL,
        FERMA,
        PUBLIC,
        RELIGIE,
        ELECTRICITATE,
        SANATATE,
        DEPOZIT,
        Strada
    }

    public tipCladire tip;

    public ABuilding() { }

    public float getTaxaCladire() => taxaCladire;
    public float getConsumElectricitate() => consumElectricitate;
    public float getVenitCladire() => venitCladire;
    public tipCladire getTip() => tip;

    public void setTaxaCladire(float taxaCladire) { this.taxaCladire = taxaCladire; }
    public void setConsumElectricitate(float consumElectricitate) { this.consumElectricitate = consumElectricitate; }

    public void setTipCladire(tipCladire tip) { this.tip = tip; }
    public void setVenitCladire(float venitCladire) { this.venitCladire = venitCladire; }

    public abstract ABuilding clone();
}
