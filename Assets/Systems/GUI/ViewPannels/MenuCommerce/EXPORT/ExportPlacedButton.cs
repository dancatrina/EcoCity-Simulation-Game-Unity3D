using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExportPlacedButton : MonoBehaviour
{
    public GameObject refToInfoPanel;

    [Header("TextMesh")]
    public TextMeshProUGUI cantitate;
    public TextMeshProUGUI pret;

    public Image imageTip;
    public TextMeshProUGUI titluTabel;

    [Header("Detalii Produs")]
    public int cantitateProdus;
    public int pretProdus;

    public enum tipMaterial
    {
        Produs,
        MateriePrima
    }

    public tipMaterial tip;

    [Header("Materie prima")]
    public EMateriePrima materiePrima;
    public EProdusIndustrial produs;

    [Header("intern")]
    public TextMeshProUGUI textTinutDeButton;
    public TextMeshProUGUI descriere;
    public string descriereActuala;

    private void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (refToInfoPanel.activeInHierarchy == false)
            {
                refToInfoPanel.SetActive(true);
            }

            if (tip == tipMaterial.Produs)
            {
                imageTip.sprite = ContainerUI.getInstance().getProdusSprite(produs);
            }
            else
            {
                imageTip.sprite = ContainerUI.getInstance().getMateriePrimaSprite(materiePrima);
            }

            titluTabel.text = textTinutDeButton.GetParsedText();
            cantitate.text = cantitateProdus + " ";
            pret.text = pretProdus + " M";

            PanelExport panel = refToInfoPanel.GetComponent<PanelExport>();
            PanelImport panelImport = refToInfoPanel.GetComponent<PanelImport>();

            if (panel != null)
            {
                panel.cantitate = cantitateProdus;
                panel.pret = pretProdus;
                panel.materiePrima = materiePrima;
                panel.produs = produs;
                panel.tipMaterial = tip;
                descriere.text = descriereActuala;
                panel.eroare.text = "";
            }else if(panelImport != null)
            {
                panelImport.cantitate = cantitateProdus;
                panelImport.pret = pretProdus;
                panelImport.materiePrima = materiePrima;
                panelImport.produs = produs;
                panelImport.tipMaterial = tip;

                descriere.text = descriereActuala;

                panelImport.eroare.text = "";
            }

        });

    }

    private void OnDisable()
    {
        if(refToInfoPanel.activeInHierarchy == true)
        {
            refToInfoPanel.SetActive(false);

        }
    }


}
