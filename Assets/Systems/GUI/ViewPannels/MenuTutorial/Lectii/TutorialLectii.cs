using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class TutorialLectii : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI descrierePanelRight;
    public TextMeshProUGUI selfTitle;
    public Image selfImg;

    public TextMeshProUGUI titleRightPanel;
    public Image imgRightPanel;

    [Header("GameObject")]
    public GameObject panelRight;

    [Header("Descriere")]
    public string descriereTutorail;

    public GameObject panelDefault;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            if(panelDefault.activeInHierarchy == true)
            {
                panelDefault.SetActive(false);
            }

            if(panelRight.activeInHierarchy == false)
            {
                panelRight.SetActive(true);
            }

            titleRightPanel.text = selfTitle.text;
            imgRightPanel.sprite = selfImg.sprite;
            descrierePanelRight.text = descriereTutorail;

        }
        );
    }

    private void OnDisable()
    {
        if(panelRight.activeInHierarchy == true)
        {
            panelRight.SetActive (false);
        }

        panelDefault.SetActive(true);
    }

}
