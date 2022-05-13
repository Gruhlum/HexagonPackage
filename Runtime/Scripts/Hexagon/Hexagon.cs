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
using Exile;

namespace HexagonPackage
{
    [ExecuteInEditMode]
    public class Hexagon : MonoBehaviour
    {
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
        public HexObject HexObject
        {
            get
            {
                return hexObject;
            }
            set
            {
                hexObject = value;
            }
        }
        [SerializeField] private HexObject hexObject = default;


        [SerializeField] private List<HexComponent> hexComponents = new List<HexComponent>();

        public HexagonData Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        [SerializeField] private HexagonData data = default;

        public HexGrid HexGrid
        {
            get
            {
                return hexGrid;
            }
            set
            {
                hexGrid = value;
            }
        }
        private HexGrid hexGrid;

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
            if (Type != null)
            {
                Type.Apply(this);
            }
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

        public virtual void Setup(Cube cube, HexagonData data, HexGrid grid)
        {
            this.Data = data;
            HexGrid = grid;
            Cube = cube;
            transform.localPosition = Data.ToWorldPosition(cube.X, cube.Y);
            name = "Hex (" + Cube.X + ", " + Cube.Y + ")";
            if (!grid.Hexagons.ContainsKey(Cube))
            {
                HexGrid.Hexagons.Add(Cube, this);
            }
            gameObject.SetActive(true);
        }
    }
}