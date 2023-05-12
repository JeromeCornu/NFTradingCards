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
        foreach (var layout in _layouts)
        {
            for (int i = 0; i < count; i++)
            {
                var ch = transform.GetChild(i);
                var pos = ch.transform.localPosition;

                ch.transform.localPosition = layout.DistributeFromLeft(pos, i, count);
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
    private float _beginning;
    public Vector3 DistributeAlternatively(Vector3 initialPos, int index)
    {
        float dir = index % 2 == 0 ? 1f : -1f;
        int step = Mathf.FloorToInt((index + 1) / 2);
        float val = _center - _spread * dir * step;
        return OverwritePos(initialPos, val);
    }
    public Vector3 DistributeFromLeft(Vector3 initialPos, int index, int totalNbOfElement, bool offsetHalfAstep = false)
    {
        float step = (-totalNbOfElement / 2f) + index;
        float val = _center - _spread * step /*+ (offsetHalfAstep ? _spread / 2f : 0)*/;
        return OverwritePos(initialPos, val); ;
    }

    private Vector3 OverwritePos(Vector3 initialPos, float val)
    {
        if ((axis & Axis.X) != 0b0)
            initialPos.x = val;
        if ((axis & Axis.Y) != 0b0)
            initialPos.y = val;
        if ((axis & Axis.Z) != 0b0)
            initialPos.z = val;
        return initialPos;
    }

    /// <summary>
    /// The axis are priorized in their natural x y z order
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns></returns>
    public bool IsAfterInLayoutOrder(Vector3 v1, Vector3 v2)
    {
#pragma warning disable CS0618 // We provided correctly the priority
        return IsAfterInLayoutOrder(v1, v2, new Axis[] { Axis.X, Axis.Y, Axis.Z });
#pragma warning restore CS0618 // We provided correctly the priority
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <param name="sort">Specify comparer to imply specific sort order</param>
    /// <returns></returns>
    public bool IsAfterInLayoutOrder(Vector3 v1, Vector3 v2, Comparison<Axis> sort)
    {
#pragma warning disable CS0618 // We provided correctly the priority by sorting a list of all the possible elements
        var priority = Enum.GetValues(typeof(Axis)).OfType<Axis>().ToList();
        priority.Sort(sort);
        return IsAfterInLayoutOrder(v1, v2, priority);
#pragma warning restore CS0618 // We provided correctly the priority by sorting a list of all the possible elements
    }
#pragma warning disable BCC2000 //The Assert causing it
    [Obsolete("Should be carefully use with an enumerable containing all the elements of the Axis enum")]
    private bool IsAfterInLayoutOrder(Vector3 v1, Vector3 v2, IEnumerable<Axis> priority)
    {
        Assert.IsTrue(priority.Intersect(Enum.GetValues(typeof(Axis)).OfType<Axis>()).Count() == Enum.GetValues(typeof(Axis)).OfType<Axis>().Count(a=>a!=0x0));
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
#pragma warning restore BCC2000 //The Assert causing it
}
