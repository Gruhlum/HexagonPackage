using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexagonPackage
{
    [CustomEditor(typeof(GridLoader))]
    public class GridLoaderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridLoader gridLoader = (GridLoader)target;
            if (gridLoader.GridToLoad != null)
            {
                if (GUILayout.Button("Load"))
                {
                    gridLoader.LoadGrid();
                }
            }       
        }
    }
}