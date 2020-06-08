using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AssetGeneration : EditorWindow
{
    // Cube generation
    public GameObject cubeParent;
    Vector3 dimensions;
    Vector3 coords;
    int count;

    [InitializeOnLoadMethod]
    static void Init()
    {

    }

    [MenuItem("Tools/Generate and Modify Scene Assets")]
    static void InitWindow()
    {
        AssetGeneration window = (AssetGeneration)EditorWindow.GetWindow(typeof(AssetGeneration));
    }
    static void CloseWindow()
    {
        AssetGeneration window = (AssetGeneration)EditorWindow.GetWindow(typeof(AssetGeneration));
        window.Close();
    }

    private void OnGUI()
    {
        // GUILayout.Label("", EditorStyles.label);
        dimensions = EditorGUILayout.Vector3Field("Generate an amount of cubes with the following properties:", dimensions);
        count = EditorGUILayout.IntField("count:", count);
        coords = EditorGUILayout.Vector3Field("Coords [Optional]", coords); // default to 0,0,0

        if (GUILayout.Button("Generate")) {
            
        }
    }
}
