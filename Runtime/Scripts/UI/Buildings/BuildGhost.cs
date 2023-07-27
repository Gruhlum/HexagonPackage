using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects.UI
{
	public class BuildGhost : MonoBehaviour
	{
        public Sprite Sprite
        {
            get
            {
                return sr.sprite;
            }
            private set
            {
                sr.sprite = value;
            }
        }

        [SerializeField] private SpriteRenderer sr = default;

        public int Rotation
        {
            get
            {
                return rotation;
            }
            private set
            {
                rotation = value;
                transform.eulerAngles = new Vector3(0, 0, 60 * rotation);
            }
        }
        [SerializeField] private int rotation = default;

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }
        public void SetSprite(Sprite sprite, Color color)
        {
            Sprite = sprite;
            sr.color = color;
        }

        private Vector2 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
        }
        public void SetToMousePosition(Vector2 offset)
        {
            transform.position = GetMousePosition() + offset;
        }
        public void SetToMousePosition()
        {
            transform.position = GetMousePosition();
        }

        public void UpdatePosition(Vector3 pos)
        {
            transform.position = pos;
        }
        public void Rotate(int rotation)
        {
            Rotation = rotation;
        }
        public void Rotate(bool clockwise)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 60);
            Rotation -= 1;
        }
	}
}