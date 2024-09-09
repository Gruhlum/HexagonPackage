﻿using HexTecGames.GridBaseSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

namespace HexagonPackage
{
    public static class Cube
    {
        private static readonly float SQRT_3 = 1.73205f;

        //public int X;
        //public int Y;
        //public int Z;

        //public Cube cameFrom;
        // Starts btm-right, goes counter-clockwise
        public static readonly List<Coord> CubeDirections = new List<Coord>
            { new Coord(1, -1), new Coord(1, 0), new Coord(0, 1),
            new Coord(-1, 1), new Coord(-1, 0), new Coord(0, -1)};

        public static Coord Zero
        {
            get
            {
                return zero;
            }
        }
        private static readonly Coord zero = new Coord(0, 0);
        public static Coord Up
        {
            get
            {
                return up;
            }
        }
        private static readonly Coord up = new Coord(0, 1);
        public static Coord Down
        {
            get
            {
                return down;
            }
        }
        private static readonly Coord down = new Coord(0, -1);
        public static Coord Left
        {
            get
            {
                return left;
            }
        }
        private static readonly Coord left = new Coord(-1, 0);
        public static Coord Right
        {
            get
            {
                return right;
            }
        }
        private static readonly Coord right = new Coord(1, 0);

        //public Cube(Cube cube)
        //{
        //    X = cube.X;
        //    Y = cube.Y;
        //    Z = cube.Z;
        //}

        //public Cube(int x, int y)
        //{
        //    X = x;
        //    Y = y;
        //    Z = -(x + y);
        //}
        //public Cube(float x, float y)
        //{
        //    this = Round(x, y, -(x + y));
        //}

        //public void SetValues(Cube cube)
        //{
        //    X = cube.X;
        //    Y = cube.Y;
        //    Z = cube.Z;
        //}
        //public void SetValues(int x, int y)
        //{
        //    X = x;
        //    Y = y;
        //    Z = -(x + y);
        //}

        //public static void Rotate(Coord center, int direction)
        //{
        //    direction = WrapDirection(direction);
        //    this -= center;
        //    for (int i = 0; i < direction; i++)
        //    {
        //        SetValues(-Y, -Z);
        //    }
        //    this += center;
        //}

        public static Vector2 ToWorldPosition(Coord coord, float verticalSpacing, float horizontalSpacing, bool flat)
        {
            if (flat)
            {
                return new Vector2(verticalSpacing * coord.x, horizontalSpacing * (coord.y + coord.x / 2f));
            }
            else return new Vector2(horizontalSpacing * (coord.x + coord.y / 2f), verticalSpacing * coord.y);
        }
        public static Coord WorldPositionToCube(float x, float y, float radius, bool flat)
        {
            if (flat) //TODO: Not working
            {
                return Round((SQRT_3 / 3 * x - 1f / 3f * y) / radius, (2f / 3f * y) / radius);
            }
            else
            {
                return Round((SQRT_3 / 3 * x - 1f / 3f * y) / radius, (2f / 3f * y) / radius);
            }
        }
        public static Coord GetNeighbour(Coord center, int direction)
        {
            return center + CubeDirections[WrapDirection(direction)];
        }
        public static List<Coord> GetNeighbours(Coord center)
        {
            List<Coord> results = new List<Coord>();
            foreach (var direction in CubeDirections)
            {
                results.Add(center + direction);
            }
            return results;
        }
        public static bool IsNeighbour(Coord coord1, Coord coord2)
        {
            if (GetNeighbours(coord1).Any(result => result == coord2))
            {
                return true;
            }
            return false;
        }
        public static Coord GetCenterCoord(List<Coord> coords)
        {
            float x = coords.Select(c => c.x).Sum() / (float)coords.Count;
            float y = coords.Select(c => c.y).Sum() / (float)coords.Count;
            return Round(x, y);
        }

