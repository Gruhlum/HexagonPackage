using HexagonPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exile
{
    public class LineAbility : Ability
    {
        public int Range
        {
            get
            {
                return range;
            }
            set
            {
                range = value;
            }
        }
        [SerializeField] private int range = default;

        public int Pierce
        {
            get
            {
                return pierce;
            }
            set
            {
                pierce = value;
            }
        }
        [SerializeField] private int pierce = 1;


        public override List<Hexagon> AquireTarget(Unit unit, HexGrid grid)
        {
            List<Hexagon> targets = new List<Hexagon>();
            Hexagon initialTarget = GetInitialTarget(unit, grid);
            targets.Add(initialTarget);

            int direction = unit.Hexagon.Cube.GetDirection(initialTarget.Cube);

            List<Cube> cubes = unit.Hexagon.Cube.GetLine(initialTarget.Cube + Cube.GetCubeFromDirection(direction) * Pierce);
            targets.AddRange(grid.GetHexagons(cubes));
            return targets;
        }

        private Hexagon GetInitialTarget(Unit unit, HexGrid grid)
        {
            List<Hexagon> targets = GetMeleeTargets(unit, grid);
            if (targets.Count != 0)
            {
                return targets[0];
            }

            int currentRange = 1;
            while (currentRange < Range)
            {
                currentRange++;
                List<Cube> results = unit.Hexagon.Cube.GetRing(currentRange);
                List<Hexagon> hexes = grid.GetHexagons(results);

                foreach (var hex in hexes)
                {
                    if (hex.HexObject is Unit target && target.Team != unit.Team)
                    {
                        return hex;
                    }
                }
            }
            return null;
        }
    }
}