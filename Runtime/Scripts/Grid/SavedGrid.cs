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
            SavedHexagonPositions.Clear();
            foreach (var hex in hexagons)
            {
                SavedHexagonPositions.Add(new SavePosition(hex.Cube, hex.IsBlocked? null : hex.Type));
            }
        }
        public void MoveAllTiles(int x, int y)
        {
            for (int i = 0; i < SavedHexagonPositions.Count; i++)
            {
                Cube c = SavedHexagonPositions[i].cube;
                c.X += x;
                c.Y += y;
                SavedHexagonPositions[i] = new SavePosition(c, SavedHexagonPositions[i].type);
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
