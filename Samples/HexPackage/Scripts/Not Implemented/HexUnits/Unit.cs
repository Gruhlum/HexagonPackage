using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HexagonPackage;

namespace Exile
{
    public enum Team { Friendly, Enemy, Neutral }
    public class Unit : MonoBehaviour
	{		
        public Team Team;

        public Resource Health;

		public Resource Energy;

        public List<Ability> Abilities = new List<Ability>();

        public Hexagon Hexagon
        {
            get
            {
                return hexagon;
            }
            set
            {
                if (hexagon != null)
                {
                    hexagon.Unit = null;
                }
                hexagon = value;
                if (hexagon != null)
                {
                    hexagon.Unit = this;
                    transform.position = hexagon.transform.position;
                }
            }
        }
        [SerializeField] private Hexagon hexagon = default;

        public Cube LastDirection
        {
            get
            {
                return myProp;
            }
            set
            {
                myProp = value;
            }
        }
        private Cube myProp;

        public void TimeAdvanced()
        {
            foreach (var ability in Abilities)
            {
                ability.CurrentCooldown++;
            }
        }
    }
}