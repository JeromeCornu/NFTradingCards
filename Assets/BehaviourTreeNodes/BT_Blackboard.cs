using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BT_Blackboard : MonoBehaviour
{
    public List<KVP<string, GameObject>> _gameObjects;
    public List<KVP<string, float>> _floats;
    public List<KVP<string, bool>> _bools;
    public List<KVP<string, object>> _objects;
    public static Dictionary<string, GameObject> GameObjects;
    public static Dictionary<string, float> Floats;
    public static Dictionary<string, bool> Bools;
    public static Dictionary<string, object> Objects;
    private void Awake()
    {
        GameObjects = new Dictionary<string, GameObject>(_gameObjects.Select(kvp=>new KeyValuePair<string, GameObject>(kvp.key,kvp.value)));
        Floats = new Dictionary<string, float>(_floats.Select(kvp=>new KeyValuePair<string, float>(kvp.key,kvp.value)));
        Bools = new Dictionary<string, bool>(_bools.Select(kvp=>new KeyValuePair<string, bool>(kvp.key,kvp.value)));
        Objects = new Dictionary<string, object>(_objects.Select(kvp=>new KeyValuePair<string, object>(kvp.key,kvp.value)));
    }
    [System.Serializable]
    public class KVP<T, V>
    {
        public T key;
        public V value;
    }
}
