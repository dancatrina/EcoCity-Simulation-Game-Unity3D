using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;

public class Achievement : MonoBehaviour
{
   [SerializeField] private string titleAchiev;
   [SerializeField] private string descriptionAchiev;
   [SerializeField] private Sprite spriteAchiev;

    private Image refToIcon;
    private TMP_Text refToTitleText;
    private TMP_Text refToDescriptionText;
    public GameObject refToParent;

    [Header("conditie")]
    public int conditie;
    public AConditieAchiev conditieAchiev;

    public string TitleAchiev { get => titleAchiev; set => titleAchiev = value; }
    public string DescriptionAchiev { get => descriptionAchiev; set => descriptionAchiev = value; }
    public Sprite SpriteAchiev { get => spriteAchiev; set => spriteAchiev = value; }

    [System.Obsolete]
    public void init(Image icon, TMP_Text title, TMP_Text description, GameObject parent)
    {
        if (icon == null) throw new System.ArgumentNullException(nameof(icon));
        if (title == null) throw new System.ArgumentNullException(nameof(title));
        if (description == null) throw new System.ArgumentNullException(nameof(description));
        if (parent == null) throw new System.ArgumentNullException(nameof(parent));

        refToIcon = icon;
        refToTitleText = title;
        refToDescriptionText = description;
        refToParent = parent;

        if (titleAchiev == null) throw new System.NullReferenceException(nameof(titleAchiev));
        if (description == null) throw new System.NullReferenceException(nameof(descriptionAchiev));
        if (spriteAchiev == null) throw new System.NullReferenceException(nameof(spriteAchiev));

        Button btnSender = gameObject.GetComponent<Button>();
        if (btnSender == null) throw new System.NullReferenceException("btnSender is null");
        btnSender.onClick.AddListener(() => sendEventAchiev());

        FunctionTimer.Create(() =>
        {
            AchievSystem.Instance.listAchievs.Add(this);
            conditieAchiev = selecteazaConditie(tipAcv);
        },1f);
        }

    private void Start()
    {


    }

    void executaActiune()
    {
        conditieAchiev.actiune(conditie);
    }

    [System.Obsolete]
    private void sendEventAchiev()
    {
        if (!refToParent.active)
        {
            refToParent.SetActive(true);
        }

        refToIcon.sprite = spriteAchiev;
        refToTitleText.text = titleAchiev;
        refToDescriptionText.text = descriptionAchiev;
    }

    public enum tipAchiev { 
        NONE,
        ZiuaPlatii,
        RegeleRecoltei,
        EraIndustriala
    }

    [Header("TIP")]
    public tipAchiev tipAcv;
    private AConditieAchiev selecteazaConditie(tipAchiev tip)
    {
        if (tip == tipAchiev.ZiuaPlatii) return new ConditieAchievBani();
        if (tip == tipAchiev.EraIndustriala) return new ConditieIndustrial();
        if (tip == tipAchiev.RegeleRecoltei) return new ConditieAchievRecolta();
        return null;
    }

    private void OnDisable()
    {
        if (refToParent.activeInHierarchy == true)
        {
            refToParent.SetActive(false);
        }
    }
}
