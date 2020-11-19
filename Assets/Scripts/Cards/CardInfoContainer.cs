using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = nameof(CardInfoContainer), menuName = "Settings/CardInfoContainer")]
public class CardInfoContainer : ScriptableObject
{
    #region Variables

    [SerializeField]
    private CardInfo[] cards;

    #endregion


    #region Properties

    public int CardsCount => cards?.Length ?? 0;

    #endregion


    #region Public methods

    public CardInfo GetRandomCardInfo()
    {
        if (cards == null || cards.Length == 0)
        {
            return null;
        }

        return cards[Random.Range(0, cards.Length)];
    }

    public void SetupSprites(List<Sprite> sprites)
    {
        if (sprites == null || cards == null)
        {
            return;
        }

        var cardCount = CardsCount;

        for (int i = 0, n = sprites.Count; i < n; i++)
        {
            if (i >= cardCount)
            {
                return;
            }
            
            cards[i].SetSprite(sprites[i]);
        }
    }
    
    #endregion
}
