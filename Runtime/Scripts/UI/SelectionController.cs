using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace HexagonPackage
{
    public class SelectionController : MonoBehaviour
    {
        public List<HexGrid> ActiveGrids = new List<HexGrid>();

        [SerializeField] private Camera mainCam = default;

        [SerializeField] private Spawner<HexHighlighter> highlightSpawner = default;

        public event Action<Hexagon> MouseHover_Changed;
        public event Action<Hexagon, int> Hexagon_Clicked;

        public Vector2 MousePosition
        {
            get
            {
                return mousePos;
            }
            set
            {
                mousePos = value;
            }
        }
        private Vector2 mousePos;

        public Hexagon HoverHexagon
        {
            get
            {
                return hoverHexagon;
            }
            set
            {
                if (hoverHexagon == value)
                {
                    return;
                }
                hoverHexagon = value;
                highlightSpawner.DeactivateAll();
                if (hoverHexagon != null)
                {
                    highlightSpawner.Spawn().Setup(hoverHexagon);
                }               
                MouseHover_Changed?.Invoke(hoverHexagon);
            }
        }
        private Hexagon hoverHexagon = default;

        private void Reset()
        {
            mainCam = Camera.main;
        }


        private void Update()
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Hexagon hex = null;
            foreach (var grid in ActiveGrids)
            {
                hex = grid.WorldPointToHexagon(mousePos);
                HoverHexagon = hex;
                if (hex != null)
                {
                    break;
                }
            }

            if (HoverHexagon == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
                Hexagon_Clicked?.Invoke(HoverHexagon, 0);
            if (Input.GetMouseButtonDown(1))
                Hexagon_Clicked?.Invoke(HoverHexagon, 1);
            if (Input.GetMouseButtonDown(2))
                Hexagon_Clicked?.Invoke(HoverHexagon, 2);
        }
    }
}