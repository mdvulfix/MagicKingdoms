using System;
using System.Reflection;
using UnityEngine;

namespace Core
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute
    {
        
        private string m_FuncName;
        
        public ButtonAttribute(string funcName)
        {
            m_FuncName = funcName;
        }

        public ButtonAttribute(string funcName, object instance, MethodInfo func)
        {
            m_FuncName = funcName;
            Instance = instance;
            Func = func;

        }


        public override DrawerAttribute Draw()
        {
            GUILayout.Label("Custom attribute drawer");
            
            if (GUILayout.Button(m_FuncName))
            {
                if (Instance == null)
                    throw new Exception("Target instance not set!");

                Func?.Invoke(Instance, null);

            }

            return this;

        }




    }

    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class CachedAttribute : DrawerAttribute
    {
        public CachedAttribute()
        {

        }

        public CachedAttribute(object instance)
        {
            Instance = instance;
        }




        public override DrawerAttribute Draw()
        {
            GUILayout.Label("Cached attribute drawer... ");
            return this;
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

        public virtual object Instance { get; set; }
        public virtual MethodInfo Func { get; set; }


        public virtual DrawerAttribute Draw() { return this; }



    }

    public delegate void Click(object instance, params object[] args);

}