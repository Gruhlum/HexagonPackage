using HexagonPackage;
using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects.UI
{
    public class BuildController1 : MonoBehaviour
    {
        [SerializeField] private BuildingInfo buildingInfo = default;
        [SerializeField] private IResourceCollection player = default;

        [SerializeField] private Transform buildingParent = default;
        [SerializeField] private BuildGhost ghost = default;
        public Hexagon HoverHex
        {
            get
            {
                return hoverHex;
            }
            private set
            {
                if (hoverHex == value)
                {
                    return;
                }
                hoverHex = value;
                UpdateLayout();
            }
        }
        private Hexagon hoverHex;

        public BuildingSlot SelectedSlot
        {
            get
            {
                return selectedSlot;
            }
            set
            {
                selectedSlot = value;
                if (selectedSlot != null)
                {
                    ghost.SetSprite(selectedSlot.Building.Sprite, selectedSlot.Building.GetColor());
                }
                ghost.gameObject.SetActive(selectedSlot != null);
                buildingSelected = (selectedSlot != null);
                UpdateLayout();
                rotation = 0;
                ghost.Rotate(0);
            }
        }
        private BuildingSlot selectedSlot;
        private bool buildingSelected;

        [SerializeField] private Spawner<HexHighlighter> highlightSpawner = default;

        private List<Building> buildBuildings = new List<Building>();

        [Space]
        [SerializeField] private HexagonGrid grid = default;

        public event Action<BuildEventArgs> BuildingPlanned;
        public event Action<Building> BuildingFinished;

        public bool RemoveSlotAfterBuild
        {
            get
            {
                return removeSlotAfterBuild;
            }
            set
            {
                removeSlotAfterBuild = value;
            }
        }
        private bool removeSlotAfterBuild;

        public Color Green;
        public Color Red;
        public Color Yellow;

        int rotation = 0;

        private bool allowedToBuild = false;

        private void Update()
        {
            if (buildingSelected)
            {
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
                HoverHex = grid.WorldPointToHexagon(mouseWorldPos);
                if (HoverHex == null)
                {
                    ghost.SetPosition(mouseWorldPos);
                }
                else
                {
                    ghost.SetPosition(HoverHex.transform.position);
                }
            }
            if (Input.mouseScrollDelta.y > 0f)
            {
                rotation = Cube.WrapDirection(rotation + 1);
                ghost.Rotate(rotation);
                UpdateLayout();
            }
            else if (Input.mouseScrollDelta.y < 0f)
            {
                rotation = Cube.WrapDirection(rotation - 1);
                ghost.Rotate(rotation);
                UpdateLayout();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rotation = Cube.WrapDirection(rotation + 1);
                    ghost.Rotate(rotation);
                }
                else
                {
                    rotation = Cube.WrapDirection(rotation - 1);
                    ghost.Rotate(rotation);
                }
                UpdateLayout();
            }
            if (Input.GetMouseButtonDown(1))
            {
                SelectedSlot = null;
                if (buildingInfo != null)
                {
                    buildingInfo.Disable();
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (SelectedSlot != null)
                {
                    TryToBuild();
                }
                else
                {
                    Hexagon hex = grid.MousePositionToHexagon();
                    if (hex == null)
                    {
                        return;
                    }
                    if (hex.HexObject == null)
                    {
                        return;
                    }
                    if (buildingInfo != null)
                    {
                        buildingInfo.Setup(hex.HexObject as Building);
                    }
                }
            }
        }

        private bool CheckIfAllowed()
        {
            if (HoverHex == null)
            {
                //Debug.Log("HoverHex is null");
                return false;
            }
            if (SelectedSlot == null)
            {
                return false;
            }

            Building building = SelectedSlot.Building;

            //if (building.IsValidPosition(HoverHex, rotation) == false)
            //{
            //    //Debug.Log("Invalid Position");
            //    return false;
            //}

            foreach (var resource in building.Costs)
            {
                if (!player.HasResource(resource))
                {
                    //Debug.Log("Not enough " + resource.Type);
                    return false;
                }
                player.ChangeResource(resource.Type, -resource.Value);
            }

            BuildEventArgs eventArgs = building.GenerateBuildEventArgs(HoverHex.Cube, ghost.Rotation);
            BuildingPlanned?.Invoke(eventArgs);
            if (eventArgs.Allow == false)
            {
                //Debug.Log("Not Allowed");
                return false;
            }
            return true;
        }
        public void DestroyAllBuildings()
        {
            for (int i = buildBuildings.Count - 1; i >= 0; i--)
            {
                buildBuildings[i].Destroy();
            }
            buildBuildings.Clear();
        }
        private void UpdateLayout()
        {
            foreach (var highlight in highlightSpawner.GetActiveBehaviours())
            {
                highlight.Disable();
            }
            allowedToBuild = CheckIfAllowed();

            if (SelectedSlot == null || HoverHex == null)
            {
                return;
            }

            List<Cube> cubes = new List<Cube>();

            if (SelectedSlot.Building is MultiTileBuilding multi)
            {
                //cubes.AddRange(multi.GetInvalidPositions(HoverHex, grid, ghost.Rotation));
            }
            if (cubes.Count == 0)
            {
                cubes = SelectedSlot.Building.GetOccupyingCubes(HoverHex.Cube, ghost.Rotation);
            }

            foreach (var cube in cubes)
            {
                highlightSpawner.Spawn().Setup(cube.ToWorldPosition(
                    grid.HexagonData.VerticalSpacing,
                    grid.HexagonData.HorizontalSpacing,
                    grid.HexagonData.Flat),
                    allowedToBuild ? Green : Red);
            }

            //List<Cube> extraCubes = SelectedSlot.Building.GetAdditonalCubes(HoverHex.Cube, rotation);
            //List<Hexagon> extraHexes = grid.GetHexagons(extraCubes);
            //if (extraCubes != null)
            //{
            //    foreach (var hex in extraHexes)
            //    {
            //        highlightSpawner.Spawn().Setup(hex, Yellow);
            //    }
            //}
        }
        private void TryToBuild()
        {
            if (allowedToBuild == false)
            {
                return;
            }

            var clone = Instantiate(SelectedSlot.Building);
            clone.transform.parent = buildingParent;
            //clone.Setup(HoverHex, rotation);
            buildBuildings.Add(clone);

            if (RemoveSlotAfterBuild)
            {
                SelectedSlot.Disable();
                SelectedSlot = null;
            }

            BuildingFinished?.Invoke(clone);
            UpdateLayout();
        }

        public void OnBuildingSlotClicked(BuildingSlot slot)
        {
            SelectedSlot = slot;
        }

        
    }
}