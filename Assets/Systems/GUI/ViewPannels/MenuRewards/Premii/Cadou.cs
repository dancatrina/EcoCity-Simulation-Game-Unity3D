using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class Cadou : MonoBehaviour
{
    [Header("Refference to object")]
    public GameObject referenceToPanelRight;

    [Header("UI")]
    public TextMeshProUGUI selfTitle;
    public TextMeshProUGUI titluPanelRight;
    public TextMeshProUGUI descrierePanelRight;
    public Image imaginePanelRight;
    public Image imagineSelf;
    public TextMeshProUGUI mesajRevendicat;

    [Header("Specific cadou")]
    public string descriere;
    public int premiu;

    [Header("ButtonPanel")]
    public Button btnPannelRight;

    [Header("Revendicat")]
    public bool revendicat;

    public GameObject defaultPanel;

    private void Start()
    {
        revendicat = false;

        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (defaultPanel.activeInHierarchy)
            {
                defaultPanel.SetActive(false);
            }

            if(referenceToPanelRight.activeInHierarchy == false)
            {
                referenceToPanelRight.SetActive(true);
            }

            titluPanelRight.text = selfTitle.text;
            descrierePanelRight.text = descriere;
            imaginePanelRight.sprite = imagineSelf.sprite;

            btnPannelRight.onClick.AddListener(() =>
           {
               if (revendicat == false)
               {
                   EconomyManager.getInstance().BaniOras += premiu;

                   revendicat = true;
                   btnPannelRight.enabled = false;
                   btnPannelRight.onClick.RemoveAllListeners();
                   mesajRevendicat.text = "Cadoul a fost revendicat";
               }
           });
        });
    }

    private void OnDisable()
    {
        if(referenceToPanelRight.activeInHierarchy == true)
        {
            referenceToPanelRight.SetActive(false);
        }

        defaultPanel.SetActive(true);
    }

}
