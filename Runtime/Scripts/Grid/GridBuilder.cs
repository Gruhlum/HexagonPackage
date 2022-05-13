using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
	public class GridBuilder : MonoBehaviour
	{
        public HexGrid ActiveGrid;

        public int Rows;
        public int Columns;


        public void ClearAll(bool destroy)
        {
            ActiveGrid.RemoveAll(destroy);
        }
        public void BuildGrid()
        {
            ClearAll(true);
            SpawnHexGrid(Rows, Columns);
        }
        public void SpawnHexGrid(int Width, int Height)
        {
            if (Width > 50 || Height > 50)
            {
                return;
            }
            int yStart = -Height / 2;
            int yEnd = yStart + Height;

            for (int y = yStart; y < yEnd; y++)
            {
                int add = 0;
                if (y < 0)
                {
                    add = 1;
                }
                int xStart;
                int xEnd;

                // Is the Width an even number?
                if (Width % 2 == 0)
                {
                    if (y % 2 == 0)
                    {
                        xStart = (-Width / 2 - ((y - add) / 2)) + 1;
                    }
                    else xStart = -Width / 2 - ((y - add) / 2);
                    xEnd = xStart + Width + (y % 2 != 0 ? 0 : -1);
                }
                else
                {
                    xStart = -Width / 2 - ((y - add) / 2);
                    xEnd = xStart + Width + (y % 2 == 0 ? 0 : -1);
                }

                for (int x = xStart; x < xEnd; x++)
                {
                    ActiveGrid.CreateHexagon(new Cube(x, y));
                }
            }
        }
	}
}