using HexagonPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exile
{
    public class MeleeAbility : Ability
    {
        public int Cleave
        {
            get
            {
                return cleave;
            }
            set
            {
                cleave = value;
            }
        }
        [SerializeField] private int cleave = 0;

        public override List<Hexagon> AquireTarget(Unit unit, HexGrid grid)
        {
            throw new System.NotImplementedException();
        }
    }
}