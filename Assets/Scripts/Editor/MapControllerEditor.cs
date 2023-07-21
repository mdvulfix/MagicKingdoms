using UnityEngine;
using System;
using System.Linq;

using App.Map;


#if UNITY_EDITOR
using UnityEditor;



/*

[CustomEditor(typeof(MapController))]
public class MapControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var controller = (MapController)target;


        //IEnumerable<MethodInfo> methods = target.GetType()
        //    .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        //    .Where(m => m.GetParameters().Length == 0);


        //var method = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        //                 .Where(x => x.GetCustomAttributes(typeof(ObjButtonAttribute), false).FirstOrDefault() != null)
        //                 .First();

        if (GUILayout.Button("Generate"))
            controller.Generate();

    }



}
*/

#endif


