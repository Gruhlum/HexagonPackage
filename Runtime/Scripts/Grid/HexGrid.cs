using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
    public class HexGrid : MonoBehaviour
    {
        public float SpacingX = 10f;
        public float SpacingY = 10f;

        [SerializeField] private GameObject hexagonPrefab = null;
        public Dictionary<Cube, Hexagon> Hexagons = new Dictionary<Cube, Hexagon>();

        private List<Hexagon> hexagonObjects = new List<Hexagon>();

        public event Action<Hexagon> HexagonAdded;
        public event Action<Hexagon> HexagonRemoved;

        public event Action<Hexagon, int> HexagonClicked;
        public event Action<Hexagon> HexagonMouseEnter;
        public event Action<Hexagon> HexagonMouseExit;

        public GameObject go;

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
            foreach (var hex in Hexagons.Values)
            {
                AddClickListener(hex);
            }
        }
        private void OnHexagon_Clicked(Hexagon hex, int btn)
        {
            HexagonClicked?.Invoke(hex, btn);
        }
        private void OnHexagon_MouseEnter(Hexagon hex)
        {
            HexagonMouseEnter?.Invoke(hex);
        }
        private void OnHexagon_MouseExit(Hexagon hex)
        {
            HexagonMouseExit?.Invoke(hex);
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
                    RemoveClickListener(Hexagons.ElementAt(i).Value);
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
            RemoveClickListener(hexagon);
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
            hex.Setup(pos, this);
            AddClickListener(hex);
            HexagonAdded?.Invoke(hex);
            return hex;
        }
        private void AddClickListener(Hexagon hex)
        {
            ClickEvents clickEvents = hex.GetComponent<ClickEvents>();
            clickEvents.HexagonClicked += OnHexagon_Clicked;
            clickEvents.HexagonMouseEnter += OnHexagon_MouseEnter;
            clickEvents.HexagonMouseExit += OnHexagon_MouseExit;
        }
        private void RemoveClickListener(Hexagon hex)
        {
            ClickEvents clickEvents = hex.GetComponent<ClickEvents>();
            clickEvents.HexagonClicked -= OnHexagon_Clicked;
            clickEvents.HexagonMouseEnter -= OnHexagon_MouseEnter;
            clickEvents.HexagonMouseExit -= OnHexagon_MouseExit;
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
            List<Hexagon> validHexes = FindHex(neighbours);
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
        public Hexagon FindHexagon(Hexagon hex)
        {
            return FindHex(hex.Cube);
        }
        public List<Hexagon> FindHex(List<Cube> cubes)
        {
            List<Hexagon> results = new List<Hexagon>();
            foreach (var cube in cubes)
            {
                Hexagon result = FindHex(cube);
                if (result != null)
                {
                    results.Add(result);
                }
            }
            return results;
        }
        public Hexagon FindHex(Cube cube)
        {
            Hexagons.TryGetValue(cube, out Hexagon hex);
            return hex;
        }

        [ContextMenu("Recalculate")]
        private void RecalculatePositions()
        {
            foreach (var hex in Hexagons.Values)
            {
                hex.Setup(hex.Cube, this);
            }
        }
    }
}