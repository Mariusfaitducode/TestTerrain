using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneTemplate;
using UnityEditor.UIElements;
using UnityEngine;

public class Case
{
    public GameObject caseCube;

    public CaseType.TerrainType typeRegion;
    //private int tabX;
    //private int tabY;

    public Vector2 tabRef;
    private Vector3 position;

    public float surfaceHeight;

    public float altitude = 60f; // dénivelé

    public bool bio;
    
    //private GameObject personnage;

    public Case( CaseType.TerrainType region, float currentHeight, int x, int y, GameObject obj, bool sortie, int level)
    {
        if (level == 0)
        {
            CaseWorld_0(region, x, y, obj, sortie, level);
        }
        else if (level == 1)
        {
            CaseWorld_1(region, x, y, obj, sortie, level);
        }
        else if (level == 2)
        {
            CaseWorld_2(region, currentHeight, x, y, obj, sortie, level);
        }


    }

    public void CaseWorld_0(CaseType.TerrainType region, int x, int y, GameObject obj, bool sortie, int level)
    {
        typeRegion = region;
        
        caseCube = obj;

        tabRef.x = x;
        tabRef.y = y;

        //float height;  //Hauteur de la surface du cube
        //float height = region.height * altitude;

        if (typeRegion.name.Equals("Water"))
        {
            //surfaceHeight = 0.4f * Constants.altitude;
            surfaceHeight = region.height * altitude ;
        }
        else
        {
            //surfaceHeight = currentHeight * Constants.altitude;
            surfaceHeight = region.height * altitude;
        }

        float size = Constants.GetConstant(level);

        position = new Vector3();
        position.x = (tabRef.x * Constants.CaseSize) - size * Constants.CaseSize / 2 + Constants.CaseSize / 2;
        position.y = surfaceHeight / 2;
        position.z = (tabRef.y * Constants.CaseSize) - size * Constants.CaseSize / 2 + Constants.CaseSize / 2;

        caseCube.transform.position = position;

        caseCube.transform.localScale = new Vector3(Constants.CaseSize, surfaceHeight, Constants.CaseSize);

        if (!sortie)
        {
            obj.GetComponent<MeshRenderer>().material.color = region.colour;
        }
        else
        {
            obj.GetComponent<MeshRenderer>().material.color = Color.yellow;
            obj.tag = String.Copy("Sortie");
            
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<SortieCube>().enabled = true;
        }
        

        obj.GetComponent<BlockCase>().tabRef = tabRef;
    }
    
    public void CaseWorld_1(CaseType.TerrainType region, int x, int y, GameObject obj, bool sortie, int level)
    {
        bio = false;
        
        typeRegion = region;
        
        caseCube = obj;

        tabRef.x = x;
        tabRef.y = y;

        //float height;  //Hauteur de la surface du cube
        //float height = region.height * altitude;

        if (typeRegion.name.Equals("Water"))
        {
            //surfaceHeight = 0.4f * Constants.altitude;
            surfaceHeight = altitude ;
        }
        else
        {
            //surfaceHeight = currentHeight * Constants.altitude;
            surfaceHeight = altitude;
        }

        float size = Constants.GetConstant(level);

        position = new Vector3();
        position.x = (tabRef.x * Constants.CaseSize) - size * Constants.CaseSize / 2 + Constants.CaseSize / 2;
        position.y = surfaceHeight / 2;
        position.z = (tabRef.y * Constants.CaseSize) - size * Constants.CaseSize / 2 + Constants.CaseSize / 2;

        caseCube.transform.position = position;

        caseCube.transform.localScale = new Vector3(Constants.CaseSize, surfaceHeight, Constants.CaseSize);

        if (!sortie)
        {
            obj.GetComponent<MeshRenderer>().material.color = region.colour;
        }
        else
        {
            obj.GetComponent<MeshRenderer>().material.color = Color.yellow;
            obj.tag = String.Copy("Sortie");
            
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<SortieCube>().enabled = true;
        }
        

        obj.GetComponent<BlockCase>().tabRef = tabRef;
    }
    
    
    public void CaseWorld_2(CaseType.TerrainType region, float currentHeight, int x, int y, GameObject obj, bool sortie, int level)
    {
        typeRegion = region;
        
        caseCube = obj;

        tabRef.x = x;
        tabRef.y = y;

        //float height;  //Hauteur de la surface du cube
        //float height = region.height * altitude;

        if (typeRegion.name.Equals("Water"))
        {
            //surfaceHeight = 0.4f * Constants.altitude;
            surfaceHeight = currentHeight * altitude ;
        }
        else
        {
            //surfaceHeight = currentHeight * Constants.altitude;
            surfaceHeight = currentHeight * altitude;
        }

        float size = Constants.GetConstant(level);

        position = new Vector3();
        position.x = (tabRef.x * Constants.CaseSize) - size * Constants.CaseSize / 2 + Constants.CaseSize / 2;
        position.y = surfaceHeight / 2;
        position.z = (tabRef.y * Constants.CaseSize) - size * Constants.CaseSize / 2 + Constants.CaseSize / 2;

        caseCube.transform.position = position;

        caseCube.transform.localScale = new Vector3(Constants.CaseSize, surfaceHeight, Constants.CaseSize);

        if (!sortie)
        {
            obj.GetComponent<MeshRenderer>().material.color = region.colour;
        }
        else
        {
            obj.GetComponent<MeshRenderer>().material.color = Color.yellow;
            obj.tag = String.Copy("Sortie");
            
            obj.GetComponent<BoxCollider>().isTrigger = true;
            obj.GetComponent<SortieCube>().enabled = true;
        }
        

        obj.GetComponent<BlockCase>().tabRef = tabRef;
    }
}
