using UnityEngine.UI;
using TMPro;

public class PanelInfoFerma : View
{
    public Button btnExit;

    public TextMeshProUGUI AngajatiVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI venitVal;
    public TextMeshProUGUI taxeVal;
    public TextMeshProUGUI totalVal;

    public Image imgMateriePrima;
    public TextMeshProUGUI outMateriePrima;


    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
