using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HexagonPackage
{
    public static class Extensions
    {
        public static T Random<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default;
            }
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}
