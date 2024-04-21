using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (MapGenerator_2))]
public class MapGeneratorEditor_2 : Editor {

    public override void OnInspectorGUI() {
        MapGenerator_2 mapGen = (MapGenerator_2)target;

        if (DrawDefaultInspector ()) {
            if (mapGen.autoUpdate) {
                mapGen.DrawMapInEditor ();
            }
        }

        if (GUILayout.Button ("Generate")) {
            mapGen.DrawMapInEditor ();
        }
    }
}