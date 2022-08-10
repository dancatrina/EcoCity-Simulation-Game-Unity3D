using System.Collections.Generic;
public class DataContainer
{
    private int populatie;
    private int populatieCuLocuinta;
    private int populatieFaraAdapost;
    private int populatieAngajata;
    private int populatieSomera;

    private float venit;
    private float taxe;

    private int capacitateResurse;
    private int resurseCurente;

    private float electricitateCurenta;
    private float electricitateConsum;

    public delegate void onDataContainer();
    public event onDataContainer onDataContainerChange;

    private int zi;

    private List<int> listProfitpeZile;

    //Profitabilitate
    private float exportTotal;
    private float importTotal;
    private float profitComert;

    //Profitabilitate Cladiri
    private float profitLocuinte;
    private float profitComercial;
    private float profitIndustrii;
    private float profitSpitale;
    private float profitFerme;
    private float profitBiserica;

    //Profit cladiri total
    private float profitCladiriTotal;

    // // //  // //  //  // //  // // // // // // // // // // // // // // // // // // // // // // /// ////


    //PRODUCTIE
    private int productieGrau;
    private int productieTutun;
    private int productieBoabe;
    private int productieCafea;
    private int productieTigari;
    private int productiePaine;

    //CONSUM
    private int consumGrau;
    private int consumTutun;
    private int consumBoabe;
    private int consumCafea;
    private int consumTigari;
    private int consumPaine;

    private int nrTotalDepozite;

    //Total resurse
    private int totalGrau;
    private int totalTutun;
    private int totalBoabe;
    private int totalCafea;
    private int totalTigari;
    private int totalPaine;

    private int coeficientFerme;
    private int coeficientElectricitate;
    private int coeficientIndustrie;
    private int coeficientComercial;

    private int puncteCercetare;
    public DataContainer() {
        listProfitpeZile = new List<int>() { 0};
    }



    public void adaugaProfit(int val)
    {
        if(listProfitpeZile.Count > 6)
        {
            listProfitpeZile.RemoveAt(0);
            listProfitpeZile.Add(val);
        }
        else
        {
            listProfitpeZile.Add(val);
        }

        onDataContainerChange();
    }

    public void schimbaMateriePrima(EMateriePrima tip, int cantitate)
    {
        if(tip == EMateriePrima.BOABE_CAFEA)
        {
            TotalBoabe = cantitate;
        }
        else if (tip == EMateriePrima.TUTUN)
        {
            TotalTutun = cantitate;
        }
        else if (tip == EMateriePrima.GRAU)
        {
            TotalGrau = cantitate;
        }
    }

    public void schimbaProdus(EProdusIndustrial tip, int cantitate)
    {
        if (tip == EProdusIndustrial.CAFEA)
        {
            TotalCafea = cantitate;
        }
        else if (tip == EProdusIndustrial.TIGARI)
        {
            TotalTigari = cantitate;
        }
        else if (tip == EProdusIndustrial.PAINE)
        {
            TotalPaine = cantitate;
        }
    }


