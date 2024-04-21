using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseType 
{
    [System.Serializable]
    public struct TerrainType 
    {
        public string name;
        public float height;
        public Color colour;
    }

    [System.Serializable]
    public struct DecorType
    {
        public string name;
        //public TerrainType region;
        public GameObject prefab;

    }
    
}
