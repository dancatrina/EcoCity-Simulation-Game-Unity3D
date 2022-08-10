using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PanelInfoComercial : View
{
    public Button btnExit;

    public TextMeshProUGUI AngajatiVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI venitVal;
    public TextMeshProUGUI taxeVal;
    public TextMeshProUGUI totalVal;

    public Image imgProdus;
    public TextMeshProUGUI inProdus;

    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
