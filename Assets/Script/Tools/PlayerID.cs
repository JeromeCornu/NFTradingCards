using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    [SerializeField/*,OnValueChanged(nameof(UpdateTags))*/]
    private bool _isPlayer;
    public int AsInt => IsPlayerAsInt(_isPlayer);
    private void Start()
    {
        UpdateTags();
    }

    private void UpdateTags()
    {
        TraverseChildren(transform, TurnManager.GetTag(_isPlayer));
    }

    void TraverseChildren(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            child.tag = tag;
            //If we use the setter we gonna be recursive, but we already are so we don't wont
            foreach (var id in GetComponents<PlayerID>())
                id._isPlayer = this._isPlayer;
            // Call the TraverseChildren function recursively on the child object's transform
            TraverseChildren(child, tag);
        }
    }

    public static int IsPlayerAsInt(bool iIsPlayer)
    {
        return iIsPlayer ? 0 : 1;
    }
}
