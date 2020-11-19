using System;
using UnityEngine;

public class CardData
{
    #region Variables

    private int mana;
    private int hp;
    private int atk;

    private Action<CardData, int> onManaChanged;
    private Action<CardData, int> onHpChanged;
    private Action<CardData, int> onAtkChanged;
    private Action<CardData> onKilled;

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

            onManaChanged?.Invoke(this, mana);
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

            onHpChanged?.Invoke(this, hp);
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

            onAtkChanged?.Invoke(this, atk);
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

    public CardData OnManaChanged(Action<CardData, int> callback)
    {
        onManaChanged += callback;

        return this;
    }

    public CardData OnHpChanged(Action<CardData, int> callback)
    {
        onHpChanged += callback;

        return this;
    }

    public CardData OnAtkChanged(Action<CardData, int> callback)
    {
        onAtkChanged += callback;

        return this;
    }

    public CardData OnKilled(Action<CardData> callback)
    {
        onKilled += callback;

        return this;
    }

    public void Kill()
    {
        onKilled?.Invoke(this);
    }

    #endregion
}
