using HexTecGames.SoundSystem;
using HexagonPackage;
using HexTecGames;
using HexTecGames.Basics;
using HexTecGames.TweenLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HexagonPackage.HexObjects.UI;

namespace HexagonPackage.HexObjects
{
    public class BuildController : MonoBehaviour
    {
        [SerializeField] private BuildingInfo buildingInfo = default;

        [SerializeField] private Transform buildingParent = default;
        public PlacementGhost Ghost
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
            private set
            {
                if (selectedBuilding == value)
                {
                    return;
                }
                BeforeSelectedBuildingChanged?.Invoke(selectedBuilding);
                if (selectedBuilding != null)
                {
                    selectedBuilding.OnBuildingSelected(false);
                }
                selectedBuilding = value;
                if (selectedBuilding != null)
                {
                    selectedBuilding.OnBuildingSelected(true);
                    if (buildSelected != null)
                    {
                        buildSelected.Play();
                    }
                }
                AfterSelectedBuildingChanged?.Invoke(selectedBuilding);
            }
        }
        [SerializeField] private Building selectedBuilding = default;

        [SerializeField] private Spawner<HexHighlight> highlightSpawner = default;

        private List<Building> buildBuildings = new List<Building>();

        [Space]
        [SerializeField] private HexagonGrid hexGrid = default;

        public event Action<BuildEventArgs> BuildingPlanned;
        public event Action<Building> AfterBuild;
        public event Action<Building> BeforeBuild;
        public event Action<Building> BeforeBuildingRemoved;
        public event Action AfterBuildingRemoved;
        public event Action<Building> AfterSelectedBuildingChanged;
        public event Action<Building> BeforeSelectedBuildingChanged;
        //public event Action<Building> BuildingDeselected;

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
                if (StateController.IsPaused)
                {
                    return false;
                }
                return allowRemove == false && removalBlocker.Count == 0;
            }
        }
        public bool CanBuild
        {
            get
            {
                if (StateController.IsPaused)
                {
                    return false;
                }
                return buildBlocker.Count == 0;
            }
        }

        private List<MonoBehaviour> removalBlocker = new List<MonoBehaviour>();
        private List<MonoBehaviour> buildBlocker = new List<MonoBehaviour>();

        private float deselectDelay;
        private Vector2 mouseOffset;
        private bool validBuildLocation = false;

        [Header("Sounds")]
        [SerializeField] private SoundClip buildLockedLight;
        [SerializeField] private SoundClip buildLocked;
        [SerializeField] private SoundClip buildComplete;
        [SerializeField] private SoundClip buildSelected;
        [SerializeField] private SoundClip buildDeselected;
        [SerializeField] private SoundClip rotateSound;

        [SerializeField] private SoundClipGroup errorSounds;
        [SerializeField] private TweenPlayer cameraShaker = default;
        bool allowLockedSound = true;

        private Vector2 lastMousePos;

#pragma warning disable IDE0052 // Remove unread private members
        [SerializeField] private string errorMsg;
