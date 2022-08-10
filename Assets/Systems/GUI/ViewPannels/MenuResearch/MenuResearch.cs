using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuResearch : View
{
    [SerializeField] Button btnExit;
    public TextMeshProUGUI punctaj;
    public override void Initialize()
    {
        btnExit.onClick.AddListener(() => Hide());
    }

    void actualizeazaUIZ()
    {
        punctaj.text = EconomyManager.getInstance().puncteCercetare + "";
    }


    private void Start()
    {
        EconomyManager.getInstance().containerDate.onDataContainerChange += actualizeazaUIZ;
    }

    private void OnEnable()
    {
        actualizeazaUIZ();
    }

}
