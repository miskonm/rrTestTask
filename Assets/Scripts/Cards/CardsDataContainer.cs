using System.Collections.Generic;

public class CardsDataContainer
{
    #region Properties

    public List<CardData> Cards = new List<CardData>();

    #endregion


    #region Public methods

    public void SetCards(List<CardData> cards)
    {
        Cards = cards;
    }

    #endregion
}
