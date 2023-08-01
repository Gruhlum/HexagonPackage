using HexagonPackage;
using HexagonPackage.HexObjects;
using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage.HexObjects
{
    public class DynamicBuilding : MultiTileBuilding
    {
        [SerializeField] private Sprite[] sprites = new Sprite[13];
        [SerializeField] private Spawner<HexHighlighter> highlightSpawner = default;
        [SerializeField] private HexagonGrid hexGrid = default;
        [SerializeField] private HexagonData hexData = default;
        [SerializeField] private Spawner<DynamicTile> tileSpawner = default;

        private List<DynamicTile> tiles = new List<DynamicTile>();

        //[Flags]
        //public enum Options
        //{
        //    BtmRight = 1,
        //    TopRight = 2,
        //    TopLeft = 4,
        //    Left = 8,
        //    BtmLeft = 16,
        //    Right = 32
        //}
        //public Options op;

        [ContextMenu("Recalculate")]
        public void Recalculate()
        {
            SpawnTiles();

            //List<Cube> neighbours = new List<Cube>();
            //if (op.HasFlag(Options.BtmRight))
            //{
            //    neighbours.Add(Cube.cubeDirections[0]);
            //}
            //if (op.HasFlag(Options.Right))
            //{
            //    neighbours.Add(Cube.cubeDirections[1]);
            //}
            //if (op.HasFlag(Options.TopRight))
            //{
            //    neighbours.Add(Cube.cubeDirections[2]);
            //}
            //if (op.HasFlag(Options.TopLeft))
            //{
            //    neighbours.Add(Cube.cubeDirections[3]);
            //}
            //if (op.HasFlag(Options.Left))
            //{
            //    neighbours.Add(Cube.cubeDirections[4]);
            //}
            //if (op.HasFlag(Options.BtmLeft))
            //{
            //    neighbours.Add(Cube.cubeDirections[5]);
            //}

            //CalculateSprite(new Cube(0, 0), neighbours);
            //highlightSpawner.TryDestroyAll();
            //foreach (var neighbour in neighbours)
            //{
            //    highlightSpawner.Spawn().Setup(hexGrid.CubeToWorldPoint(neighbour), Color.cyan);
            //}
        }
        public override void Setup(Cube center, HexagonGrid grid, int rotation, bool block = true)
        {
            highlightSpawner.AddInstances(highlightSpawner.Parent.GetComponentsInChildren<HexHighlighter>().ToList());
            highlightSpawner.DeactivateAll();
            SpawnTiles();
            //Debug.Log("hi");
            base.Setup(center, grid, rotation, block);
        }
        public void Setup(Vector3 position, int rotation)
        {
            highlightSpawner.AddInstances(highlightSpawner.Parent.GetComponentsInChildren<HexHighlighter>().ToList());
            highlightSpawner.DeactivateAll();
            SpawnTiles();
            transform.position = position;          
            Rotate(rotation);
        }
        public void SetHighlightBorder(bool active)
        {
            if (active == false)
            {
                highlightSpawner.DeactivateAll();
                return;
            }
            if (!gameObject.activeSelf)
            {
                return;
            }
            foreach (var tile in GetTiles())
            {
                HexHighlighter highlight = highlightSpawner.Spawn();
                highlight.Setup(tile.transform.position, 0.1f);
                highlight.SetSortingOrder(sr.sortingOrder - 1);
            }
        }
        public List<DynamicTile> GetTiles()
        {
            List<DynamicTile> result = new List<DynamicTile>();
            result.AddRange(tiles);
            return result;
        }
        public override List<Cube> GetOccupyingCubes(Cube center, int rotation = 0)
        {
            List<Cube> results = new List<Cube>();
            for (int i = Size.Count - 1; i >= 0; i--)
            {
                Cube c = Size[i] + center;
                c.Rotate(center, rotation);
                results.Add(c);
                //Debug.Log(Size[i] + " - " + c);
            }
            return results;
        }
        public void SpawnTiles()
        {
            tileSpawner.AddInstances(GetComponentsInChildren<DynamicTile>().ToList());
            ClearAllTiles();
            
            for (int i = 0; i < Size.Count; i++)
            {
                DynamicInfo info = CalculateSprite(Size[i], GetNeighbours(Size[i]));;
                info.Position = Size[i].ToWorldPosition(hexData.VerticalSpacing, hexData.HorizontalSpacing, hexData.Flat);
                info.SortingOrder = sr.sortingOrder;
                DynamicTile tile = tileSpawner.Spawn();
                tile.Setup(info);
                tile.SetColor(sr.color);
                tiles.Add(tile);
            }
        }
        public override void Destroy()
        {
            ClearAllTiles();
            base.Destroy();
        }
        public void ClearAllTiles()
        {
            tileSpawner.DeactivateAll();
            tiles = new List<DynamicTile>();
        }
        public override void SetSortingOrder(int value)
        {
            foreach (var highlight in highlightSpawner.GetBehaviours())
            {
                highlight.SetSortingOrder(value - 1);
            }
            foreach (var tile in tiles)
            {
                tile.SetSortingOrder(value);
            }
            base.SetSortingOrder(value);
        }
        public List<Cube> GetNeighbours(Cube cube)
        {
            List<Cube> results = cube.GetNeighbours();
            for (int i = results.Count - 1; i >= 0; i--)
            {
                if (!Size.Contains(results[i]))
                {
                    results.RemoveAt(i);
                }
            }
            return results;
        }
        public DynamicInfo CalculateSprite(Cube cube, List<Cube> neighbours)
        {
            DynamicInfo info = new DynamicInfo();

            if (neighbours.Count == 0)
            {
                info.Sprite = sprites[0];
            }
            if (neighbours.Count == 1)
            {
                info.Sprite = sprites[1];
                int rotation = cube.GetNeighbourDirection(neighbours[0]);
                info.Rotation = rotation - 1;
            }
            if (neighbours.Count == 2)
            {
                int dir1 = cube.GetNeighbourDirection(neighbours[0]);
                int dir2 = cube.GetNeighbourDirection(neighbours[1]);
                int distance = CalculateNeighbourDistance(dir1, dir2);
                switch (Mathf.Abs(distance))
                {
                    case 1:
                        info.Sprite = sprites[2];
                        if (distance == -1)
                        {
                            info.Rotation = dir1 - 2;
                        }
                        else info.Rotation = dir1 - 1;

                        break;
                    case 2:
                        info.Sprite = sprites[3];
                        if (distance > 0)
                        {
                            info.Rotation = dir1 - 1;
                        }
                        else info.Rotation = dir1 + 3;
                        break;
                    case 3:
                        info.Sprite = sprites[4];
                        info.Rotation = (dir1 - 1);
                        break;
                    default:
                        break;
                }

            }
            if (neighbours.Count == 3)
            {
                int dir1 = cube.GetNeighbourDirection(neighbours[0]);
                int dir2 = cube.GetNeighbourDirection(neighbours[1]);
                int dir3 = cube.GetNeighbourDirection(neighbours[2]);
                int dist1 = CalculateNeighbourDistance(dir1, dir2);
                int dist2 = CalculateNeighbourDistance(dir2, dir3);

                if ((dist1 == 1 || dist1 == -2) && (dist2 == 1 || dist2 == -2))
                {
                    info.Sprite = sprites[5];
                    if (dist1 == -2)
                    {
                        info.Rotation = (dir1 + 3);
                    }
                    else if (dist2 == -2)
                    {
                        info.Rotation = (dir1 - 2);
                    }
                    else
                    {
                        info.Rotation = (dir1 - 1);
                    }
                }
                else if (dist1 == 2 && dist2 == 2)
                {
                    info.Sprite = sprites[7];
                    info.Rotation = (dir1 + 1);
                }
                else
                {
                    //0, 1, 3 F
                    //0, 1, 4
                    //1, 2, 4 F
                    //1, 2, 5
                    //2, 3, 5 F
                    //2, 3, 0
                    //3, 4, 0 F
                    //3, 4, 1
                    //4, 5, 1 F
                    //4, 5, 2
                    //0, 5, 2 F
                    //0, 5, 3

                    info.Sprite = sprites[6];
                    //Debug.Log(dir1 + " - " + dir2 + " - " + dir3);
                    if (dir1 == 0 && dir2 == 1 && dir3 == 3)
                    {
                        //info.Flip = true;
                        info.Rotation = (dir1 - 1);
                    }
                    else if (dir1 == 0 && dir2 == 1 && dir3 == 4)
                    {
                        info.Rotation = (dir1);
                        info.Flip = true;
                    }
                    else if (dir1 == 1 && dir2 == 2 && dir3 == 4)
                    {
                        info.Rotation = (dir1 - 1);
                        //info.Flip = true;
                    }
                    else if (dir1 == 1 && dir2 == 2 && dir3 == 5)
                    {
                        info.Rotation = (dir1);
                        info.Flip = true;
                    }
                    else if (dir1 == 2 && dir2 == 3 && dir3 == 5)
                    {
                        info.Rotation = (dir2 - 2);
                        //info.Flip = true;
                    }
                    else if (dir1 == 0 && dir2 == 2 && dir3 == 3)
                    {
                        info.Rotation = (dir2);
                        info.Flip = true;
                    }
                    else if (dir1 == 0 && dir2 == 3 && dir3 == 4)
                    {
                        info.Rotation = (dir2 - 1);
                        //info.Flip = true;
                    }
                    else if (dir1 == 1 && dir2 == 3 && dir3 == 4)
                    {
                        info.Rotation = (dir2);
                        info.Flip = true;
                    }
                    else if (dir1 == 1 && dir2 == 4 && dir3 == 5)
                    {
                        info.Rotation = (dir2 - 1);
                        //info.Flip = true;
                    }
                    else if (dir1 == 2 && dir2 == 4 && dir3 == 5)
                    {
                        info.Rotation = (dir2);
                        info.Flip = true;
                    }
                    else if (dir1 == 0 && dir2 == 2 && dir3 == 5)
                    {
                        info.Rotation = (dir1 - 2);
                        //info.Flip = true;
                    }
                    else if (dir1 == 0 && dir2 == 3 && dir3 == 5)
                    {
                        info.Rotation = (dir1 - 1);
                        info.Flip = true;
                    }
                }
            }
            if (neighbours.Count == 4)
            {
                int gap1 = 0;
                int gap2 = 0;
                bool foundFirst = false;
                for (int i = 0; i < 6; i++)
                {
                    Cube neighbour = cube.GetNeighbour(i);
                    if (!neighbours.Contains(neighbour))
                    {
                        if (foundFirst == false)
                        {
                            gap1 = i;
                            foundFirst = true;
                        }
                        else
                        {
                            gap2 = i;
                            break;
                        }
                    }
                }
                int gapDist = CalculateNeighbourDistance(gap1, gap2);
                //Debug.Log(gapDist);
                if (gapDist == 1 || gapDist == -1)
                {
                    info.Sprite = sprites[8];
                    if (gapDist == -1)
                    {
                        info.Rotation = (gap1);
                    }
                    else info.Rotation = (gap1 + 1);
                }
                if (gapDist == 3)
                {
                    info.Sprite = sprites[9];
                    info.Rotation = (gap1);
                }
                if (gapDist == 2 || gapDist == -2)
                {
                    info.Sprite = sprites[10];
                    if (gapDist == 2)
                    {
                        info.Rotation = (gap1);
                    }
                    else
                    {
                        info.Rotation = (gap1 - 2);
                    }
                }
            }
            if (neighbours.Count == 5)
            {
                info.Sprite = sprites[11];
                for (int i = 0; i < 6; i++)
                {
                    if (!neighbours.Contains(cube.GetNeighbour(i)))
                    {
                        info.Rotation = i;
                        break;
                    }
                }
            }
            if (neighbours.Count == 6)
            {
                info.Sprite = sprites[12];
            }
            return info;
        }
        private int CalculateNeighbourDistance(int dir1, int dir2)
        {
            int result = Mathf.Max(dir1, dir2) - Mathf.Min(dir1, dir2);
            //Debug.Log(dir1 + " - " + dir2);
            if (result > 3)
            {
                return result - 6;

                //1 = 1;
                //2 = 2;
                //3 = 3;
                //4 = -2;
                //5 = -1;
            }
            return result;
        }
        public override void SetColor(Color col)
        {
            foreach (var tile in tiles)
            {
                tile.SetColor(col);
            }
            base.SetColor(col);
        }
        public void ActivateParticleSystem(bool active)
        {
            foreach (var tile in tiles)
            {
                tile.ActivateParticleSystem(active);
            }
        }
        public void BurstParticleSystem(int particles)
        {
            foreach (var tile in tiles)
            {
                tile.BurstParticleSystem(particles);
            }
        }
        public struct DynamicInfo
        {
            public Sprite Sprite;
            public Vector3 Position;
            public int Rotation;
            public bool Flip;
            public int SortingOrder;
        }
    }
}