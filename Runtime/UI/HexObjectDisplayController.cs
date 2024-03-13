using HexagonPackage.HexObjects;
using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.UI
{
	public class HexObjectDisplayController : MonoBehaviour
	{
		[SerializeField] private Spawner<HexObjectDisplay> displaySpawner = default;

		[SerializeField] private List<HexObject> hexObjects = default;

		[SerializeField] private PlacementController pc = default;

		[ContextMenu("Generate Displays")]
		public void GenerateDisplays()
		{
			displaySpawner.DeactivateAll();
			foreach (var hexObject in hexObjects)
			{
				displaySpawner.Spawn().Setup(hexObject, this);
			}
		}

		public void OnDisplayClicked(HexObjectDisplay display)
		{
			pc.SetSelectedObject( display.HexObject);
		}
	}
}