    public int Populatie { get => populatie; set
        {
            if (populatie == value) return;
            populatie = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int PopulatieCuLocuinta { get => populatieCuLocuinta; set
        {
            if (populatieCuLocuinta == value) return;
            populatieCuLocuinta = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int PopulatieFaraAdapost { get => populatieFaraAdapost; set
        {
            if (populatieFaraAdapost == value) return;
            populatieFaraAdapost = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int PopulatieAngajata { get => populatieAngajata; set
        {
            if (populatieAngajata == value) return;
            populatieAngajata = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int PopulatieSomera { get => populatieSomera; set
        {
            if (populatieSomera == value) return;
            populatieSomera = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float Venit { get => venit; set
        {
            if (venit == value) return;
            venit = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float Taxe { get => taxe; set
        {
            if (taxe == value) return;
            taxe = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int CapacitateResurse { get => capacitateResurse;
        set
        {
            if (capacitateResurse == value) return;
            capacitateResurse = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ResurseCurente { get => resurseCurente; set
        {
            if (ResurseCurente == value) return;
            resurseCurente = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ElectricitateCurenta { get => electricitateCurenta; set
        {
            if (electricitateCurenta == value) return;
            electricitateCurenta = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ElectricitateConsum { get => electricitateConsum; set
        {
            if (electricitateConsum == value) return;
            electricitateConsum = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public int Zi { get => zi; set
        {
            if (zi == value) return;
            zi = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public List<int> ListProfitpeZile { get => listProfitpeZile;}
    public float ExportTotal { get => exportTotal; set
        {
            if (exportTotal == value) return;
            exportTotal = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ImportTotal { get => importTotal; set
        {
            if (importTotal == value) return;
            importTotal = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitComert { get => profitComert; set
        {
            if (profitComert == value) return;
            profitComert = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitLocuinte { get => profitLocuinte; set
        {
            if (profitLocuinte == value) return;
            profitLocuinte = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitComercial { get => profitComercial; set
        {
            if (profitComercial == value) return;
            profitComercial = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitIndustrii { get => profitIndustrii; set
        {
            if (profitIndustrii == value) return;
            profitIndustrii = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitSpitale { get => profitSpitale; set
        {
            if (profitSpitale == value) return;
            profitSpitale = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitFerme { get => profitFerme; set
        {
            if (profitFerme == value) return;
            profitFerme = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitBiserica { get => profitBiserica; set
        {
            if (profitBiserica == value) return;
            profitBiserica = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public float ProfitCladiriTotal { get => profitCladiriTotal; set
        {
            if (profitCladiriTotal == value) return;
            profitCladiriTotal = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public int ProductieGrau { get => productieGrau; set
        {
            if (productieGrau == value) return;
            productieGrau = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ProductieTutun { get => productieTutun; set
        {
            if (productieTutun == value) return;
            productieTutun = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ProductieBoabe { get => productieBoabe; set
        {
            if (productieBoabe == value) return;
            productieBoabe = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ProductieCafea { get => productieCafea; set
        {
            if (productieCafea == value) return;
            productieCafea = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ProductieTigari { get => productieTigari; set
        {
            if (productieTigari == value) return;
            productieTigari = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ProductiePaine { get => productiePaine; set
        {
            if (productiePaine == value) return;
            productiePaine = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ConsumGrau { get => consumGrau; set
        {
            if (consumGrau == value) return;
            consumGrau = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ConsumTutun { get => consumTutun; set
        {
            if (consumTutun == value) return;
            consumTutun = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ConsumBoabe { get => consumBoabe; set
        {
            if (consumBoabe == value) return;
            consumBoabe = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ConsumCafea { get => consumCafea; set
        {
            if (consumCafea == value) return;
            consumCafea = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ConsumTigari { get => consumTigari; set
        {
            if (consumTigari == value) return;
            consumTigari = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int ConsumPaine { get => consumPaine; set
        {
            if (consumPaine == value) return;
            consumPaine = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public int NrTotalDepozite { get => nrTotalDepozite; set
        {
            if (nrTotalDepozite == value) return;
            nrTotalDepozite = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public int TotalGrau { get => totalGrau; set
        {
            if (totalGrau == value) return;
            totalGrau = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int TotalTutun { get => totalTutun; set
        {
            if (totalTutun == value) return;
            totalTutun = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int TotalBoabe { get => totalBoabe; set
        {
            if (totalBoabe == value) return;
            totalBoabe = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int TotalCafea { get => totalCafea; set
        {
            if (totalCafea == value) return;
            totalCafea = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int TotalTigari { get => totalTigari; set
        {
            if (totalTigari == value) return;
            totalTigari = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int TotalPaine { get => totalPaine; set
        {
            if (totalPaine == value) return;
            totalPaine = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public int CoeficientFerme { get => coeficientFerme; set
        {
            if (coeficientFerme == value) return;
            coeficientFerme = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int CoeficientElectricitate { get => coeficientElectricitate; set
        {
            if (coeficientElectricitate == value) return;
            coeficientElectricitate = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int CoeficientIndustrie { get => coeficientIndustrie; set
        {
            if (coeficientIndustrie == value) return;
            coeficientIndustrie = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
    public int CoeficientComercial { get => coeficientComercial; set
        {
            if (coeficientComercial == value) return;
            coeficientComercial = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }

    public int PuncteCercetare { get => puncteCercetare; set
        {
            if (puncteCercetare == value) return;
            puncteCercetare = value;
            if (onDataContainerChange != null)
                onDataContainerChange();
        }
    }
}
