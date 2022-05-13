using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexagonPackage
{
    [CustomEditor(typeof(GridBuilder))]
    public class GridBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GridBuilder myTarget = (GridBuilder)target;

            if (GUILayout.Button("Build"))
            {
                myTarget.BuildGrid();
            }
            if (GUILayout.Button("Clear"))
            {
                myTarget.ClearAll(true);
            }
        }
    }
}