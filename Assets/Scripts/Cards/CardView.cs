using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    #region Variables

    [Header("Canvas")]
    [SerializeField]
    private Canvas canvas;

    [Header("Damage Canvas Group")]
    [SerializeField]
    private CanvasGroup damageCanvasGroup;

    [Header("Image")]
    [SerializeField]
    private Image mainIcon;

    [Header("Labels")]
    [SerializeField]
    private TextMeshProUGUI nameTextLabel;

    [SerializeField]
    private TextMeshProUGUI descriptionTextLabel;

    [SerializeField]
    private TextMeshProUGUI manaTextLabel;

    [SerializeField]
    private TextMeshProUGUI hpTextLabel;

    [SerializeField]
    private TextMeshProUGUI atkTextLabel;

    [Header("Transforms")]
    [SerializeField]
    private RectTransform manaRectTransform;

    [SerializeField]
    private RectTransform hpRectTransform;

    [SerializeField]
    private RectTransform atkRectTransform;

    [Header("Animation")]
    [SerializeField]
    private float inAnimationTime;

    [SerializeField]
    private float outAnimationTime;

    private RectTransform cachedRectTransform;

    #endregion


    #region Properties

    public float Width => cachedRectTransform.rect.width;

    #endregion


    #region Unity lifecycle

    private void Awake()
    {
        cachedRectTransform = GetComponent<RectTransform>();
    }

    #endregion


    #region Public methods

    public void Setup(CardData cardData)
    {
        cardData.OnManaChanged(ChangeManaValue).OnHpChanged(ChangeHpValue).OnAtkChanged(ChangeAtkValue);

        mainIcon.sprite = cardData.Sprite;

        nameTextLabel.text = cardData.Name;
        descriptionTextLabel.text = cardData.Description;
        manaTextLabel.text = cardData.Mana.ToString();
        hpTextLabel.text = cardData.Hp.ToString();
        atkTextLabel.text = cardData.Atk.ToString();
    }

    #endregion


    #region Private methods

    private void ChangeManaValue(int mana)
    {
        manaTextLabel.text = mana.ToString();
        Animate(manaRectTransform, manaTextLabel);
    }

    private void ChangeHpValue(int hp)
    {
        hpTextLabel.text = hp.ToString();
        Animate(hpRectTransform, hpTextLabel);
    }

    private void ChangeAtkValue(int atk)
    {
        atkTextLabel.text = atk.ToString();
        Animate(atkRectTransform, atkTextLabel);
    }

    private void Animate(RectTransform rectTransform, TextMeshProUGUI textLabel)
    {
        SetActiveSwitchCanvasOverrideSortingOrder(true);

        rectTransform.DOScale(2.3f, inAnimationTime).SetEase(Ease.InOutBounce).OnComplete(() =>
        {
            rectTransform.DOScale(1f, outAnimationTime).SetEase(Ease.Flash).OnComplete(() =>
            {
                SetActiveSwitchCanvasOverrideSortingOrder(false);
            });
        });

        textLabel.DOColor(Color.red, inAnimationTime).OnComplete(() =>
        {
            textLabel.DOColor(Color.white, outAnimationTime);
        });

        damageCanvasGroup.DOFade(1f, inAnimationTime).OnComplete(() =>
        {
            damageCanvasGroup.DOFade(0f, outAnimationTime);
        });
    }

    private void SetActiveSwitchCanvasOverrideSortingOrder(bool isActive)
    {
        canvas.overrideSorting = isActive;
    }

    #endregion
}
