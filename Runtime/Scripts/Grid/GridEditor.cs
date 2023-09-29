using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HexagonPackage.HexagonComponents;
using UnityEditor;
using HexTecGames.Basics;

namespace HexagonPackage
{
    [RequireComponent(typeof(SelectionController))]
    public class GridEditor : MonoBehaviour
    {
        public HexagonGrid ActiveGrid;
        public HexagonGrid EditGrid;

        public HexagonType SelectedType
        {
            get
            {
                return selectedType;
            }
            private set
            {
                selectedType = value;
            }
        }
        [SerializeField] private HexagonType selectedType = default;


        private bool enableHoverClick = false;
        int lastButton;

        [SerializeField] private SelectionController selectionController = default;

        public bool AllowRightClick
        {
            get
            {
                return allowRightClick;
            }
            set
            {
                allowRightClick = value;
            }
        }
        [SerializeField] private bool allowRightClick = true;

        public event Action<HexagonType> OnSelectedTypeChanged;

        public readonly Blocker allowInput = new Blocker();

        private void Reset()
        {
            selectionController = GetComponent<SelectionController>();
        }
        private void Awake()
        {
            if (selectionController == null)
            {
                Debug.LogWarning("No SelectionController assigned");
                return;
            }
            ActiveGrid.HexagonAdded += ActiveGrid_HexagonCreated;
            ActiveGrid.HexagonRemoved += ActiveGrid_HexagonRemoved;
            ActiveGrid.GridRemoved += ActiveGrid_GridRemoved;
            selectionController.OnHexagonClicked += Hexagon_Clicked;
            selectionController.OnMouseHoverChanged += EditGrid_MouseEnter;
        }

        private void ActiveGrid_GridRemoved()
        {
            CreateEditGrid();
        }

        private void Start()
        {
            CreateEditGrid();
        }
        private void Update()
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                enableHoverClick = false;
                lastButton = -1;
            }
        }
        private void ActiveGrid_HexagonRemoved(Hexagon hex)
        {
            List<Cube> neighbours = hex.Cube.GetNeighbours();

            foreach (var neighbour in neighbours)
            {
                if (EditGrid.Contains(neighbour))
                {

                    if (IsAdjacentToActiveHex(neighbour, hex))
                    {
                        EditGrid.RemoveHexagon(neighbour);
                    }
                }
            }
            foreach (var neighbour in neighbours)
            {
                if (ActiveGrid.Contains(neighbour))
                {
                    EditGrid.CreateHexagon(hex.Cube);
                    return;
                }
            }
        }

        private bool IsAdjacentToActiveHex(Cube neighbour, Hexagon origin)
        {
            foreach (var neighboursNeighbour in neighbour.GetNeighbours())
            {
                if (neighboursNeighbour != origin.Cube && ActiveGrid.Contains(neighboursNeighbour))
                {
                    return false;
                }
            }
            return true;
        }

        private void ActiveGrid_HexagonCreated(Hexagon hex)
        {
            EditGrid.RemoveHexagon(hex.Cube);
            List<Cube> neighbours = hex.Cube.GetNeighbours();
            foreach (var neighbour in neighbours)
            {
                // Is it already in the list or does a hex already exist at that position?
                if (!ActiveGrid.Contains(neighbour) && !EditGrid.Contains(neighbour))
                {
                    EditGrid.CreateHexagon(neighbour);
                }
            }
        }

        private void CreateEditGrid()
        {
            EditGrid.RemoveAll();
            foreach (var hex in ActiveGrid.Hexagons.Values)
            {
                List<Cube> neighbours = hex.Cube.GetNeighbours();
                foreach (var neighbour in neighbours)
                {
                    // Is it already in the list or does a hex already exist at that position?
                    if (!ActiveGrid.Contains(neighbour) && !EditGrid.Contains(neighbour))
                    {
                        EditGrid.CreateHexagon(neighbour);
                    }
                }
            }
        }
       
        public void SetSelectedType(HexagonType type)
        {
            SelectedType = type;
            OnSelectedTypeChanged?.Invoke(type);
        }

        private void BuildHexagon(Cube cube)
        {           
            Hexagon hex = ActiveGrid.CreateHexagon(cube);
            hex.HexType = SelectedType;
        }

        private void EditGrid_MouseEnter(Hexagon hex)
        {
            if (hex == null)
            {
                return;
            }
            if (hex.HexGrid == EditGrid)
            {
                if (enableHoverClick && lastButton == 0)
                {
                    BuildHexagon(hex.Cube);
                }
            }
            else if (hex.HexGrid == ActiveGrid)
            {
                if (enableHoverClick)
                {
                    if (lastButton == 1)
                    {
                        if (ActiveGrid.Hexagons.Count > 1)
                        {
                            ActiveGrid.RemoveHexagon(hex);
                        }
                    }
                    else if (lastButton == 0)
                    {
                        hex.HexType = SelectedType;
                    }
                }
            }       
        }

        private void Hexagon_Clicked(Hexagon hex, int btn)
        {
            if (!allowInput.Allowed)
            {
                return;
            }
            if (btn == 1 && allowRightClick == false)
            {
                return;
            }
            if (hex.HexGrid == EditGrid)
            {
                lastButton = btn;
                if (btn == 0)
                {
                    if (hex.HexObject == null)
                    {
                        BuildHexagon(hex.Cube);
                    }
                    
                }
                enableHoverClick = true;
            }
            else if (hex.HexGrid == ActiveGrid)
            {
                lastButton = btn;
                if (btn == 1)
                {
                    if (ActiveGrid.Hexagons.Count > 1 && hex.HexObject == null)
                    {
                        ActiveGrid.RemoveHexagon(hex);
                    }
                }
                else if (btn == 0)
                {
                    hex.HexType = SelectedType;
                }
                enableHoverClick = true;
            }
        }               
    }
}
