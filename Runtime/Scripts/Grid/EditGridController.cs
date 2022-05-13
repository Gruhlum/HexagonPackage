using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    public class EditGridController : MonoBehaviour
    {
        [SerializeField] private HexGrid activeGrid = null;
        [SerializeField] private HexGrid editGrid = null;

        [SerializeField] private HexagonType editType = default;

        //[SerializeField] private BuildController buildController = default;

        //private void RemoveEditNeighbours(Hexagon hex)
        //{
        //    List<Cube> neighbours = hex.Cube.GetNeighbours();
        //    List<Cube> editHexesToRemove = new List<Cube>();

        //    foreach (var neighbour in neighbours)
        //    {
        //        if (activeGrid.ActiveHexagons[neighbour].HexagonType == EditType)
        //        {
        //            if (activeGrid.GetNeighbours(neighbour).Any(x => x.HexagonType != EditType) == false)
        //            {
        //                editHexesToRemove.Add(neighbour);
        //            }
        //        }
        //    }
        //    for (int i = editHexesToRemove.Count - 1; i >= 0; i--)
        //    {
        //        activeGrid.RemoveHexagon(editHexesToRemove[i]);
        //    }
        //}

        private void Start()
        {
            CreateEditGrid();
            activeGrid.HexagonAdded += ActiveGrid_HexagonCreated;
            activeGrid.HexagonRemoved += ActiveGrid_HexagonRemoved;
        }

        //public void DeactivateGrid()
        //{
        //    editGrid.gameObject.SetActive(false);
        //}

        private void ActiveGrid_HexagonRemoved(Hexagon hex)
        {
            List<Cube> neighbours = hex.Cube.GetNeighbours();

            foreach (var neighbour in neighbours)
            {
                if (editGrid.Contains(neighbour))
                {

                    if (IsAdjacentToActiveHex(neighbour, hex))
                    {
                        editGrid.RemoveHexagon(neighbour);
                    }
                }
            }
            foreach (var neighbour in neighbours)
            {
                if (activeGrid.Contains(neighbour))
                {
                    editGrid.CreateHexagon(hex.Cube).Type = editType;
                    return;
                }
            }
        }

        private bool IsAdjacentToActiveHex(Cube neighbour, Hexagon origin)
        {
            foreach (var neighboursNeighbour in neighbour.GetNeighbours())
            {
                if (neighboursNeighbour != origin.Cube && activeGrid.Contains(neighboursNeighbour))
                {
                    return false;
                }
            }
            return true;
        }

        private void ActiveGrid_HexagonCreated(Hexagon hex)
        {
            editGrid.RemoveHexagon(hex.Cube);
            List<Cube> neighbours = hex.Cube.GetNeighbours();
            foreach (var neighbour in neighbours)
            {
                // Is it already in the list or does a hex already exist at that position?
                if (!activeGrid.Contains(neighbour) && !editGrid.Contains(neighbour))
                {
                    editGrid.CreateHexagon(neighbour).Type = editType;
                }
            }
        }

        private void CreateEditGrid()
        {
            editGrid.RemoveAll();
            foreach (var hex in activeGrid.Hexagons.Values)
            {
                List<Cube> neighbours = hex.Cube.GetNeighbours();
                foreach (var neighbour in neighbours)
                {
                    // Is it already in the list or does a hex already exist at that position?
                    if (!activeGrid.Contains(neighbour) && !editGrid.Contains(neighbour))
                    {
                        editGrid.CreateHexagon(neighbour).Type = editType;
                    }
                }
            }
        }
    }
}
