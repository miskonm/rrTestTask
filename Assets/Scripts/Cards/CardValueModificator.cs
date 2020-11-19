using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardValueModificator : MonoBehaviour
{
    #region Variables


    [SerializeField]
    private int minCardsNumber;

    [SerializeField]
    private int maxCardsNumber;

    private CardsDataContainer cardsDataContainer;

    #endregion


    #region Public methods

    public void Setup(CardsDataContainer cardsDataContainer)
    {
        this.cardsDataContainer = cardsDataContainer;
    }

    public void ModifyCardsSequentially(Action completeCallback)
    {
        if (cardsDataContainer.Cards == null || cardsDataContainer.Cards.Count == 0)
        {
            completeCallback?.Invoke();
            
            return;
        }
        
        StartCoroutine(ModifyCards(completeCallback));
    }

    #endregion


    #region Private methods

    private IEnumerator ModifyCards(Action completeCallback)
    {
        var cardsToModifyCount = Random.Range(minCardsNumber, maxCardsNumber + 1);
        var cardIndex = 0;

        for (int i = 0; i < cardsToModifyCount; i++)
        {
            cardIndex = GetValidIndex(cardIndex);
            ModifyCardData(cardsDataContainer.Cards[cardIndex]);
            
            yield return new WaitForSeconds(1f);

            cardIndex++;
        }
        
        completeCallback?.Invoke();
    }

    private int GetValidIndex(int currentIndex)
    {
        if (currentIndex < 0 || currentIndex >= cardsDataContainer.Cards.Count)
        {
            return 0;
        }

        return currentIndex;
    }

    private void ModifyCardData(CardData cardData)
    {
        var randomDmgValue = Random.Range(0, 3);

        switch (randomDmgValue)
        {
            case 0:
            {
                cardData.Mana--;
                
                break;
            }
            case 1:
            {
                cardData.Hp--;
                
                break;
            }
            case 2:
            {
                cardData.Atk--;
                
                break;
            }
        }
    }

    #endregion
}
