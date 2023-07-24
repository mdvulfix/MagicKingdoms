using System;
using System.Reflection;
using UnityEngine;

namespace Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute
    {
        public ButtonAttribute()
        {

        }

        public override void Draw(object instance)
        {
            GUILayout.Label("Button attribute drawer... ");

        }

    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CachedAttribute : DrawerAttribute
    {
        public CachedAttribute()
        {

        }


        public override void Draw(object instance)
        {

            GUILayout.Label("Cached attribute drawer... ");

        }



    }




    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CustomInspectorDrawerAttribute : DrawerAttribute
    {
        public CustomInspectorDrawerAttribute()
        {

        }
    }


    public abstract class DrawerAttribute : Attribute
    {
        public virtual void Draw(object instance) { }
    }

}