#pragma warning restore IDE0052 // Remove unread private members

        float dist = 0.01f;
        float timer = 0;

        [SerializeField] private Cube debugHoverCube;

        private IEnumerator Shaker()
        {
            dist = 1f;
            timer = 0.5f;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            dist = 0.01f;
        }

        private void Update()
        {
            if (HoverCube.HasValue)
            {
                debugHoverCube = HoverCube.Value;
            }

            if (deselectDelay > 0)
            {
                deselectDelay -= Time.deltaTime;
            }
            Vector2 currentMousePos = mouseOffset + Camera.main.GetMousePosition();
            if (Vector2.Distance(lastMousePos, currentMousePos) > dist)
            {
                MoveTo(currentMousePos);
                lastMousePos = currentMousePos;
            }
            if (Input.mouseScrollDelta.y > 0f)
            {
                Rotate(1);
            }
            else if (Input.mouseScrollDelta.y < 0f)
            {
                Rotate(-1);
            }
            if (SelectedBuilding != null)
            {
                if (Input.GetKeyDown(KeyCode.R) && SelectedBuilding != null)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        Rotate(1);
                    }
                    else Rotate(-1);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Rotate(1);
                }
                if (Input.GetKeyDown(KeyCode.E))

                {
                    Rotate(-1);
                }        
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
        public void MoveTo(Vector2 point)
        {
            MoveTo(hexGrid.WorldPointToCube(point));
        }
        public void MoveTo(Cube point)
        {
            //Debug.Log("hi");
            if (SelectedBuilding == null)
            {
                return;
            }
            if (SelectedBuilding.IsValidPosition(point, hexGrid, Rotation, false))
            {
                Ghost.SetPosition(hexGrid.CubeToWorldPoint(point));
                if (allowLockedSound == true && buildLocked != null)
                {
                    buildLocked.Play();
                    allowLockedSound = false;
                }
            }
            else
            {
                if (allowLockedSound && buildLockedLight != null)
                {
                    buildLockedLight.Play();
                    allowLockedSound = false;
                }
                Ghost.SetPosition(hexGrid.CubeToWorldPoint(point));
            }
            HoverCube = point;
        }
        public void Rotate(int value)
        {
            if (SelectedBuilding == null)
            {
                return;
            }
            Rotation = Cube.WrapDirection(Rotation + value);
            Ghost.Rotate(Rotation);
            if (buildLocked != null && HoverCube.HasValue && SelectedBuilding.IsValidPosition(HoverCube.Value, hexGrid, Rotation, false))
            {
                buildLocked.Play();
            }
            if (rotateSound != null)
            {
                rotateSound.Play();
            }
            UpdateLayout();
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
            Debug.Log(m.gameObject.name + "  " + allow);
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
            DeselectBuilding();
        }

        public bool BuildingIsInsideGrid()
        {
            if (HoverCube.HasValue == false)
            {
                return false;
            }
            return hexGrid.ContainsAny(SelectedBuilding.GetOccupyingCubes(HoverCube.Value, Rotation));
        }
        private void LeftClickAction()
        {
            if (deselectDelay > 0)
            {
                return;
            }
            if (SelectedBuilding != null)
            {
                if (!BuildingIsInsideGrid())
                {
                    DeselectBuilding();
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
                DestroyBuilding(hex.HexObject as Building);
            }
        }
        public void DeselectBuilding()
        {
            if (buildDeselected != null && SelectedBuilding != null)
            {
                buildDeselected.Play();
            }
            SelectedBuilding = null;
            HoverCube = null;
            SetSelectedSlot(null);
            if (buildingInfo != null)
            {
                buildingInfo.Disable();
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
        public void DisableTemporary(float duration, MonoBehaviour m)
        {
            StartCoroutine(StartDisableTemporary(duration, m));
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
            if (building == null)
            {
                return;
            }
            BeforeBuildingRemoved?.Invoke(building);

            buildBuildings.Remove(building);
            building.Destroy();
            UpdateLayout();
            AfterBuildingRemoved?.Invoke();
        }
        public void UpdateLayout()
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

            if (SelectedBuilding is MultiTileBuilding multi)
            {
                List<Cube> cubes = multi.GetOccupyingCubes(HoverCube.Value, Ghost.Rotation);
                if (!hexGrid.ContainsAny(cubes))
                {
                    return;
                }
                List<Hexagon> hexagons = hexGrid.GetHexagons(cubes);
                if (cubes.Count == 0)
                {
                    return;// cubes = SelectedBuilding.GetOccupyingCubes(HoverCube.Value, Ghost.Rotation);
                }

                BuildEventArgs eventArgs = SelectedBuilding.GenerateBuildEventArgs(HoverCube.Value, Ghost.Rotation);
                BuildingPlanned?.Invoke(eventArgs);

                foreach (var cube in cubes)
                {
                    Hexagon hex = hexGrid.GetHexagon(cube);
                    if (eventArgs.Allow == false)
                    {
                        highlightSpawner.Spawn().Setup(hexGrid.CubeToWorldPoint(cube), Red);
                        continue;
                    }
                    if (hex == null)
                    {
                        highlightSpawner.Spawn().Setup(hexGrid.CubeToWorldPoint(cube), Red);
                    }
                    else if (hex.IsBlocked)
                    {
                        highlightSpawner.Spawn().Setup(hex.transform.position, Red);
                    }
                    else highlightSpawner.Spawn().Setup(hex.transform.position, Green);
                }
            }
        }
        public Building Build(Building building, Cube center, int rotation, bool overwrite = false)
        {
            //Debug.Log("Build 1 " + (CanBuild == false));
            if (!overwrite && CanBuild == false)
            {
                return null;
            }
            //Debug.Log("Build 2");
            if (building == null)
            {
                return null;
            }
            //Debug.Log("Build 3");
            if (!overwrite && validBuildLocation == false)
            {
                errorSounds.Play(1f, UnityEngine.Random.Range(0.8f, 1.2f));
                cameraShaker.Play();
                if (timer > 0)
                {
                    timer = 0.5f;
                }
                else StartCoroutine(Shaker());
                return null;
            }
            //Debug.Log("Build 4");
            BeforeBuild?.Invoke(building);
            var clone = Instantiate(building);
            clone.Origin = building;
            clone.transform.parent = buildingParent;
            clone.Setup(center, hexGrid, rotation);
            buildBuildings.Add(clone);
            if (RemoveSlotAfterBuild)
            {
                if (SelectedSlot != null)
                {
                    SelectedSlot.Disable();
                }
                SetSelectedSlot(null);
            }
            HoverCube = null;
            UpdateLayout();
            if (buildComplete != null)
                buildComplete.Play();
            AfterBuild?.Invoke(clone);
            return clone;
        }
        public Building Build()
        {
            if (HoverCube.HasValue == false)
            {
                return null;
            }
            return Build(SelectedBuilding, HoverCube.Value, Rotation);
        }
        public void SetSelectedBuilding(Building building, int rotation = 0)
        {
            SelectedBuilding = building;
            this.Rotation = rotation;
            if (Ghost != null)
            {
                if (building != null)
                {
                    ghost.SetPosition(building.transform.position);
                }
                Ghost.SetActive(SelectedBuilding != null);
                Ghost.Rotate(rotation);

                if (SelectedBuilding != null)
                {
                    Ghost.SetSprite(SelectedBuilding.Sprite, SelectedBuilding.GetColor());
                }
            }

            if (SelectedBuilding != null)
            {
                deselectDelay = 0.1f;
            }
            HoverCube = hexGrid.WorldPointToCube(Camera.main.GetMousePosition() + mouseOffset);
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
            else SetSelectedBuilding(slot.Building, rotation);
        }
        public List<Building> GetBuildBuildings()
        {
            List<Building> buildings = new List<Building>();
            buildings.AddRange(buildBuildings);
            return buildings;
        }

        private IEnumerator StartDisableTemporary(float duration, MonoBehaviour m)
        {
            AllowBuild(false, m);
            yield return new WaitForSeconds(duration);
            AllowBuild(true, m);
        }
    }
}