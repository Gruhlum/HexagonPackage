using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexagonPackage.HexObjects
{
    public interface IResourceCollection
    {
        public bool HasResource(IntValue value);
        public bool IsFull(ValType type);
        public int GetAmount(ValType type);
        public void ChangeResource(ValType type, int amount);
        //public void ChangeResource(IntValue value)
        //{
        //    ChangeResource(value.Type, value.Value);
        //}
        public void ChangeMaxValue(ValType type, int change);
    }
}