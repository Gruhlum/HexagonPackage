using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exile
{
	public class BuildGhost : MonoBehaviour
	{
        public Sprite Sprite
        {
            get
            {
                return sr.sprite;
            }
            set
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
            set
            {
                rotation = value;
            }
        }
        [SerializeField] private int rotation = default;


        public void UpdatePosition(Vector3 pos)
        {
			transform.position = pos;
        }

        public void Rotate(bool clockwise)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 60);
            Rotation -= 1;
        }
	}
}