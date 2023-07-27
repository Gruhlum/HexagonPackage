using HecTecGames.SoundSystem;
using HexagonPackage;
using HexTecGames.Basics;
using HexTecGames.TweenLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects.UI
{
    public class BuildController : MonoBehaviour
    {
        [SerializeField] private BuildingInfo buildingInfo = default;
        [SerializeField] private IResourceCollection player = default;

        [SerializeField] private Transform buildingParent = default;
        public BuildGhost Ghost
        {
            get
            {
                return ghost;
            }
            private set
            {
                ghost = value;
            }
        }
        [SerializeField] private BuildGhost ghost = default;

        public Cube? HoverCube
        {
            get
            {
                return hoverCube;
            }
            private set
            {
                if (hoverCube == value)
                {
                    return;
                }
                allowLockedSound = true;
                hoverCube = value;
                UpdateLayout();
            }
        }
        private Cube? hoverCube;

        public BuildingSlot SelectedSlot
        {
            get
            {
                return selectedSlot;
            }
            private set
            {
                selectedSlot = value;
            }
        }
        private BuildingSlot selectedSlot;

        public Building SelectedBuilding
        {
            get
            {
                return selectedBuilding;
            }
            set
            {
                selectedBuilding = value;
                Cursor.visible = selectedBuilding == null;
                SelectedBuildingChanged?.Invoke(selectedBuilding);
            }
        }
        [SerializeField] private Building selectedBuilding = default;


        [SerializeField] private Spawner<HexHighlighter> highlightSpawner = default;

        private List<Building> buildBuildings = new List<Building>();

        [Space]
        [SerializeField] private HexagonGrid hexGrid = default;

        public event Action<BuildEventArgs> BuildingPlanned;
        public event Action<Building> AfterBuild;
        public event Action<Building> BeforeBuild;
        public event Action<Building> BeforeBuildingRemoved;
        public event Action AfterBuildingRemoved;
        public event Action<Building> SelectedBuildingChanged;
        public event Action<Building> BuildingDeselected;

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
        [SerializeField] private bool removeSlotAfterBuild;

        public Color Green;
        public Color Red;
        public Color Yellow;

        public int Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
        private int rotation;

        [SerializeField] private bool allowRemove = default;

        public bool CanRemove
        {
            get
            {
                return allowRemove == false && removalBlocker.Count == 0;
            }
        }
        public bool CanBuild
        {
            get
            {
                return buildBlocker.Count == 0;
            }
        }

        private List<MonoBehaviour> removalBlocker = new List<MonoBehaviour>();
        private List<MonoBehaviour> buildBlocker = new List<MonoBehaviour>();

        private float deselectDelay;
        private Vector2 mouseOffset;
        private bool validBuildLocation = false;

        [Header("Sounds")]
        [SerializeField] private SoundClip buildLocked;
        [SerializeField] private SoundClip buildComplete;
        [SerializeField] private SoundClipGroup errorSounds;
        bool allowLockedSound = true;

        private string errorMsg;

        private void Update()
        {
            if (deselectDelay > 0)
            {
                deselectDelay -= Time.deltaTime;
            }
            if (SelectedBuilding != null)
            {
                Cube c = hexGrid.WorldPointToCube(mouseOffset + Camera.main.GetMousePosition());
                if (SelectedBuilding.IsValidPosition(c, hexGrid, Rotation))
                {
                    Ghost.UpdatePosition(hexGrid.CubeToWorldPoint(c));
                    HoverCube = c;
                    if (allowLockedSound == true)
                    {
                        buildLocked.Play();
                        allowLockedSound = false;
                    }
                }
                else
                {
                    Ghost.SetToMousePosition(mouseOffset);
                    HoverCube = null;
                }
            }
            if (Input.mouseScrollDelta.y > 0f)
            {
                Rotation = Cube.WrapDirection(Rotation + 1);
                Ghost.Rotate(Rotation);
                UpdateLayout();
            }
            else if (Input.mouseScrollDelta.y < 0f)
            {
                Rotation = Cube.WrapDirection(Rotation - 1);
                Ghost.Rotate(Rotation);
                UpdateLayout();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    Rotation = Cube.WrapDirection(Rotation + 1);
                    Ghost.Rotate(Rotation);
                }
                else
                {
                    Rotation = Cube.WrapDirection(Rotation - 1);
                    Ghost.Rotate(Rotation);
                }
                UpdateLayout();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RightClickAction();
            }
            if (Input.GetMouseButtonDown(0))
            {
                LeftClickAction();
            }
        }
        public void AllowRemoval(bool allow, MonoBehaviour m)
        {
            if (!allow)
            {
                if (removalBlocker.Contains(m))
                {
                    return;
                }
                removalBlocker.Add(m);

            }
            else removalBlocker.Remove(m);
        }
        public void AllowBuild(bool allow, MonoBehaviour m)
        {
            if (!allow)
            {
                if (buildBlocker.Contains(m))
                {
                    return;
                }
                buildBlocker.Add(m);

            }
            else buildBlocker.Remove(m);
        }
        public void ReleaseAllRemovalBlocker()
        {
            removalBlocker.Clear();
        }
        public void ReleaseAllBuildBlocker()
        {
            buildBlocker.Clear();
        }
        private void RightClickAction()
        {
            BuildingDeselected?.Invoke(SelectedBuilding);
            SetSelectedSlot(null);
            if (buildingInfo != null)
            {
                buildingInfo.Disable();
            }
        }
        private void LeftClickAction()
        {
            if (SelectedBuilding != null)
            {
                if (HoverCube.HasValue == false)
                {
                    if (deselectDelay <= 0)
                    {
                        BuildingDeselected?.Invoke(SelectedBuilding);
                        SetSelectedSlot(null);
                    }                   
                }
                else Build(SelectedBuilding, hoverCube.Value, Rotation);
                return;
            }
            Hexagon hex = hexGrid.MousePositionToHexagon();
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
            if (CanRemove)
            {               
                SetSelectedBuilding(hex.HexObject.Prefab as Building, hex.HexObject.transform.position, hex.HexObject.Rotation);
                buildBuildings.Remove(hex.HexObject as Building);
                BeforeBuildingRemoved?.Invoke(hex.HexObject as Building);
                hex.HexObject.Destroy();
                UpdateLayout();
                AfterBuildingRemoved?.Invoke();
            }
        }
        private bool CheckIfAllowed()
        {
            if (HoverCube.HasValue == false)
            {
                errorMsg = "No HoverCube";
                return false;
            }
            if (SelectedBuilding == null)
            {
                errorMsg = "No Building selected";
                return false;
            }

            if (SelectedBuilding.IsValidPosition(HoverCube.Value, hexGrid, Rotation) == false)
            {
                errorMsg = "Invalid Position";
                return false;
            }

            //foreach (var resource in SelectedBuilding.Costs)
            //{
            //    if (!player.HasResource(resource))
            //    {
            //        Debug.Log("Not enough " + resource.Type);
            //        return false;
            //    }
            //    player.ChangeResource(resource.Type, -resource.Value);
            //}

            BuildEventArgs eventArgs = SelectedBuilding.GenerateBuildEventArgs(HoverCube.Value, Ghost.Rotation);
            BuildingPlanned?.Invoke(eventArgs);
            if (eventArgs.Allow == false)
            {
                errorMsg = "Other Script blocked build";
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
        public void DestroyBuilding(Building building)
        {
            buildBuildings.Remove(building);
            building.Destroy();
        }
        private void UpdateLayout()
        {
            foreach (var highlight in highlightSpawner.GetActiveBehaviours())
            {
                highlight.Disable();
            }
            validBuildLocation = CheckIfAllowed();

            if (SelectedBuilding == null || HoverCube.HasValue == false)
            {
                return;
            }

            List<Cube> cubes = new List<Cube>();

            if (SelectedBuilding is MultiTileBuilding multi)
            {
                cubes.AddRange(multi.GetInvalidPositions(HoverCube.Value, hexGrid, Ghost.Rotation));
            }
            if (cubes.Count == 0)
            {
                cubes = SelectedBuilding.GetOccupyingCubes(HoverCube.Value, Ghost.Rotation);
            }

            foreach (var cube in cubes)
            {
                highlightSpawner.Spawn().Setup(cube.ToWorldPosition(
                    hexGrid.HexagonData.VerticalSpacing,
                    hexGrid.HexagonData.HorizontalSpacing,
                    hexGrid.HexagonData.Flat),
                    validBuildLocation ? Green : Red);
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
        public Building Build(Building building, Cube center, int rotation, bool overwrite = false)
        {
            if (!overwrite && CanBuild == false)
            {
                return null;
            }
            if (!overwrite && validBuildLocation == false)
            {
                errorSounds.Play(pitchMulti: UnityEngine.Random.Range(0.8f, 1.2f));
                building.GetComponent<TweenPlayer>().Play();
                return null;
            }
            if (building == null)
            {
                return null;
            }
            BeforeBuild?.Invoke(building);
            var clone = Instantiate(building);
            clone.Prefab = building;
            clone.transform.parent = buildingParent;
            clone.Setup(center, hexGrid, rotation);
            clone.transform.position = building.transform.position;
            buildBuildings.Add(clone);
            if (RemoveSlotAfterBuild)
            {
                if (SelectedSlot != null)
                {
                    SelectedSlot.Disable();
                }
                SetSelectedSlot(null);
            }
            UpdateLayout();
            buildComplete.Play();
            AfterBuild?.Invoke(clone);
            return clone;
        }
        public void SetSelectedBuilding(Building building, int rotation = 0)
        {
            SelectedBuilding = building;
            Ghost.gameObject.SetActive(SelectedBuilding != null);
            this.Rotation = rotation;
            Ghost.Rotate(rotation);
            if (SelectedBuilding != null)
            {
                Ghost.SetSprite(SelectedBuilding.Sprite, SelectedBuilding.GetColor());
                deselectDelay = 0.1f;
            }
            UpdateLayout();
        }
        public void SetSelectedBuilding(Building building, Vector2 offset, int rotation = 0)
        {
            this.mouseOffset = offset - Camera.main.GetMousePosition();
            SetSelectedBuilding(building, rotation);
        }
        public void SetSelectedSlot(BuildingSlot slot, int rotation = 0)
        {
            SelectedSlot = slot;
            if (slot == null)
            {
                SetSelectedBuilding(null);
            }
            else SetSelectedBuilding(slot.Building);
        }
        public List<Building> GetBuildBuildings()
        {
            List<Building> buildings = new List<Building>();
            buildings.AddRange(buildBuildings);
            return buildings;
        }
    }
}