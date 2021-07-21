using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HexagonPackage
{
    [RequireComponent(typeof(Collider2D), typeof(Hexagon))]
    public class ClickEvents : HexComponent, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<Hexagon, int> HexagonClicked;
        public event Action<Hexagon> HexagonMouseEnter;
        public event Action<Hexagon> HexagonMouseExit;

        //[Tooltip("Should the collider extend if the hexagons are spaced out?")]
        //public bool ExtendCollider = true;

        //private void Reset()
        //{
        //    colliderT = GetComponent<Collider2D>().transform;
        //}

        private void Awake()
        {
            //if (colliderT == null)
        //    {
        //        colliderT = GetComponent<Collider2D>().transform;
        //    }
        //    if (colliderT == null)
        //    {
        //        Debug.LogError("Missing Collider for ClickEvents!");
        //    }
        }
        private void Start()
        {          
            //if (ExtendCollider)
            //{
            //    colliderT.localScale = new Vector3(
            //        (hexagon.Radius + hexagon.SpacingX / 2) / hexagon.Radius * 1.025f,
            //        (hexagon.Radius + hexagon.SpacingY / 2) / hexagon.Radius * 1.025f,
            //        0);
            //}
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                HexagonClicked?.Invoke(hexagon, 0);
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                HexagonClicked?.Invoke(hexagon, 1);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HexagonMouseEnter?.Invoke(hexagon);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HexagonMouseExit?.Invoke(hexagon);
        }
    }
}
