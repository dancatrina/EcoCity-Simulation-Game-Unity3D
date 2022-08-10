using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelStatisticProductie : View
{
    public EconomyManager refEconomyeManager;

    [Header("Resurse")]
    public TextMeshProUGUI capacitate;
    public TextMeshProUGUI stoc;

    [Header("Productie")]
    public TextMeshProUGUI productieGrau;
    public TextMeshProUGUI productieTutun;
    public TextMeshProUGUI productieBoabe;
    public TextMeshProUGUI productieCafea;
    public TextMeshProUGUI productieTigari;
    public TextMeshProUGUI productiePaine;

    [Header("Cosum")]
    public TextMeshProUGUI consumGrau;
    public TextMeshProUGUI consumTutun;
    public TextMeshProUGUI consumBoabe;
    public TextMeshProUGUI consumCafea;
    public TextMeshProUGUI consumTigari;
    public TextMeshProUGUI consumPaine;

    [Header("Depozit")]
    public TextMeshProUGUI numarTotalDepozite;
    public TextMeshProUGUI textBar;
    public Image capacitateImagine;

    [Header("Total")]
    public TextMeshProUGUI totalGrau;
    public TextMeshProUGUI totalTutun;
    public TextMeshProUGUI totalBoabe;
    public TextMeshProUGUI totalCafea;
    public TextMeshProUGUI totalTigari;
    public TextMeshProUGUI totalPaine;

    public override void Initialize()
    {
        refEconomyeManager.containerDate.onDataContainerChange += actualizeazaUI;
    }

    private void OnDisable()
    {
        Hide();
    }

    public void actualizeazaUI()
    {
        capacitate.text = refEconomyeManager.containerDate.CapacitateResurse + "";
        stoc.text = refEconomyeManager.containerDate.ResurseCurente + "";

        productieGrau.text = refEconomyeManager.containerDate.ProductieGrau + "";
        productieTutun.text = refEconomyeManager.containerDate.ProductieTutun + "";
        productieBoabe.text = refEconomyeManager.containerDate.ProductieBoabe + "";
        productieCafea.text = refEconomyeManager.containerDate.ProductieCafea + "";
        productieTigari.text = refEconomyeManager.containerDate.ProductieTigari + "";
        productiePaine.text = refEconomyeManager.containerDate.ProductiePaine + "";

        consumGrau.text = refEconomyeManager.containerDate.ConsumGrau + "";
        consumTutun.text = refEconomyeManager.containerDate.ConsumTutun + "";
        consumBoabe.text = refEconomyeManager.containerDate.ConsumBoabe + "";
        consumCafea.text = refEconomyeManager.containerDate.ConsumCafea + "";
        consumTigari.text = refEconomyeManager.containerDate.ConsumTigari + "";
        consumPaine.text = refEconomyeManager.containerDate.ConsumPaine + "";
        numarTotalDepozite.text = refEconomyeManager.containerDate.NrTotalDepozite + "";

        totalGrau.text = refEconomyeManager.containerDate.TotalGrau + "";
        totalTutun.text = refEconomyeManager.containerDate.TotalTutun + "";
        totalBoabe.text = refEconomyeManager.containerDate.TotalBoabe + "";
        totalCafea.text = refEconomyeManager.containerDate.TotalCafea + "";
        totalTigari.text = refEconomyeManager.containerDate.TotalTigari + "";
        totalPaine.text = refEconomyeManager.containerDate.TotalPaine + "";

    textBar.text = refEconomyeManager.containerDate.ResurseCurente + "/" + refEconomyeManager.containerDate.CapacitateResurse;
        if (refEconomyeManager.containerDate.CapacitateResurse != 0)
        {
            capacitateImagine.fillAmount = (float)refEconomyeManager.containerDate.ResurseCurente / refEconomyeManager.containerDate.CapacitateResurse;
        }


}
}
