using System;
using System.Reflection;
using System.Linq;

using UnityEngine;
using Core;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true)]
public class EditorDrawer : Editor
{
    private Attribute[] m_Attributes;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var attr in m_Attributes)
            if (attr is DrawerAttribute)
                ((DrawerAttribute)attr).Draw(target);


    }


    private void OnEnable()
    {
        m_Attributes = GetAtributes();

    }

    private Attribute[] GetAtributes()
    {
        var attr = target.GetType()
                         .GetCustomAttributes()
                         .Where(a => a is DrawerAttribute);

        var attrMethod = target.GetType()
                               .GetMethods()
                               .SelectMany(m => m.GetCustomAttributes())
                               .Where(a => a is DrawerAttribute);

        return attr.Concat(attrMethod).ToArray();

    }
}

public struct AttributeInfo
{
    public object Instance { get; private set; }
    public Attribute Attribute { get; private set; }
    public Action Action { get; private set; }

    public AttributeInfo(object instance, Attribute attribute, Action action)
    {
        Instance = instance;
        Attribute = attribute;
        Action = action;
    }
}

#endif