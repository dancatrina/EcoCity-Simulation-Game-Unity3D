using UnityEngine.UI;
using TMPro;

public class PanelInfoLocuinte : View
{

    public Button btnExit;

    public TextMeshProUGUI LocuitoriVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI venitVal;
    public TextMeshProUGUI taxeVal;
    public TextMeshProUGUI totalVal;


    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
