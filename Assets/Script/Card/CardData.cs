using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "NFTC/New card")]
public class CardData : ScriptableObject, /*IEnumerable<Value>, */IEnumerable<(CardData.Pillar pillar, Value value)>
{
    [Header("Look")]
    [SerializeField]
    private string _name;
    [SerializeField]
    private Sprite _sprite;
    [SerializeField]
    private string _descrition = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor.";
    [SerializeField]
    private string _quote = "Ut velit mauris, egestas sed, gravida nec, ornare ut, mi.Aenean ut orci vel massa suscipit pulvinar.";
    [Header("Values")]
    [SerializeField] private int _cost;
    [SerializeField] private Value _economic;
    [SerializeField] private Value _social;
    [SerializeField] private Value _ecologic;
    public Value this[Pillar t]
    {
        get
        {
            switch (t)
            {
                case Pillar.Economic:
                    return _economic;
                case Pillar.Social:
                    return _social;
                case Pillar.Ecologic:
                    return _ecologic;
                default:
                    throw new KeyNotFoundException(t + " Not founrd");
            }
        }
    }
    public Sprite Sprite { get => _sprite; }
    public string Name { get => _name; }
    public string Descrition { get => _descrition; }
    public int Cost { get => _cost; }
    //First will shortcut all the ordering after the first as been ordered
    public Pillar _majorTypes => this.Where(kv => kv.value.Val == this.Max((v) => Mathf.Abs(v.value.Val)))
                .Select(kv => kv.pillar)
                .Aggregate((curr, next) => curr &= next);

    public string Quote { get => _quote; }
    public Color Color { get => new Color(1f, 215f / 255f, 0); }

    [Flags]
    public enum Pillar
    {
        Economic = 1 << 0, Social = 1 << 1, Ecologic = 1 << 2
    }
    public enum Effect
    {
        Neutral = 0, Bonus = -1, Malus = 1
    }

    public IEnumerator<Value> GetEnumerator()
    {
        yield return this[Pillar.Economic];
        yield return this[Pillar.Ecologic];
        yield return this[Pillar.Social];
    }
    IEnumerator<(CardData.Pillar pillar, Value value)> IEnumerable<(CardData.Pillar pillar, Value value)>.GetEnumerator()
    {
        yield return (Pillar.Economic, this[Pillar.Economic]);
        yield return (Pillar.Ecologic, this[Pillar.Ecologic]);
        yield return (Pillar.Social, this[Pillar.Social]);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield return GetEnumerator();
    }
    public override string ToString()
    {
        return _name + " cost : " + _cost + ", " + String.Join(',', this.Select(s => ("," + (s.pillar + " : " + s.value.ToString()))));
    }
}
[System.Serializable]
public class Value
{
    public const int MAX = 99;
    [Range(-MAX, MAX), SerializeField]
    private int _value;
    private Value(int value)
    {
        _value = value;
    }
    public static Value Default => new Value(0);

    public CardData.Effect Effect => _value == 0 ? CardData.Effect.Neutral :
        _value < 0 ? CardData.Effect.Malus : CardData.Effect.Bonus;

    public int Val { get => _value; }
    public override string ToString()
    {
        return _value.ToString();
    }
}
