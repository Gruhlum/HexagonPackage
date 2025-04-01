using System.Collections;
using System.Collections.Generic;
using HexTecGames.GridBaseSystem;
using HexTecGames.GridBaseSystem.Shapes;
using UnityEngine;

namespace HexTecGames.GridHexSystem.Shapes
{
    [System.Serializable]
    public class HexagonShape : Shape
    {
        [SerializeField] private int radius = 1;

        public override List<Coord> GetCoords(Coord center)
        {
            return Cube.GetArea(center, radius);
        }
    }
}