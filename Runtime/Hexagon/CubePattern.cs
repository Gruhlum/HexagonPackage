//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace HexTecGames.GridHexSystem
//{
//    public class CubePattern
//    {
//        public enum Direction { BtmRight, BtmLeft, Left, TopLeft, TopRight, Right }

//        public static List<List<Cube>> SortCubesByDirection(List<Cube> cubes, Direction direction)
//        {
//            List<List<Cube>> results = new List<List<Cube>>();

//            foreach (var cube in cubes)
//            {
//                switch (direction)
//                {
//                    case Direction.BtmRight:
//                        int minX = cubes.Min(c => c.X);
//                        results[cube.X - minX].Add(cube);
//                        break;
//                    case Direction.BtmLeft:
//                        int minY = cubes.Min(c => c.Y);
//                        results[cube.Y - minY].Add(cube);
//                        break;
//                    case Direction.Left:
//                        int minZ = cubes.Min(c => c.Z);
//                        results[cube.Z - minZ].Add(cube);
//                        break;
//                    case Direction.TopLeft:
//                        int maxX = cubes.Max(c => c.X);
//                        results[maxX - cube.X].Add(cube);
//                        break;
//                    case Direction.TopRight:
//                        int maxY = cubes.Max(c => c.Y);
//                        results[maxY - cube.Y].Add(cube);
//                        break;
//                    case Direction.Right:
//                        int maxZ = cubes.Max(c => c.Z);
//                        results[maxZ - cube.Z].Add(cube);
//                        break;
//                    default:
//                        break;
//                }
//            }
//            return results;
//        }

//        public static List<List<Cube>> FromCenterToOutside(List<Cube> cubes)
//        {
//            List<List<Cube>> results = new List<List<Cube>>();
//            Cube center = Cube.GetCenterCube(cubes);
//            int maxDistance = 0;
//            foreach (var cube in cubes)
//            {
//                int distance = center.GetDistance(cube);
//                if (distance > maxDistance)
//                {
//                    maxDistance = distance;
//                }
//            }
//            for (int i = 0; i < maxDistance; i++)
//            {
//                results[i] = center.GetRing(i);
//            }            
//            return results;
//        }
//    }
//}