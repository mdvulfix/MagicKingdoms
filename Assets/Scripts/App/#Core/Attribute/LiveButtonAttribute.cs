using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

#endif



public class CustomButtonAttribute : PropertyAttribute
{
    Type Type { get; }

    public CustomButtonAttribute(Type type)
    {
        Type = type;
    }
}




public class ObjButtonAttribute : PropertyAttribute
{
    Type Type { get; }

    public ObjButtonAttribute(Type type)
    {
        Type = type;
    }
}

public interface IContext
{
    object Instance { get; }
}


/*

public static class DecoratorDrawerExtansion
{

    public static object Instance { get; }

    [CustomEditor(typeof(MapController))]
    private class EditorDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var method = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                             .Where(x => x.GetCustomAttributes(typeof(ObjButtonAttribute), false).FirstOrDefault() != null)
                             .First();

            if (GUILayout.Button(method.Name))
                method.Invoke(target, null);










        }








        public override void OnGUI(Rect position)
        {
            //string methodName = (attribute as LiveButtonAttribute).MethodName;
            var target = Obj.targetObject;

            var method = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                            .Where(x => x.GetCustomAttributes(typeof(CustomAttribute), false).FirstOrDefault() != null)
                            .First();



            if (method == null)
            {
                GUI.Label(position, "Method could not be found. Is it public?");
                return;
            }
            if (method.GetParameters().Length > 0)
            {
                GUI.Label(position, "Method cannot have parameters.");
                return;
            }
            if (GUI.Button(position, method.Name))
                method.Invoke(target, null);

        }

    }






    public static void OnGUI(this DecoratorDrawer drawer, Rect position, SerializedObject obj)
    {





    }

}



*/

/*
public static class EditorExtansion
{

    [CustomPropertyDrawer(typeof(CustomButtonAttribute))]
    private class EditorDrawer : DecoratorDrawer
    {
        public SerializedObject Obj { get; }

        public EditorDrawer(SerializedObject obj)
        {
            Obj = obj;
        }

        public override void OnGUI(Rect position)
        {
            //string methodName = (attribute as LiveButtonAttribute).MethodName;
            var target = Obj.targetObject;

            var method = null //target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
            //                .Where(x => x.GetCustomAttributes(typeof(CustomAttribute), false).FirstOrDefault() != null)
            //                .First();



            if (method == null)
            {
                GUI.Label(position, "Method could not be found. Is it public?");
                return;
            }
            if (method.GetParameters().Length > 0)
            {
                GUI.Label(position, "Method cannot have parameters.");
                return;
            }
            if (GUI.Button(position, method.Name))
                method.Invoke(target, null);

        }

    }


    public static void OnGUI(this Editor editor)
    {
        var drawer = new EditorDrawer(editor.serializedObject);




    }



}
*/


[CustomPropertyDrawer(typeof(ObjButtonAttribute))]
public class ObjButtonAttributeDrawer : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //IEnumerable<MethodInfo> methods = target.GetType()
        //    .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        //    .Where(m => m.GetParameters().Length == 0);


        var method = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
                         .Where(x => x.GetCustomAttributes(typeof(ObjButtonAttribute), false).FirstOrDefault() != null)
                         .First();

        if (GUILayout.Button(method.Name))
            method.Invoke(target, null);

    }





    /*
    public override void OnGUI(Rect position)
    {
        var target = pro

        var method = target.GetType().GetMethods()
                         .Where(x => x.GetCustomAttributes(typeof(ObjButtonAttribute), false).FirstOrDefault() != null)
                         .First();


        if (method == null)
        {
            GUI.Label(position, "Method could not be found. Is it public?");
            return;
        }
        if (method.GetParameters().Length > 0)
        {
            GUI.Label(position, "Method cannot have parameters.");
            return;
        }

        if (GUI.Button(position, method.Name))
            method.Invoke(target, null);


    }

    public static MethodInfo GetMetods(Type type)
    {
        //var assembly = Assembly.Load(assemblyName);
        var method = type.GetMethods()
                         .Where(x => x.GetCustomAttributes(typeof(LiveButtonAttribute), false).FirstOrDefault() != null)
                         .First();


        //var types = assembly.GetTypes().Where(x => x.IsClass);
        //var methods = types.SelectMany(m => m.GetMethods());
        //var methodsWithAttr = types.SelectMany(t => t.GetMethods()).Where(x => x.GetCustomAttributes(typeof(LiveButtonAttribute), false).FirstOrDefault() != null);
        //var methodsArr = methodsWithAttr.Length > 0 ? methodsWithAttr.ToArray() : throw new Exception();


        return method;
    }
    */

}




public class LiveButtonAttribute : PropertyAttribute
{
    public string MethodName { get; }

    public LiveButtonAttribute(string methodName)
    {
        MethodName = methodName;
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(LiveButtonAttribute))]
public class LiveButtonAttributeDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string methodName = (attribute as LiveButtonAttribute).MethodName;
        object target = property.serializedObject.targetObject;
        System.Type type = target.GetType();
        System.Reflection.MethodInfo method = type.GetMethod(methodName);


        if (method == null)
        {
            GUI.Label(position, "Method could not be found. Is it public?");
            return;
        }
        if (method.GetParameters().Length > 0)
        {
            GUI.Label(position, "Method cannot have parameters.");
            return;
        }
        if (GUI.Button(position, method.Name))
            method.Invoke(target, null);

    }

    public static MethodInfo GetMetods(Type type)
    {
        //var assembly = Assembly.Load(assemblyName);
        var method = type.GetMethods()
                         .Where(x => x.GetCustomAttributes(typeof(LiveButtonAttribute), false).FirstOrDefault() != null)
                         .First();


        //var types = assembly.GetTypes().Where(x => x.IsClass);
        //var methods = types.SelectMany(m => m.GetMethods());
        //var methodsWithAttr = types.SelectMany(t => t.GetMethods()).Where(x => x.GetCustomAttributes(typeof(LiveButtonAttribute), false).FirstOrDefault() != null);
        //var methodsArr = methodsWithAttr.Length > 0 ? methodsWithAttr.ToArray() : throw new Exception();


        return method;
    }


}
#endif


