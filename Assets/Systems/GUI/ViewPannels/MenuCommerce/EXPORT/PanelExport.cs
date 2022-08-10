using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PanelExport : MonoBehaviour
{
    public Button btnAction;

    [Header("Sepcific")]
    public int cantitate;
    public int pret;

    [Header("Tipuri produse")]
    public ExportPlacedButton.tipMaterial tipMaterial;
    public EMateriePrima materiePrima;
    public EProdusIndustrial produs;

    public TextMeshProUGUI eroare;

    void exporta(int cantitate, int pret)
    {   bool error = true;
        int cantitateCurentaDinProdus = 0;
        if (tipMaterial == ExportPlacedButton.tipMaterial.MateriePrima)
        {
            cantitateCurentaDinProdus = EconomyManager.getInstance().containerResurse.getCantitateMateriePrima(materiePrima);
        }
        else if (tipMaterial == ExportPlacedButton.tipMaterial.Produs)
        {
            cantitateCurentaDinProdus = EconomyManager.getInstance().containerResurse.getCantitateProdus(produs);
        }

        if (cantitateCurentaDinProdus - cantitate >= 0)
        {
            if (tipMaterial == ExportPlacedButton.tipMaterial.MateriePrima)
            {
                EconomyManager.getInstance().containerResurse.setCantitateMateriePrima(materiePrima,
                    EconomyManager.getInstance().containerResurse.getCantitateMateriePrima(materiePrima) - cantitate);
                EconomyManager.getInstance().BaniOras += pret;
                EconomyManager.getInstance().containerDate.schimbaMateriePrima(materiePrima, EconomyManager.getInstance().containerResurse.getCantitateMateriePrima(materiePrima));
                EconomyManager.getInstance().containerDate.ExportTotal = EconomyManager.getInstance().exportTotal += pret;
                EconomyManager.getInstance().containerDate.ResurseCurente = EconomyManager.getInstance().resurseCurente = EconomyManager.getInstance().containerResurse.getTotalResurse();
                error = false;
            }
            else
                if (tipMaterial == ExportPlacedButton.tipMaterial.Produs)
            {
                EconomyManager.getInstance().containerResurse.setCantitateProdus(produs,
                EconomyManager.getInstance().containerResurse.getCantitateProdus(produs) - cantitate);
                EconomyManager.getInstance().BaniOras += pret;
                EconomyManager.getInstance().containerDate.ExportTotal = EconomyManager.getInstance().exportTotal += pret;
                EconomyManager.getInstance().containerDate.schimbaProdus(produs, EconomyManager.getInstance().containerResurse.getCantitateProdus(produs));
                EconomyManager.getInstance().containerDate.ResurseCurente = EconomyManager.getInstance().resurseCurente = EconomyManager.getInstance().containerResurse.getTotalResurse();
                error = false;
            }
        }
        if(error == true)
        {
            eroare.text = "Nu dispuneti de sufiecente resurse in depozit";
        }
        else
        {
            eroare.text = "Exportul s-a realizat cu succes";
        }

        }

    private void Start()
    {
            btnAction.onClick.AddListener(() => exporta(cantitate, pret));
    }
}
