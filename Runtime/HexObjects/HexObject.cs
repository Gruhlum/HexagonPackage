//using HexTecGames.Basics;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HexagonPackage.HexObjects
//{
//    [RequireComponent(typeof(SpriteRenderer))]
//    public class HexObject : MonoBehaviour
//    {
//        public Hexagon Hexagon
//        {
//            get
//            {
//                return hexagon;
//            }
//            protected set
//            {
//                hexagon = value;
//            }
//        }
//        private Hexagon hexagon = default;

//        public Sprite Sprite
//        {
//            get
//            {
//                return sr.sprite;
//            }
//        }

//        [SerializeField] protected SpriteRenderer sr = default;

//        public Cube Center
//        {
//            get
//            {
//                return center;
//            }
//            set
//            {
//                center = value;
//            }
//        }
//        private Cube center = default;


//        public HexObject Origin
//        {
//            get
//            {
//                return prefab;
//            }
//            set
//            {
//                prefab = value;
//            }
//        }
//        private HexObject prefab;

//        public int Rotation
//        {
//            get
//            {
//                return rotation;
//            }
//            protected set
//            {
//                rotation = value;
//            }
//        }
//        private int rotation;

//        protected HexagonGrid hexagonGrid;


//        protected virtual void Awake()
//        {
//            sr = GetComponent<SpriteRenderer>();
//        }
//        protected virtual void Reset()
//        {
//            sr = GetComponent<SpriteRenderer>();
//        }
//        protected virtual void SetPosition(Cube cube)
//        {
//            transform.position = hexagonGrid.CubeToWorldPoint(cube);
//        }
//        public virtual void Setup(Cube center, HexagonGrid grid, int rotation = 0, bool block = true)
//        {
//            hexagonGrid = grid;
//            SetPosition(center);
//            Rotate(rotation);
//            ClearHexagon();
//            grid.Hexagons.TryGetValue(center, out Hexagon hex);
//            if (hex != null)
//            {
//                if (block)
//                {
//                    hex.HexObject = this;
//                }
//            }
//            Hexagon = hex;
//            Center = center;
//        }

//        public virtual List<Cube> GetOccupyingCubes(Cube center, int rotation = 0)
//        {
//            return new List<Cube>() { center };
//        }
//        public virtual List<Cube> GetAdditonalCubes(Cube center, int rotation = 0)
//        {
//            return null;
//        }
//        public virtual void Rotate(int value)
//        {
//            transform.eulerAngles = new Vector3(0, 0, 60 * value);
//            Rotation = value;
//        }
//        public virtual bool IsValidPosition(Cube cube, HexagonGrid grid, int rotation = 0, bool checkForBuildings = true)
//        {
//            Hexagon hex = grid.GetHexagon(cube);
//            if (hex == null || (checkForBuildings && hex.HexObject != null))
//            {
//                return false;
//            }
//            return true;
//        }
//        public Color GetColor()
//        {
//            return sr.color;
//        }
//        public virtual void SetColor(Color col)
//        {
//            sr.color = col;
//        }
//        public virtual void SetSortingOrder(int value)
//        {
//            sr.sortingOrder = value;
//        }
//        public void MoveTo(Hexagon hex)
//        {
//            if (Hexagon != null && Hexagon.HexObject == this)
//            {
//                Hexagon.HexObject = null;
//            }
//            Hexagon = hex;
//            Hexagon.HexObject = this;
//            transform.position = Hexagon.transform.position;
//        }
//        public void MoveTo(Vector3 targetPos, float duration)
//        {
//            StartCoroutine(AnimateMoveTo(targetPos, duration));
//        }
//        public virtual void Destroy()
//        {
//            ClearHexagon();
//            Destroy(gameObject);
//        }
//        protected virtual void ClearHexagon()
//        {
//            if (Hexagon != null && Hexagon.HexObject == this)
//            {
//                Hexagon.HexObject = null;
//            }
//        }
//        public virtual BuildEventArgs GenerateBuildEventArgs(Cube center, int rotation = 0)
//        {
//            return new BuildEventArgs(new List<Cube>() { center }, this);
//        }

//        public virtual void OnBuildingSelected(bool selected) { }

//        public static void SwapObjects(HexObject unit1, HexObject unit2)
//        {
//            Hexagon hex1 = unit1.Hexagon;
//            unit1.MoveTo(unit2.Hexagon);
//            unit2.MoveTo(hex1);
//        }

//        private IEnumerator AnimateMoveTo(Vector2 targetPos, float duration)
//        {
//            Vector2 startPos = transform.position;
//            for (float i = 0; i < duration;)
//            {
//                float x = Mathf.SmoothStep(startPos.x, targetPos.x, i / duration);
//                float y = Mathf.SmoothStep(startPos.y, targetPos.y, i / duration);
//                transform.position = new Vector2(x, y);
//                yield return new WaitForEndOfFrame();
//                i += Time.deltaTime;
//            }
//        }
//    }
//}