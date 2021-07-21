using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexagonComponents
{
	[RequireComponent(typeof(Hexagon))]
    public class Background : MonoBehaviour
	{
        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                sr.transform.localScale = scale;
            }
        }
        [SerializeField] private Vector3 scale = new Vector3(1.1f, 1.1f, 1.1f);



        private SpriteRenderer sr;
        private void Awake()
        {
            CreateBackground();
        }
        //private void OnValidate()
        //{
        //    if (sr == null)
        //    {
        //        CreateBackground();
        //    }
        //}

        private void CreateBackground()
        {
            if (sr != null)
            {
                return;
            }
            GameObject go = new GameObject();
            go.name = "HexBackground";
            go.transform.SetParent(gameObject.transform);
            go.transform.localScale = Scale;
            sr = go.AddComponent<SpriteRenderer>();
        }
        public void ChangeColor(Color color)
        {
            sr.color = color;
        }
    }
}