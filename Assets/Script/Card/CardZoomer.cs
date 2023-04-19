using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardZoomer : MonoBehaviour
{
    private Card _card;
    private Transform parent;
    private Vector3 _startPos;
    private Vector3 _startScale;
    private Vector3 _endScale;
    private Vector3 _endPos;

    [SerializeField]
    private float _moveDuration = 1.0f;

    [SerializeField]
    private Ease _moveEase = Ease.Linear;

    public bool _zoomEnabled = true;
    private bool _isScaling = false;

    void Update()
    {
        if (_zoomEnabled && Input.GetMouseButtonDown(1) && !_isScaling)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.tag == "Player")
                {
                    _card = hit.collider.transform.parent.parent.GetComponent<Card>();

                    if (_card != null)
                    {
                        _startPos = _card.transform.position;
                        _startScale = _card.transform.localScale;

                        _endScale = new Vector3(2.0f, 2.0f, 2.0f); // Set the new scale value
                        _endPos = new Vector3(0f, 0f, 0f); // Set the new pos value

                        _card.transform.DOMove(_endPos, _moveDuration).SetEase(_moveEase);
                        _card.transform.DOScale(_endScale, _moveDuration).SetEase(_moveEase);

                        if (_isScaling == false)
                        {
                            // MAYBE
                            parent = _card.transform.parent;
                        }

                        _card.transform.SetParent(null);
                        _isScaling = true;
                    }
                }
            }
        }

        if (_zoomEnabled && Input.GetMouseButtonDown(1) && _isScaling && _card != null && !EventSystem.current.IsPointerOverGameObject())
        {
            _card.transform.DOMove(_startPos, _moveDuration).SetEase(_moveEase);
            _card.transform.DOScale(_startScale, _moveDuration).SetEase(_moveEase);

            _card = null;
            _isScaling = false;
            // MAYBE
            _card.transform.SetParent(parent);

            StartCoroutine(EnableZoom());
        }

        // Check if the left mouse button was clicked and if the card is currently being scaled
        if (_zoomEnabled && Input.GetMouseButtonDown(0) && _isScaling && _card != null && !EventSystem.current.IsPointerOverGameObject())
        {
            // Set the card's position and scale to their initial values
            _card.transform.position = _startPos;
            _card.transform.localScale = _startScale;

            _card = null;
            _isScaling = false;
            // MAYBE
            _card.transform.SetParent(parent);

            StartCoroutine(EnableZoom());
        }
    }

    IEnumerator EnableZoom()
    {
        _zoomEnabled = false;
        yield return new WaitForSeconds(1.0f);
        _zoomEnabled = true;
    }
}
