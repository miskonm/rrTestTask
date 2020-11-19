using UnityEngine;

[CreateAssetMenu(fileName = "CardInfo", menuName = "Settings/CardInfo")]
public class CardInfo : ScriptableObject
{
    #region Variables

    [SerializeField]
    private string cardName;

    [SerializeField]
    private string cardDescription;

    [SerializeField]
    private int cardMana;

    [SerializeField]
    private int cardHp;

    [SerializeField]
    private int cardAtk;

    #endregion


    #region Properties

    public string CardName => cardName;
    public string CardDescription => cardDescription;
    public int CardMana => cardMana;
    public int CardHp => cardHp;
    public int CardAtk => cardAtk;
    public Sprite CardSprite { get; private set; }

    #endregion


    #region Unity lifecycle

    private void OnEnable()
    {
        CardSprite = null;
    }

    #endregion


    #region Public methods

    public void SetSprite(Sprite sprite)
    {
        CardSprite = sprite;
    }

    #endregion
}
