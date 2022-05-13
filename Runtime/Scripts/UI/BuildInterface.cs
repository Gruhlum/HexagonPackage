using Exile;
using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
    public class BuildInterface : MonoBehaviour
    {
        //[SerializeField] private SelectionController selectionController = default;

        //public HexObject SelectedObject
        //{
        //    get
        //    {
        //        if (selectedDisplay == null)
        //        {
        //            return null;
        //        }
        //        else return selectedDisplay.Item as HexObject;
        //    }
        //}
        //public UIDisplay SelectedDisplay
        //{
        //    get
        //    {
        //        return selectedDisplay;
        //    }
        //    set
        //    {
        //        if (selectedDisplay != null)
        //        {
        //            selectedDisplay.ChangeHighlight(false);                    
        //        }

        //        selectedDisplay = value;

        //        if (selectedDisplay != null)
        //        {
        //            selectedDisplay.ChangeHighlight(true);
        //            //ghost.Sprite = (HexObject)selectedDisplay.Item.Sprite;
        //            ghost.gameObject.SetActive(true);
        //        }
        //        else ghost.gameObject.SetActive(false);
        //    }
        //}
        //[SerializeField] private UIDisplay selectedDisplay = default;

        //[SerializeField] private BuildGhost ghost = default;

        //protected void Awake()
        //{
        //    selectionController.Hexagon_Clicked += SelectionController_Hexagon_Clicked;
        //}

        //private void Update()
        //{
        //    if (ghost.gameObject.activeInHierarchy)
        //    {
        //        if (Input.GetKeyDown(KeyCode.R))
        //        {
        //            ghost.Rotate(true);
        //            if (selectionController.HoverHexagon != null)
        //            {
        //                selectionController.highlightSpawner.DeactivateAll();
        //                foreach (var cube in SelectedObject.GetOccupyingCubes(selectionController.HoverHexagon, ghost.Rotation))
        //                {
        //                    selectionController.HoverHexagon.HexGrid.Hexagons.TryGetValue(cube, out Hexagon hex);                           
        //                    selectionController.highlightSpawner.Spawn().Setup(hex);
        //                }
        //            }
        //        }
        //        if (selectionController.HoverHexagon != null)
        //        {
        //            ghost.UpdatePosition(selectionController.HoverHexagon.transform.position);
        //        }
        //        else ghost.UpdatePosition(selectionController.MousePosition);
        //    }
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        if (SelectedDisplay != null)
        //        {
        //            SelectedDisplay = null;
        //        }
        //    }
        //}

        //protected void Display_OnClicked(UIDisplay display)
        //{
        //    SelectedDisplay = display;
        //}

        //private void SelectionController_Hexagon_Clicked(Hexagon hex, int btn)
        //{
        //    if (btn == 0) //left click
        //    {
        //        if (SelectedObject != null && SelectedObject.ValidPosition(hex, ghost.Rotation))
        //        {
        //            HexObject obj = Instantiate(SelectedObject);
        //            obj.transform.eulerAngles = new Vector3(0, 0, ghost.Rotation * 60);
        //            obj.Hexagon = hex;
        //        }
        //    }
        //}
    }
}