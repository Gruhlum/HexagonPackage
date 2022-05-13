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
        public int X;
        public int Y;
        public int Z;

        //public Cube cameFrom;
        // Starts btm-right, goes counter-clockwise
        public static readonly List<Cube> cubeDirections = new List<Cube>
            { new Cube(1, -1), new Cube(1, 0), new Cube(0, 1),
            new Cube(-1, 1), new Cube(-1, 0), new Cube(0, -1)};


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

        public Cube GetNeighbour(int direction)
        {
            return this + cubeDirections[direction];
        }
        public List<Cube> GetNeighbours()
        {
            List<Cube> Results = new List<Cube>();
            foreach (var direction in cubeDirections)
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

        public Path[][] GetPathingMap(List<Cube> positions, int range = 20)
        {
            Path[][] cubePaths = new Path[range][];
            cubePaths[0] = new Path[] { new Path(this, this) };
            List<Cube> visited = new List<Cube>() { this };
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
        public Cube[] GetPath(Cube target, Path[][] path)
        {
            if (target == null)
            {
                return null;
            }
            Cube[] cubePath;
            for (int i = 0; i < path.Length; i++)
            {
                if (Array.Exists(path[i], x => x.Cube == target))
                {
                    Path targetPath = Array.Find(path[i], x => x.Cube == target);
                    cubePath = new Cube[i + 1];
                    cubePath[i] = targetPath.Cube;
                    int distance = i - 1;
                    while (distance > 0)
                    {
                        targetPath = Array.Find(path[distance], x => x.Cube == targetPath.CameFrom);
                        distance--;
                        cubePath[distance] = targetPath.Cube;
                    }
                    return cubePath;
                }
            }
            return null;
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
            Cube cube = this + cubeDirections[2] * radius;

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
            //Debug.Log(target.ToString() + " - " + this.ToString() + " - " + c.ToString());
            if (cubeDirections.Any(x => x == c))
            {
                return cubeDirections.IndexOf(c);
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
            return cubeDirections.IndexOf(firstCube);
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
            return cubeDirections[direction];
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