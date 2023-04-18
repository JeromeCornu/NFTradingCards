using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerID : MonoBehaviour
{
    [SerializeField/*,OnValueChanged(nameof(UpdateTags))*/]
    private bool _isPlayer;
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
            // Call the TraverseChildren function recursively on the child object's transform
            TraverseChildren(child,tag);
        }
    }
}
