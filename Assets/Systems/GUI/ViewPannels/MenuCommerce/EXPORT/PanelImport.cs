using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PanelImport : MonoBehaviour
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

    void importa(int cantitate, int pret)
    {
        int error = 1;
        if (EconomyManager.getInstance().baniOras - pret > 0)
        {
            if (cantitate + EconomyManager.getInstance().resurseCurente <= EconomyManager.getInstance().capacitateResurse)
            {
                if (tipMaterial == ExportPlacedButton.tipMaterial.MateriePrima)
                {
                    EconomyManager.getInstance().containerResurse.setCantitateMateriePrima(materiePrima,
                        EconomyManager.getInstance().containerResurse.getCantitateMateriePrima(materiePrima) + cantitate);
                    EconomyManager.getInstance().BaniOras -= pret;
                    EconomyManager.getInstance().importTotal = EconomyManager.getInstance().containerDate.ImportTotal += pret;
                    EconomyManager.getInstance().containerDate.schimbaMateriePrima(materiePrima, EconomyManager.getInstance().containerResurse.getCantitateMateriePrima(materiePrima));
                    EconomyManager.getInstance().containerDate.ResurseCurente = EconomyManager.getInstance().resurseCurente = EconomyManager.getInstance().containerResurse.getTotalResurse();
                }
                else
                    if (tipMaterial == ExportPlacedButton.tipMaterial.Produs)
                {
                    EconomyManager.getInstance().containerResurse.setCantitateProdus(produs,
                    EconomyManager.getInstance().containerResurse.getCantitateProdus(produs) + cantitate);
                   EconomyManager.getInstance().importTotal =  EconomyManager.getInstance().containerDate.ImportTotal += pret;
                    EconomyManager.getInstance().containerDate.schimbaProdus(produs, EconomyManager.getInstance().containerResurse.getCantitateProdus(produs));
                    EconomyManager.getInstance().BaniOras -= pret;
                    EconomyManager.getInstance().containerDate.ResurseCurente = EconomyManager.getInstance().resurseCurente = EconomyManager.getInstance().containerResurse.getTotalResurse();

                }
            }
            else
            {
                error = 0;
            }
        }
        else
        {
            error = 2;
        }

        if(error == 0)
        {
            eroare.text = "Nu dispuneti de destul spatiu de depozitare";
        }else if( error == 1)
        {
            eroare.text = "Importul s-a realizat cu succes";
        }else if(error == 2)
        {
            eroare.text = "Fonduri insuficiente";
        }

    }

    private void Start()
    {
        btnAction.onClick.AddListener(() => importa(cantitate, pret));
    }
}
