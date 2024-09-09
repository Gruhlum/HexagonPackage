//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//namespace HexagonPackage.HexObjects.UI
//{
//	public class BuildingInfo : MonoBehaviour
//	{
//		[SerializeField] private Image icon = default;
//		[SerializeField] private TextMeshProUGUI title = default;
//		[SerializeField] private TextMeshProUGUI info = default;

//		public void Setup(Building building)
//		{
//			icon.sprite = building.Sprite;
//			title.text = building.Name;
//			info.text = building.Description;
//			gameObject.SetActive(true);
//		}
//		public void Disable()
//		{
//			gameObject.SetActive(false);
//		}
//		public void OnDestroy_Clicked()
//		{

//		}
//		public void OnMove_Clicked()
//		{

//		}
//	}
//}