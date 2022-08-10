using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PanelInfoIndustrie : View
{
    public Image iconIn;
    public Image iconOut;

    [SerializeField] private Button btnExit;

    public TextMeshProUGUI muncitoriVal;
    public TextMeshProUGUI consumEnergieVal;
    public TextMeshProUGUI venitVal;
    public TextMeshProUGUI taxeVal;
    public TextMeshProUGUI inVal;
    public TextMeshProUGUI outVal;
    public TextMeshProUGUI totalVal;

    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => { Hide(); ContainerUI.getInstance().infoPanelStrategy = null; });
    }
}
