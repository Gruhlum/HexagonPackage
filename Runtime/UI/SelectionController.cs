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

        public event Action<Hexagon> OnMouseHoverChanged;
        public event Action<Hexagon, int> OnHexagonClicked;

        public HexHighlightController HighlightSpawner
        {
            get
            {
                return this.highlightSpawner;
            }
            private set
            {
                this.highlightSpawner = value;
            }
        }
        [SerializeField] private HexHighlightController highlightSpawner;

        private HexHighlight lastHighlight;

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

        public bool showMouseHover = true;

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
                HandleHighlights();
                OnMouseHoverChanged?.Invoke(hoverHexagon);
            }
        }

       

        private Hexagon hoverHexagon = default;

        public bool DisableClickEvents;

        private Vector2 lastMousePos = default;

        private void Reset()
        {
            mainCam = Camera.main;
        }


        private void OnEnable()
        {
            lastMousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Update()
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, lastMousePos) < 0.01f)
            {
                HandleClickEvents();
                return;
            }
            lastMousePos = mousePos;
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

            HandleClickEvents();
        }

        private void HandleClickEvents()
        {
            if (HoverHexagon == null)
            {
                return;
            }

            if (DisableClickEvents)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
                OnHexagonClicked?.Invoke(HoverHexagon, 0);
            if (Input.GetMouseButtonDown(1))
                OnHexagonClicked?.Invoke(HoverHexagon, 1);
            if (Input.GetMouseButtonDown(2))
                OnHexagonClicked?.Invoke(HoverHexagon, 2);
        }

        private void HandleHighlights()
        {
            if (lastHighlight != null)
            {
                lastHighlight.Disable(highlightSpawner.FadeOut);
                lastHighlight = null;
            }

            if (hoverHexagon != null && showMouseHover)
            {
                lastHighlight = HighlightSpawner.Spawn(HoverHexagon);
            }
        }
    }
}