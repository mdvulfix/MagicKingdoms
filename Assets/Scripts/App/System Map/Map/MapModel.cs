using System;
using UnityEngine;

namespace Core.Map
{
    [Serializable]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public abstract class MapModel : MonoBehaviour
    {

        public bool isInitialized => m_isInitialized;


        private bool m_isInitialized;



        public virtual void Init(params object[] args)
            => m_isInitialized = true;


        public abstract void Draw();
        public abstract void Display();
        public abstract void Close();


    }

    public struct MapConfig
    {
        public INoise Noise { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Scale { get; private set; }
        public int Seed { get; private set; }
        public int Octaves { get; private set; }
        public float Persistence { get; private set; }
        public float Lacunarity { get; private set; }


        public MapConfig(INoise noise, int width, int height, int scale, int seed, int octaves, float persistence, float lacunarity)
        {
            Noise = noise;
            Width = width;
            Height = height;
            Scale = scale;
            Seed = seed;
            Octaves = octaves;
            Persistence = persistence;
            Lacunarity = lacunarity;
        }




    }



}


namespace Core
{


    [AttributeUsage(AttributeTargets.Class)]
    public class ModelDrawerAttribute : Attribute
    {
        public Type InstanceType { get; }
        public ModelDrawerAttribute(Type instanceType)
        {
            InstanceType = instanceType;
        }


    }
}