using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;

namespace HexagonPackage
{
    public class SelectionController : MonoBehaviour
    {
        public List<HexagonGrid> ActiveGrids = new List<HexagonGrid>();

        [SerializeField] private Camera mainCam = default;

        public event Action<Hexagon> MouseHover_Changed;
        public event Action<Hexagon, int> Hexagon_Clicked;
        [SerializeField] private Spawner<HexHighlighter> highlightSpawner;
        private HexHighlighter lastHighlight;
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
                if (lastHighlight != null)
                {
                    lastHighlight.Disable(0.1f);
                    lastHighlight = null;
                }
                
                if (hoverHexagon != null)
                {
                    lastHighlight = highlightSpawner.Spawn();
                    lastHighlight.SetPosition(HoverHexagon);
                }               
                MouseHover_Changed?.Invoke(hoverHexagon);
            }
        }
        private Hexagon hoverHexagon = default;

        public bool Lock;

        private void Reset()
        {
            mainCam = Camera.main;
        }


        private void Update()
        {
            if (Lock)
            {
                return;
            }
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Hexagon hex = null;
            foreach (var grid in ActiveGrids)
            {
                hex = grid.WorldPointToHexagon(mousePos);                
                if (hex != null)
                {
                    break;
                }
            }
            HoverHexagon = hex;

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