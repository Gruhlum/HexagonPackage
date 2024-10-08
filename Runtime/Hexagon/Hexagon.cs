﻿//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using HexagonPackage.HexObjects;
//using System.Xml.Linq;

//namespace HexagonPackage
//{
//    [ExecuteInEditMode]
//    public class Hexagon : MonoBehaviour
//    {
//        public HexagonType HexType
//        {
//            get
//            {
//                return type;
//            }
//            set
//            {
//                type = value;
//                if (type != null)
//                {
//                    type.Apply(this);
//                }
//                else SpriteRenderer.color = startColor;
//            }
//        }
//        [SerializeField] private HexagonType type = default;
//        public SpriteRenderer SpriteRenderer
//        {
//            get
//            {
//                return spriteRenderer;
//            }
//            private set
//            {
//                spriteRenderer = value;
//            }
//        }
//        [SerializeField] private SpriteRenderer spriteRenderer = default;
//        public Cube Cube
//        {
//            get
//            {
//                return cube;
//            }
//            protected set
//            {
//                cube = value;
//            }
//        }
//        [HideInInspector][SerializeField] private Cube cube;
//        public HexObject HexObject
//        {
//            get
//            {
//                return hexObject;
//            }
//            set
//            {
//                hexObject = value;
//            }
//        }
//        private HexObject hexObject = default;

//        private List<MonoBehaviour> otherObjects = new List<MonoBehaviour>();

//        public HexagonData Data
//        {
//            get
//            {
//                return data;
//            }
//            set
//            {
//                data = value;
//            }
//        }
//        private HexagonData data = default;

//        public HexagonGrid HexGrid
//        {
//            get
//            {
//                return hexGrid;
//            }
//            set
//            {
//                hexGrid = value;
//            }
//        }
//        [SerializeField] private HexagonGrid hexGrid;

//        public string Text
//        {
//            get
//            {
//                if (HexText == null)
//                {
//                    return null;
//                }
//                return HexText.Text;
//            }
//        }
//        public HexagonText HexText
//        {
//            get
//            {
//                return this.hexText;
//            }
//            private set
//            {
//                this.hexText = value;
//            }
//        }
//        private HexagonText hexText;

//        private Color startColor;

//        public bool IsBlocked
//        {
//            get
//            {
//                return (HexObject != null);
//            }
//        }

//        public static readonly Vector3 RotationVector = new Vector3(0, 0, 60);

//        private void Reset()
//        {
//            SpriteRenderer = GetComponent<SpriteRenderer>();
//        }

//        private void Awake()
//        {
//            startColor = spriteRenderer.color;
//        }

//        private void OnValidate()
//        {
//            if (HexType != null)
//            {
//                HexType.Apply(this);
//            }
//        }
//        private void OnDisable()
//        {
//            if (HexText != null)
//            {
//                HexText.Disable();
//                HexText = null;
//            }
//            HexObject = null;
//        }

//        public void AddOtherObject(MonoBehaviour m)
//        {
//            otherObjects.Add(m);
//            //Debug.Log("Add " + otherObjects.Count);
//        }
//        public void RemoveOtherObject(MonoBehaviour m)
//        {
//            otherObjects.Remove(m);
//            //Debug.Log("Remove " + otherObjects.Count);
//        }
//        public List<T> GetOtherObjects<T>() where T : MonoBehaviour
//        {
//            List<T> results = new List<T>();
//            foreach (var m in otherObjects)
//            {
//                if (m is T t)
//                {
//                    results.Add(t);
//                }
//            }
//            //Debug.Log("R: " + results.Count + " . " + otherObjects.Count);
//            return results;
//        }
//        public void DisableText()
//        {
//            if (HexText != null)
//            {
//                HexText.Disable();
//            }
//            HexText = null;
//        }
//        public void SetText(string text)
//        {
//            if (HexGrid == null)
//            {
//                return;
//            }
//            if (string.IsNullOrEmpty(text))
//            {
//                DisableText();
//                return;
//            }
//            if (!Application.isPlaying && HexText != null)
//            {
//                DestroyImmediate(HexText.gameObject);
//            }
//            if (HexText == null)
//            {
//                HexText = HexGrid.GenerateHexText();
//            }            
//            HexText.Setup(transform, text);
//        }
//        public void SetColor(Color col)
//        {
//            SpriteRenderer.color = col;
//        }
//        public virtual void Setup(Cube cube, HexagonData data, HexagonGrid grid, HexagonType hexType = null)
//        {           
//            this.Data = data;
//            HexGrid = grid;
//            Cube = cube;
//            transform.localPosition = cube.ToWorldPosition(Data.VerticalSpacing, Data.HorizontalSpacing, Data.Flat);
//            name = "Hex (" + Cube.X + ", " + Cube.Y + ")";
//            HexType = hexType;
//            if (!grid.Hexagons.ContainsKey(Cube))
//            {
//                HexGrid.Hexagons.Add(Cube, this);
//            }
//            gameObject.SetActive(true);
//        }
//    }
//}