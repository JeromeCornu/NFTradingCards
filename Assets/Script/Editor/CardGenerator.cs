using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardGenerator : EditorWindow
{
    private string[] _animalNames = { "Lion", "Tiger", "Bear", "Elephant", "Giraffe", "Kangaroo", "Hippo", "Rhino", "Zebra", "Monkey" };

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
        for (int i = 0; i < 10; i++)
        {
            CardData card = ScriptableObject.CreateInstance<CardData>();
            var cardT = card.GetType();
            SetField("_name", card, "Card " + _animalNames[i], cardT);
            SetField("_sprite", card, null, cardT);
            SetField("_description", card, "Lorem ipsum dolor sit amet, consectetur adipiscing elit.", cardT);
            SetField("_quote", card, "Ut velit mauris, egestas sed, gravida nec, ornare ut, mi.Aenean ut orci vel massa suscipit pulvinar.", cardT);
            SetField("_cost", card, Random.Range(1, 11), cardT);
            Type valueType = typeof(Value); // get the type of the object
            // Generate random values between -10 and 10
            foreach (var v in card)
            {
                FieldInfo valueField = valueType.GetField("_value", BindingFlags.NonPublic | BindingFlags.Instance); // get the private field "_value"
                valueField.SetValue(v, Random.Range(-10, 11));
            }

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
