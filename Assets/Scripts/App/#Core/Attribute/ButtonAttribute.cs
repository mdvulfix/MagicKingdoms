using System;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class BtnAttribute : Attribute
    {

        public class DrawerAttribute : PropertyDrawer
        {


        }





        public BtnAttribute()
        {

        }






    }


    [CustomPropertyDrawer(typeof(BtnAttribute))]
    public class BtnAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //string methodName = (attribute as ButtonAttribute).MethodName;

            var target = property.serializedObject.targetObject;

            var methods = target
                .GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(BtnAttribute), false).Length > 0)
                .ToArray();

            if (methods.Length == 0)
            {
                GUI.Label(position, "Methods could not be found. Is it public?");
                return;
            }


            foreach (var method in methods)
            {
                if (method.GetParameters().Length > 0)
                {
                    GUI.Label(position, "Method cannot have parameters.");
                    return;
                }

                if (GUI.Button(position, method.Name))
                {
                    method.Invoke(target, null);
                }
            }
        }
    }














    public class ButtonAttribute : PropertyAttribute
    {
        public ButtonAttribute()
        {

        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //string methodName = (attribute as ButtonAttribute).MethodName;

            var target = property.serializedObject.targetObject;

            var methods = target
                .GetType()
                .GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(ButtonAttribute), false).Length > 0)
                .ToArray();

            if (methods.Length == 0)
            {
                GUI.Label(position, "Methods could not be found. Is it public?");
                return;
            }


            foreach (var method in methods)
            {
                if (method.GetParameters().Length > 0)
                {
                    GUI.Label(position, "Method cannot have parameters.");
                    return;
                }

                if (GUI.Button(position, method.Name))
                {
                    method.Invoke(target, null);
                }
            }
        }
    }

#endif

}

