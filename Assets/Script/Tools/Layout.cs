using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Layout : MonoBehaviour
{
    [SerializeField, AllowNesting, OnValueChanged(nameof(UpdateLayout))]
    private List<LayoutElement> _layouts;
    void UpdateLayout()
    {
        int count = transform.childCount;
        foreach (var layout in _layouts)
        {
            for (int i = 0; i < count; i++)
            {
                var ch = transform.GetChild(count - (i + 1));
                var pos = ch.transform.localPosition;
                float val = Mathf.Lerp(layout.layout.x, layout.layout.y, (float)i / (count - 1));
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

}
[System.Serializable]
public class LayoutElement
{
    [SerializeField, MinMaxSlider(-10, 10)]
    public Vector2 layout;
    [SerializeField]
    public Axis axis;

}
