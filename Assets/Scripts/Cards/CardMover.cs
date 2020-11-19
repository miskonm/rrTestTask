using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMover : MonoBehaviour
{
    #region Variables

    private CardsHand cardsHand;
    private CardsDataContainer cardsDataContainer;
    private Camera cachedCamera;

    private CardView cardUnderCursor;
    private bool isCardUnderCursor;

    #endregion


    #region Unity lifecycle

    private void Awake()
    {
        cachedCamera = Camera.main;
    }

    private void OnEnable()
    {
        CardView.OnButtonClicked += CardView_OnButtonClicked;
    }

    private void OnDisable()
    {
        CardView.OnButtonClicked -= CardView_OnButtonClicked;
    }

    private void Update()
    {
        if (isCardUnderCursor)
        {
            if (Input.GetMouseButton(0))
            {
                var pos = cachedCamera.ScreenToViewportPoint(Input.mousePosition);
                pos.x *= Screen.width;
                pos.y *= Screen.height;
                
                cardUnderCursor.SetPosition(pos);
            }
            else
            {
                isCardUnderCursor = false;

                cardUnderCursor.DropCard();
                
                if (IsUnderDropZone(out var dropZone))
                {
                    cardUnderCursor.transform.parent = dropZone.transform;
                    cardsDataContainer.Remove(cardUnderCursor.CardData);
                    cardUnderCursor.DeactivateCard();
                }
                else
                {
                    cardsHand.AddCard(cardUnderCursor);
                    cardsHand.PlaceAndRotate();
                }

                cardUnderCursor = null;
            }
        }
    }

    #endregion


    #region Public methods

    public void Setup(CardsHand cardsHand,CardsDataContainer cardsDataContainer)
    {
        this.cardsHand = cardsHand;
        this.cardsDataContainer = cardsDataContainer;
    }

    #endregion


    #region Private methods

    private bool IsUnderDropZone(out GameObject dropZone)
    {
        dropZone = null;
        
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
                {position = Input.mousePosition};

        List<RaycastResult> results = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("DropZone"))
            {
                dropZone = result.gameObject;
                
                return true;
            }
        }

        return false;
    }

    #endregion
    

    #region Event handlers

    private void CardView_OnButtonClicked(CardView cardView)
    {
        if (isCardUnderCursor)
        {
            return;
        }
        
        isCardUnderCursor = true;
        cardUnderCursor = cardView;

        cardUnderCursor.TakeCard();
        cardsHand.RemoveCard(cardUnderCursor);
        cardsHand.PlaceAndRotate();
    }

    #endregion
}
