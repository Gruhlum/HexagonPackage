using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    public class HexObject : MonoBehaviour
    {
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
                    hexagon.HexObject = null;
                }
                hexagon = value;
                if (hexagon != null)
                {
                    hexagon.HexObject = this;
                    transform.position = hexagon.transform.position;
                }
            }
        }
        [SerializeField] private Hexagon hexagon = default;

        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            set
            {
                sprite = value;
            }
        }
        [SerializeField] private Sprite sprite = default;


        public List<Cube> Size = new List<Cube>() { new Cube(0, 0) };

        public List<Cube> GetOccupyingCubes(Hexagon hex, int rotation)
        {
            List<Cube> cubes = new List<Cube>();
            foreach (var position in Size)
            {
                Cube cube = position + hex.Cube;
                cube.Rotate(hex.Cube, rotation);
                cubes.Add(cube);
            }
            return cubes;
        }

        public bool ValidPosition(Hexagon hex, int rotation)
        {
            foreach (var position in Size)
            {
                Cube cube = position + hex.Cube;
                cube.Rotate(hex.Cube, rotation);
                Debug.Log(cube.ToString());
                hex.HexGrid.Hexagons.TryGetValue(cube, out Hexagon targetHex);
                if (targetHex == null)// || targetHex.HexObject != null)
                {
                    return false;
                }
            }
            return true;
        }

        public void MoveTo(Hexagon hex)
        {
            if (Hexagon != null && Hexagon.HexObject == this)
            {
                Hexagon.HexObject = null;
            }
            Hexagon = hex;
            Hexagon.HexObject = this;
            transform.position = Hexagon.transform.position;
        }

        public static void SwapObjects(HexObject unit1, HexObject unit2)
        {
            Hexagon hex1 = unit1.Hexagon;
            unit1.MoveTo(unit2.Hexagon);
            unit2.MoveTo(hex1);
        }
    }
}