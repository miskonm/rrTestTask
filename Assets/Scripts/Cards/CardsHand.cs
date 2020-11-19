using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardsHand : MonoBehaviour
{
    #region Variables

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

    [SerializeField]
    private float movingAnimationTime;

    private List<CardView> cards = new List<CardView>();

    #endregion


    #region Public methods

    public void CreateCards(List<CardData> cards)
    {
        if (cards == null)
        {
            return;
        }

        InstantiateCards(cards);
        StartCoroutine(PlaceAndRotateAfterDelay());
    }

    #endregion


    #region Private methods

    private void InstantiateCards(List<CardData> cards)
    {
        for (int i = 0, n = cards.Count; i < n; i++)
        {
            var cardView = Instantiate(cardViewPrefab, cardsParentRectTransform);
            cardView.Setup(cards[i]);

            this.cards.Add(cardView);
        }
    }

    private IEnumerator PlaceAndRotateAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMove);

        PlaceAndRotate();
    }

    private void PlaceAndRotate()
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

            float cardRotation = startRotation - i * itemRotationDelta;

            card.transform.DORotate(new Vector3(0f, 0f, cardRotation), movingAnimationTime, RotateMode.FastBeyond360);

            float yPositionFromRotationOffset = Mathf.Abs(cardRotation) * yPositionFromRotationScalingFactor;
            float xPositionFromRotationOffset = cardRotation * xPositionFromRotationScalingFactor;

            var position = leftPoint + new Vector3(i * gapDelta + xPositionFromRotationOffset,
                leftPoint.y - yPositionFromRotationOffset, leftPoint.z);

            card.transform.DOLocalMove(position, movingAnimationTime);
        }
    }

    #endregion
}
