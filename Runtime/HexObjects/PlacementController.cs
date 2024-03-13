using HexagonPackage.HexObjects.UI;
using HexTecGames.Basics;
using HexTecGames.SoundSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects
{
    public class PlacementController : MonoBehaviour
    {
        [SerializeField] private HexagonGrid hexGrid = default;

        //[SerializeField] private Spawner<HexHighlighter> highlightSpawner = default;

        [SerializeField] private SoundClip rotateSound;
        [SerializeField] private Transform placementParent = default;

        public HexObject SelectedObject
        {
            get
            {
                return selectedObject;
            }
            private set
            {
                selectedObject = value;
            }
        }
        [SerializeField] private HexObject selectedObject = default;

        public PlacementGhost Ghost
        {
            get
            {
                return ghost;
            }
            set
            {
                ghost = value;
            }
        }
        [SerializeField] private PlacementGhost ghost = default;

        public Cube? HoverCube
        {
            get
            {
                return hoverCube;
            }
            set
            {
                if (hoverCube == value)
                {
                    return;
                }
                hoverCube = value;
            }
        }
        private Cube? hoverCube;

        public bool CanRemove
        {
            get
            {
                return RemovalBlocker.Allowed;
            }
        }
        public bool CanBuild
        {
            get
            {
                return PlacementBlocker.Allowed;
            }
        }

        public PermissionGroup RemovalBlocker = new PermissionGroup();
        public PermissionGroup PlacementBlocker = new PermissionGroup();

        [HideInInspector] List<HexObject> placedObjects = new List<HexObject>();
        private bool validBuildLocation = false;


        public event Action<HexObject> OnObjectPlaced;
        public event Action<HexObject> OnObjectSelected;
        public event Action<HexObject> OnObjectDeselected;


        private void Update()
        {
            UpdateGhostPosition();

            if (Input.mouseScrollDelta.y > 0f)
            {
                Rotate(1);
            }
            else if (Input.mouseScrollDelta.y < 0f)
            {
                Rotate(-1);
            }
            if (Input.GetMouseButtonDown(0))
            {
                LeftClickAction();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RightClickAction();
            }
        }
        private void LeftClickAction()
        {
            if (SelectedObject != null)
            {
                Build(SelectedObject, hoverCube.Value, Ghost.Rotation);
                return;
            }
            //Hexagon hex = hexGrid.MousePositionToHexagon();
            //if (hex == null)
            //{
            //    return;
            //}
            //if (hex.HexObject == null)
            //{
            //    return;
            //}
            //if (buildingInfo != null)
            //{
            //    buildingInfo.Setup(hex.HexObject as Building);
            //}
            //if (CanRemove)
            //{
            //    DestroyBuilding(hex.HexObject as Building);
            //}
        }
        private void RightClickAction()
        {
            if (SelectedObject == null)
            {
                Hexagon hex = hexGrid.MousePositionToHexagon();
                if (hex != null && hex.HexObject != null)
                {
                    if (placedObjects.Contains(hex.HexObject))
                    {
                        DestroyObject(hex.HexObject);
                    }                   
                }
            }
            else SetSelectedObject(null);
        }
        public void DestroyObject(HexObject obj)
        {
            obj.Destroy();
            placedObjects.Remove(obj);
        }
        public void UpdateGhostPosition()
        {
            Cube point = hexGrid.MousePositionToCube();
            if (SelectedObject == null)
            {
                return;
            }

            if (HoverCube.HasValue && HoverCube.Value == point)
            {
                return;
            }
            HoverCube = point;
            UpdateLayout();

            if (SelectedObject.IsValidPosition(point, hexGrid, Ghost.Rotation, false))
            {
                Ghost.SetPosition(hexGrid.CubeToWorldPoint(point));
            }
            else
            {
                Ghost.SetPosition(hexGrid.CubeToWorldPoint(point));
            }

        }
        private void SetupGhost(HexObject hexObj, int rotation = 0)
        {
            if (Ghost == null)
            {
                return;
            }

            Ghost.SetActive(hexObj != null);

            if (hexObj != null)
            {
                Ghost.Rotate(rotation);
                Ghost.SetSprite(hexObj.Sprite, hexObj.GetColor());
                UpdateGhostPosition();
            }
        }
        public void SetSelectedObject(HexObject hexObj, int rotation = 0)
        {
            if (hexObj == null)
            {
                HoverCube = null;
                if (SelectedObject != null)
                {
                    OnObjectDeselected?.Invoke(SelectedObject);
                }               
            }
            else
            {
                HoverCube = hexGrid.WorldPointToCube(Camera.main.GetMousePosition());
                OnObjectSelected?.Invoke(hexObj);
            }
            SelectedObject = hexObj;
            SetupGhost(hexObj, rotation);
            UpdateLayout();
        }

        public void Rotate(int value)
        {
            if (SelectedObject == null)
            {
                return;
            }
            Ghost.Rotate(Cube.WrapDirection(Ghost.Rotation + value));

            if (rotateSound != null)
            {
                rotateSound.Play();
            }
            UpdateLayout();
        }

        private bool CheckIfAllowed()
        {
            if (HoverCube.HasValue == false)
            {
                return false;
            }
            if (SelectedObject == null)
            {
                return false;
            }
            if (SelectedObject.IsValidPosition(HoverCube.Value, hexGrid, ghost.Rotation) == false)
            {
                return false;
            }
            return true;
        }
        public void UpdateLayout()
        {
            validBuildLocation = CheckIfAllowed();

            //foreach (var highlight in highlightSpawner.GetActiveBehaviours())
            //{
            //    highlight.Disable();
            //}

            //if (SelectedObject == null || HoverCube.HasValue == false)
            //{
            //    return;
            //}

            //if (SelectedBuilding is MultiTileBuilding multi)
            //{
            //    List<Cube> cubes = multi.GetOccupyingCubes(HoverCube.Value, Ghost.Rotation);
            //    if (!hexGrid.ContainsAny(cubes))
            //    {
            //        return;
            //    }
            //    List<Hexagon> hexagons = hexGrid.GetHexagons(cubes);
            //    if (cubes.Count == 0)
            //    {
            //        return;// cubes = SelectedBuilding.GetOccupyingCubes(HoverCube.Value, Ghost.Rotation);
            //    }

            //    foreach (var cube in cubes)
            //    {
            //        Hexagon hex = hexGrid.GetHexagon(cube);
            //        if (eventArgs.Allow == false)
            //        {
            //            highlightSpawner.Spawn().Setup(hexGrid.CubeToWorldPoint(cube), Red);
            //            continue;
            //        }
            //        if (hex == null)
            //        {
            //            highlightSpawner.Spawn().Setup(hexGrid.CubeToWorldPoint(cube), Red);
            //        }
            //        else if (hex.IsBlocked)
            //        {
            //            highlightSpawner.Spawn().Setup(hex.transform.position, Red);
            //        }
            //        else highlightSpawner.Spawn().Setup(hex.transform.position, Green);
            //    }
            //}
        }
        public HexObject Build(HexObject selectedObject, Cube center, int rotation = 0, bool overwrite = false)
        {
            if (!overwrite && CanBuild == false)
            {
                return null;
            }
            if (selectedObject == null)
            {
                return null;
            }
            if (!overwrite && validBuildLocation == false)
            {
                return null;
            }
            var clone = Instantiate(selectedObject);
            clone.Origin = selectedObject;
            clone.transform.parent = placementParent;
            clone.transform.position = Ghost.transform.position;
            clone.Setup(center, hexGrid, rotation);
            placedObjects.Add(clone);

            HoverCube = null;
            UpdateLayout();
            OnObjectPlaced?.Invoke(clone);
            return clone;
        }
    }
}