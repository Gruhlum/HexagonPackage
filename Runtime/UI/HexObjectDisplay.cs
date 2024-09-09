//using HexagonPackage.HexObjects;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//namespace HexagonPackage.UI
//{
//	public class HexObjectDisplay : MonoBehaviour
//	{
//		[SerializeField] private Image img = default;
//		[SerializeField] private TextMeshProUGUI nameGUI = default;

//		[HideInInspector][SerializeField]private HexObjectDisplayController controller;

//		public HexObject HexObject
//		{
//			get
//			{
//				return hexObject;
//			}
//			set
//			{
//				hexObject = value;
//			}
//		}
//		[SerializeField] private HexObject hexObject = default;


//		public void Setup(HexObject hexObj, HexObjectDisplayController controller)
//		{
//			this.HexObject = hexObj;
//			this.controller = controller;
//			img.sprite = hexObj.Sprite;
//			nameGUI.text = hexObj.name;
//		}
//		public void OnClicked()
//		{
//			controller.OnDisplayClicked(this);
//		}
//	}
//}