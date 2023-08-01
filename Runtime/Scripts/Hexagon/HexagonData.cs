using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    [System.Serializable]
    public class HexagonData
    {
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

        public Cube WorldPositionToCube()
        {
            return Cube.WorldPositionToCube(VerticalSpacing, HorizontalSpacing, Radius, Flat);
        }
    }
}