#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HexagonPackage
{
    public class GridSaver : MonoBehaviour
    {
        [TextArea]
        public string folderLocation = "Assets/HexagonPackage/Scripts/SavedGrids/Grids";
        public new string name;
        public HexagonGrid hexagonGrid;

        public bool OverrideSave
        {
            get
            {
                return overrideSave;
            }
            private set
            {
                overrideSave = value;
            }
        }
        private bool overrideSave;

        private HexagonGrid oldGrid;

        private void Awake()
        {
            Application.quitting += Application_quitting;
        }
        private void Start()
        {
            if (!AssetDatabase.IsValidFolder(folderLocation))
            {
                Debug.LogWarning("Unable to save Grid. Folder \"" + folderLocation + "\" does not exist!");
            }
        }
        private void Application_quitting()
        {
            SavePositions(name, true);
        }

        private void OnValidate()
        {
            if ((name == "" && hexagonGrid != null) || (oldGrid != hexagonGrid && oldGrid != null && name == oldGrid.name))
            {
                name = hexagonGrid.name;
                oldGrid = hexagonGrid;
            }
        }

        private bool AssetAlreadyExist(string name)
        {
            string[] results = AssetDatabase.FindAssets(name, new string[] { folderLocation });
            if (results.Length == 0)
            {
                return false;
            }
            return true;
        }
        public SavedGrid GenerateSavedGrid(List<Hexagon> hexagons)
        {
            SavedGrid savedGrid = ScriptableObject.CreateInstance<SavedGrid>();
            savedGrid.SaveGridData(hexagons);
            return savedGrid;
        }
        public void SavePositions(string name, bool forceSave = false)
        {
            if (!AssetDatabase.IsValidFolder(folderLocation))
            {
                Debug.LogWarning("Unable to save Grid. Folder \"" + folderLocation + "\" does not exist!");
                return;
            }
            if (name == "")
            {
                name = "unnamed";
            }
            if (forceSave == false && overrideSave == false && AssetAlreadyExist(name))
            {
                Debug.Log(name + " already exists. Click again to override.");
                overrideSave = true;
                return;
            }
            overrideSave = false;

            SavedGrid savedGrid = GenerateSavedGrid(hexagonGrid.Hexagons.Values.ToList());         
            
            AssetDatabase.CreateAsset(savedGrid, folderLocation + "/" + name + ".asset");
            Debug.Log("Save Successful");
        }
    }
}
#endif