using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BT_Blackboard : MonoBehaviour
{
    public List<KVP<string, GameObject>> _gameObjects;
    public List<KVP<string, float>> _floats;
    public List<KVP<string, bool>> _bools;
    public List<KVP<string, object>> _objects;

    [System.Serializable]
    public class KVP<T, V>
    {
        public T key;
        public V value;
    }
}
