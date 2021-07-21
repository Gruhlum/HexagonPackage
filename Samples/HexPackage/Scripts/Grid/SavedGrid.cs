using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
    public class SavedGrid : ScriptableObject
    {
        public List<SavePosition> SavedHexagonPositions = new List<SavePosition>();

        public void SaveGridData(List<Hexagon> hexagons)
        {
            foreach (var hex in hexagons)
            {
                SavedHexagonPositions.Add(new SavePosition(hex.Cube, hex.Type));
            }
        }
    }

    [System.Serializable]
    public struct SavePosition
    {
        public Cube cube;
        public HexagonType type;

        public SavePosition(Cube cube, HexagonType type)
        {
            this.cube = cube;
            this.type = type;
        }
    }
}
