using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using static UnityEditor.PlayerSettings;

public class Layout : MonoBehaviour, IEnumerable<Transform>
{
    [SerializeField]
    private bool _updateAuto = false;
    [SerializeField, AllowNesting, OnValueChanged(nameof(UpdateLayout))]
    private List<LayoutElement> _layouts;
    private void Update()
    {
        if (!_updateAuto)
            return;
        UpdateLayout();
    }
    public int GetCorrectIndex(Vector3 position)
    {
        int i = 0;
        foreach (var layout in _layouts)
        {
            i = 0;
            foreach (var child in this)
            {
                if (layout.IsAfter(position, child.position, i))
                    break;
                i++;
            }
        }
        return i;
    }
    [Button("Force Update Layout")]
    public void UpdateLayout()
    {
        int count = transform.childCount;
        bool center = count % 2 == 0;
        foreach (var layout in _layouts)
        {
            for (int i = 0; i < count; i++)
            {
                var ch = transform.GetChild(i);
                var pos = ch.transform.localPosition;

                ch.transform.localPosition = layout.CalculatePosition(pos, i, center);
            }
        }
    }

    public IEnumerator<Transform> GetEnumerator()
    {
        var enumerator = transform.GetEnumerator();
        while (enumerator.MoveNext())
        {
            yield return (Transform)enumerator.Current;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
[System.Serializable]
public class LayoutElement
{
    [SerializeField]
    private float _center;
    [SerializeField]
    private float _spread;
    [SerializeField]
    public Axis axis;

    public bool IsAfter(Vector3 v1, Vector3 v2, int i)
    {
        return IsSuperior(CalculatePosition(v1, i), CalculatePosition(v2, i));
    }

    private bool IsSuperior(Vector3 v1, Vector3 v2)
    {
        if ((axis & Axis.X) != 0b0)
            return v1.x > v2.x;
        if ((axis & Axis.Y) != 0b0)
            return v1.y > v2.y;
        if ((axis & Axis.Z) != 0b0)
            return v1.z > v2.z;
        return false;
    }

    public Vector3 CalculatePosition(Vector3 initialPos, int index, bool offsetHalfAstep = false)
    {
        float dir = index % 2 == 0 ? 1f : -1f;
        int step = Mathf.FloorToInt((index + 1) / 2);
        float val = _center + _spread * dir * step + (offsetHalfAstep ? _spread / 2f : 0);
        if ((axis & Axis.X) != 0b0)
            initialPos.x = val;
        if ((axis & Axis.Y) != 0b0)
            initialPos.y = val;
        if ((axis & Axis.Z) != 0b0)
            initialPos.z = val;
        return initialPos;
    }
}
