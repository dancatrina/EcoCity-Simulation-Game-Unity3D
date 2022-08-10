using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class PanelStatisticsSumar : View
{
    public EconomyManager refEconomyeManager;

    [Header("Total")]
    public TextMeshProUGUI populatie;
    public TextMeshProUGUI populatieCuLocuinta;
    public TextMeshProUGUI populatieFaraAdapost;
    public TextMeshProUGUI populatieAngajata;
    public TextMeshProUGUI populatieSomera;

    public TextMeshProUGUI venit;
    public TextMeshProUGUI taxe;

    public TextMeshProUGUI capacitateResurse;
    public TextMeshProUGUI resurseCurente;

    public TextMeshProUGUI electricitateCurenta;
    public TextMeshProUGUI electricitateConsum;

    [Header("Charts")]
    public PieGraph refToChartLocuitori;
    public PieGraph refToChartAngajati;


    [Header("Grafic line")]
    public Window_Graph graficLinie;

    public override void Initialize()
    {
        refEconomyeManager.containerDate.onDataContainerChange += actualieazaUiTotal;
        refEconomyeManager.containerDate.onDataContainerChange += showPieGraph;
        refEconomyeManager.containerDate.onDataContainerChange += afiseazaGraficLinie;
    }

    private void OnEnable()
    {
        actualieazaUiTotal();
        showPieGraph();
        afiseazaGraficLinie();
    }

    private void OnDisable()
    {
        Hide();
    }

    public void showPieGraph()
    {
        float populatieCuLocuinta = refEconomyeManager.containerDate.PopulatieCuLocuinta;
        float populatieFaraAdapost = refEconomyeManager.containerDate.PopulatieFaraAdapost;
        refToChartLocuitori.showGraph(populatieCuLocuinta, populatieFaraAdapost);

        float populatieAngajata = refEconomyeManager.containerDate.PopulatieAngajata;
        float populatieSomera = refEconomyeManager.containerDate.PopulatieSomera;
        refToChartAngajati.showGraph(populatieAngajata, populatieSomera);
    }

    public void afiseazaGraficLinie()
    {
        if (gameObject.activeInHierarchy == true)
        {
            List<int> venituriAux = refEconomyeManager.containerDate.ListProfitpeZile;
            if (venituriAux.Count <= 7)
            {
                graficLinie.afiseazaGrafic(refEconomyeManager.containerDate.ListProfitpeZile);
            }
            else
            {
                for (int i = 0; i < venituriAux.Count; i++)
                {
                    graficLinie.UpdateValue(i, venituriAux[i]);
                }
            }
        }
        }

    public void actualieazaUiTotal()
    {
        populatie.text = refEconomyeManager.containerDate.Populatie + "";
        populatieCuLocuinta.text = refEconomyeManager.containerDate.PopulatieCuLocuinta + "";
        populatieFaraAdapost.text = refEconomyeManager.containerDate.PopulatieFaraAdapost + "";
        populatieAngajata.text = refEconomyeManager.containerDate.PopulatieAngajata + "";
        populatieSomera.text = refEconomyeManager.containerDate.PopulatieSomera + "";
        venit.text = refEconomyeManager.containerDate.Venit + " M";
        taxe.text = refEconomyeManager.containerDate.Taxe + " M";
        capacitateResurse.text = refEconomyeManager.containerDate.CapacitateResurse + "";
        resurseCurente.text = refEconomyeManager.containerDate.ResurseCurente + "";
        electricitateCurenta.text = refEconomyeManager.containerDate.ElectricitateCurenta + "";
        electricitateConsum.text = refEconomyeManager.containerDate.ElectricitateConsum + "";
    }

}
