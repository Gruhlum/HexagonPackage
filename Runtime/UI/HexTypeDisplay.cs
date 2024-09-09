//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace HexagonPackage
//{
//	public class HexTypeDisplay : MonoBehaviour
//	{
//        [SerializeField] private Image image = null;
//        [SerializeField] private TextMeshProUGUI hotKeyText = null;

//        [SerializeField] private Color selectedColor = Color.green;
//        [SerializeField] private Color defaultColor = Color.white;

//        public HexagonType HexType;

//        public HotKey HotKey;

//        [SerializeField][HideInInspector]private HexTypeManager manager;

//        public void Reset()
//        {
//            image = GetComponentInChildren<Image>();
//            hotKeyText = GetComponentInChildren<TextMeshProUGUI>();
//        }

//        public void Setup(HexagonType hexType, HexTypeManager manager, HotKey hotKey)
//        {
//            this.manager = manager;
//            HexType = hexType;
//            HotKey = hotKey;
//            this.hotKeyText.text = hotKey.DisplayText;
//            if (hexType != null)
//            {
//                image.color = hexType.Color;
//                this.image.sprite = hexType.sprite;
//            }
//            else this.image.sprite = null;            
//        }

//        public void ToggleColor(bool active)
//        {
//            if (active)
//            {
//                hotKeyText.color = selectedColor;
//            }
//            else hotKeyText.color = defaultColor;
//        }
//        public void OnClicked()
//        {
//            manager.OnButtonClicked(this);
//        }
//    }
//}