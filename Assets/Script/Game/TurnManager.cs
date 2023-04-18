using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private bool _isPlayerTurn = true;
    public UnityEvent<bool> TurnChanged;
    [SerializeField, Tag] private string _playerTag;
    [SerializeField, Tag] private string _otherTag;
    [SerializeField]
    public static string PlayerTag;
    [SerializeField]
    public static string OtherTag;
    public static string GetTag(bool playerTag) => playerTag ? PlayerTag : OtherTag;
    public string GetTag() => GetTag(_isPlayerTurn);
    private void Awake()
    {
        PlayerTag = _playerTag;
        OtherTag = _otherTag;
    }
    private void Start()
    {
        IsPlayerTurn = true;
    }
    public bool IsPlayerTurn
    {
        get => _isPlayerTurn; set
        {
            _isPlayerTurn = value;
            TurnChanged.Invoke(_isPlayerTurn);
        }
    }
    public void SwitchTurn()
    {
        IsPlayerTurn = !IsPlayerTurn;
    }
}
