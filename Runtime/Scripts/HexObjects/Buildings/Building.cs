using HexagonPackage;
using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects
{

    public abstract class Building : HexObject
	{
        public string Text{ get; set; }

        public string Name;
        [TextArea]
        public string Description;

		public List<IntValue> Costs = new List<IntValue>();

        protected override void Reset()
        {
            base.Reset();
            if (Name == null)
            {
                Name = name;
            }
        }       
    }
}