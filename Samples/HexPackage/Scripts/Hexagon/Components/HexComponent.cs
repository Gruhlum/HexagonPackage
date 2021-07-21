using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HexagonPackage
{
    [RequireComponent(typeof(Hexagon))]
    public class HexComponent : MonoBehaviour
    {
        [SerializeField][HideInInspector] protected Hexagon hexagon = default;

        protected virtual void Reset()
        {
            hexagon = GetComponent<Hexagon>();
            hexagon.AddHexComponent(this);
        }
        protected virtual void OnDestroy()
        {
            hexagon.RemoveComponent(this);
        }
    }
}