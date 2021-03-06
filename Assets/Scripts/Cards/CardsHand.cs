﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardsHand : MonoBehaviour
{
    #region Variables

    [Header("Main Canvas Container")]
    [SerializeField]
    private RectTransform mainCanvasRectTransform;

    [Header("Prefab")]
    [SerializeField]
    private CardView cardViewPrefab;

    [Header("Hand")]
    [SerializeField]
    private RectTransform cardsParentRectTransform;

    [SerializeField]
    private RectTransform leftLimitRectTransform;

    [SerializeField]
    private RectTransform rightLimitRectTransform;

    [Header("Rotation")]
    [SerializeField]
    private int minZRotation;

    [SerializeField]
    private int maxZRotation;

    [SerializeField]
    private float yPositionFromRotationScalingFactor = 0.1f;

    [SerializeField]
    private float xPositionFromRotationScalingFactor = 0.1f;

    [Header("Animation")]
    [SerializeField]
    private float delayBeforeMove = 1f;

    private List<CardView> cards = new List<CardView>();

    #endregion


    #region Public methods

    public List<CardView> CreateCards(List<CardData> cards)
    {
        if (cards == null)
        {
            return null;
        }

        InstantiateCards(cards);
        StartCoroutine(PlaceAndRotateAfterDelay());

        return this.cards;
    }

    public void PlaceAndRotate()
    {
        var leftPoint = leftLimitRectTransform.localPosition;
        var rightPoint = rightLimitRectTransform.localPosition;
        var delta = (rightPoint - leftPoint).magnitude;
        var count = cards.Count;
        var gapDelta = count - 1 == 0 ? 0 : delta / (count - 1);

        var rotationDelta = Mathf.Abs(minZRotation) + Mathf.Abs(maxZRotation);
        var itemRotationDelta = count - 1 == 0 ? 0 : rotationDelta / (count - 1);
        float startRotation = rotationDelta * 0.5f;

        for (int i = 0; i < count; i++)
        {
            var card = cards[i];
            card.SetIndex(i);
            
            card.transform.SetSiblingIndex(i);

            float cardRotation = startRotation - i * itemRotationDelta;

            card.DoRotate(new Vector3(0f, 0f, cardRotation));

            float yPositionFromRotationOffset = Mathf.Abs(cardRotation) * yPositionFromRotationScalingFactor;
            float xPositionFromRotationOffset = cardRotation * xPositionFromRotationScalingFactor;

            var position = leftPoint + new Vector3(i * gapDelta + xPositionFromRotationOffset,
                leftPoint.y - yPositionFromRotationOffset, leftPoint.z);

            card.DoMove(position);
        }
    }

    public void RemoveCard(CardView cardView)
    {
        cardView.transform.parent = mainCanvasRectTransform;
        cards.RemoveAt(cardView.Index);
    }

    public void AddCard(CardView cardView)
    {
        cardView.transform.parent = cardsParentRectTransform;
        cards.Insert(cardView.Index, cardView);
    }

    #endregion


    #region Private methods

    private void InstantiateCards(List<CardData> cards)
    {
        for (int i = 0, n = cards.Count; i < n; i++)
        {
            var cardView = Instantiate(cardViewPrefab, cardsParentRectTransform);
            cardView.Setup(cards[i]);
            cardView.SetIndex(i);

            this.cards.Add(cardView);
        }
    }

    private IEnumerator PlaceAndRotateAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        PlaceAndRotate();
    }

    #endregion
}
