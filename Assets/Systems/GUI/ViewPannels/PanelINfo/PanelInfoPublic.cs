using UnityEngine.UI;
using TMPro;

public class PanelInfoPublic : View
{
    public Button btnExit;

    public TextMeshProUGUI angajatiVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI totalPuncte;



    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
