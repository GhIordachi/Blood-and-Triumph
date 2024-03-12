using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GI
{
    [System.Serializable]
    public class SerializableDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Tkey> keys = new List<Tkey>();
        [SerializeField] private List<TValue> values = new List<TValue>();

        //Called right before serialization
        //Saves the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach(KeyValuePair<Tkey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        //Called right after serialization
        //Load the dictionary from lists
        public void OnAfterDeserialize()
        {
            Clear();

            if(keys.Count != values.Count)
            {
                Debug.LogError("Error: The amount of keys does not match the amount of values!");
            }

            for(int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}