        //public static Cube[] GetShortestPath(Cube start, Cube end, List<Cube> positions, int range = 50)
        //{
        //    Path[][] cubePaths = new Path[range][];
        //    cubePaths[0] = new Path[] { new Path(start, start) };
        //    List<Cube> visited = new List<Cube>() { start };
        //    for (int i = 1; i < range; i++)
        //    {
        //        List<Path> allowedNeighbours = new List<Path>();
        //        foreach (var preCube in cubePaths[i - 1])
        //        {
        //            List<Cube> neighbours = preCube.Cube.GetNeighbours();
        //            foreach (var neighbour in neighbours)
        //            {
        //                if (!visited.Contains(neighbour) && positions.Contains(neighbour))
        //                {
        //                    allowedNeighbours.Add(new Path(neighbour, preCube.Cube));
        //                    if (neighbour == end)
        //                    {
        //                        return GetPath(start, cubePaths);
        //                    }
        //                    visited.Add(neighbour);
        //                }
        //            }
        //        }
        //        cubePaths[i] = allowedNeighbours.ToArray();
        //    }
        //    return null;

        //    //List<Cube> path = new List<Cube>();
        //    //List<Cube> line = start.GetLine(end);
        //    //int direction = start.GetDirection(end);
        //    //List<Cube> checkedCubes = new List<Cube>();
        //    //Cube lastCube = start;
        //    //int count = 0;

        //    //for (int i = 0; i < 5; i++)
        //    //{
        //    //    int[] directions = new int[5] { 0, 1, -1, 2, -2 };
        //    //    Cube neigh = lastCube.GetNeighbour(direction + directions[i]);
        //    //    if (positions.Contains(neigh))
        //    //    {
        //    //        path.Add(neigh);
        //    //        lastCube = neigh;
        //    //        break;
        //    //    }
        //    //}

        //    //while (lastCube != end || count > 100)
        //    //{
        //    //    count++;

        //    //    List<Cube> neighbours = lastCube.GetNeighbours();
        //    //    List<Cube> freeNeighbours = new List<Cube>();
        //    //    foreach (var neighbour in neighbours)
        //    //    {
        //    //        if (checkedCubes.Contains(neighbour) || positions.Contains(neighbour))
        //    //        {
        //    //            freeNeighbours.Add(neighbour);
        //    //        }
        //    //    }

        //    //    checkedCubes.AddRange(neighbours);

        //    //    foreach (var neighbour in freeNeighbours)
        //    //    {
        //    //        if (line.Contains(neighbour))
        //    //        {
        //    //            path.Add(neighbour);
        //    //            lastCube = neighbour;
        //    //            break;
        //    //        }
        //    //    }
        //    //}
        //    //return path;
        //}
        //public static Cube[] GetPath(Cube start, Cube end, List<Cube> positions, int range = 20)
        //{
        //    return GetPath(end, GetPathingMap(start, positions, range));
        //}
        //public static Cube[] GetPath(Cube start, Path[][] path)
        //{
        //    if (start == null)
        //    {
        //        return null;
        //    }
        //    Cube[] cubePath;
        //    for (int i = 0; i < path.Length; i++)
        //    {
        //        if (Array.Exists(path[i], x => x.Cube == start))
        //        {
        //            Path targetPath = Array.Find(path[i], x => x.Cube == start);
        //            cubePath = new Cube[i + 1];
        //            cubePath[i] = targetPath.Cube;
        //            //Debug.Log(i);
        //            int distance = i - 1;
        //            while (distance > 0)
        //            {
        //                // To alternate between left and right
        //                //Debug.Log("hi");
        //                //Array.Reverse(path[distance]);
        //                //if (distance % 2 != 0)
        //                //{
        //                //    Debug.Log("hi");
        //                //    Array.Reverse(path[distance]);
        //                //}
        //                targetPath = Array.Find(path[distance], x => x.Cube == targetPath.CameFrom);
        //                cubePath[distance] = targetPath.Cube;
        //                distance--;
        //            }

        //            cubePath[0] = path[0][0].Cube;

