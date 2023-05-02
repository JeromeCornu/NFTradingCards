using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;
using Lean.Touch;
using UniBT;
using Lean.Common;

public class CardZoomer : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private CardOnlyDisplay _cardToDisplayInfo;
    [SerializeField] private GameObject gameObjectToHide;
    [SerializeField] private LeanSelectByFinger _selector;
    private Vector3 _newScale;

    private Lean.Common.LeanSelectable _lastSelected;

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
    public void UpdateCardUnderFinger(LeanFinger finger, Vector2 screenPosition)
    {
        _lastSelected = _selector.ScreenQuery.Query<LeanSelectable>(gameObject, screenPosition);
    }
    private void Update()
    {
        switch (_selector.Selectables.Count)
        {
            //If we have nothing selected
            case 0:
                //And if we have a click somewhere (which is not a selectable thing, other wise we wouln't be here
                if (_lastSelected!=null /* i.e we do effectively have a card to hide*//* || InputInterface.GetInputDown(0) || InputInterface.GetInputDown(1)*/)
                {
                    _lastSelected = null;
                    HideZoomedCard();
                }
                break;
            case 1:
                var selected = _selector.Selectables[0];
                if (_lastSelected == selected)
                    return;
                _lastSelected = selected;
                var card = _lastSelected.GetComponent<Card>();
                Assert.IsTrue(card != null);
                if (card.Selectable.BelongsToPlayer || card.Selectable.IsInAZone)
                {
                    gameObjectToHide.SetActive(false);
                    soundManager.PlaySound(zoomBeginSound);
                    _cardToDisplayInfo.CardData = card.CardData;
                    _cardToDisplayInfo.ChangeVisibility(true);
                    _cardToDisplayInfo.transform.localScale = Vector3.zero;
                    _cardToDisplayInfo.transform.DOScale(_newScale, 0.5f);
                    Debug.Log("Selec zooming");
                }
                break;
            default:
                Debug.LogWarning("Unhandled number of selectables objects currently selected");
                break;
        }
    }

    private void HideZoomedCard()
    {
        // Add a zoom-out animation to the CardOnlyDisplay game object
        _cardToDisplayInfo.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            _cardToDisplayInfo.ChangeVisibility(false);
            gameObjectToHide.SetActive(true);
        });
    }
}
