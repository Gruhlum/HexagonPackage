using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexagonPackage
{
    [CustomEditor(typeof(GridSaver))]
    public class GridSaverEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GridSaver gridSaver = (GridSaver)target;
            if (gridSaver.OverrideSave)
            {
                EditorGUILayout.HelpBox("File with the name '" + gridSaver.name + "' already exist. Click again to overwrite.", MessageType.Info);

                if (GUILayout.Button("Overwrite"))
                {
                    gridSaver.SavePositions(gridSaver.name);
                }
            }
            else
            {
                if (GUILayout.Button("Save"))
                {
                    gridSaver.SavePositions(gridSaver.name);
                }
            }           
        }
    }
}
