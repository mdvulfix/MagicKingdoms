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
            MapDisplay();

        if (GUILayout.Button("Update"))
            MapDisplay();


    }

    private void OnEnable()
    {
        MapDisplay();
    }


    private void MapDisplay()
    {
        Map.Display(MapDisplayMode.Color);
    }

}



#endif