        //            return cubePath;
        //        }
        //    }
        //    return null;
        //}
        //public static bool IsPathAvailable(Cube start, Cube end, List<Cube> positions)
        //{
        //    Path[][] cubePaths = new Path[100][];
        //    cubePaths[0] = new Path[] { new Path(start, start) };
        //    List<Cube> visited = new List<Cube>() { start };
        //    for (int i = 1; i < 100; i++)
        //    {
        //        List<Path> allowedNeighbours = new List<Path>();
        //        foreach (var preCube in cubePaths[i - 1])
        //        {
        //            List<Cube> neighbours = preCube.Cube.GetNeighbours();
        //            foreach (var neighbour in neighbours)
        //            {
        //                if (!visited.Contains(neighbour) && positions.Contains(neighbour))
        //                {
        //                    if (neighbour == end)
        //                    {
        //                        return true;
        //                    }
        //                    allowedNeighbours.Add(new Path(neighbour, preCube.Cube));
        //                    visited.Add(neighbour);
        //                }
        //            }
        //        }
        //        cubePaths[i] = allowedNeighbours.ToArray();
        //    }
        //    return false;
        //}
        //public static Cube? GetNextTile(Cube current, Path[][] PathingMap)
        //{
        //    if (current == null)
        //    {
        //        return null;
        //    }
        //    for (int i = 0; i < PathingMap.Length; i++)
        //    {
        //        if (!Array.Exists(PathingMap[i], x => x.Cube == current))
        //        {
        //            continue;
        //        }
        //        if (i == 0)
        //        {
        //            return null;
        //        }

        //        Path path = Array.Find(PathingMap[i], x => x.Cube == current);
        //        return path.CameFrom;
        //    }
        //    return null;
        //}
        //public static Path[][] GetPathingMap(Cube start, List<Cube> positions, int range = 20)
        //{
        //    Path[][] cubePaths = new Path[range][];
        //    cubePaths[0] = new Path[] { new Path(start, start) };
        //    List<Cube> visited = new List<Cube>() { start };
        //    for (int i = 1; i < range; i++)
        //    {
        //        List<Path> allowedNeighbours = new List<Path>();
        //        foreach (var preCube in cubePaths[i - 1])
        //        {
        //            List<Cube> neighbours = preCube.Cube.GetNeighbours();
        //            foreach (var neighbour in neighbours)
        //            {
        //                if (!visited.Contains(neighbour) && positions.Contains(neighbour))
        //                {
        //                    allowedNeighbours.Add(new Path(neighbour, preCube.Cube));
        //                    visited.Add(neighbour);
        //                }
        //            }
        //        }
        //        cubePaths[i] = allowedNeighbours.ToArray();
        //    }
        //    return cubePaths;
        //}

        public static Coord GetClosestCube(Coord center, List<Coord> coords, int maxDistance = 20)
        {
            if (coords.Count <= 1)
            {
                return center;
            }
            for (int i = 0; i < maxDistance; i++)
            {
                List<Coord> results = GetRing(center, i);
                if (results.Count <= 0)
                {
                    continue;
                }
                foreach (var result in results)
                {
                    if (coords.Contains(result))
                    {
                        return result;
                    }
                }
            }
            return center;
        }

        public static List<Coord> GetLine(Coord coord1, Coord coord2)
        {
            List<Coord> pathCubes = new List<Coord>();

            if (coord1 == coord2)
            {
                return pathCubes;
            }

            int distance = GetDistance(coord1, coord2);

            for (int i = 0; i <= distance; i++)
            {
                float t = ((float)i / (float)distance);
                pathCubes.Add(Lerp(coord1, coord2, t));
            }
            return pathCubes;
        }

