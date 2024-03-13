using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage
{
	public class CameraController : MonoBehaviour
	{
        public int ScrollSpeed = 50;
        public int MinDistance = 1;
        Camera mainCamera;
        private void Awake()
        {
            mainCamera = Camera.main;
        }
        private void Update()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if (mainCamera.orthographic)
                {
                    if (Input.mouseScrollDelta.y > 0 && mainCamera.orthographicSize <= MinDistance)
                    {
                        return;
                    }
                    else mainCamera.orthographicSize -= Input.mouseScrollDelta.y * ScrollSpeed;
                }
                else mainCamera.transform.position -= new Vector3(0, 0, Input.mouseScrollDelta.y * ScrollSpeed);
            }
        }
    }
}