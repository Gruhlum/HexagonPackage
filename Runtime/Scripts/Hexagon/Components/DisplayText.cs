using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonPackage
{
    public class DisplayText : HexComponent
    {
        [SerializeField] private TextMeshProUGUI hexText;

        private void Awake()
        {
            GetComponentInChildren<Canvas>().worldCamera = Camera.main;
        }
        
        protected override void Reset()
        {
            base.Reset();
            //if (GetComponentInChildren<Canvas>() == null)
            //{
            //    CreateTextCanvas();
            //}
        }

        private void CreateTextCanvas()
        {
            GameObject child = new GameObject("Canvas");
            child.transform.SetParent(transform);
            child.transform.position = transform.position;
            Canvas canvas = child.AddComponent<Canvas>();
            canvas.sortingOrder = 10;
            RectTransform rectT = canvas.GetComponent<RectTransform>();
            rectT.localScale = new Vector3(0.01f, 0.01f, 1);
            rectT.sizeDelta = new Vector2(200, 150);
            canvas.worldCamera = Camera.main;

            GameObject childTwo = new GameObject("Text");
            childTwo.transform.SetParent(child.transform, false);
            childTwo.transform.position = transform.position;
            hexText = childTwo.AddComponent<TextMeshProUGUI>();
            rectT = hexText.GetComponent<RectTransform>();
            rectT.anchorMin = new Vector2(0, 0);
            rectT.anchorMax = new Vector2(1, 1);
            rectT.sizeDelta = new Vector2(0, 0);
            hexText.alignment = TextAlignmentOptions.Midline;
            hexText.color = Color.black;
            hexText.fontSize = 80;
        }

        public void SetText(string text)
        {
            hexText.text = text;
        }
        public string GetText()
        {
            return hexText.text;
        }
    }
}