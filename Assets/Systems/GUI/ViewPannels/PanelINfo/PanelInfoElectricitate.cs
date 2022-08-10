using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class PanelInfoElectricitate : View
{
    public Button btnExit;

    public TextMeshProUGUI AngajatiVal;
    public TextMeshProUGUI taxeVal;
    public TextMeshProUGUI totalVal;

    public TextMeshProUGUI outElectricitate;

    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
