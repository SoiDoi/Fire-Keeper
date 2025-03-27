using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor // Use for Add button in Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target; // object for cusstom editor

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpadate)
            {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
