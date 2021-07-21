using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    public class HexTypeManager : MonoBehaviour
    {
        public GameObject HexTypePrefab;
        public GridEditor GridEditor;
        [SerializeField] private List<HexagonType> hexTypes = default;

        public List<HotKey> HotKeys;

        private List<HexTypeDisplay> displays = new List<HexTypeDisplay>();
        private void Awake()
        {
            CreateDisplays();
            if (displays.Count != 0)
            {
                OnButtonClicked(displays[0]);
            }
        }
        private void Update()
        {
            foreach (var hotKey in HotKeys)
            {
                if (Input.GetKeyDown(hotKey.KeyCode))
                {
                    HexTypeDisplay display = displays.Find(x => x.HotKey.KeyCode == hotKey.KeyCode);
                    if (display != null)
                    {
                        OnButtonClicked(display);
                    }
                }
            }
        }
        private void OnValidate()
        {
            if (HotKeys == null || HotKeys.Count == 0)
            {
                CreateDefaultHotkeys();
            }
        }

        private void CreateDefaultHotkeys()
        {
            HotKeys = new List<HotKey>();
            for (int i = 1; i < 9; i++)
            {
                HotKey hotkey = new HotKey
                {
                    KeyCode = (KeyCode)(48 + i),
                    DisplayText = i.ToString()
                };
                HotKeys.Add(hotkey);
            }
        }
        public void CreateDisplays()
        {
            if (displays.Count != 0)
            {
                for (int i = displays.Count - 1; i >= 0; i--)
                {
                    Destroy(displays[i].gameObject);
                }
            }
            for (int i = 0; i < hexTypes.Count; i++)
            {
                GameObject hexTypeGO = Instantiate(HexTypePrefab, transform);
                HexTypeDisplay hexTypeDisplay = hexTypeGO.GetComponent<HexTypeDisplay>();
                hexTypeDisplay.Setup(hexTypes[i], this, GetHotKey(i));
                displays.Add(hexTypeDisplay);
            }
        }
        private HotKey GetHotKey(int index)
        {
            if (index > HotKeys.Count - 1)
            {
                return null;
            }
            return HotKeys[index];
        }

        public void OnButtonClicked(HexTypeDisplay selectedDisplay)
        {
            foreach (var display in displays)
            {
                display.ToggleColor(selectedDisplay == display);
            }
            GridEditor.selectedType = selectedDisplay.HexType;
        }
    }
}