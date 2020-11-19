using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardView : MonoBehaviour, IPointerDownHandler
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

    [SerializeField]
    private float movingAnimationTime;

    [SerializeField]
    private float killAnimationTime;

    [Header("Shine")]
    [SerializeField]
    private CanvasGroup shineCanvasGroup;

    private RectTransform cachedRectTransform;

    private Tweener moveTweener;
    private Tweener rotateTweener;

    private bool isCardAlive = true;

    #endregion


    #region Events

    public static event Action<CardView> OnButtonClicked;

    #endregion


    #region Properties

    public int Index { get; private set; }
    public CardData CardData { get; private set; }

    #endregion


    #region Unity lifecycle

    private void Awake()
    {
        cachedRectTransform = GetComponent<RectTransform>();
    }

    #endregion


    #region Public methods

    public void SetPosition(Vector3 position)
    {
        cachedRectTransform.position = position;
    }

    public void Setup(CardData cardData)
    {
        CardData = cardData;

        cardData.OnManaChanged(ChangeManaValue).OnHpChanged(ChangeHpValue).OnAtkChanged(ChangeAtkValue)
               .OnKilled(KillCard);

        mainIcon.sprite = cardData.Sprite;

        nameTextLabel.text = cardData.Name;
        descriptionTextLabel.text = cardData.Description;
        manaTextLabel.text = cardData.Mana.ToString();
        hpTextLabel.text = cardData.Hp.ToString();
        atkTextLabel.text = cardData.Atk.ToString();
    }

    public void SetIndex(int index)
    {
        Index = index;
    }

    public void TakeCard()
    {
        SetActiveSwitchCanvasOverrideSortingOrder(true);

        moveTweener?.Kill();
        rotateTweener?.Kill();

        shineCanvasGroup.alpha = 1f;

        cachedRectTransform.localRotation = Quaternion.identity;
    }

    public void DropCard()
    {
        shineCanvasGroup.alpha = 0f;

        SetActiveSwitchCanvasOverrideSortingOrder(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isCardAlive)
        {
            return;
        }

        OnButtonClicked?.Invoke(this);
    }

    public void DoMove(Vector3 localPosition)
    {
        moveTweener = cachedRectTransform.DOLocalMove(localPosition, movingAnimationTime);
    }

    public void DoRotate(Vector3 eulerAngle)
    {
        rotateTweener = cachedRectTransform.DORotate(eulerAngle, movingAnimationTime);
    }

    public void DeactivateCard()
    {
        isCardAlive = false;
    }

    #endregion


    #region Private methods

    private void ChangeManaValue(CardData cardData, int mana)
    {
        manaTextLabel.text = mana.ToString();
        Animate(manaRectTransform, manaTextLabel);
    }

    private void ChangeHpValue(CardData cardData, int hp)
    {
        if (hp <= 0)
        {
            return;
        }

        hpTextLabel.text = hp.ToString();
        Animate(hpRectTransform, hpTextLabel);
    }

    private void ChangeAtkValue(CardData cardData, int atk)
    {
        atkTextLabel.text = atk.ToString();
        Animate(atkRectTransform, atkTextLabel);
    }

    private void KillCard(CardData cardData)
    {
        DeactivateCard();

        cachedRectTransform.DOScale(0f, killAnimationTime).OnComplete(() => Destroy(gameObject));
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
