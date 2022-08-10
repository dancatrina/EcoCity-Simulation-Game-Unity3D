using TMPro;
using UnityEngine.UI;

public class PanelInfoReligie : View
{
    public Button btnExit;

    public TextMeshProUGUI AngajatiVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI venitVal;
    public TextMeshProUGUI taxeVal;

    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}

