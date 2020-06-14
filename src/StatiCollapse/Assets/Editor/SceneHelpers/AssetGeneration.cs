using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SceneHelper : EditorWindow
{
    // Cube generation
    public GameObject cubeParent;
    Vector3 dimensions;
    Vector3 coords;
    int count;
    float seed = 100.0f;
    private float margin = 0.01f;
    private float scale = 1.0f;
    private Material material;

    // Noise and Randomization
    Texture2D coordTexture;

    [InitializeOnLoadMethod]
    static void Init()
    {
        Debug.Log("INIT");
    }

    [MenuItem("Tools/Generate_Assets")]
    static void InitWindow()
    {
        Debug.Log("Init window");
        SceneHelper window = (SceneHelper)EditorWindow.GetWindow(typeof(SceneHelper));
        window.Show();
    }
    static void CloseWindow()
    {
        SceneHelper window = (SceneHelper)EditorWindow.GetWindow(typeof(SceneHelper));
        window.Close();
    }

    void OnGUI()
    {
        // NEW SECTION
        // ---------------
        EditorGUILayout.Space();
        GUILayout.Label("-------------------------------------------------------------------", EditorStyles.label);
        GUILayout.Label("Generating Cubes", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        dimensions = EditorGUILayout.Vector3Field("Generate an amount of cubes with the following properties:", dimensions);
        count = EditorGUILayout.IntField("count:", count);
        coords = EditorGUILayout.Vector3Field("Coords [Optional]", coords); // default to 0,0,0
        material = (Material)EditorGUILayout.ObjectField("Material for cubes [Optional]", material, typeof(Material), true);
        cubeParent = (GameObject)EditorGUILayout.ObjectField("Parent of Cubes [Optional]", cubeParent, typeof(GameObject), true);


        if (GUILayout.Button("Generate"))
        {
            // Remove all existing objects
            if (cubeParent != null) {
                while (cubeParent.transform.childCount != 0) {
                    Transform child = cubeParent.transform.GetChild(0);
                    DestroyImmediate(child.gameObject);
                }                
            }

            // Create cubes
            for (int i = 0; i < count; i++) {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.name = "Cube" + i;
                cube.transform.position = new Vector3(coords.x + ((i) * dimensions.x) + margin, coords.y, coords.z + ((i % 2) * dimensions.z) + margin);
                cube.transform.localScale = dimensions;
                if (material != null) {
                    cube.GetComponent<MeshRenderer>().material = material;
                }

                if (cubeParent != null) {
                    cube.transform.parent = cubeParent.transform;
                }
            }
        }

        // NEW SECTION
        // ---------------
        EditorGUILayout.Space();
        GUILayout.Label("-------------------------------------------------------------------", EditorStyles.label);
        GUILayout.Label("Testing Perlin noise modulation", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // coordTexture = (Texture2D)EditorGUILayout.ObjectField("Texture to use for noise", coordTexture, typeof(Texture2D), true);
        scale = EditorGUILayout.FloatField("Scale", scale);
        if (GUILayout.Button("Moderate noise")) {
            generateNoiseSample(coordTexture);
        }
    }

    private void generateNoiseSample(Texture2D texture)
    {
        int i = 0;
        Vector3 loc = new Vector3();
        // int x = 0, y = 0;
        while (i < cubeParent.transform.childCount) {
            for (float y = 0; y < 100; y++)
            {
                for (float x = 0; x < 100; x++)
                {
                    if (cubeParent.transform.childCount > i) {
                        Transform child = cubeParent.transform.GetChild(i);
                        if (child)
                        {
                            float seedx = 2 / (seed + x);
                            float seedy = 2 / (seed + y);

                            int row = (i % 2) * (i / 2);
                            int column = (i % 3) * (i / 3);

                            float sample = Mathf.PerlinNoise(seedx, seedy);

                            float width = child.localScale.x;
                            float height = child.localScale.y;

                            float horiz = loc.x + ((width + (sample * scale)) * pickRandomOp());
                            float vert = loc.y + ((height + (sample * scale)) * pickRandomOp());
                            float z = 0;

                            if (horiz == 0 || vert == 0) {
                                Debug.LogWarning("something went wrong");
                            }

                            Debug.Log($"[{horiz}, {vert}, {z}]");                            
                            child.localPosition = new Vector3(horiz, vert, z);
                            // Debug.Log("Set position to: " + cubeParent.transform.GetChild(i).localPosition);
                            i++;
                            loc = child.localPosition;
                        }
                        else
                        {
                            Debug.Log("Did not find child at " + x + ", " + y);
                        }
                    }
                }
            }
        }
        Debug.Log("Noise generated");
    }

    float pickRandomOp() {
        float val = Random.value;
        if (val < 0.5) {
            return 1.0f;
        }
        return -1.0f;
    }
}
