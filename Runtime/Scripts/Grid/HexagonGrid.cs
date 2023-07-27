using HexTecGames.Basics;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
    [ExecuteAlways]
    public class HexagonGrid : MonoBehaviour
    {
        public Dictionary<Cube, Hexagon> Hexagons = new Dictionary<Cube, Hexagon>();

        [SerializeField] private Spawner<Hexagon> hexagonSpawner = default;

        public event Action<Hexagon> HexagonAdded;
        public event Action<Hexagon> HexagonRemoved;

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

        [SerializeField] private Spawner<HexagonText> textSpawner = default;

        public event Action GridLoaded;

        [SerializeField] private bool showCoordinates;

        private void OnValidate()
        {
            hexagonSpawner.DestroyUnused();
            GetChildrenHexagons();
            hexagonSpawner.RemoveEmptyElements();
        }
        private void Awake()
        {
            GetChildrenHexagons();
            foreach (var hex in Hexagons.Values)
            {
                hex.Data = HexagonData;
                hex.HexGrid = this;
            }
        }
        private void Reset()
        {
            hexagonSpawner = new Spawner<Hexagon>();
            if (transform.Find("Hexes") == false)
            {
                GameObject go = new GameObject("Hexes");
                go.transform.SetParent(this.transform);
            }
        }
        public Hexagon ScreenPointToHexagon(Vector2 point)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(point);
            return WorldPointToHexagon(pos);
        }
        public Hexagon WorldPointToHexagon(Vector2 point)
        {
            Cube c = WorldPointToCube(point);
            Hexagons.TryGetValue(c, out Hexagon hex);
            return hex;
        }
        public Cube WorldPointToCube(Vector2 point)
        {
            return Cube.WorldPositionToCube(point.x - transform.position.x, point.y - transform.position.y, HexagonData.Radius, hexagonData.Flat);
        }
        public Hexagon MousePositionToHexagon()
        {
            return WorldPointToHexagon(Camera.main.GetMousePosition());
        }
        public Cube MousePositionToCube()
        {
            return WorldPointToCube(Camera.main.GetMousePosition());
        }
        public Vector2 CubeToWorldPoint(Cube cube)
        {
            return cube.ToWorldPosition(HexagonData.VerticalSpacing, HexagonData.HorizontalSpacing, HexagonData.Flat);
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
            if (hexagonSpawner == null || hexagonSpawner.Parent == null)
            {
                return;
            }
            if (hexagonSpawner.Parent.childCount == 0 || hexagonSpawner.Parent.childCount == Hexagons.Count)
            {
                return;
            }
            Hexagons = new Dictionary<Cube, Hexagon>();
            Hexagon[] hexes = hexagonSpawner.Parent.GetComponentsInChildren<Hexagon>();
            foreach (var hex in hexes)
            {
                if (hex.gameObject.activeSelf)
                {
                    Hexagons.Add(hex.Cube, hex);
                }
            }
        }

        public void RemoveAll(bool destroy = false)
        {
            if (destroy)
            {
                hexagonSpawner.TryDestroyAll();
            }
            else hexagonSpawner.DeactivateAll();

            Hexagons.Clear();
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

        public Hexagon CreateHexagon(Cube position)
        {
            if (Hexagons.ContainsKey(position))
            {
                Debug.LogWarning("Hex with pos " + position + " already exists");
                return null;
            }
            Hexagon hex = hexagonSpawner.Spawn();
            hex.Setup(position, HexagonData, this);
            //hex.gameObject.transform.SetParent(transform.GetChild(0));

            if (showCoordinates)
            {
                hex.SetText(hex.Cube.X + "," + hex.Cube.Y);
            }

            HexagonAdded?.Invoke(hex);
            return hex;
        }
        public List<Hexagon> CreateHexagons(List<Cube> positions)
        {
            List<Hexagon> hexes = new List<Hexagon>();
            foreach (var position in positions)
            {
                hexes.Add(CreateHexagon(position));
            }
            GridLoaded?.Invoke();
            return hexes;
        }
        public List<Hexagon> CreateHexagons(SavedGrid savedGrid, List<HexagonType> ignoreTypes = null)
        {
            List<Hexagon> hexes = new List<Hexagon>();
            foreach (var position in savedGrid.SavedHexagonPositions)
            {
                Hexagon hex = CreateHexagon(position.cube);
                //if (ignoreTypes != null && ignoreTypes.Any(x => x == position.type))
                //{
                //    hex.Type = null;
                //}
                //else hex.Type = position.type;
                hexes.Add(hex);
            }
            GridLoaded?.Invoke();
            return hexes;
        }

        //private Hexagon GetEmptyHexagon()
        //{
        //    if (hexagonObjects.Any(x => x == null))
        //    {
        //        hexagonObjects.Clear();
        //    }
        //    if (hexagonObjects.Any(x => x.gameObject.activeSelf == false))
        //    {
        //        return hexagonObjects.Find(x => x.gameObject.activeSelf == false);
        //    }
        //    else
        //    {
        //        GameObject hexGO = Instantiate(hexagonPrefab, transform);
        //        Hexagon hex = hexGO.GetComponent<Hexagon>();
        //        hexagonObjects.Add(hex);
        //        return hex;
        //    }
        //}
        public List<Hexagon> GetNeighbours(Cube cube, bool unblockedOnly)
        {
            List<Cube> neighbours = cube.GetNeighbours();
            List<Hexagon> validHexes = GetHexagons(neighbours, unblockedOnly);
            return validHexes;
        }
        public List<Hexagon> GetNeighbours(Hexagon hex, bool unblockedOnly)
        {
            return GetNeighbours(hex.Cube, unblockedOnly);
        }

        public HexagonText GenerateHexText()
        {
            return textSpawner.Spawn();
        }
        [ContextMenu("Toggle Coordinates")]
        public void ToggleCoordinates()
        {
            showCoordinates = !showCoordinates;

            if (showCoordinates)
            {
                foreach (var hex in Hexagons.Values)
                {
                    hex.SetText(hex.Cube.X + "," + hex.Cube.Y);
                }
            }
            else
            {
                foreach (var hex in Hexagons.Values)
                {
                    hex.SetText(string.Empty);
                }
            }
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
        public List<Cube> GetCubes(bool unblockedOnly)
        {
            List<Cube> cubes = new List<Cube>();
            foreach (var hex in Hexagons.Values)
            {
                if (unblockedOnly && hex.IsBlocked)
                {
                    continue;
                }
                cubes.Add(hex.Cube);
            }
            return cubes;
        }
        public List<Hexagon> GetHexagons(bool unblockedOnly)
        {
            List<Hexagon> results = new List<Hexagon>();
            if (unblockedOnly == false)
            {
                results.AddRange(Hexagons.Values);
            }
            else
            {
                foreach (var hex in Hexagons.Values)
                {
                    if (hex.IsBlocked == false)
                    {
                        results.Add(hex);
                    }
                }
            }           
            return results;
        }
        public List<Hexagon> GetHexagons(List<Cube> cubes, bool unblockedOnly)
        {
            if (cubes == null)
            {
                return null;
            }
            List<Hexagon> results = new List<Hexagon>();
            foreach (var cube in cubes)
            {
                Hexagon result = GetHexagon(cube);
                if (result != null && (unblockedOnly == false || result.IsBlocked == false))
                {
                    results.Add(result);
                }
            }
            return results;
        }
        public List<Hexagon> GetHexagons(HexagonType type, bool unblockedOnly)
        {
            List<Hexagon> results = new List<Hexagon>();
            foreach (var hex in Hexagons.Values)
            {
                if (hex.Type == type && (unblockedOnly == false || hex.IsBlocked == false))
                {
                    results.Add(hex);
                }
            }
            return results;
        }
        public List<Hexagon> GetHexagons(Cube[] cubes)
        {
            if (cubes == null)
            {
                return null;
            }
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

        [ContextMenu("Recalculate Hexagons")]
        private void RecalculatePositions()
        {
            foreach (var hex in Hexagons.Values)
            {
                hex.Setup(hex.Cube, HexagonData, this);
            }
        }
    }
}