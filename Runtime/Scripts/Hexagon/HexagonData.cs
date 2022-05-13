using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    [CreateAssetMenu(menuName = "HexPackage/HexagonData")]
    public class HexagonData : ScriptableObject
    {
        private static readonly float SQRT_3 = 1.73205f;
        private static readonly float WIDTH_MULTIPLIER = 0.866025f; //Mathf.Sqrt(3) / 2;

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
        public float VerticalSpacing
        {
            get
            {
                return HexHeight * 0.75f + SpacingY;
            }
        }
        public float HorizontalSpacing
        {
            get
            {
                return HexWidth + SpacingX;
            }
        }
        public float SpacingX
        {
            get
            {
                return spacingX;
            }
            set
            {
                spacingX = value;
            }
        }
        [SerializeField] private float spacingX = default;

        public float SpacingY
        {
            get
            {
                return spacingY;
            }
            set
            {
                spacingY = value;
            }
        }
        [SerializeField] private float spacingY = default;

        public bool Flat
        {
            get
            {
                return flat;
            }
            set
            {
                flat = value;
            }
        }
        [SerializeField] private bool flat = default;

        public Vector2 ToWorldPosition(int x, int y)
        {
            if (Flat)
            {
                return new Vector2(VerticalSpacing * x, HorizontalSpacing * (y + x / 2f));
            }
            else return new Vector2(HorizontalSpacing * (x + y / 2f), VerticalSpacing * y);
        }
        public Cube WorldPositionToCube(float x, float y)
        {
            if (Flat)
            {
                return new Cube();
            }
            else
            {
                return new Cube((SQRT_3 / 3 * x - 1f / 3f * y) / Radius, (2f / 3f * y) / Radius);
            }
        }        
    }
}