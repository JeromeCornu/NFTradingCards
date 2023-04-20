using UnityEngine;
using DG.Tweening;

public class CardZoomer : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private CardOnlyDisplay _cardToDisplayInfo;
    private Vector3 _newScale;

    private Card _card;

    [Header("Sound")]
    private SoundManager soundManager;
    public AudioClip zoomBeginSound;

    private void Start()
    {
        soundManager = Camera.main.GetComponent<SoundManager>();
        _newScale = new Vector3(1.5f, 1.5f, 1.5f);
        _cardToDisplayInfo.gameObject.SetActive(false);
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1<<6))
            {
                _card = hit.collider.transform.parent.parent.GetComponent<Card>();

                if (_card.transform.parent.name == "PlayerHand" || _card.transform.parent.parent.name == "Zone")
                {
                    soundManager.PlaySound(zoomBeginSound);
                    _cardToDisplayInfo.CardData = _card.CardData;
                    _cardToDisplayInfo.gameObject.SetActive(true);
                    // Add a zoom-in animation to the CardOnlyDisplay game object
                    _cardToDisplayInfo.transform.localScale = Vector3.zero;
                    _cardToDisplayInfo.transform.DOScale(_newScale, 0.5f);
                }
            }
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _card = null;
            // Add a zoom-out animation to the CardOnlyDisplay game object
            _cardToDisplayInfo.transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => {
                _cardToDisplayInfo.gameObject.SetActive(false);
            });
        }
    }
}
