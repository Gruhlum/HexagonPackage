using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using HexagonPackage.HexagonComponents;

namespace HexagonPackage
{
    [ExecuteInEditMode]
    public class Hexagon : MonoBehaviour
    {
        public static readonly float WIDTH_MULTIPLIER = 0.866025f; //Mathf.Sqrt(3) / 2;        

        public float Radius
        {
            get
            {
                return radius;
            }
        }
        [SerializeField] private float radius = 1.5f;
        public float HexHeight
        {
            get
            {
                return Radius * 2;
            }
        }
        public float HexWidth
        {
            get
            {
                return WIDTH_MULTIPLIER * HexHeight;
            }
        }
        public float HexVerticalSpacing
        {
            get
            {
                return HexHeight * 0.75f + hexGrid.SpacingX;
            }
        }
        public float HexHorizontalSpacing
        {
            get
            {
                return HexWidth + hexGrid.SpacingY;
            }
        }

        public HexagonType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                if (type != null)
                {
                    type.Apply(this);
                }
            }
        }
        [SerializeField] private HexagonType type = default;


        public SpriteRenderer SpriteRenderer
        {
            get
            {
                return spriteRenderer;
            }
            private set
            {
                spriteRenderer = value;
            }
        }
        [SerializeField] private SpriteRenderer spriteRenderer = default;


        public Cube Cube
        {
            get
            {
                return cube;
            }
            protected set
            {
                cube = value;
            }
        }
        [SerializeField] private Cube cube;

        public HexGrid HexGrid
        {
            get
            {
                return hexGrid;
            }
            private set
            {
                hexGrid = value;
            }
        }
        private HexGrid hexGrid;

        [SerializeField] private List<HexComponent> hexComponents = new List<HexComponent>();

        private void OnValidate()
        {
            List<HexComponent> components = new List<HexComponent>();
            foreach (var component in hexComponents)
            {
                if (component != null)
                {
                    components.Add(component);
                }
            }
            hexComponents.Clear();
            hexComponents.AddRange(components);
        }

        public void AddHexComponent(HexComponent component)
        {
            hexComponents.Add(component);
        }
        public void RemoveComponent(HexComponent component)
        {
            hexComponents.Remove(component);
        }
        public bool ContainsHexComponent<T>() where T : HexComponent
        {
            if (hexComponents.Any(x => x is T))
            {
                return true;
            }
            else return false;
        }
        public T GetHexComponent<T>() where T : HexComponent
        {
            return hexComponents.Find(x => x is T) as T;
        }

        public virtual void Setup(Cube cube, HexGrid grid)
        {
            HexGrid = grid;
            Cube = cube;
            transform.localPosition = ToWorldPosition(cube.X, cube.Y);
            name = "Hex (" + Cube.X + ", " + Cube.Y + ")";
            if (!grid.Hexagons.ContainsKey(Cube))
            {
                HexGrid.Hexagons.Add(Cube, this);
            }
            gameObject.SetActive(true);
        }

        public Vector2 ToWorldPosition(int x, int y)
        {
            return new Vector2(HexHorizontalSpacing * (x + y / 2f), HexVerticalSpacing * y);
        }

        public virtual void Highlight(bool value)
        {
            //if (value)
            //{
            //    GetComponent<SpriteRenderer>().color = HexagonType.Color + new Color(0.2f, 0.2f, 0.2f);
            //}
            //else
            //{
            //    GetComponent<SpriteRenderer>().color = HexagonType.Color;
            //}
        }
    }
}