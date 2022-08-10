using TMPro;
using UnityEngine.UI;
using UnityEngine;

public abstract class ATehnologie : MonoBehaviour
{   [Header("Specific Tehnologie")]
    public int costCercetare;
    public Image imagineCercetare;
    public TextMeshProUGUI textPret;
    public string descriere;

    [Header("Cercetare")]
    public bool cercetat;

    [Header("Urmatorul nod")]
    public ATehnologie anterior;

    [Header("Button")]
    public Button btn;

    public abstract void cerceteaza();

    private void Start()
    {
        btn.onClick.AddListener(() => cerceteaza());
    }

}
