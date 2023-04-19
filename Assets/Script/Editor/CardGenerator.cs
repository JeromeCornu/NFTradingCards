using System;
using System.Numerics;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using Random = UnityEngine.Random;

public class CardGenerator : EditorWindow
{
    private string[] _names =
        {
            "Solar energy companies",
            "Public transportation",
            "Community gardens",
            "Sustainable fashion brands",
            "Recycling facilities",
            "Fossil fuel companies",
            "Single-use plastic manufacturers",
            "Fast fashion brands",
            "Coal mining companies",
            "Disposable diaper companies",
            "Luxury brands",
            "Meat industry",
            "Renewable energy companies",
            "Electric car manufacturers",
            "Fair trade brands",
            "Organic farming",
            "Plastic waste cleanup initiatives",
            "Overfishing",
            "Large-scale mining",
            "Water privatization initiatives",
            "Planned obsolescence technology companies",
            "Massive industrial agriculture",
            "Sustainable development consulting firms"
        };

    private string[] _descriptions =
        {
            "Brightening the world",
            "Ride with us",
            "Growing together",
            "Wear the change",
            "Reduce, reuse, recycle",
            "Fueling destruction",
            "Convenience kills",
            "Fashion over destruction",
            "The dark side of energy",
            "Ditch the disposable",
            "More money than sense",
            "The cost of carnivores",
            "Powering the future",
            "Charge up your life",
            "Fairness for all",
            "Farming for the future",
            "Clean oceans, happy planet",
            "Emptying the oceans",
            "Extracting disaster",
            "Drinking water for the rich",
            "Obsolete on arrival",
            "Destroying our soil",
            "Greenwashing for profit"
        };

    private string[] _quotes =
        {
            "Shine on!",
            "Get on board",
            "Planting joy",
            "Fashionably green",
            "Waste not",
            "Digging our graves",
            "Disposable is not the answer",
            "Sustainable is the new black",
            "Mining misery",
            "Cloth is cool",
            "Conspicuous consumption",
            "Beefing up destruction",
            "Energy for all",
            "Drive into the future",
            "Trade with a heart",
            "Growing with nature",
            "Pick up your trash",
            "Fish no more",
            "Digging our graves deeper",
            "Monetizing a human right",
            "Making garbage, not products",
            "Putting profits over people",
            "Sustainability sold separately"
        };

    private int[] _economic = { -2, 2, -1, 4, -2, 5, 2, 3, 4, 1, 4, 3, 4, 2, -1, 1, 2, -3, -1, -3, -4, -3, -2 };

    private int[] _social = { 3, 4, 5, 3, 3, -3, -2, -1, -3, -1, -2, -1, 2, 3, 4, 3, 3, -5, -1, -2, -2, -4, 0 };

    private int[] _ecologic = { 4, 4, 2, 1, 3, -5, -4, -3, -5, -3, 0, -4, 4, 4, 1, 3, 4, 2, -5, -2, -2, -3, -5, -1 };



    [MenuItem("Custom/Card Generator")]
    public static void ShowWindow()
    {
        GetWindow<CardGenerator>("Card Generator");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate Cards"))
        {
            GenerateCards();
        }
    }

    private void GenerateCards()
    {
        for (int i = 0; i < _names.Length; i++)
        {
            // price
            var price = (_ecologic[i] + _social[i] + _economic[i]) * 2;
            if (price == 0) { price = 2; }
            if (price < 0) { price = price - 2 * price; }

            // fill card
            CardData card = ScriptableObject.CreateInstance<CardData>();
            var cardT = card.GetType();
            SetField("_name", card, _names[i], cardT);
            SetField("_sprite", card, null, cardT);
            SetField("_description", card, _descriptions[i], cardT);
            SetField("_quote", card, _quotes[i], cardT);
            SetField("_cost", card, price, cardT);
            Type valueType = typeof(Value); // get the type of the object

            FieldInfo valueField = valueType.GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance); // get the private field "_value"
            valueField.SetValue(card[CardData.Pillar.Ecologic], _ecologic[i]);
            valueField.SetValue(card[CardData.Pillar.Social], _social[i]);
            valueField.SetValue(card[CardData.Pillar.Economic], _economic[i]);

            AssetDatabase.CreateAsset(card, "Assets/Data/Generated/" + card.Name + ".asset");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static void SetField(string fieldName, CardData instance, object val, Type cardT)
    {
        FieldInfo valueField = cardT.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        valueField.SetValue(instance, val);
    }
}
