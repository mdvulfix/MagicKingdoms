using UnityEngine;
using App.Map;
using Core.Map;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MapDefault))]
public class EditorMap : Editor
{

    private MapDefault Map => (MapDefault)target;


    public override void OnInspectorGUI()
    {
        var instance = (MapDefault)target;

        var isUpdate = DrawDefaultInspector();

        if (isUpdate)
            Map.DisplayColorMap();

        if (GUILayout.Button("Noise"))
            DisplayNoiseMap();

        if (GUILayout.Button("Color"))
            DisplayColorMap();

        if (GUILayout.Button("Mesh"))
            DisplayMesh();
    }

    private void OnEnable()
    {
        DisplayMesh();
    }

    private void DisplayNoiseMap()
    {
        Map.Init();
        Map.DisplayNoiseMap();
    }

    private void DisplayColorMap()
    {
        Map.Init();
        Map.DisplayColorMap();
    }

    private void DisplayMesh()
    {
        Map.Init();
        Map.DisplayMesh();
    }

}



#endif