using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Layout : MonoBehaviour
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
                //float offset = !layout.center ? 0f : .5f - (count - 1) / 2f / count;
                float val = Mathf.Lerp(layout.layout.x, layout.layout.y, (count != 1 ? (float)i / (count - 1) : (layout.center ? .5f : 0f)));
                /*if (float.IsNaN(val))
                    return;*/
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
    [SerializeField, Tooltip("Will set x to -y")]
    private bool _symetrize;
    [SerializeField]
    public bool center=false;
    [SerializeField, MinMaxSlider(-10, 10), OnValueChanged(nameof(Symetrize))]
    [Tooltip("Will set x to -y if symetrizez")]
    public Vector2 layout;
    [SerializeField]
    public Axis axis;
    public void Symetrize()
    {
        if (!_symetrize)
            return;
        layout.x = -layout.y;
    }
}