        public static List<Coord> GetArea(Coord center, int radius)
        {
            List<Coord> results = new List<Coord>();
            for (int i = 0; i < radius; i++)
            {
                results.AddRange(GetRing(center, i));
            }
            return results;
        }
        public static List<Coord> GetRing(Coord center, int radius)
        {
            List<Coord> results = new List<Coord>();
            if (radius == 0)
            {
                results.Add(center);
                return results;
            }
            Coord cube = center + CubeDirections[4] * radius;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    cube = GetNeighbour(cube, i);
                    results.Add(cube);
                }
            }
            return results;
        }

        public static int GetNeighbourDirection(Coord coord1, Coord coord2)
        {
            Coord normalized = coord1 - coord2;
            //Debug.Log(c + " - " + target);
            //Debug.Log(target.ToString() + " - " + this.ToString() + " - " + c.ToString());
            if (CubeDirections.Any(c => c == normalized))
            {
                return CubeDirections.IndexOf(normalized);
            }
            else return -1;
        }

        public static int GetDirection(Coord coord1, Coord coord2)
        {
            if (coord1 == null || coord1 == coord2)
            {
                Debug.Log("target and start are the same!");
                return -1;
            }

            List<Coord> path = GetLine(coord1, coord2);
            if (path.Count <= 0)
            {
                Debug.Log("count is 0");
                return -1;
            }

            Coord firstCube = path.ElementAt(1) - coord1;
            return CubeDirections.IndexOf(firstCube);
        }
        //public int GetDirection(float x, float y)
        //{
        //    float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 180f;
        //    angle = Mathf.Abs(angle);
        //    int direction = 4 - Mathf.RoundToInt(angle / 60f);
        //    direction = WrapDirection(direction);
        //    //Debug.Log(angle + " - " + direction);
        //    return direction;
        //}
        public static int GetDistance(Coord coord1, Coord coord2)
        {
            return (Mathf.Abs(coord1.x - coord2.x) + Mathf.Abs(coord1.y - coord2.y));
        }

        //public Cube GetMirrorVertical(int centerY = 0)
        //{
        //    //TODO: Make it work with a center other then 0
        //    int diff = (Y - centerY) * 2;
        //    return new Cube(X, Y - diff);
        //}
        //public Cube GetMirrorHorizontal()
        //{
        //    //TODO: Make it work with a center other then 0, 0
        //    return new Cube(X * -1, Y - X);
        //}

        private static Coord Lerp(Coord coord1, Coord coord2, float t)
        {
            float x = Mathf.Lerp(coord1.x, coord2.x, t);
            float y = Mathf.Lerp(coord1.y, coord2.y, t);

            return Round(x, y);
        }
        public static Coord Round(float x, float y)
        {
            return Round(x, y, -(x + y));
        }
        public static Coord Round(float x, float y, float z)
        {
            int rX = Mathf.RoundToInt(x - 0.1f);
            int rY = Mathf.RoundToInt(y);
            int rZ = Mathf.RoundToInt(z);

            float xDiff = Mathf.Abs(rX - x);
            float yDiff = Mathf.Abs(rY - y);
            float zDiff = Mathf.Abs(rZ - z);

            if (xDiff > yDiff && xDiff > zDiff)
            {
                rX = -rY - rZ;
            }
            else if (yDiff > zDiff)
            {
                rY = -rX - rZ;
            }
            return new Coord(rX, rY);
        }

        //public static Cube VectorToCube(Vector2 input)
        //{
        //    int x = 0;
        //    int y = 0;
        //    if (input.x > 0.33f)
        //    {
        //        x = 1;
        //    }
        //    else if (input.x < -0.33f)
        //    {
        //        x = -1;
        //    }

        //    if (input.y > 0.33f)
        //    {
        //        y = 1;
        //    }
        //    else if (input.y < -0.33f)
        //    {
        //        y = -1;
        //    }
        //    return new Cube(x, y);
        //}

        //public static Cube GetCubeFromDirection(int direction)
        //{
        //    direction = WrapDirection(direction);
        //    return CubeDirections[direction];
        //}
        public static int WrapDirection(int value)
        {
            return (value % 6 + 6) % 6;
        }

       
        //public struct Path
        //{
        //    public readonly Cube Cube;
        //    public readonly Cube CameFrom;

        //    public Path(Cube c, Cube cameFrom)
        //    {
        //        Cube = c;
        //        CameFrom = cameFrom;
        //    }
        //}
    }
}