using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelInfoDepozit : View
{

    [SerializeField] private Button btnExit;

    public TextMeshProUGUI consumEnergie;
    public TextMeshProUGUI taxe;
    public TextMeshProUGUI totalResurse;
    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }

}
