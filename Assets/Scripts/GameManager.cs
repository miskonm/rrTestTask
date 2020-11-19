using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Variables

    [Header("Loader Object")]
    [SerializeField]
    private GameObject loaderGameObject;

    [Header(nameof(TexturesLoader))]
    [SerializeField]
    private TexturesLoader texturesLoader;

    [Header(nameof(CardInfoContainer))]
    [SerializeField]
    private CardInfoContainer cardInfoContainer;

    [Header(nameof(CardsCreator))]
    [SerializeField]
    private CardsCreator cardsCreator;

    [Header(nameof(CardsHand))]
    [SerializeField]
    private CardsHand cardsHand;

    [Header("Damage Button")]
    [SerializeField]
    private Button damageButton;

    [Header(nameof(CardValueModificator))]
    [SerializeField]
    private CardValueModificator cardValueModificator;
    
    [Header(nameof(CardMover))]
    [SerializeField]
    private CardMover cardMover;

    private CardsDataContainer cardsDataContainer;
    private CardDestroyer cardDestroyer;

    #endregion


    #region Unity lifecycle

    private void Awake()
    {
        cardsDataContainer = new CardsDataContainer();
        
        cardValueModificator.Setup(cardsDataContainer);
        cardMover.Setup(cardsHand, cardsDataContainer);
        
        cardDestroyer = new CardDestroyer(cardsDataContainer, cardsHand);

        SetActiveLoader(true);
        SetActiveDamageButton(false);

        LoadSprites(() =>
        {
            SetActiveLoader(false);
            SetActiveDamageButton(true);

            CreateCardsData();
            
            cardDestroyer.AddCards(InstantiateCardViews());
        });
    }

    private void OnEnable()
    {
        damageButton.onClick.AddListener(DamageButtonClicked);
    }

    private void OnDisable()
    {
        damageButton.onClick.RemoveListener(DamageButtonClicked);
    }

    #endregion


    #region Private methods

    private void LoadSprites(Action completeCallback)
    {
        texturesLoader.LoadTextures(cardInfoContainer.CardsCount, sprites =>
        {
            cardInfoContainer.SetupSprites(sprites);

            completeCallback?.Invoke();
        });
    }

    private void SetActiveLoader(bool isActive)
    {
        loaderGameObject.SetActive(isActive);
    }

    private void SetActiveDamageButton(bool isActive)
    {
        damageButton.gameObject.SetActive(isActive);
    }

    private void CreateCardsData()
    {
        cardsDataContainer.SetCards(cardsCreator.CreateCards());
    }

    private List<CardView> InstantiateCardViews()
    {
        var cards = cardsDataContainer.Cards;

        if (cards == null)
        {
            return null;
        }

        return cardsHand.CreateCards(cards);
    }

    private void DamageButtonClicked()
    {
        SetActiveDamageButton(false);

        StartDamagingCards(() => SetActiveDamageButton(true));
    }

    private void StartDamagingCards(Action completeCallback = null)
    {
        cardValueModificator.ModifyCardsSequentially(completeCallback);
    }

    #endregion
}
