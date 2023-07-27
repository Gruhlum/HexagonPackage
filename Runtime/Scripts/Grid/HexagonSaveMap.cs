using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    [System.Serializable]
    public class HexagonSaveMap : Dictionary<Cube, Hexagon>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<Cube> keys = new List<Cube>();

        [SerializeField]
        private List<Hexagon> values = new List<Hexagon>();
       
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<Cube, Hexagon> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
    }
}