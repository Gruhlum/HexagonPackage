using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HexagonPackage
{
    [ExecuteAlways]
    public class GridLoader : MonoBehaviour
	{
        public HexGrid Grid;
        public SavedGrid GridToLoad;

        void Start()
        {
            if (Application.IsPlaying(gameObject))
            {
                // Play logic
            }
            else
            {
                LoadGrid();
            }
        }

        public void LoadGrid()
        {
            if (Grid == null)
            {
                Debug.LogError("Target grid is null");
                return;
            }
            Grid.GetChildrenHexagons();
            Grid.RemoveAll(true);
            foreach (var pos in GridToLoad.SavedHexagonPositions)
            {
                Grid.CreateHexagon(pos.cube).Type = pos.type;
            }
        }
//        public void FindAsset()
//        {
//#if (UNITY_EDITOR)
//            if (gridEditor == null)
//            {
//                gridEditor = GetComponent<GridEditor>();
//            }
//            if (FindGrid == "")
//            {
//                Debug.LogError("Empty name");
//                return;
//            }

//            string[] assetPaths = AssetDatabase.FindAssets(FindGrid, new string[] { folderLocation });
//            if (assetPaths.Length == 0)
//            {
//                Debug.LogWarning(FindGrid + " could not be found");
//                return;
//            }
//            var obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(GetBestResult(assetPaths, FindGrid)), typeof(object));
//            FindGrid = "";
//            GridToLoad = obj as SavedGrid;
//#endif
//        }
//        private string GetBestResult(string[] paths, string searchName)
//        {
//            List<string> pathNames = paths.ToList();
//            if (pathNames.Any(x => x == searchName))
//            {
//                return pathNames.Find(x => x == searchName);
//            }
//            else return paths[0];
//        }
	}
}