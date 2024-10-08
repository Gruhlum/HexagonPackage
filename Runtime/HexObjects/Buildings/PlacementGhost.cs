//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HexagonPackage.HexObjects.UI
//{
//	public class PlacementGhost : MonoBehaviour
//	{
//        public Sprite Sprite
//        {
//            get
//            {
//                return sr.sprite;
//            }
//            private set
//            {
//                sr.sprite = value;
//            }
//        }

//        [SerializeField] private SpriteRenderer sr = default;

//        public int Rotation
//        {
//            get
//            {
//                return rotation;
//            }
//            set
//            {
//                rotation = value;
//                transform.eulerAngles = new Vector3(0, 0, 60 * rotation);
//            }
//        }
//        [SerializeField] private int rotation = default;

//        public Hexagon Position
//        {
//            get
//            {
//                return position;
//            }
//            private set
//            {
//                position = value;
//            }
//        }
//        [SerializeField] private Hexagon position = default;


//        public void SetSprite(Sprite sprite)
//        {
//            Sprite = sprite;
//        }
//        public void SetSprite(Sprite sprite, Color color)
//        {
//            Sprite = sprite;
//            sr.color = color;
//        }

//        public void SetActive(bool active)
//        {
//            gameObject.SetActive(active);
//        }

//        private Vector2 GetMousePosition()
//        {
//            return Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
//        }
//        public void SetToMousePosition(Vector2 offset)
//        {
//            transform.position = GetMousePosition() + offset;
//        }
//        public void SetToMousePosition()
//        {
//            transform.position = GetMousePosition();
//        }
//        public void SetPosition(Hexagon hex)
//        {
//            Position = hex;
//            SetPosition(hex.transform.position);
//        }
//        public void SetPosition(Vector3 pos)
//        {
//            transform.position = pos;
//        }
//        public void Rotate(int rotation)
//        {
//            Rotation = rotation;
//        }
//        public void Rotate(bool clockwise)
//        {
//            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 60);
//            Rotation -= clockwise? 1 : -1;
//        }
//	}
//}