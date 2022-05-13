using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exile
{
	[System.Serializable]
	public class Resource
	{
        public ResourceType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        [SerializeField] private ResourceType type;

        public int CurrentValue
        {
            get
            {
                return currentValue;
            }
            set
            {
                currentValue = value;
            }
        }
        [SerializeField] private int currentValue = 100;

        public int MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
            }
        }
        [SerializeField] private int maxValue = 100;
    }
}