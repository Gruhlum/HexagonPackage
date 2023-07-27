using HexagonPackage.HexagonComponents;
using HexagonPackage.HexObjects.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexagonPackage.HexObjects
{
    public class MultiTileBuilding : Building
    {
        public List<Cube> Size = new List<Cube>();
        private List<Hexagon> occupyingHexes;
        public override void Setup(Cube center, HexagonGrid grid, int rotation, bool block = true)
        {
            Center = center;
            ClearHexagon();
            occupyingHexes = grid.GetHexagons(GetOccupyingCubes(center, rotation), true);
            Rotate(rotation);
            //Hexagon = hexes[0];
            if (block)
            {
                //Debug.Log(occupyingHexes.Count + " . ");
                foreach (var hex in occupyingHexes)
                {
                    
                    hex.HexObject = this;
                }
            }
        }
        protected override void ClearHexagon()
        {
            if (occupyingHexes == null)
            {
                return;
            }
            foreach (var hex in occupyingHexes)
            {
                if (hex.HexObject == this)
                {
                    hex.HexObject = null;
                }                
            }
            occupyingHexes.Clear();
        }
        public override List<Cube> GetOccupyingCubes(Cube center, int rotation = 0)
        {
            
            List<Cube> relativeCubes = new List<Cube>();
            foreach (var cube in Size)
            {
                Cube c = cube;// + center;
                //c.Rotate(center, rotation);
                relativeCubes.Add(c);
            }
            return relativeCubes;
        }
        public override bool IsValidPosition(Cube center, HexagonGrid grid, int rotation)
        {
            Hexagon hex = grid.GetHexagon(center);
            foreach (var position in Size)
            {
                Cube cubeOffset = position + center;
                cubeOffset.Rotate(center, rotation);
                //Debug.Log(cube.ToString());
                grid.Hexagons.TryGetValue(cubeOffset, out Hexagon targetHex);
                if (targetHex == null || targetHex.HexObject != null)
                {
                    return false;
                }
            }
            return true;
        }
        public List<Cube> GetInvalidPositions(Cube center, HexagonGrid grid, int rotation)
        {
            List<Cube> results = new List<Cube>();
            foreach (var position in Size)
            {
                Cube cube = position + center;
                cube.Rotate(center, rotation);
                //Debug.Log(cube.ToString());
                grid.Hexagons.TryGetValue(cube, out Hexagon targetHex);
                if (targetHex == null || targetHex.HexObject != null)
                {
                    results.Add(cube);
                }
            }
            return results;
        }
        public override BuildEventArgs GenerateBuildEventArgs(Cube center, int rotation)
        {
            return new BuildEventArgs(GetOccupyingCubes(center, rotation), this);
        }
    }
}