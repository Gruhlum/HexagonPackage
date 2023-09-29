using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexagonPackage
{
	public class HexagonText : MonoBehaviour
	{
        public TextMeshProUGUI TextGUI
        {
            get
            {
                return this.textGUI;
            }
            private set
            {
                this.textGUI = value;
            }
        }
        [SerializeField] private TextMeshProUGUI textGUI = default;

        public string Text
		{
			get
			{
				if (TextGUI == null)
				{
					return null;
				}
				return TextGUI.text;
			}
		}

        
        public void Disable()
		{
			gameObject.SetActive(false);
		}
		public void Setup(Transform t, string text)
		{
			TextGUI.text = text;
			transform.position = t.position;
		}
		public void ChangeText(string text)
		{
			TextGUI.text = text;
		}
	}
}