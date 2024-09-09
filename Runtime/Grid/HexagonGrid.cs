using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    public class HexagonGrid : BaseGrid
    {
        public HexagonData hexagonData;

        //    public Dictionary<Cube, Hexagon> Hexagons = new Dictionary<Cube, Hexagon>();

        //    [SerializeField] private Spawner<Hexagon> hexagonSpawner = default;

        //    public event Action<Hexagon> HexagonAdded;
        //    public event Action<Hexagon> HexagonRemoved;
        //    public event Action GridLoaded;
        //    public event Action GridRemoved;

        //    public HexagonData HexagonData
        //    {
        //        get
        //        {
        //            return hexagonData;
        //        }
        //        set
        //        {
        //            hexagonData = value;
        //        }
        //    }
        //    [SerializeField] private HexagonData hexagonData = default;

        //    [SerializeField] private Spawner<HexagonText> textSpawner = default;


        //    [SerializeField] private bool showCoordinates;


        //    private void Reset()
        //    {
        //        hexagonSpawner = new Spawner<Hexagon>();
        //        if (transform.Find("Hexagons") == false)
        //        {
        //            GameObject go = new GameObject("Hexagons");
        //            go.transform.SetParent(this.transform);
        //            hexagonSpawner.Parent = go.transform;
        //        }
        //    }
        //    private void OnValidate()
        //    {
        //        hexagonSpawner.DestroyUnused();
        //        GetChildrenHexagons();
        //        hexagonSpawner.RemoveEmptyElements();
        //    }
        //    private void Awake()
        //    {
        //        GetChildrenHexagons();
        //        foreach (var hex in Hexagons.Values)
        //        {
        //            hex.Data = HexagonData;
        //            hex.HexGrid = this;
        //        }
        //    }

        //public Hexagon ScreenPointToHexagon(Vector2 point)
        //{
        //    Vector3 pos = Camera.main.ScreenToWorldPoint(point);
        //    return WorldPointToHexagon(pos);
        //}
        //public Hexagon WorldPointToHexagon(Vector2 point)
        //{
        //    Cube c = WorldPointToCube(point);
        //    Hexagons.TryGetValue(c, out Hexagon hex);
        //    return hex;
        //}

        //public Vector2 RoundToGridPosition(Vector2 point)
        //{
        //    Cube c = WorldPointToCube(point);
        //    return CubeToWorldPoint(c);
        //}

        //    //public List<Cube> GetEmptyCubes()
        //    //{
        //    //    List<Cube> emptyHexes = new List<Cube>();
        //    //    foreach (var hexPair in Hexagons)
        //    //    {
        //    //        Hexagon hex = hexPair.Value;
        //    //        if (hex.HexObject == null)
        //    //        {
        //    //            emptyHexes.Add(hexPair.Key);
        //    //        }
        //    //    }
        //    //    return emptyHexes;
        //    //}

        //    public void GetChildrenHexagons()
        //    {
        //        if (hexagonSpawner == null || hexagonSpawner.Parent == null)
        //        {
        //            return;
        //        }
        //        if (hexagonSpawner.Parent.childCount == 0 || hexagonSpawner.Parent.childCount == Hexagons.Count)
        //        {
        //            return;
        //        }
        //        Hexagons = new Dictionary<Cube, Hexagon>();
        //        Hexagon[] hexes = hexagonSpawner.Parent.GetComponentsInChildren<Hexagon>();
        //        foreach (var hex in hexes)
        //        {
        //            if (!Hexagons.ContainsKey(hex.Cube) && hex.gameObject.activeSelf)
        //            {
        //                Hexagons.Add(hex.Cube, hex);
        //            }
        //        }
        //    }

        //    public void RemoveAll(bool destroy = false)
        //    {
        //        if (destroy)
        //        {
        //            hexagonSpawner.TryDestroyAll();
        //        }
        //        else hexagonSpawner.DeactivateAll();

        //        Hexagons.Clear();
        //        GridRemoved?.Invoke();
        //    }

        //    public void RemoveHexagon(Cube cube)
        //    {
        //        Hexagons.TryGetValue(cube, out Hexagon hex);
        //        if (hex == null)
        //        {
        //            Debug.LogWarning("Hex with pos " + cube + " doesn't exist");
        //            return;
        //        }
        //        RemoveHexagon(hex);
        //    }
        //    public void RemoveHexagon(Hexagon hexagon)
        //    {
        //        if (hexagon == null)
        //        {
        //            Debug.LogWarning("Hexagon to be removed is null");
        //            return;
        //        }
        //        Hexagons.Remove(hexagon.Cube);
        //        hexagon.gameObject.SetActive(false);
        //        HexagonRemoved?.Invoke(hexagon);
        //    }

        //    public void SetHexagons(List<Cube> cubes)
        //    {
        //        RemoveAll();
        //        CreateHexagons(cubes);
        //    }
        //    public void SetHexagons(SavedGrid savedGrid)
        //    {
        //        RemoveAll();
        //        CreateHexagons(savedGrid);
        //    }

        //    public Hexagon CreateHexagon(Cube position)
        //    {
        //        if (Hexagons.ContainsKey(position))
        //        {
        //            Debug.LogWarning("Hex with pos " + position + " already exists");
        //            return null;
        //        }
        //        Hexagon hex = hexagonSpawner.Spawn();
        //        hex.Setup(position, HexagonData, this);

        //        if (showCoordinates)
        //        {
        //            hex.SetText(hex.Cube.X + "," + hex.Cube.Y);
        //        }

        //        HexagonAdded?.Invoke(hex);
        //        return hex;
        //    }
        //    public List<Hexagon> CreateHexagons(List<Cube> positions)
        //    {
        //        List<Hexagon> hexes = new List<Hexagon>();
        //        foreach (var position in positions)
        //        {
        //            hexes.Add(CreateHexagon(position));
        //        }
        //        GridLoaded?.Invoke();
        //        return hexes;
        //    }
        //    public List<Hexagon> CreateHexagons(SavedGrid savedGrid, bool ignoreTypes = false)
        //    {
        //        List<Hexagon> hexes = new List<Hexagon>();
        //        foreach (var position in savedGrid.SavedHexagonPositions)
        //        {
        //            Hexagon hex = CreateHexagon(position.cube);
        //            if (!ignoreTypes)
        //            {
        //                hex.HexType = position.type;
        //            }               
        //            hexes.Add(hex);
        //        }
        //        GridLoaded?.Invoke();
        //        return hexes;
        //    }

        //    public List<Hexagon> GetNeighbours(Cube cube, bool unblockedOnly)
        //    {
        //        List<Cube> neighbours = cube.GetNeighbours();
        //        List<Hexagon> validHexes = GetHexagons(neighbours, unblockedOnly);
        //        return validHexes;
        //    }
        //    public List<Hexagon> GetNeighbours(Hexagon hex, bool unblockedOnly)
        //    {
        //        return GetNeighbours(hex.Cube, unblockedOnly);
        //    }

        //    public HexagonText GenerateHexText()
        //    {
        //        return textSpawner.Spawn();
        //    }
        //    [ContextMenu("Toggle Coordinates")]
        //    public void ToggleCoordinates()
        //    {
        //        showCoordinates = !showCoordinates;

        //        if (showCoordinates)
        //        {
        //            foreach (var hex in Hexagons.Values)
        //            {
        //                hex.SetText(hex.Cube.X + "," + hex.Cube.Y);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var hex in Hexagons.Values)
        //            {
        //                hex.SetText(string.Empty);
        //            }
        //            if (Application.isPlaying == false)
        //            {
        //                textSpawner.TryDestroyAll();
        //            }
        //        }
        //    }
        //    public bool ContainsAny(List<Cube> cubes)
        //    {
        //        foreach (var cube in cubes)
        //        {
        //            if (Hexagons.ContainsKey(cube))
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //    public bool Contains(Cube cube)
        //    {
        //        if (Hexagons.ContainsKey(cube))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    public bool Contains(Hexagon hex)
        //    {
        //        if (Hexagons.ContainsKey(hex.Cube))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    public Hexagon GetHexagon(Hexagon hex)
        //    {
        //        return GetHexagon(hex.Cube);
        //    }
        //    public Hexagon GetHexagon(Cube cube)
        //    {
        //        Hexagons.TryGetValue(cube, out Hexagon hex);
        //        return hex;
        //    }
        //    public List<Cube> GetCubes(bool unblockedOnly = false)
        //    {
        //        List<Cube> cubes = new List<Cube>();
        //        foreach (var hex in Hexagons.Values)
        //        {
        //            if (unblockedOnly && hex.IsBlocked)
        //            {
        //                continue;
        //            }
        //            cubes.Add(hex.Cube);
        //        }
        //        return cubes;
        //    }
        //    public List<Hexagon> GetHexagons(bool unblockedOnly = false)
        //    {
        //        List<Hexagon> results = new List<Hexagon>();
        //        if (unblockedOnly == false)
        //        {
        //            results.AddRange(Hexagons.Values);
        //        }
        //        else
        //        {
        //            foreach (var hex in Hexagons.Values)
        //            {
        //                if (hex.IsBlocked == false)
        //                {
        //                    results.Add(hex);
        //                }
        //            }
        //        }           
        //        return results;
        //    }
        //    public List<Hexagon> GetHexagons(List<Cube> cubes, bool unblockedOnly = false)
        //    {
        //        if (cubes == null)
        //        {
        //            return null;
        //        }
        //        List<Hexagon> results = new List<Hexagon>();
        //        foreach (var cube in cubes)
        //        {
        //            Hexagon result = GetHexagon(cube);
        //            if (result != null && (unblockedOnly == false || result.IsBlocked == false))
        //            {
        //                results.Add(result);
        //            }
        //        }
        //        return results;
        //    }
        //    public List<Hexagon> GetHexagons(HexagonType type, bool unblockedOnly = false)
        //    {
        //        List<Hexagon> results = new List<Hexagon>();
        //        foreach (var hex in Hexagons.Values)
        //        {
        //            if (hex.HexType == type && (unblockedOnly == false || hex.IsBlocked == false))
        //            {
        //                results.Add(hex);
        //            }
        //        }
        //        return results;
        //    }
        //    public List<Hexagon> GetHexagons(Cube[] cubes)
        //    {
        //        if (cubes == null)
        //        {
        //            return null;
        //        }
        //        List<Hexagon> results = new List<Hexagon>();
        //        foreach (var cube in cubes)
        //        {
        //            Hexagon result = GetHexagon(cube);
        //            if (result != null)
        //            {
        //                results.Add(result);
        //            }
        //        }
        //        return results;
        //    }

        //    public List<Cube> GetAllCubes()
        //    {
        //        List<Cube> cubes = new List<Cube>();
        //        foreach (var item in Hexagons)
        //        {
        //            cubes.Add(item.Key);
        //        }
        //        return cubes;
        //    }

        //    [ContextMenu("Recalculate Hexagons")]
        //    private void RecalculatePositions()
        //    {
        //        foreach (var hex in Hexagons.Values)
        //        {
        //            hex.Setup(hex.Cube, HexagonData, this);
        //        }
        //    }
        public override List<Coord> GetArea(Coord center, int radius)
        {
            return Cube.GetArea(center, radius);
        }

        public override List<Coord> GetRing(Coord center, int radius)
        {
            return Cube.GetRing(center, radius);
        }

        public override Coord WorldPositionToCoord(Vector3 position)
        {
            Coord result = Cube.WorldPositionToCube(
                position.x - transform.position.x, 
                position.y - transform.position.y, 
                hexagonData.Radius, false);
            return result;
        }
        public override Vector3 CoordToWorldPoint(Coord coord)
        {
            Vector2 result = Cube.ToWorldPosition(coord, hexagonData.VerticalSpacing, hexagonData.HorizontalSpacing, false);
            return result += (Vector2)transform.position;
        }

        public override List<Coord> GetNeighbourCoords(Coord center)
        {
            return Cube.GetNeighbours(center);
        }

        public override Coord GetDirectionCoord(Coord coord, int direction)
        {
            throw new NotImplementedException();
        }

        public override int GetDirection(Coord center, Coord coord)
        {
            throw new NotImplementedException();
        }

        public override Coord GetRotatedCoord(Coord center, Coord coord, int rotation)
        {
            throw new NotImplementedException();
        }

        public override List<Coord> GetAdjacents(Coord center)
        {
            throw new NotImplementedException();
        }
    }
}