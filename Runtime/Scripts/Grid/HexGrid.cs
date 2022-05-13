using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
    public class HexGrid : MonoBehaviour
    {
        [SerializeField] private GameObject hexagonPrefab = null;
        public Dictionary<Cube, Hexagon> Hexagons = new Dictionary<Cube, Hexagon>();

        private List<Hexagon> hexagonObjects = new List<Hexagon>();

        public event Action<Hexagon> HexagonAdded;
        public event Action<Hexagon> HexagonRemoved;

        public SavedGrid GridToLoad;

        public HexagonData HexagonData
        {
            get
            {
                return hexagonData;
            }
            set
            {
                hexagonData = value;
            }
        }
        [SerializeField] private HexagonData hexagonData = default;


        private void OnValidate()
        {
            //for (int i = hexagonObjects.Count - 1; i >= 0; i--)
            //{
            //    if (hexagonObjects[i].gameObject.activeSelf == false)
            //    {
            //        DestroyImmediate(hexagonObjects[i].gameObject);
            //    }
            //}
            //GetChildrenHexagons();
            //CleanUp();
        }
        private void Awake()
        {
            GetChildrenHexagons();
            foreach (var hex in hexagonObjects)
            {
                hex.Data = HexagonData;
                hex.HexGrid = this;
            }
        }
        public Hexagon ScreenPointToHexagon(Vector2 point)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return WorldPointToHexagon(pos);
        }
        public Hexagon WorldPointToHexagon(Vector2 point)
        {
            Cube c = HexagonData.WorldPositionToCube(point.x - transform.position.x, point.y - transform.position.y);           
            Hexagons.TryGetValue(c, out Hexagon hex);
            return hex;
        }

        public Hexagon MouseToHexagon()
        {
            return ScreenPointToHexagon(Input.mousePosition);
        }

        //public List<Cube> GetEmptyCubes()
        //{
        //    List<Cube> emptyHexes = new List<Cube>();
        //    foreach (var hexPair in Hexagons)
        //    {
        //        Hexagon hex = hexPair.Value;
        //        if (hex.HexObject == null)
        //        {
        //            emptyHexes.Add(hexPair.Key);
        //        }
        //    }
        //    return emptyHexes;
        //}

        public void GetChildrenHexagons()
        {
            Hexagons = new Dictionary<Cube, Hexagon>();
            hexagonObjects = new List<Hexagon>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Hexagon hex = transform.GetChild(i).GetComponent<Hexagon>();
                if (hex.gameObject.activeSelf == false)
                {
                    hexagonObjects.Add(hex);
                }               
                else
                {
                    Hexagons.Add(hex.Cube, hex);
                    hexagonObjects.Add(hex);
                }
            }
        }
        private void CleanUp()
        {
            for (int i = hexagonObjects.Count - 1; i >= 0; i--)
            {
                if (hexagonObjects[i] == null)
                {
                    hexagonObjects.RemoveAt(i);
                }
            }
        }
        public void RemoveAll(bool destroy = false)
        {
            //foreach (var hex in Hexagons.Values)
            //{
            //    
            //    DestroyImmediate(hex.gameObject);
            //}
            //for (int i = 0; i < Hexagons.Count; i++)
            //{
            //    Hexagons.ToLookup(x => x.Value.gameObject);
            //    Hexagons.ElementAt(i).Value.gameObject.SetActive(false);
            //}
            for (int i = Hexagons.Count - 1; i >= 0; i--)
            {
                if (Hexagons.ElementAt(i).Value == null)
                {
                    
                    Hexagons.Remove(Hexagons.ElementAt(i).Key);
                }
                else
                {
                    if (destroy)
                    {
                        hexagonObjects.Remove(Hexagons.ElementAt(i).Value);
                        DestroyImmediate(Hexagons.ElementAt(i).Value.gameObject);
                        Hexagons.Remove(Hexagons.ElementAt(i).Key);
                    }
                    else
                    {
                        Hexagons.ElementAt(i).Value.gameObject.SetActive(false);
                        Hexagons.Remove(Hexagons.ElementAt(i).Key);
                    }
                } 
            }
            Hexagons = new Dictionary<Cube, Hexagon>();
        }
        public void RemoveHexagon(Cube cube)
        {
            Hexagons.TryGetValue(cube, out Hexagon hex);
            if (hex == null)
            {
                Debug.LogWarning("Hex with pos " + cube + " doesn't exist");
                return;
            }
            RemoveHexagon(hex);
        }
        public void RemoveHexagon(Hexagon hexagon)
        {
            if (hexagon == null)
            {
                Debug.LogWarning("Hexagon to be removed is null");
                return;
            }
            Hexagons.Remove(hexagon.Cube);
            hexagon.gameObject.SetActive(false);
            HexagonRemoved?.Invoke(hexagon);
        }

        public Hexagon CreateHexagon(Cube pos)
        {
            if (Hexagons.ContainsKey(pos))
            {
                Debug.LogWarning("Hex with pos " + pos + " already exists");
                return null;
            }
            Hexagon hex = GetEmptyHexagon();
            hex.Setup(pos, HexagonData, this);
            HexagonAdded?.Invoke(hex);
            return hex;
        }

        private Hexagon GetEmptyHexagon()
        {
            if (hexagonObjects.Any(x => x == null))
            {
                hexagonObjects.Clear();
            }
            if (hexagonObjects.Any(x => x.gameObject.activeSelf == false))
            {
                return hexagonObjects.Find(x => x.gameObject.activeSelf == false);
            }
            else
            {
                GameObject hexGO = Instantiate(hexagonPrefab, transform);
                Hexagon hex = hexGO.GetComponent<Hexagon>();
                hexagonObjects.Add(hex);
                return hex;
            }
        }
        public List<Hexagon> GetNeighbours(Cube cube)
        {
            List<Cube> neighbours = cube.GetNeighbours();
            List<Hexagon> validHexes = GetHexagons(neighbours);
            return validHexes;
        }
        public List<Hexagon> GetNeighbours(Hexagon hex)
        {
            return GetNeighbours(hex.Cube);
        }

        public bool Contains(Cube cube)
        {
            if (Hexagons.ContainsKey(cube))
            {
                return true;
            }
            return false;
        }
        public bool Contains(Hexagon hex)
        {
            if (Hexagons.ContainsKey(hex.Cube))
            {
                return true;
            }
            return false;
        }
        public Hexagon GetHexagon(Hexagon hex)
        {
            return GetHexagon(hex.Cube);
        }
        public Hexagon GetHexagon(Cube cube)
        {
            Hexagons.TryGetValue(cube, out Hexagon hex);
            return hex;
        }
        public List<Hexagon> GetHexagons(List<Cube> cubes)
        {
            List<Hexagon> results = new List<Hexagon>();
            foreach (var cube in cubes)
            {
                Hexagon result = GetHexagon(cube);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            return results;
        }
        [ContextMenu("Load Grid")]
        public void LoadGrid()
        {
            if (GridToLoad == null)
            {
                Debug.LogWarning("GridToLoad is null!");
                return;
            }
            GetChildrenHexagons();
            RemoveAll(true);
            foreach (var pos in GridToLoad.SavedHexagonPositions)
            {
                CreateHexagon(pos.cube).Type = pos.type;
            }
        }             

        [ContextMenu("Recalculate")]
        private void RecalculatePositions()
        {
            foreach (var hex in Hexagons.Values)
            {
                hex.Setup(hex.Cube, HexagonData, this);
            }
        }
    }
}