using UnityEngine.UI;
using TMPro;

public class PanelInfoSanatate : View
{
    public Button btnExit;

    public TextMeshProUGUI AngajatiVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI venitVal;
    public TextMeshProUGUI taxeVal;
    public TextMeshProUGUI totalNounascutiVal;

    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
