using System;
using UnityEngine;
using System.Reflection;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

namespace Core
{

    public class FieldAttribute : PropertyAttribute
    {
        private Type m_Type;

        public FieldAttribute(Type type)
        {
            m_Type = type;



            foreach (Type mytype in Assembly.GetExecutingAssembly().GetTypes()
                            .Where(mytype => mytype.GetInterfaces()
                .Contains(typeof(IInvokable))))
            {
                mytype.GetMethod("Invoke").Invoke(mytype, null);
            }

        }


    }

    public interface IInvokable
    {
        void Invoke();
    }

    [CustomPropertyDrawer(typeof(FieldAttribute))]
    public class FieldAttributeDrawer : PropertyDrawer
    {







    }
}

#endif
