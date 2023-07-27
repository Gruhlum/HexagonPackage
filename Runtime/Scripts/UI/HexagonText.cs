using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexagonPackage
{
	public class HexagonText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI textGUI = default;

		public string Text
		{
			get
			{
				if (textGUI == null)
				{
					return null;
				}
				return textGUI.text;
			}
		}

		public void Disable()
		{
			gameObject.SetActive(false);
		}
		public void Setup(Transform t, string text)
		{
			textGUI.text = text;
			transform.position = t.position;
		}
		public void ChangeText(string text)
		{
			textGUI.text = text;
		}
	}
}