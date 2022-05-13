using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
	public class HexHighlighter : MonoBehaviour
	{
        public void Setup(Hexagon hex)
        {
            if (hex == null)
            {
                return;
            }
            transform.position = hex.transform.position;
        }
        //public Vector2 ToWorldPosition(int x, int y, bool flat = false)
        //{
        //    if (flat)
        //    {
        //        return new Vector2(HexVerticalSpacing * x, HexHorizontalSpacing * (y + x / 2f));
        //    }
        //    else return new Vector2(HexHorizontalSpacing * (x + y / 2f), HexVerticalSpacing * y);
        //}
    }
}