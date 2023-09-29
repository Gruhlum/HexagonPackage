using HexagonPackage.HexagonComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects
{
    public class DynamicTile : MonoBehaviour
    {
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


        public void Setup(Sprite sprite, Vector3 position, int rotation, bool flip, int sortingOrder)
        {
            SpriteRenderer.sprite = sprite;
            transform.localPosition = position;
            transform.localEulerAngles = new Vector3(0, 0, 60 * rotation);
            SpriteRenderer.flipY = flip;
            SpriteRenderer.sortingOrder = sortingOrder;
        }
        public void Setup(DynamicBuilding.DynamicInfo info)
        {
            Setup(info.Sprite, info.Position, info.Rotation, info.Flip, info.SortingOrder);
        }
        public void SetColor(Color col)
        {
            SpriteRenderer.color = col;
        }
        public void SetSortingOrder(int value)
        {
            SpriteRenderer.sortingOrder = value;
        }

    }
}