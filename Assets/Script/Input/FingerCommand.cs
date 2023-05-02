using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FingerCommand : MonoBehaviour
{
    [SerializeField, Range(2, 10)]
    private int _nbOfFingersForSpecialCommand = 4;
    [SerializeField]
    private UnityEvent<bool> _onSpecialCommandToggle;
    private bool _toggle = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount >= _nbOfFingersForSpecialCommand)
            _onSpecialCommandToggle.Invoke(_toggle = !_toggle);
    }
}
