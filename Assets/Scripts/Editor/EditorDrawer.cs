using System;
using System.Reflection;
using System.Linq;

using UnityEngine;
using Core;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
public class EditorDrawer : Editor
{
    private Attribute[] m_Attributes;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var attr in GetAtributes())
            attr.Draw();

    }


    private IEnumerable<DrawerAttribute> GetAtributes()
    {
        var classAttr = target.GetType()
                         .GetCustomAttributes()
                         .Where(a => a is DrawerAttribute)
                         .Cast<DrawerAttribute>();

        foreach (var a in classAttr)
            a.Instance = target;


        var methods = target.GetType().GetMethods();
        var attr = classAttr.ToList();

        foreach (var method in methods)
        {
            var methodAttrList = method.GetCustomAttributes()
                                       .Where(a => a is DrawerAttribute)
                                       .Cast<DrawerAttribute>();

            foreach (var a in methodAttrList)
            {
                a.Instance = target;
                a.Func = method;

                attr.Add(a);
            }
        }

        return attr;

    }
}


#endif