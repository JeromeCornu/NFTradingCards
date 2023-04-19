using UnityEngine;

public class CardZoomer : MonoBehaviour
{
    private Camera _mainCamera;
    [SerializeField] private CardOnlyDisplay _cardToDisplayInfo;

    private Card _card;

    private void Start()
    {
        _cardToDisplayInfo.gameObject.SetActive(false);
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Player"))
            {
                _card = hit.collider.transform.parent.parent.GetComponent<Card>();
                _cardToDisplayInfo.CardData = _card.CardData;
                _cardToDisplayInfo.gameObject.SetActive(true);
            }
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _card = null;
            _cardToDisplayInfo.gameObject.SetActive(false);
        }
    }
}
