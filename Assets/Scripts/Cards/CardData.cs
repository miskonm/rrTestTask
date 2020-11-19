using System;
using UnityEngine;

public class CardData
{
    #region Variables

    private int mana;
    private int hp;
    private int atk;
    
    private  Action<int> onManaChanged;
    private  Action<int> onHpChanged;
    private  Action<int> onAtkChanged;

    #endregion


    #region Properties

    public string Name { get; }
    public string Description { get; }
    
    public Sprite Sprite { get; }

    public int Mana
    {
        get => mana; 
        set
        {
            if (mana == value)
            {
                return;
            }

            mana = value;

            onManaChanged?.Invoke(mana);
        }
    }

    public int Hp
    {
        get => hp;
        set
        {
            if (hp == value)
            {
                return;
            }

            hp = value;

            onHpChanged?.Invoke(hp);
        }
    }

    public int Atk
    {
        get => atk;
        set
        {
            if (atk == value)
            {
                return;
            }

            atk = value;

            onAtkChanged?.Invoke(atk);
        }
    }

    #endregion


    #region Constructors

    public CardData(CardInfo cardInfo)
    {
        Name = cardInfo.CardName;
        Description = cardInfo.CardDescription;
        Sprite = cardInfo.CardSprite;

        mana = cardInfo.CardMana;
        hp = cardInfo.CardHp;
        atk = cardInfo.CardAtk;
    }

    #endregion


    #region Public methods

    public CardData OnManaChanged(Action<int> callback)
    {
        onManaChanged += callback;
        
        return this;
    }
    
    public CardData OnHpChanged(Action<int> callback)
    {
        onHpChanged += callback;
        
        return this;
    }
    
    public CardData OnAtkChanged(Action<int> callback)
    {
        onAtkChanged += callback;
        
        return this;
    }

    #endregion
}
