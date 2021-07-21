using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    [CreateAssetMenu(menuName = "HexPackage/HexType")]
    public class HexagonType : ScriptableObject
	{
        public Sprite sprite;
        public Color color = Color.white;

        public void Apply(Hexagon hex)
        {
            hex.SpriteRenderer.sprite = sprite;
            hex.SpriteRenderer.color = color;
        }
	}
}