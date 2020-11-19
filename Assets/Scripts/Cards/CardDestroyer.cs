using System.Collections.Generic;

public class CardDestroyer
{
    #region Variables

    private readonly CardsDataContainer cardsDataContainer;
    private readonly CardsHand cardsHand;

    // Some kind of kostil
    private Dictionary<CardData, CardView> map = new Dictionary<CardData, CardView>();

    #endregion


    #region Constructors

    public CardDestroyer(CardsDataContainer cardsDataContainer, CardsHand cardsHand)
    {
        this.cardsDataContainer = cardsDataContainer;
        this.cardsHand = cardsHand;
    }

    #endregion


    #region Public methods

    public void AddCards(List<CardView> cardViews)
    {
        if (cardViews == null)
        {
            return;
        }

        foreach (var cardView in cardViews)
        {
            cardView.CardData.OnHpChanged(CheckHpChange);

            map.Add(cardView.CardData, cardView);
        }
    }

    #endregion


    #region Private methods

    private void CheckHpChange(CardData cardData, int hp)
    {
        if (hp <= 0)
        {
            cardData.Kill();
            cardsDataContainer.Remove(cardData);
            cardsHand.RemoveCard(map[cardData]);
            cardsHand.PlaceAndRotate();

            map.Remove(cardData);
        }
    }

    #endregion
}
