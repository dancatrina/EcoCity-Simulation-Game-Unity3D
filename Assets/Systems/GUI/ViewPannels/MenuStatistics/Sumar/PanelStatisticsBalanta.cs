using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelStatisticsBalanta : View
{
    public EconomyManager refEconomyeManager;

    [Header("Profitabilitate generala")]
    public TextMeshProUGUI venitTotal;
    public TextMeshProUGUI taxeTotal;
    public TextMeshProUGUI profit;

    [Header("Comert")]
    public TextMeshProUGUI exportTotal;
    public TextMeshProUGUI importTotal;
    public TextMeshProUGUI profitComert;

    [Header("Profitabilitate cladiri")]
    public TextMeshProUGUI profitLocuinte;
    public TextMeshProUGUI profitComercial;
    public TextMeshProUGUI profitIndustrii;
    public TextMeshProUGUI profitSpitale;
    public TextMeshProUGUI profitFerme;
    public TextMeshProUGUI profitBiserica;

    [Header("TOTAL")]
    public TextMeshProUGUI profitCladiriTotal;


    public override void Initialize()
    {
        refEconomyeManager.containerDate.onDataContainerChange += actualizeazaUI;
    }

    private void OnDisable()
    {
        Hide();
    }

    void actualizeazaUI()
    {
        venitTotal.text = refEconomyeManager.containerDate.Venit + " M";
        taxeTotal.text = refEconomyeManager.containerDate.Taxe + " M";
        profit.text = (refEconomyeManager.containerDate.Venit - refEconomyeManager.containerDate.Taxe) + " M";

        exportTotal.text = refEconomyeManager.containerDate.ExportTotal + " M";
        importTotal.text = refEconomyeManager.containerDate.ImportTotal + " M";
        profitComert.text = (refEconomyeManager.containerDate.ExportTotal - refEconomyeManager.containerDate.ImportTotal) + " M";


        profitLocuinte.text = refEconomyeManager.containerDate.ProfitLocuinte + " M";
        profitComercial.text = refEconomyeManager.containerDate.ProfitComercial + " M";
        profitIndustrii.text = refEconomyeManager.containerDate.ProfitIndustrii + " M";
        profitSpitale.text = refEconomyeManager.containerDate.ProfitSpitale + " M";
        profitFerme.text = refEconomyeManager.containerDate.ProfitFerme + " M";
        profitBiserica.text = refEconomyeManager.containerDate.ProfitBiserica + " M";
        profitCladiriTotal.text = refEconomyeManager.containerDate.ProfitCladiriTotal + " M";
    }
}
