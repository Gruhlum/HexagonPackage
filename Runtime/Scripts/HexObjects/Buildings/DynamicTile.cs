using HexagonPackage.HexagonComponents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects
{
    public class DynamicTile : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr = default;
        [SerializeField] private ParticleSystem ps = default;

        public void Setup(Sprite sprite, Vector3 position, int rotation, bool flip, int sortingOrder)
        {
            sr.sprite = sprite;
            transform.localPosition = position;
            transform.localEulerAngles = new Vector3(0, 0, 60 * rotation);
            sr.flipY = flip;
            sr.sortingOrder = sortingOrder;
        }
        public void Setup(DynamicBuilding.DynamicInfo info)
        {
            Setup(info.Sprite, info.Position, info.Rotation, info.Flip, info.SortingOrder);
        }
        public void SetColor(Color col)
        {
            sr.color = col;
            if (ps != null)
            {
                var module = ps.main;
                module.startColor = col;
            }
        }
        public void SetSortingOrder(int value)
        {
            sr.sortingOrder = value;
        }

        public void ActivateParticleSystem(bool active)
        {
            if (active)
            {
                ps.Play();
            }
            else ps.Stop();
        }
        public void BurstParticleSystem(int particles)
        {
            ps.Emit(particles);
        }
    }
}