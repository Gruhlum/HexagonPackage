using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
    [System.Serializable]
    public struct Cube
    {
        private static readonly float SQRT_3 = 1.73205f;

        public int X;
        public int Y;
        public int Z;

        //public Cube cameFrom;
        // Starts btm-right, goes counter-clockwise
        public static readonly List<Cube> CubeDirections = new List<Cube>
            { new Cube(1, -1), new Cube(1, 0), new Cube(0, 1),
            new Cube(-1, 1), new Cube(-1, 0), new Cube(0, -1)};


        public static Cube Zero
        {
            get
            {
                return new Cube(0, 0);
            }
        }

        public Cube(Cube cube)
        {
            X = cube.X;
            Y = cube.Y;
            Z = cube.Z;
        }

        public Cube(int x, int y)
        {
            X = x;
            Y = y;
            Z = -(x + y);
        }
        public Cube(float x, float y)
        {
            this = Round(x, y, -(x + y));
        }

        public void SetValues(Cube cube)
        {
            X = cube.X;
            Y = cube.Y;
            Z = cube.Z;
        }
        public void SetValues(int x, int y)
        {
            X = x;
            Y = y;
            Z = -(x + y);
        }
        public Vector2 ToWorldPosition(float verticalSpacing, float horizontalSpacing, bool flat)
        {
            if (flat)
            {
                return new Vector2(verticalSpacing * X, horizontalSpacing * (Y + X / 2f));
            }
            else return new Vector2(horizontalSpacing * (X + Y / 2f), verticalSpacing * Y);
        }
        public static Cube WorldPositionToCube(float x, float y, float radius, bool flat)
        {
            if (flat)
            {
                return new Cube();
            }
            else
            {
                return new Cube((SQRT_3 / 3 * x - 1f / 3f * y) / radius, (2f / 3f * y) / radius);
            }
        }
        public Cube GetNeighbour(int direction)
        {
            return this + CubeDirections[WrapDirection(direction)];
        }
        public List<Cube> GetNeighbours()
        {
            List<Cube> Results = new List<Cube>();
            foreach (var direction in CubeDirections)
            {
                Cube cube = this + direction;
                Results.Add(cube);
            }
            return Results;
        }
        public bool IsNeighbour(Cube cube)
        {
            if (GetNeighbours().Any(x => x == cube))
            {
                return true;
            }
            return false;
        }
        public static Cube GetCenterCube(List<Cube> cubes)
        {
            float x = cubes.Select(c => c.X).Sum() / (float)cubes.Count;
            float y = cubes.Select(c => c.Y).Sum() / (float)cubes.Count;
            float z = cubes.Select(c => c.Z).Sum() / (float)cubes.Count;
            return Round(x, y, z);
        }
        public void Rotate(Cube center, int direction)
        {
            direction = WrapDirection(direction);
            this -= center;
            for (int i = 0; i < direction; i++)
            {
                SetValues(-Y, -Z);
            }
            this += center;
        }
        public static Cube[] GetShortestPath(Cube start, Cube end, List<Cube> positions, int range = 50)
        {
            Path[][] cubePaths = new Path[range][];
            cubePaths[0] = new Path[] { new Path(start, start) };
            List<Cube> visited = new List<Cube>() { start };
            for (int i = 1; i < range; i++)
            {
                List<Path> allowedNeighbours = new List<Path>();
                foreach (var preCube in cubePaths[i - 1])
                {
                    List<Cube> neighbours = preCube.Cube.GetNeighbours();
                    foreach (var neighbour in neighbours)
                    {
                        if (!visited.Contains(neighbour) && positions.Contains(neighbour))
                        {
                            allowedNeighbours.Add(new Path(neighbour, preCube.Cube));
                            if (neighbour == end)
                            {
                                return GetPath(start, cubePaths);
                            }
                            visited.Add(neighbour);
                        }
                    }
                }
                cubePaths[i] = allowedNeighbours.ToArray();
            }
            return null;

            //List<Cube> path = new List<Cube>();
            //List<Cube> line = start.GetLine(end);
            //int direction = start.GetDirection(end);
            //List<Cube> checkedCubes = new List<Cube>();
            //Cube lastCube = start;
            //int count = 0;

            //for (int i = 0; i < 5; i++)
            //{
            //    int[] directions = new int[5] { 0, 1, -1, 2, -2 };
            //    Cube neigh = lastCube.GetNeighbour(direction + directions[i]);
            //    if (positions.Contains(neigh))
            //    {
            //        path.Add(neigh);
            //        lastCube = neigh;
            //        break;
            //    }
            //}

            //while (lastCube != end || count > 100)
            //{
            //    count++;

            //    List<Cube> neighbours = lastCube.GetNeighbours();
            //    List<Cube> freeNeighbours = new List<Cube>();
            //    foreach (var neighbour in neighbours)
            //    {
            //        if (checkedCubes.Contains(neighbour) || positions.Contains(neighbour))
            //        {
            //            freeNeighbours.Add(neighbour);
            //        }
            //    }

            //    checkedCubes.AddRange(neighbours);

            //    foreach (var neighbour in freeNeighbours)
            //    {
            //        if (line.Contains(neighbour))
            //        {
            //            path.Add(neighbour);
            //            lastCube = neighbour;
            //            break;
            //        }
            //    }
            //}
            //return path;
        }
        public static Cube[] GetPath(Cube start, Cube end, List<Cube> positions, int range = 20)
        {
            return GetPath(end, GetPathingMap(start, positions, range));
        }
        public static Cube[] GetPath(Cube start, Path[][] path)
        {
            if (start == null)
            {
                return null;
            }
            Cube[] cubePath;
            for (int i = 0; i < path.Length; i++)
            {
                if (Array.Exists(path[i], x => x.Cube == start))
                {
                    Path targetPath = Array.Find(path[i], x => x.Cube == start);
                    cubePath = new Cube[i + 1];
                    cubePath[i] = targetPath.Cube;
                    //Debug.Log(i);
                    int distance = i - 1;
                    while (distance > 0)
                    {
                        // To alternate between left and right
                        //Debug.Log("hi");
                        //Array.Reverse(path[distance]);
                        //if (distance % 2 != 0)
                        //{
                        //    Debug.Log("hi");
                        //    Array.Reverse(path[distance]);
                        //}
                        targetPath = Array.Find(path[distance], x => x.Cube == targetPath.CameFrom);
                        cubePath[distance] = targetPath.Cube;
                        distance--;
                    }

                    cubePath[0] = path[0][0].Cube;

                    return cubePath;
                }
            }
            return null;
        }
        public static bool IsPathAvailable(Cube start, Cube end, List<Cube> positions)
        {
            Path[][] cubePaths = new Path[100][];
            cubePaths[0] = new Path[] { new Path(start, start) };
            List<Cube> visited = new List<Cube>() { start };
            for (int i = 1; i < 100; i++)
            {
                List<Path> allowedNeighbours = new List<Path>();
                foreach (var preCube in cubePaths[i - 1])
                {
                    List<Cube> neighbours = preCube.Cube.GetNeighbours();
                    foreach (var neighbour in neighbours)
                    {
                        if (!visited.Contains(neighbour) && positions.Contains(neighbour))
                        {
                            if (neighbour == end)
                            {
                                return true;
                            }
                            allowedNeighbours.Add(new Path(neighbour, preCube.Cube));
                            visited.Add(neighbour);
                        }
                    }
                }
                cubePaths[i] = allowedNeighbours.ToArray();
            }
            return false;
        }
        public static Cube? GetNextTile(Cube current, Path[][] PathingMap)
        {
            if (current == null)
            {
                return null;
            }
            for (int i = 0; i < PathingMap.Length; i++)
            {
                if (!Array.Exists(PathingMap[i], x => x.Cube == current))
                {
                    continue;
                }
                if (i == 0)
                {
                    return null;
                }

                Path path = Array.Find(PathingMap[i], x => x.Cube == current);
                return path.CameFrom;
            }
            return null;
        }
        public static Path[][] GetPathingMap(Cube start, List<Cube> positions, int range = 20)
        {
            Path[][] cubePaths = new Path[range][];
            cubePaths[0] = new Path[] { new Path(start, start) };
            List<Cube> visited = new List<Cube>() { start };
            for (int i = 1; i < range; i++)
            {
                List<Path> allowedNeighbours = new List<Path>();
                foreach (var preCube in cubePaths[i - 1])
                {
                    List<Cube> neighbours = preCube.Cube.GetNeighbours();
                    foreach (var neighbour in neighbours)
                    {
                        if (!visited.Contains(neighbour) && positions.Contains(neighbour))
                        {
                            allowedNeighbours.Add(new Path(neighbour, preCube.Cube));
                            visited.Add(neighbour);
                        }
                    }
                }
                cubePaths[i] = allowedNeighbours.ToArray();
            }
            return cubePaths;
        }

        public static Cube GetClosestCube(Cube center, List<Cube> cubes, int maxDistance = 20)
        {
            if (cubes.Count <= 1)
            {
                return center;
            }
            for (int i = 0; i < maxDistance; i++)
            {
                List<Cube> results = center.GetRing(i);
                if (results.Count != 0)
                {
                    return results[0];
                }
            }
            return center;
        }

        public List<Cube> GetLine(Cube cube)
        {
            List<Cube> pathCubes = new List<Cube>();

            if (this == cube)
            {
                return pathCubes;
            }

            int distance = GetDistance(cube);

            for (int i = 0; i <= distance; i++)
            {
                float t = ((float)i / (float)distance);
                pathCubes.Add(Lerp(this, cube, t));
            }
            return pathCubes;
        }

        public List<Cube> GetRing(int radius)
        {
            List<Cube> results = new List<Cube>();
            Cube cube = this + CubeDirections[4] * radius;

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    cube = cube.GetNeighbour(i);
                    results.Add(cube);
                }
            }
            return results;
        }

        public int GetNeighbourDirection(Cube target)
        {
            Cube c = target - this;
            //Debug.Log(c + " - " + target);
            //Debug.Log(target.ToString() + " - " + this.ToString() + " - " + c.ToString());
            if (CubeDirections.Any(x => x == c))
            {
                return CubeDirections.IndexOf(c);
            }
            else return -1;
        }

        public int GetDirection(Cube target)
        {
            if (target == null || this == target)
            {
                Debug.Log("target and start are the same!");
                return -1;
            }

            List<Cube> path = GetLine(target);
            if (path.Count <= 0)
            {
                Debug.Log("count is 0");
                return -1;
            }

            Cube firstCube = path.ElementAt(1) - this;
            return CubeDirections.IndexOf(firstCube);
        }
        public int GetDirection(float x, float y)
        {
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg - 180f;
            angle = Mathf.Abs(angle);
            int direction = 4 - Mathf.RoundToInt(angle / 60f);
            direction = WrapDirection(direction);
            //Debug.Log(angle + " - " + direction);
            return direction;
        }
        public int GetDistance(Cube cube)
        {
            return (Mathf.Abs(X - cube.X) + Mathf.Abs(Y - cube.Y) + Mathf.Abs(Z - cube.Z)) / 2;
        }

        public Cube GetMirrorVertical(int centerY = 0)
        {
            //TODO: Make it work with a center other then 0
            int diff = (Y - centerY) * 2;
            return new Cube(X, Y - diff);
        }
        public Cube GetMirrorHorizontal()
        {
            //TODO: Make it work with a center other then 0, 0
            return new Cube(X * -1, Y - X);
        }

        private static Cube Lerp(Cube c1, Cube c2, float t)
        {
            float x = Mathf.Lerp(c1.X, c2.X, t);
            float y = Mathf.Lerp(c1.Y, c2.Y, t);
            float z = Mathf.Lerp(c1.Z, c2.Z, t);

            return Round(x, y, z);
        }
        private static Cube Round(float x, float y, float z)
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
            else rZ = -rX - rY;
            return new Cube(rX, rY);
        }

        public static Cube GetCubeFromDirection(int direction)
        {
            if (direction < 0)
            {
                direction += 6;
            }
            if (direction > 5)
            {
                direction -= 6;
            }
            return CubeDirections[direction];
        }
        public static int WrapDirection(int value)
        {
            int result = value % 6;
            if (result < 0)
            {
                result += 6;
            }
            return result;
        }

        private bool Compare(Cube cube)
        {
            if (X == cube.X && Y == cube.Y)
            {
                return true;
            }
            else return false;
        }

        public static Cube operator +(Cube c1, Cube c2)
        {
            return new Cube(c1.X + c2.X, c1.Y + c2.Y);
        }
        public static Cube operator -(Cube c1, Cube c2)
        {
            return new Cube(c1.X - c2.X, c1.Y - c2.Y);
        }
        public static Cube operator *(Cube c1, int value)
        {
            return new Cube(c1.X * value, c1.Y * value);
        }

        public static bool operator ==(Cube cube1, Cube cube2)
        {
            return cube1.Compare(cube2);
        }

        public static bool operator !=(Cube cube1, Cube cube2)
        {
            return !(cube1 == cube2);
        }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public override bool Equals(object obj)
        {
            if (obj is Cube cube)
            {
                if (this.X == cube.X && this.Y == cube.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (this == null) return 0;
            return X.GetHashCode() + Y.GetHashCode();
        }
        public struct Path
        {
            public readonly Cube Cube;
            public readonly Cube CameFrom;

            public Path(Cube c, Cube cameFrom)
            {
                Cube = c;
                CameFrom = cameFrom;
            }
        }
    }
}