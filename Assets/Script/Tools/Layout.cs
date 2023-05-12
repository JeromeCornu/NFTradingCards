using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;
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
        position = transform.InverseTransformPoint(position);
        int i = 0;
        //We check layouts first like that we treat them in order of priority for every child
        foreach (var layout in _layouts)
        {
            i = 0;
            foreach (var child in this)
            {
                Debug.Log(layout.axis + " In layout, comparing with childIndex : " + i + " which name is ; " + child.gameObject.name + " trying to find pos : " + position);
                if (!layout.IsAfterInLayoutOrder(position, child.localPosition))
                {
                    //We return here to break out of the two loops
                    return i;
                }
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
    /// <summary>
    /// The axis are priorized in their natural x y z order
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public bool IsAfterInLayoutOrder(Vector3 v1, Vector3 v2)
    {
        return IsAfterInLayoutOrder(v1, v2, Axis.X, Axis.Y, Axis.Z);
    }
    public bool IsAfterInLayoutOrder(Vector3 v1, Vector3 v2, params Axis[] priority)
    {
        Assert.IsTrue(priority.Length==Enum.GetValues(typeof(Axis)).Length && priority.Contains(Axis.X) && priority.Contains(Axis.Y) && priority.Contains(Axis.Z));
        foreach (var ax in priority)
        {
            if ((axis & ax & Axis.X) != 0b0)
                return v1.x > v2.x;
            if ((axis & ax & Axis.Y) != 0b0)
                return v1.y > v2.y;
            if ((axis & ax & Axis.Z) != 0b0)
                return v1.z > v2.z;
        }
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
