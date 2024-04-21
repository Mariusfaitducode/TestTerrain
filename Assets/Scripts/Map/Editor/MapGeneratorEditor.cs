using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI(){
        MapGenerator mapGen = (MapGenerator)target;
        //PlateauJeu plateau = (PlateauJeu) target;

        if (DrawDefaultInspector ()){
            //mapGen.GenerateMap();
            //mapGen.Generate3dMap();
        }

        if (GUILayout.Button ("Generate")){
            mapGen.GenerateMap ();
            //mapGen.Generate3dMap();
        }
    }
}
