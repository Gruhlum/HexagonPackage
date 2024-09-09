//using HexTecGames.Basics;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace HexagonPackage
//{
//    [System.Serializable]
//    public class HexHighlightController
//    {
//        [SerializeField] private Spawner<HexHighlight> highlightSpawner = default;

//        public float FadeOut
//        {
//            get
//            {
//                return fadeOut;
//            }
//            set
//            {
//                fadeOut = value;
//            }
//        }
//        [SerializeField] private float fadeOut = default;

//        public float FadeIn
//        {
//            get
//            {
//                return fadeIn;
//            }
//            set
//            {
//                fadeIn = value;
//            }
//        }
//        [SerializeField] private float fadeIn = default;

//        public Color Color
//        {
//            get
//            {
//                return color;
//            }
//            private set
//            {
//                color = value;
//            }
//        }
//        [SerializeField] private Color color = Color.white;

//        public HexHighlight Spawn()
//        {
//            return highlightSpawner.Spawn();
//        }
//        public HexHighlight Spawn(Hexagon hex)
//        {
//            HexHighlight highlight = highlightSpawner.Spawn();
//            highlight.Setup(hex, Color, FadeIn);
//            return highlight;
//        }
//        public HexHighlight Spawn(Vector3 position)
//        {
//            HexHighlight highlight = highlightSpawner.Spawn();
//            highlight.Setup(position, Color, FadeIn);
//            return highlight;
//        }
//        public void SetColor(Color col, bool ignoreAlpha = false)
//        {
//            if (ignoreAlpha)
//            {
//                col.a = Color.a;
//            }
//            Color = col;
//            foreach (var hexHighlighter in highlightSpawner.GetActiveBehaviours())
//            {
//                hexHighlighter.SetColor(Color, FadeIn);
//            }
//        }
//    }
//}