using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;
using Lean.Touch;

public class CardZoomer : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private CardOnlyDisplay _cardToDisplayInfo;
    [SerializeField] private GameObject gameObjectToHide;
    [SerializeField] private LeanSelectByFinger _selector;
    private Vector3 _newScale;

    private Card _card;

    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;
    public AudioClip zoomBeginSound;

    private void Start()
    {
        _newScale = new Vector3(1.5f, 1.5f, 1.5f);
        _cardToDisplayInfo.ChangeVisibility(false);
        _mainCamera = Camera.main;
    }
    public void OnCardTapped()
    {

    }

    private void Update()
    {
        switch (_selector.Selectables.Count)
        {
            //If we have nothing selected
            case 0:
                //And if we have a click somewhere (which is not a selectable thing, other wise we wouln't be here
                if (InputInterface.GetInputDown(0) || InputInterface.GetInputDown(1))
                {
                    HideZoomedCard();
                }              
                break;
            case 1:
                _card = _selector.Selectables[0].GetComponent<Card>();
                Assert.IsTrue(_card != null);
                if (_card.Selectable.IsInPlayerHand || _card.Selectable.IsInAZone)
                {
                    gameObjectToHide.SetActive(false);
                    soundManager.PlaySound(zoomBeginSound);
                    _cardToDisplayInfo.CardData = _card.CardData;
                    _cardToDisplayInfo.ChangeVisibility(true);
                    // Add a zoom-in animation to the CardOnlyDisplay game object
                    _cardToDisplayInfo.transform.localScale = Vector3.zero;
                    _cardToDisplayInfo.transform.DOScale(_newScale, 0.5f);
                }
                break;
            default: Debug.LogWarning("Unhandled number of selectables objects currently selected");
                    break;
        }
    }

    private void HideZoomedCard()
    {
        _card = null;
        // Add a zoom-out animation to the CardOnlyDisplay game object
        _cardToDisplayInfo.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            _cardToDisplayInfo.ChangeVisibility(false);
            gameObjectToHide.SetActive(true);
        });
    }
}
