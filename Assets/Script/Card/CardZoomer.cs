using UnityEngine;
using DG.Tweening;
using UnityEngine.Assertions;
using Lean.Touch;
using UniBT;
using Lean.Common;
using System;
using DG.Tweening.Core;
using Options = DG.Tweening.Plugins.Options;

public class CardZoomer : MonoBehaviour
{
    [SerializeField] private CardOnlyDisplay _cardToDisplayInfo;
    [SerializeField] private GameObject gameObjectToHide;
    [SerializeField, Tooltip("We only need the reference so we can copy the query settings")] private LeanSelectByFinger _selector;
    private Vector3 _newScale;

    private (Lean.Common.LeanSelectable selected, LeanFinger finger) _selected;
    private Lean.Common.LeanSelectable _lastZoomed;

    private TweenerCore<Vector3, Vector3, Options.VectorOptions> unzoomTween;

    [Header("Sound")]
    [SerializeField]
    private SoundManager soundManager;
    public AudioClip zoomBeginSound;
    private void Awake()
    {
        LeanFingerDown fgDown;
        //We find the LeanFingerDown object (which appears to be on same object as the selector but could be elswhere, in case we find one in scene, if not, there should be one)
        if (!_selector.gameObject.TryGetComponent<LeanFingerDown>(out fgDown))
            if ((fgDown = FindObjectOfType<LeanFingerDown>()) == null)
                throw new EntryPointNotFoundException("No LeanFingerDown found in scene, please add one (on the same object as Lean selector) or add one to the scene");
        //We hook on the event to perform our own checks, without selection logic
        fgDown.OnFinger.AddListener(UpdateCardUnderFinger);
        //We are going to use the query so we make sure it has a camera set
        _selector.ScreenQuery.Camera ??= Camera.main;
    }
    private void Start()
    {
        _newScale = new Vector3(1.5f, 1.5f, 1.5f);
        _cardToDisplayInfo.ChangeVisibility(false);
    }
    /// <summary>
    /// To be called from OnFingerDownEvent
    /// </summary>
    /// <param name="finger"></param>
    /// <param name="screenPosition"></param>
    public void UpdateCardUnderFinger(LeanFinger finger)
    {
        _selected = (_selector.ScreenQuery.Query<LeanSelectable>(null/* null is ok as long as the query has a camera, which we made sure was the case*/, finger.ScreenPosition), finger);
        Debug.Log("Queried : "+(_selected.selected==null));
    }
    private void Update()
    {
        Debug.Log("Queried finger ;" + _selected.finger.Down);
        if (_selected.selected == null)
            return;
        if (_selected.finger.Set/* i.e the selected card is selected by a finger still down*/)
        {
            //Assign the _lastZoomed, if it changed, we effectively trigger zoom, if not we return
            if (_lastZoomed == (_lastZoomed = _selected.selected))
                return;
            var card = _lastZoomed.GetComponent<Card>();
            Assert.IsTrue(card != null);
            if (card.Selectable.BelongsToPlayer || card.Selectable.IsInAZone)
            {
                //If we happen to be still unzooming from a card, we kill the tween to make sure nothing weird happens
                unzoomTween.Kill();
                gameObjectToHide.SetActive(false);
                soundManager.PlaySound(zoomBeginSound);
                _cardToDisplayInfo.CardData = card.CardData;
                _cardToDisplayInfo.ChangeVisibility(true);
                _cardToDisplayInfo.transform.localScale = Vector3.zero;
                _cardToDisplayInfo.transform.DOScale(_newScale, 0.5f);
                Debug.Log("Selec zooming");
            }
        }
        //Else we unzoom it, and set everything to null to make sure we don't enter here again before a new zoom
        else
        {
            _lastZoomed = null;
            //Just as a safe if we try to acces the finger from elswere than this if
            _selected.selected = null;
            _selected.finger = null;
            HideZoomedCard();
        }
    }
    private void HideZoomedCard()
    {
        // Add a zoom-out animation to the CardOnlyDisplay game object
       unzoomTween =_cardToDisplayInfo.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            _cardToDisplayInfo.ChangeVisibility(false);
            gameObjectToHide.SetActive(true);
        });
    }
}
