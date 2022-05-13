using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HexagonPackage.HexagonComponents;
using UnityEditor;

namespace HexagonPackage
{
    public class GridEditor : MonoBehaviour
    {
        public HexGrid ActiveGrid;
        public HexGrid EditGrid;

        public HexagonType selectedType;

        private bool enableHoverClick = false;
        int lastButton;

        [SerializeField] private SelectionController hexagonSelector = default;

        public bool ShowCoordinates
        {
            get
            {
                return showCoordinates;
            }
            set
            {
                showCoordinates = value;
            }
        }
        [SerializeField] private bool showCoordinates = default;

        private void OnValidate()
        {
            if (showCoordinates)
            {
                
                foreach (var hex in ActiveGrid.GetComponentsInChildren<Hexagon>(false))
                {
                    hex.GetHexComponent<DisplayText>().SetText(hex.Cube.X + "," + hex.Cube.Y);
                }
            }
            else
            {
                foreach (var hex in ActiveGrid.GetComponentsInChildren<Hexagon>(false))
                {
                    hex.GetHexComponent<DisplayText>().SetText("");
                }
            }
        }
        private void Awake()
        {
            hexagonSelector.Hexagon_Clicked += Hexagon_Clicked;
            hexagonSelector.MouseHover_Changed += EditGrid_MouseEnter;
        }
        private void Update()
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                enableHoverClick = false;
                lastButton = -1;
            }
        }

        private void BuildHexagon(Cube cube)
        {
            Hexagon hex = ActiveGrid.CreateHexagon(cube);
            hex.Type = selectedType;
            
            DisplayText text = hex.GetHexComponent<DisplayText>();
            if (text != null)
            {
                if (ShowCoordinates)
                {
                    text.SetText(cube.X + ", " + cube.Y);
                }
                else text.SetText("");
            }
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
                        hex.Type = selectedType;
                    }
                }
            }       
        }

        private void Hexagon_Clicked(Hexagon hex, int btn)
        {
            if (hex.HexGrid == EditGrid)
            {
                lastButton = btn;
                if (btn == 0)
                {
                    BuildHexagon(hex.Cube);
                }
                enableHoverClick = true;
            }
            else if (hex.HexGrid == ActiveGrid)
            {
                Debug.Log("hi");
                lastButton = btn;
                if (btn == 1)
                {
                    Debug.Log("hi2");
                    if (ActiveGrid.Hexagons.Count > 1)
                    {
                        Debug.Log("hi3");

                        ActiveGrid.RemoveHexagon(hex);
                    }
                }
                else if (btn == 0)
                {
                    hex.Type = selectedType;
                }
                enableHoverClick = true;
            }
        }               
    }
}
