using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    public class HexHighlighter : MonoBehaviour
    {
        //public float FadeIn = 0;
        //public float FadeOut = 0;

        [SerializeField] private SpriteRenderer sr = default;

        public Hexagon Hexagon
        {
            get
            {
                return hexagon;
            }
            private set
            {
                hexagon = value;
            }
        }
        private Hexagon hexagon;


        public void Flash(Hexagon hex, float fadeIn, float holdTime, float fadeOut)
        {
            SetPosition(hex);
            StartCoroutine(StartFlash(fadeIn, holdTime, fadeOut));
        }
        public void FadeOut(float time)
        {
            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            StartCoroutine(StartFadeOut(time));
        }
        public void Setup(Hexagon hex, float fadeIn = 0)
        {
            Hexagon = hex;
            SetPosition(hex);
            if (fadeIn > 0 && gameObject.activeInHierarchy)
            {
                StartCoroutine(StartFadeIn(fadeIn));
            }
        }
        public void Setup(Hexagon hex, Color col, float fadeIn = 0)
        {
            sr.color = col;
            Setup(hex, fadeIn);
        }
        public void Setup(Vector3 pos, Color col, float fadeIn = 0)
        {
            sr.color = col;
            Setup(pos, fadeIn);
        }
        public void Setup(Vector3 pos, float fadeIn = 0)
        {
            SetPosition(pos);
            if (fadeIn > 0)
            {
                StartCoroutine(StartFadeIn(fadeIn));
            }
        }
        public void SetPosition(Hexagon hex)
        {
            if (hex == null)
            {
                return;
            }
            transform.position = hex.transform.position;
        }
        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
        public void Disable(float fadeOut = 0)
        {
            if (fadeOut >= 0)
            {
                StartCoroutine(StartFadeOut(fadeOut));
            }
            else gameObject.SetActive(false);
        }
        public void SetColor(Color col, float fadeIn = 0)
        {
            if (fadeIn >= 0)
            {
                StartCoroutine(StartColorChange(fadeIn, col));
            }
            sr.color = col;
        }
        public void SetSortingOrder(int order)
        {
            sr.sortingOrder = order;
        }
        private IEnumerator StartFlash(float fadeIn, float holdTimer, float fadeOut)
        {
            StartCoroutine(StartFadeIn(fadeIn));
            yield return new WaitForSeconds(fadeIn + holdTimer);
            StartCoroutine(StartFadeOut(fadeOut));
        }
        private IEnumerator StartColorChange(float fadeIn, Color col)
        {
            Color startCol = sr.color;
            for (float i = 0; i < fadeIn; i += Time.deltaTime)
            {
                sr.color = Color.Lerp(startCol, col, i / fadeIn);
                yield return new WaitForEndOfFrame();
            }
        }
        private IEnumerator StartFadeIn(float fadeIn)
        {
            float startAlpha = sr.color.a;
            for (float i = 0; i < fadeIn; i += Time.deltaTime)
            {
                Color newCol = sr.color;
                newCol.a = startAlpha * i / fadeIn;
                sr.color = newCol;
                yield return new WaitForEndOfFrame();
            }
        }
        private IEnumerator StartFadeOut(float fadeOut)
        {
            float startAlpha = sr.color.a;
            for (float i = 0; i < fadeOut; i += Time.deltaTime)
            {
                yield return new WaitForEndOfFrame();
                Color newCol = sr.color;
                newCol.a = startAlpha - (i / fadeOut);
                sr.color = newCol;
            }
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startAlpha);
            gameObject.SetActive(false);
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