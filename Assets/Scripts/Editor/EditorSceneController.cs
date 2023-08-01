using UnityEngine;
using App.Map;

#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(MapController))]
public class EditorSceneController : Editor
{

    private MapController m_Controller => (MapController)target;


    public override void OnInspectorGUI()
    {
        var instance = (MapController)target;

        var isUpdate = DrawDefaultInspector();

        if (m_Controller.AutoUpdate && isUpdate)
            MapDisplay();

        if (GUILayout.Button("Generate"))
            MapDisplay();


    }

    private void OnEnable()
    {
        MapDisplay();
    }


    private void MapDisplay()
    {
        m_Controller.Setup();
        m_Controller.MapDisplay();
    }

}

#endif