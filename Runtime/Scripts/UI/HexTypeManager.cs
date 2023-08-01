using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexagonPackage
{
    public class HexTypeManager : MonoBehaviour
    {
        public GridEditor GridEditor;
        [SerializeField] private List<HexagonType> hexTypes = default;

        public List<HotKey> HotKeys;
        [SerializeField] private Spawner<HexTypeDisplay> displaySpawner = default;

        private void Update()
        {
            foreach (var hotKey in HotKeys)
            {
                if (Input.GetKeyDown(hotKey.KeyCode))
                {
                    List<HexTypeDisplay> displays = GetComponentsInChildren<HexTypeDisplay>().ToList();
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
        [ContextMenu("Create Displays")]
        public void CreateDisplays()
        {
            displaySpawner.TryDestroyAll();
            for (int i = 0; i < hexTypes.Count; i++)
            {
                HexTypeDisplay hexTypeDisplay = displaySpawner.Spawn();
                hexTypeDisplay.Setup(hexTypes[i], this, GetHotKey(i));
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
            foreach (var display in displaySpawner.GetActiveBehaviours())
            {
                display.ToggleColor(selectedDisplay == display);
            }
            GridEditor.SelectedType = selectedDisplay.HexType;
        }
    }
}