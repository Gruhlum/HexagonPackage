//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//namespace HexagonPackage.HexObjects.UI
//{
//	public class BuildingSlot : MonoBehaviour
//	{
//		[SerializeField] private TextMeshProUGUI Title = default;
//		[SerializeField] private Image Image = default;

//		public Building Building
//		{
//			get
//			{
//				return building;
//			}
//			private set
//			{
//				building = value;
//			}
//		}
//        [SerializeField] private Building building;

//        [SerializeField] private BuildController ui;

//		public void Setup(Building building, BuildController ui)
//		{
//			Building = building;
//			this.ui = ui;
//			Title.text = building.Name;
//			Image.sprite = building.Sprite;
//			Image.color = building.GetColor();
//		}
//		public void OnClicked()
//		{
//			ui.SetSelectedSlot(this);
//		}

//		public void Disable()
//		{
//			gameObject.SetActive(false);
//		}
//    }
//}