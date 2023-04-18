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
    private string _description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed non risus. Suspendisse lectus tortor, dignissim sit amet, adipiscing nec, ultricies sed, dolor.";
    [SerializeField]
    private string _quote = "Ut velit mauris, egestas sed, gravida nec, ornare ut, mi.Aenean ut orci vel massa suscipit pulvinar.";
    [Header("Values")]
    [SerializeField] private int _cost;
    [SerializeField] private Value _economic = new Value(0);
    [SerializeField] private Value _social = new Value(0);
    [SerializeField] private Value _ecologic = new Value(0);
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
                    throw new KeyNotFoundException();
            }
        }
    }
    public Sprite Sprite { get => _sprite; }
    public string Name { get => _name; }
    public string Descrition { get => _description; }
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
        Economic = 1 << 1, Social = 1 << 2, Ecologic = 1 << 3
    }
    public enum Effect
    {
        Neutral = 0, Bonus = -1, Malus = 1
    }

    public IEnumerator<Value> GetEnumerator()
    {
            yield return this[Pillar.Ecologic];
            yield return this[Pillar.Economic];
            yield return this[Pillar.Social];
    }
    IEnumerator<(CardData.Pillar pillar, Value value)> IEnumerable<(CardData.Pillar pillar, Value value)>.GetEnumerator()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Pillar)).Length; i++)
        {
            yield return ((Pillar)i, this[(Pillar)i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        yield return GetEnumerator();
    }

    /*public bool AssertUniqueKeys(List<Couple> toAssert)
{
return toAssert.GroupBy(c => c.type)
         .All(g => g.Count() == 1);
*//*
 HashSet<CardData.Type> seenTypes = new HashSet<CardData.Type>();
foreach (Couple couple in toAssert)
{
if (seenTypes.Contains(couple.type))
{
    // We've already seen a couple with this type, so the assertion fails
    return false;
}
seenTypes.Add(couple.type);
}
return true;
 *//*
}*/
}
/*[System.Serializable]
public class Couple
{
    public CardData.Type type;
    public Value value;
    public Couple(CardData.Type t, Value v)
    {
        type = t;
        value = v;
    }
}
[System.Serializable]
public class KVP<T, V>
{
    public T V1;
    public V V2;
    public KVP(T t, V v)
    {
        V1 = t;
        V2 = v;
    }
}*/
[System.Serializable]
public class Value
{
    public const int MAX = 99;
    [Range(-MAX, MAX), SerializeField]
    private int _value;
    public Value(int value)
    {
        _value = value;
    }
    public static Value Default => new Value(0);

    public CardData.Effect Effect => _value == 0 ? CardData.Effect.Neutral :
        _value < 0 ? CardData.Effect.Malus : CardData.Effect.Bonus;

    public int Val { get => _value; }
}
