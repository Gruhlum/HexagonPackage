using HexagonPackage.HexObjects;
using HexagonPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildEventArgs
{
    public List<Cube> Tiles;
    public HexObject HexObject;
    public bool Allow;

    public BuildEventArgs(List<Cube> location, HexObject hexObject)
    {
        Tiles = location;
        HexObject = hexObject;
        Allow = true;
    }
}
