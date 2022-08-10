using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EconomyManager : MonoBehaviour
{
    private static EconomyManager instance;

    [Header("Populatie")]
    public int populatieMaxima;
    public int populatieOras;
    public int populatieCuLocuinta;
    public int populatieCuLocDeMunca;
    public float coeficientDeCrestereAPopulatiei;

    [Header("Zi")]
    public int zi;

    [Header("Bani")]
    public float baniOras;
    public delegate void onBaniOrasChangeDelegate(float val);
    public event onBaniOrasChangeDelegate onBaniOrasChange;

    public TextMeshProUGUI refTextBani;

    [Header("Economie-Specific")]
    public float consumEnergieTotal;
    public float taxeTotale;
    public float venitTotal;

    [Header("Ferme")]
    public int coeficientProductieFerme;

    [Header("Electricitate")]
    public float electricitateaTotala;
    public int coeficientProductieEnergie;

    [Header("Public")]
    public int coeficientProductiePuncteCercetare;
    public int puncteCercetare;

    [Header("Industrie")]
    public int coeficientProductieIndustrieProdus;

    [Header("Comercial")]
    public int coeficientComercialDeVenit;

    [Header("Timer Update")]
    private float timpRamas;
    public float valoareTimpStart;

    [Header("Depozit")]
    public int capacitateResurse;
    public int resurseCurente;
    public ContainerResurse containerResurse;


    public List<BuildingDepozit> listDepozit;
    public List<BuildingIndustrie> listIndustri;
    public List<BuildingComercial> listComercial;
    public List<BuildingFerme> listFerme;
    public List<BuildingElectricitate> listElectricitate;
    public List<BuildingLocuinta> listLocuinte;
    public List<BuildingSanatate> listSanatate;
    public List<BuildingPublic> listPublic;
    public List<BuildingBiserica> listReligie;

    public List<ABuilding> listABuildingCareOferaLocuriDeMunca;

    [Header("Container Date")]
    public DataContainer containerDate = new DataContainer();


    [Header("Productie")]
    public int productieGrau;
    public int productieTutun;
    public int productieBoabe;
    public int productieCafea;
    public int productieTigari;
    public int productiePaine;

    [Header("Cosum")]
    public int consumGrau;
    public int consumTutun;
    public int consumBoabe;
    public int consumCafea;
    public int consumTigari;
    public int consumPaine;

    [Header("Depozit")]
    public int numarTotalDepozite;

    void actualizeazaDateProductie()
    {
        productieGrau = listFerme.Where(x => x.MateriePrimaCatProduceSiCe.TipMateriaPrima == EMateriePrima.GRAU).Sum(y => y.MateriePrimaCatProduceSiCe.CantitateProdus);
        productieTutun = listFerme.Where(x => x.MateriePrimaCatProduceSiCe.TipMateriaPrima == EMateriePrima.TUTUN).Sum(y => y.MateriePrimaCatProduceSiCe.CantitateProdus);
        productieBoabe = listFerme.Where(x => x.MateriePrimaCatProduceSiCe.TipMateriaPrima == EMateriePrima.BOABE_CAFEA).Sum(y => y.MateriePrimaCatProduceSiCe.CantitateProdus);

        productieCafea = listIndustri.Where(x => x.CantitateaProdusaDeIndustrie.getTipProdus() == EProdusIndustrial.CAFEA).Sum(y => y.CantitateaProdusaDeIndustrie.getCantitateProdus());
        productieTigari = listIndustri.Where(x => x.CantitateaProdusaDeIndustrie.getTipProdus() == EProdusIndustrial.TIGARI).Sum(y => y.CantitateaProdusaDeIndustrie.getCantitateProdus());
        productiePaine = listIndustri.Where(x => x.CantitateaProdusaDeIndustrie.getTipProdus() == EProdusIndustrial.PAINE).Sum(y => y.CantitateaProdusaDeIndustrie.getCantitateProdus());
    
        consumGrau = listIndustri.Where(x => x.CantitateNecesaraPentruProducere.TipMateriaPrima == EMateriePrima.GRAU).Sum(y => y.CantitateNecesaraPentruProducere.CantitateProdus);
        consumTutun = listIndustri.Where(x => x.CantitateNecesaraPentruProducere.TipMateriaPrima == EMateriePrima.TUTUN).Sum(y => y.CantitateNecesaraPentruProducere.CantitateProdus);
        consumBoabe = listIndustri.Where(x => x.CantitateNecesaraPentruProducere.TipMateriaPrima == EMateriePrima.BOABE_CAFEA).Sum(y => y.CantitateNecesaraPentruProducere.CantitateProdus);
    
        consumCafea = listComercial.Where(x => x.CantitateNecesareMagazinuluiDeAVinde.getTipProdus() == EProdusIndustrial.CAFEA).Sum(y => y.CantitateNecesareMagazinuluiDeAVinde.getCantitateProdus());
        consumTigari = listComercial.Where(x => x.CantitateNecesareMagazinuluiDeAVinde.getTipProdus() == EProdusIndustrial.TIGARI).Sum(y => y.CantitateNecesareMagazinuluiDeAVinde.getCantitateProdus());
        consumPaine = listComercial.Where(x => x.CantitateNecesareMagazinuluiDeAVinde.getTipProdus() == EProdusIndustrial.PAINE).Sum(y => y.CantitateNecesareMagazinuluiDeAVinde.getCantitateProdus());

        numarTotalDepozite = listDepozit.Count;

    }

    void Start()
    {
        instance = this;

        listDepozit = new List<BuildingDepozit>();
        listIndustri = new List<BuildingIndustrie>();
        listComercial = new List<BuildingComercial>();
        listFerme = new List<BuildingFerme>();
        listElectricitate = new List<BuildingElectricitate>();
        listLocuinte = new List<BuildingLocuinta>();
        listSanatate = new List<BuildingSanatate>();
        listPublic = new List<BuildingPublic>();
        listReligie = new List<BuildingBiserica>();

        listABuildingCareOferaLocuriDeMunca = new List<ABuilding>();

        populatieCuLocuinta = 0;
        populatieCuLocDeMunca = 0;
        timpRamas = valoareTimpStart;

        refTextBani.text = baniOras + " M";

        containerResurse = new ContainerResurse();

        onBaniOrasChange += actualieazaVizualBani;
        actualizeazaContainerDate();
    }

    public static EconomyManager getInstance() => instance;


    [Header("Profitabilitate Comert")]
    public float exportTotal;
    public float importTotal;
    public float profitComert;

    [Header("Profitabilitate cladiri")]
    public float profitLocuinte;
    public float profitComercial;
    public float profitIndustrii;
    public float profitSpitale;
    public float profitFerme;
    public float profitBiserica;

    [Header("TOTAL")]
    public float profitCladiriTotal;

    void actualieazaDateProfit()
    {
        profitLocuinte = listLocuinte.Sum(p => p.getVenitCladire()) - listLocuinte.Sum(p => p.getTaxaCladire());
        profitComercial = listComercial.Sum(p => p.getVenitCladire()) - listComercial.Sum(p => p.getTaxaCladire());
        profitIndustrii = listIndustri.Sum(p => p.getVenitCladire()) - listIndustri.Sum(p => p.getTaxaCladire());
        profitSpitale = listSanatate.Sum(p => p.getVenitCladire()) - listSanatate.Sum(p => p.getTaxaCladire());
        profitFerme = listFerme.Sum(p => p.getVenitCladire()) - listFerme.Sum(p => p.getTaxaCladire());
        profitBiserica = listReligie.Sum(p => p.getVenitCladire()) - listReligie.Sum(p => p.getTaxaCladire());

        profitCladiriTotal = profitLocuinte + profitComercial + profitIndustrii + profitSpitale + profitFerme + profitBiserica;

    }

    private void Update()
    {
        if(timpRamas > 0)
        {
            timpRamas -= Time.deltaTime;

        }
        else
        {
            venitTotal = 0;
            crestePopulatia(coeficientDeCrestereAPopulatiei);
            asigeanzaPopulatieLocMunca();
            asigneazaPopulatie();
            obtineProductia();
            alocaResurseDepozit();
            electricitateaTotala = getElectricitateTotala();
            manageriazaComercial();
            ContainerUI.getInstance().invokeEvent();
            actualieazaDateProfit();
            producePuncteCercetare();
            actualizeazaDateProductie();
            actualizeazaContainerDate();
            zi++;
            timpRamas = valoareTimpStart;
            BaniOras += (venitTotal - taxeTotale);
        }
    }

    //Events
    public float BaniOras
    {
        get => baniOras;
        set
        {
            if (baniOras == value) return;
            baniOras = value;
            if (onBaniOrasChange != null)
                onBaniOrasChange(baniOras);
        }
    }

    void actualizeazaContainerDate()
    {
        containerDate.Populatie = populatieOras;
        containerDate.PopulatieCuLocuinta = populatieCuLocuinta;
        containerDate.PopulatieFaraAdapost = populatieOras - populatieCuLocuinta;
        containerDate.PopulatieAngajata = populatieCuLocDeMunca;
        containerDate.PopulatieSomera = populatieOras - populatieCuLocDeMunca;
        containerDate.Venit = venitTotal;
        containerDate.Taxe = taxeTotale;
        containerDate.CapacitateResurse = capacitateResurse;
        containerDate.ResurseCurente = resurseCurente;
        containerDate.ElectricitateCurenta = electricitateaTotala;
        containerDate.ElectricitateConsum = consumEnergieTotal;
        containerDate.Zi = zi;
        containerDate.adaugaProfit((int)venitTotal);

        containerDate.ExportTotal = exportTotal;
        containerDate.ImportTotal  =importTotal;
        containerDate.ProfitComercial=  profitComert;

        containerDate.ProfitLocuinte =  profitLocuinte;
        containerDate.ProfitComercial = profitComercial;
        containerDate.ProfitIndustrii = profitIndustrii;
        containerDate.ProfitSpitale = profitSpitale;
        containerDate.ProfitFerme = profitFerme;
        containerDate.ProfitBiserica = profitBiserica;

        containerDate.ProfitCladiriTotal = profitCladiriTotal;

        containerDate.ProductieGrau = productieGrau;
        containerDate.ProductieTutun = productieTutun;
        containerDate.ProductieBoabe = productieBoabe;

        containerDate.ProductieCafea = productieCafea;
        containerDate.ProductieTigari = productieTigari;
        containerDate.ProductiePaine = productiePaine;

        containerDate.ConsumGrau = consumGrau;
        containerDate.ConsumTutun = consumTutun;
        containerDate.ConsumBoabe = consumBoabe;

        containerDate.ConsumCafea = consumCafea;
        containerDate.ConsumTigari = consumTigari;
        containerDate.ConsumPaine = consumPaine;

        containerDate.NrTotalDepozite = numarTotalDepozite;

        containerDate.CapacitateResurse = capacitateResurse;
        containerDate.ResurseCurente = resurseCurente;

        containerDate.TotalCafea = containerResurse.getCantitateProdus(EProdusIndustrial.CAFEA);
        containerDate.TotalTigari = containerResurse.getCantitateProdus(EProdusIndustrial.TIGARI);
        containerDate.TotalPaine = containerResurse.getCantitateProdus(EProdusIndustrial.PAINE);

        containerDate.TotalBoabe = containerResurse.getCantitateMateriePrima(EMateriePrima.BOABE_CAFEA);
        containerDate.TotalGrau = containerResurse.getCantitateMateriePrima(EMateriePrima.GRAU);
        containerDate.TotalTutun = containerResurse.getCantitateMateriePrima(EMateriePrima.TUTUN);

        containerDate.PuncteCercetare = puncteCercetare;




    }

    void actualieazaVizualBani(float val) => refTextBani.text = baniOras + " M";

    void crestePopulatia(float procent)
    {
        if (populatieOras <= populatieMaxima)
        {
            populatieOras = (int)(populatieOras * procent);
        }
        else if(populatieOras > 0)
        {
            coeficientDeCrestereAPopulatiei = 1;
        }
    }


    float getElectricitateTotala()
    {
        int sum = 0;
        if (listElectricitate != null && listElectricitate.Count > 0)
        {
           for(int i =0; i< listElectricitate.Count; i++)
            {
                sum += listElectricitate[i].NumarCataElectricitatePoateProduce;
            }
            return sum;
        }
        return 0;
    }
    
    public void asigneazaPopulatie()
    {
        int auxPop = populatieOras;
        populatieCuLocuinta = 0;

        if (listLocuinte != null && listLocuinte.Count > 0)
        {
            for (int i = 0; i < listLocuinte.Count; i++)
            {
                if (auxPop - listLocuinte[i].getNumarMaximLocuitori() > 0)
                {
                    listLocuinte[i].setNumarCurentLocuitori(listLocuinte[i].getNumarMaximLocuitori());
                    auxPop -= listLocuinte[i].getNumarMaximLocuitori();
                    populatieCuLocuinta += listLocuinte[i].getNumarMaximLocuitori();
                    listLocuinte[i].setVenitCladire(listLocuinte[i].getTaxaCladire() / listLocuinte[i].getNumarMaximLocuitori() * listLocuinte[i].getNumarCurentLocuitori() * 1.5f);
                    venitTotal += listLocuinte[i].getVenitCladire();
                    listLocuinte[i].VenitTotal += listLocuinte[i].getVenitCladire();
                }
                else if (auxPop - listLocuinte[i].getNumarMaximLocuitori() <= 0)
                {
                    listLocuinte[i].setNumarCurentLocuitori(auxPop);
                    populatieCuLocuinta += auxPop;
                    auxPop -= auxPop;
                    listLocuinte[i].setVenitCladire(listLocuinte[i].getTaxaCladire() / listLocuinte[i].getNumarMaximLocuitori() * listLocuinte[i].getNumarCurentLocuitori() * 1.5f);
                    venitTotal += listLocuinte[i].getVenitCladire();
                    listLocuinte[i].VenitTotal += listLocuinte[i].getVenitCladire();
                    break;
                }
            }
        }
    }

    public void producePuncteCercetare()
    {
        if (listPublic.Count > 0)
        {
            for (int i = 0; i < listPublic.Count; i++)
            {
                listPublic[i].PuncteTotalCercetare += listPublic[i].NumarCurentAngajati * (int)(listPublic[i].NumarMaximAngajati / 1.4f) * coeficientProductiePuncteCercetare;
                containerDate.PuncteCercetare = puncteCercetare += listPublic[i].NumarCurentAngajati * (int)(listPublic[i].NumarMaximAngajati / 1.4f) * coeficientProductiePuncteCercetare;
            }
        }
    }

    public void asigeanzaPopulatieLocMunca()
    {
        int auxPop = populatieOras;
        populatieCuLocDeMunca = 0;
        if (listABuildingCareOferaLocuriDeMunca != null && listABuildingCareOferaLocuriDeMunca.Count > 0)
        {
            for (int i = 0; i < listABuildingCareOferaLocuriDeMunca.Count; i++)
            {
                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.ELECTRICITATE)
                {
                    BuildingElectricitate building = (BuildingElectricitate)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        building.NumarCataElectricitatePoateProduce = building.NumarCurentAngajati * (int)(building.NumarMaximAngajati / 1.2f) * coeficientProductieEnergie;

                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.NumarCataElectricitatePoateProduce = building.NumarCurentAngajati * (int)(building.NumarMaximAngajati / 1.2f) * coeficientProductieEnergie;
                        break;
                    }
                }
                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.RELIGIE)
                {
                    BuildingBiserica building = (BuildingBiserica)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        venitTotal += building.getVenitCladire();
                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        venitTotal += building.getVenitCladire();
                        break;
                    }
                }
                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.FERMA)
                {
                    BuildingFerme building = (BuildingFerme)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        building.MateriePrimaCatProduceSiCe.CantitateProdus = building.NumarCurentAngajati * (building.NumarMaximAngajati / 2) * coeficientProductieFerme;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        venitTotal += building.getVenitCladire();
                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        building.MateriePrimaCatProduceSiCe.CantitateProdus = building.NumarCurentAngajati * (building.NumarMaximAngajati / 2) * coeficientProductieFerme;
                        venitTotal += building.getVenitCladire();
                        break;
                    }

                }
                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.PUBLIC)
                {
                    BuildingPublic building = (BuildingPublic)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        building.PuncteTotalCercetare += building.NumarCurentAngajati * (int)(building.NumarMaximAngajati / 1.4f) * coeficientProductiePuncteCercetare;
                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.PuncteTotalCercetare += building.NumarCurentAngajati * (int)(building.NumarMaximAngajati / 1.4f) * coeficientProductiePuncteCercetare;
                        break;
                    }


                }
                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.INDUSTRIE)
                {
                    BuildingIndustrie building = (BuildingIndustrie)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        building.CantitateaProdusaDeIndustrie.setCantitateProdus(building.NumarCurentAngajati * (int)(building.NumarMaximAngajati / 0.9f) * coeficientProductieIndustrieProdus);
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        venitTotal += building.getVenitCladire();
                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.CantitateaProdusaDeIndustrie.setCantitateProdus(building.NumarCurentAngajati * (int)(building.NumarMaximAngajati / 0.9f) * coeficientProductieIndustrieProdus);
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        venitTotal += building.getVenitCladire();
                        break;
                    }
                }
                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.SANATATE)
                {
                    BuildingSanatate building = (BuildingSanatate)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        building.NouNascutiTotal += (populatieOras % 10);
                        venitTotal += building.getVenitCladire();
                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * building.NumarCurentAngajati * 1.3f);
                        building.NouNascutiTotal += (populatieOras % 10);
                        venitTotal += building.getVenitCladire();
                        break;
                    }

                }

                if (listABuildingCareOferaLocuriDeMunca[i].getTip() == ABuilding.tipCladire.COMERCIAL)
                {
                    BuildingComercial building = (BuildingComercial)listABuildingCareOferaLocuriDeMunca[i];

                    if (auxPop - building.NumarMaximAngajati > 0)
                    {
                        building.NumarCurentAngajati = building.NumarMaximAngajati;
                        auxPop -= building.NumarMaximAngajati;
                        populatieCuLocDeMunca += building.NumarMaximAngajati;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * (building.NumarCurentAngajati * 2f) * coeficientComercialDeVenit);
                        venitTotal += building.getVenitCladire();
                    }
                    else if (auxPop - building.NumarMaximAngajati <= 0)
                    {
                        building.NumarCurentAngajati = auxPop;
                        populatieCuLocDeMunca += auxPop;
                        auxPop -= auxPop;
                        building.setVenitCladire(building.getTaxaCladire() / building.NumarMaximAngajati * (building.NumarCurentAngajati * 2f) * coeficientComercialDeVenit);
                        venitTotal += building.getVenitCladire();
                        break;
                    }
                }
            }
        }
    }

    public void obtineProductia()
    {
        if (listIndustri.Count > 0 || listFerme.Count > 0)
        { // Semn aici
            if (capacitateResurse > 0 && resurseCurente <= capacitateResurse)
            {
                for (int i = 0; i < listIndustri.Count; i++)
                {
                    if (electricitateaTotala >= consumEnergieTotal)
                    {
                        if (containerResurse.getCantitateMateriePrima(listIndustri[i].CantitateNecesaraPentruProducere.TipMateriaPrima) > listIndustri[i].CantitateNecesaraPentruProducere.CantitateProdus)
                        {
                            EMateriePrima tipMateriePrima = listIndustri[i].CantitateNecesaraPentruProducere.TipMateriaPrima;
                            EProdusIndustrial tipProdusIndustrial = listIndustri[i].CantitateaProdusaDeIndustrie.getTipProdus();

                            int cantitateaNecesaraMateriePrima = listIndustri[i].CantitateNecesaraPentruProducere.CantitateProdus;
                            int cantitateaPeCareOProduceIndustria = listIndustri[i].CantitateaProdusaDeIndustrie.getCantitateProdus();

                            if (containerResurse.getTotalResurse() + cantitateaPeCareOProduceIndustria < capacitateResurse)
                            {

                                containerResurse.setCantitateMateriePrima(tipMateriePrima, containerResurse.getCantitateMateriePrima(tipMateriePrima) - cantitateaNecesaraMateriePrima);

                                containerResurse.setCantitateProdus(tipProdusIndustrial, containerResurse.getCantitateProdus(tipProdusIndustrial) + cantitateaPeCareOProduceIndustria);

                                listIndustri[i].NumarTotalDeProduseFabricate += cantitateaPeCareOProduceIndustria;
                                resurseCurente = containerResurse.getTotalResurse();
                            }
                            else if (containerResurse.getTotalResurse() + cantitateaPeCareOProduceIndustria > capacitateResurse)
                            {
                                int producMaiPutin = capacitateResurse - resurseCurente;
                                resurseCurente += producMaiPutin;

                                int calculNouMateriePrima = (cantitateaNecesaraMateriePrima * producMaiPutin) / cantitateaPeCareOProduceIndustria;

                                containerResurse.setCantitateMateriePrima(tipMateriePrima, containerResurse.getCantitateMateriePrima(tipMateriePrima) - calculNouMateriePrima); // De revizuit forumula
                                containerResurse.setCantitateProdus(tipProdusIndustrial, containerResurse.getCantitateProdus(tipProdusIndustrial) + producMaiPutin);
                                listIndustri[i].NumarTotalDeProduseFabricate += producMaiPutin;
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Nu exista curent sufiecient");
                    }
                }


                for (int i = 0; i < listFerme.Count; i++)
                {
                    if (electricitateaTotala >= consumEnergieTotal)
                    {
                        EMateriePrima tipMateriePrima = listFerme[i].MateriePrimaCatProduceSiCe.TipMateriaPrima;
                        int cantitatePeCareOProduce = listFerme[i].MateriePrimaCatProduceSiCe.CantitateProdus;
                        if (containerResurse.getTotalResurse() + cantitatePeCareOProduce < capacitateResurse)
                        {

                            containerResurse.setCantitateMateriePrima(tipMateriePrima, containerResurse.getCantitateMateriePrima(tipMateriePrima) + cantitatePeCareOProduce);


                            listFerme[i].NumarTotalDeMateriePrimaCreata += cantitatePeCareOProduce;
                            resurseCurente = containerResurse.getTotalResurse();
                        }
                        else if (containerResurse.getTotalResurse() + cantitatePeCareOProduce > capacitateResurse)
                        {
                            int producMaiPutin = capacitateResurse - resurseCurente;
                            resurseCurente += producMaiPutin;

                            int calculNouMateriePrima = (cantitatePeCareOProduce * producMaiPutin) / cantitatePeCareOProduce;

                            containerResurse.setCantitateMateriePrima(tipMateriePrima, containerResurse.getCantitateMateriePrima(tipMateriePrima) + calculNouMateriePrima); // De revizuit forumula

                            listFerme[i].NumarTotalDeMateriePrimaCreata += producMaiPutin;
                        }
                    }
                    else
                    {
                        Debug.Log("Nu exista curent suficient");
                    }
                }
            } else
            {
                Debug.Log("Nu exista depozit");
            }
        }
    }

    void manageriazaComercial()
    {
        if (listComercial.Count > 0)
        {
            if (electricitateaTotala >= consumEnergieTotal)
            {
                for (int i = 0; i < listComercial.Count; i++)
                {
                    EProdusIndustrial tipResursaFolosita = listComercial[i].CantitateNecesareMagazinuluiDeAVinde.getTipProdus();
                    int cantitateResursaFolosita = listComercial[i].CantitateNecesareMagazinuluiDeAVinde.getCantitateProdus();

                    if (containerResurse.getCantitateProdus(tipResursaFolosita) - cantitateResursaFolosita > 0)
                    {
                        containerResurse.setCantitateProdus(tipResursaFolosita, containerResurse.getCantitateProdus(tipResursaFolosita) - cantitateResursaFolosita);
                        listComercial[i].NumarTotalDeProduseVandute += cantitateResursaFolosita;
                    }
                }
            }
            else
            {
                Debug.Log("Magazinul nu are destul curent");
            }
        }
    }

    public void alocaResurseDepozit()
    {
        int auxCapacitate = resurseCurente;
        if (listDepozit != null && listDepozit.Count > 0)
        {
            for (int i = 0; i < listDepozit.Count; i++)
            {
                if (auxCapacitate - listDepozit[i].NumarMaximDeResurse> 0)
                {
                    listDepozit[i].NumarCurentDeResurse = listDepozit[i].NumarMaximDeResurse;
                    auxCapacitate -= listDepozit[i].NumarMaximDeResurse;
                }
                else if (auxCapacitate - listDepozit[i].NumarMaximDeResurse <= 0)
                {
                    listDepozit[i].NumarCurentDeResurse = auxCapacitate;
                    auxCapacitate -= auxCapacitate;
                    break;
                }
            }
        }
    }

    public void addBasedOnType(ABuilding building)
    {
        if (building.getTip() == ABuilding.tipCladire.ELECTRICITATE)
        {
            listElectricitate.Add((BuildingElectricitate)building);

            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingElectricitate refToBuilding = (BuildingElectricitate)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
               containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

            refToBuilding.NumarCataElectricitatePoateProduce = refToBuilding.NumarCurentAngajati * (int)(refToBuilding.NumarMaximAngajati / 1.2f) * coeficientProductieEnergie;
          containerDate.ElectricitateCurenta =  electricitateaTotala += refToBuilding.NumarCataElectricitatePoateProduce;
        }
        if (building.getTip() == ABuilding.tipCladire.RELIGIE)
        {
            listReligie.Add((BuildingBiserica)building);

            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingBiserica refToBuilding = (BuildingBiserica)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

            refToBuilding.setVenitCladire(refToBuilding.getTaxaCladire() / refToBuilding.NumarMaximAngajati * refToBuilding.NumarCurentAngajati * 1.7f);


        }
        if (building.getTip() == ABuilding.tipCladire.FERMA)
        {
            listFerme.Add((BuildingFerme)building);

            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingFerme refToBuilding = (BuildingFerme)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

           refToBuilding.MateriePrimaCatProduceSiCe.CantitateProdus = refToBuilding.NumarCurentAngajati * ( refToBuilding.NumarMaximAngajati / 2 ) * coeficientProductieFerme;
           refToBuilding.setVenitCladire(refToBuilding.getTaxaCladire() / refToBuilding.NumarMaximAngajati * refToBuilding.NumarCurentAngajati * 1.4f);

        }
        if (building.getTip() == ABuilding.tipCladire.PUBLIC)
        {
            listPublic.Add((BuildingPublic)building);

            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingPublic refToBuilding = (BuildingPublic)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

            refToBuilding.PuncteTotalCercetare = refToBuilding.NumarCurentAngajati * (int)(refToBuilding.NumarMaximAngajati / 1.4f) * coeficientProductiePuncteCercetare;


        }
        if (building.getTip() == ABuilding.tipCladire.INDUSTRIE)
        {
            listIndustri.Add((BuildingIndustrie)building);

            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingIndustrie refToBuilding = (BuildingIndustrie)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
              containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

            refToBuilding.CantitateaProdusaDeIndustrie.setCantitateProdus(refToBuilding.NumarCurentAngajati * (int)(refToBuilding.NumarMaximAngajati / 0.9f) * coeficientProductieIndustrieProdus);
            refToBuilding.setVenitCladire(refToBuilding.getTaxaCladire() / refToBuilding.NumarMaximAngajati * refToBuilding.NumarCurentAngajati * 1.3f);



        }
        if (building.getTip() == ABuilding.tipCladire.LOCUINTA)
        {
            listLocuinte.Add((BuildingLocuinta)building);

            BuildingLocuinta refToBuilding = (BuildingLocuinta)building;
            if (populatieCuLocuinta + refToBuilding.getNumarMaximLocuitori() <= populatieOras)
            {

                refToBuilding.setNumarCurentLocuitori(refToBuilding.getNumarMaximLocuitori());
                containerDate.PopulatieCuLocuinta = populatieCuLocuinta += refToBuilding.getNumarMaximLocuitori();
            }
            else if (populatieCuLocuinta + refToBuilding.getNumarMaximLocuitori() > populatieOras)
            {
                refToBuilding.setNumarCurentLocuitori(populatieOras - populatieCuLocuinta);
             containerDate.PopulatieCuLocuinta = populatieCuLocuinta += populatieOras - populatieCuLocuinta;

            }

            refToBuilding.setVenitCladire(refToBuilding.getTaxaCladire() / refToBuilding.getNumarMaximLocuitori() * refToBuilding.getNumarCurentLocuitori() * 1.5f);
        }
        if (building.getTip() == ABuilding.tipCladire.SANATATE)
        {
            listSanatate.Add((BuildingSanatate)building);
            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingSanatate refToBuilding = (BuildingSanatate)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

            refToBuilding.setVenitCladire(refToBuilding.getTaxaCladire() / refToBuilding.NumarMaximAngajati * (refToBuilding.NumarCurentAngajati * 1.3f));

        }
        if (building.getTip() == ABuilding.tipCladire.COMERCIAL)
        {
            listComercial.Add((BuildingComercial)building);

            listABuildingCareOferaLocuriDeMunca.Add(building);

            BuildingComercial refToBuilding = (BuildingComercial)building;
            if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati <= populatieOras)
            {

                refToBuilding.NumarCurentAngajati = refToBuilding.NumarMaximAngajati;
                containerDate.PopulatieAngajata = populatieCuLocDeMunca += refToBuilding.NumarMaximAngajati;
            }
            else if (populatieCuLocDeMunca + refToBuilding.NumarMaximAngajati > populatieOras)
            {
                refToBuilding.NumarCurentAngajati = populatieOras - populatieCuLocDeMunca;
              containerDate.PopulatieAngajata = populatieCuLocDeMunca += populatieOras - populatieCuLocDeMunca;

            }

            refToBuilding.setVenitCladire(refToBuilding.getTaxaCladire() / refToBuilding.NumarMaximAngajati * (refToBuilding.NumarCurentAngajati * 2f) * coeficientComercialDeVenit);
        }
        if((building.getTip() == ABuilding.tipCladire.DEPOZIT)){
            BuildingDepozit refToBuilding = (BuildingDepozit)building;
            listDepozit.Add(refToBuilding);
           containerDate.CapacitateResurse = capacitateResurse += refToBuilding.NumarMaximDeResurse;
        }

       containerDate.ElectricitateConsum = consumEnergieTotal += building.getConsumElectricitate();
       containerDate.Taxe = taxeTotale += building.getTaxaCladire();
       containerDate.Venit = venitTotal += building.getVenitCladire();
    }

    public void removeBuilding(ABuilding building)
    {
        if (building.getTip() == ABuilding.tipCladire.ELECTRICITATE)
        {
            BuildingElectricitate aux = (BuildingElectricitate)building;
           
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;

            listElectricitate.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);

        }
        if (building.getTip() == ABuilding.tipCladire.RELIGIE)
        {
            BuildingBiserica aux = (BuildingBiserica)building;
            
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;
            containerDate.Venit = venitTotal -= aux.getVenitCladire();
            containerDate.Taxe = taxeTotale -= aux.getTaxaCladire();
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();
            
            listReligie.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);
        }
        if (building.getTip() == ABuilding.tipCladire.FERMA)
        {
            BuildingFerme aux = (BuildingFerme)building;
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;
            containerDate.Venit = venitTotal -= aux.getVenitCladire();
            containerDate.Taxe = taxeTotale -= aux.getTaxaCladire();
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();

            listFerme.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);

        }
        if (building.getTip() == ABuilding.tipCladire.PUBLIC)
        {
            BuildingPublic aux = (BuildingPublic)building;
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();

            listPublic.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);
        }
        if (building.getTip() == ABuilding.tipCladire.INDUSTRIE)
        {
            BuildingIndustrie aux = (BuildingIndustrie)building;
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();
            containerDate.Taxe = taxeTotale -= aux.getTaxaCladire();
            containerDate.Venit = venitTotal -= aux.getVenitCladire();

            listIndustri.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(aux);
        }
        if (building.getTip() == ABuilding.tipCladire.LOCUINTA)
        {
            BuildingLocuinta aux =  (BuildingLocuinta)building;
            containerDate.Venit = venitTotal -= aux.getVenitCladire();
            containerDate.Taxe = taxeTotale -= aux.getTaxaCladire();
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();
            containerDate.PopulatieCuLocuinta = populatieCuLocuinta -= aux.getNumarCurentLocuitori();

            listLocuinte.Remove(aux);
        }
        if (building.getTip() == ABuilding.tipCladire.SANATATE)
        {
            BuildingSanatate aux = (BuildingSanatate)building;
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();
            containerDate.Venit = venitTotal -= aux.getVenitCladire();
            containerDate.Taxe = taxeTotale -= aux.getTaxaCladire();

            listSanatate.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);
        }
        if (building.getTip() == ABuilding.tipCladire.COMERCIAL)
        {
           BuildingComercial aux = (BuildingComercial)building;
            containerDate.PopulatieAngajata = populatieCuLocDeMunca -= aux.NumarCurentAngajati;
            containerDate.ElectricitateConsum = consumEnergieTotal -= aux.getConsumElectricitate();
            containerDate.Venit = venitTotal -= aux.getVenitCladire();
            containerDate.Taxe = taxeTotale -= aux.getTaxaCladire();
            
            listComercial.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);

        }
        if ((building.getTip() == ABuilding.tipCladire.DEPOZIT))
        {
            BuildingDepozit aux = (BuildingDepozit)building;
            taxeTotale -= aux.getTaxaCladire();
            consumEnergieTotal -= aux.getConsumElectricitate();
            containerResurse.eliminaResurse(aux.NumarCurentDeResurse);
             

            resurseCurente = aux.NumarCurentDeResurse;
            containerDate.ResurseCurente = resurseCurente = containerResurse.getTotalResurse();
            containerDate.CapacitateResurse = capacitateResurse = capacitateResurse - aux.NumarMaximDeResurse;

            listDepozit.Remove(aux);
            listABuildingCareOferaLocuriDeMunca.Remove(building);

        }
    }
}

