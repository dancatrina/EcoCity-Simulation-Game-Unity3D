using TMPro;
using UnityEngine;
using UnityEngine.UI;
public abstract class AObiectiv : MonoBehaviour
{
    [Header("Specific clasa")]
    public int castig;
    public string descriere;
    public int obiectiv;
    public string descriereObiectiv;

    [Header("UI")]
    public Image iconPanelRight;
    public Image currentIcon;
    public TextMeshProUGUI currentText;
    public TextMeshProUGUI textPanelTitle;
    public TextMeshProUGUI descrierePanelRight;
    public TextMeshProUGUI obiectivPanelRight;

    [Header("GameObject")]
    public GameObject obj;
    public GameObject defaultPanel;

    [Header("Completat")]
    public TextMeshProUGUI completat;

    public bool revendicat = false;
    void Start()
    {

        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if(obj.activeInHierarchy == false)
            {
                obj.SetActive(true);
            }

            if(defaultPanel.activeInHierarchy == true)
            {
                defaultPanel.SetActive(false);
            }

            iconPanelRight.sprite = currentIcon.sprite;
             textPanelTitle.text = currentText.text;
            obiectivPanelRight.text = descriereObiectiv + "";
            descrierePanelRight.text = descriere;

            if(revendicat == true)
            {
                completat.text = "COMPLETAT";
            }else
            {
                completat.text = "";
            }


        });

        EconomyManager.getInstance().containerDate.onDataContainerChange += verificaObiectiv;
    }

    public abstract void verificaObiectiv();

    private void OnDisable()
    {
            obj.SetActive(false);
      
    }
}
