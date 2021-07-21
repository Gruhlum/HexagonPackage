using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HexagonPackage.HexagonComponents
{
    public class Rotation : MonoBehaviour
    {
        public int RotationTicks
        {
            get
            {
                return rotationTicks;
            }
            set
            {
                if (value % rotationSpeed != 0)
                {
                    Debug.LogWarning("rotationTicks is not divisible by rotationSpeed: " + rotationSpeed + " Ticks: " + value);
                    return;
                }
                rotationTicks = value;
            }
        }
        private int rotationTicks = 0;

        private int rotationSpeed = 4;

        public UnityEvent RotationFinished;

        private void FixedUpdate()
        {
            if (RotationTicks != 0)
            {
                //if (RotationTicks % 30 == 0)
                //{
                //    if (RotationClip.AudioClip != null)
                //    {
                //        RotationClip.PlaySound();
                //    }
                //    else RotationClip.RequestSound(true);
                //}

                //image.sortingOrder = 2;
                if (RotationTicks % rotationSpeed != 0)
                {
                    Debug.LogWarning("rotationTicks is not divisible by rotationSpeed: " + rotationSpeed);
                }
                if (RotationTicks > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotationSpeed);
                    RotationTicks -= rotationSpeed;
                }
                else if (RotationTicks < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - rotationSpeed);
                    RotationTicks += rotationSpeed;
                }
                if (RotationTicks == 0 && Mathf.RoundToInt(transform.eulerAngles.z) % 360 == 0)
                {
                    RotationFinished.Invoke();
                }
            }
        }
    }
}