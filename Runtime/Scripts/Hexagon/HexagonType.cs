using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    [CreateAssetMenu(menuName = "HexPackage/HexType")]
    public class HexagonType : ScriptableObject
	{
        public Sprite sprite;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        [SerializeField] private Color color = Color.white;
        public bool ignoreColor;

        public void Apply(Hexagon hex)
        {
            if (sprite != null)
            {
                hex.SpriteRenderer.sprite = sprite;
            }
            
            if (!ignoreColor)
            {
                hex.SpriteRenderer.color = color;
            }
            else hex.SpriteRenderer.color = Color.white;
        }
	}
}