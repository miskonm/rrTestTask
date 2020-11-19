using System.Collections.Generic;
using UnityEngine;

public class CardsCreator : MonoBehaviour
{
    #region Variables

    [Header("Cards Settings")]
    [SerializeField]
    private int minCards;

    [SerializeField]
    private int maxCards;

    [Header(nameof(CardInfoContainer))]
    [SerializeField]
    private CardInfoContainer cardInfoContainer;

    #endregion


    #region Public methods

    public List<CardData> CreateCards()
    {
        var cardsCount = Random.Range(minCards, maxCards + 1);

        var cards = new List<CardData>();

        for (int i = 0; i < cardsCount; i++)
        {
            cards.Add(new CardData(cardInfoContainer.GetRandomCardInfo()));
        }

        return cards;
    }

    #endregion
}
