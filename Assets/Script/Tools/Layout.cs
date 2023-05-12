using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class Layout : MonoBehaviour,IEnumerable<Transform>
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
                float dir = i % 2 == 0 ? 1f : -1f;
                int step = (i + 1) / 2;
                float val = layout.layout.x + layout.layout.y * dir * step;
                if (count % 2 == 0)
                    val += layout.layout.y / 2f;
                //Mathf.Lerp(layout.layout.x, layout.layout.y, (count != 1 ? (float)i / (count - 1) : (layout.center ? .5f : 0f)));
                if ((layout.axis & Axis.X) != 0b0)
                    pos.x = val;
                if ((layout.axis & Axis.Y) != 0b0)
                    pos.y = val;
                if ((layout.axis & Axis.Z) != 0b0)
                    pos.z = val;
                ch.transform.localPosition = pos;
            }
        }
    }

    public IEnumerator<Transform> GetEnumerator()
    {
        var enumerator= transform.GetEnumerator();
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
    public bool center = false;
    [SerializeField, MinMaxSlider(-10, 10)/*, OnValueChanged(nameof(Symetrize))*/]
    [Tooltip("Will set x to -y if symetrizez")]
    public Vector2 layout;
    [SerializeField]
    public Axis axis;
    public void Symetrize()
    {
        layout.x = -layout.y;
    }
}
