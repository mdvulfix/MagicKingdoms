using System;
using UnityEngine;

namespace Core.Map
{
    public abstract class MapModel : MonoBehaviour
    {

        public bool isInitialized => m_isInitialized;

        private bool m_isInitialized;



        public virtual void Init(params object[] args)
            => m_isInitialized = true;

        public abstract void Display(MapDisplayMode mode);
        public abstract void Close();


    }

    public enum MapDisplayMode
    {
        None,
        Noise,
        Color,
        Terrain
    }

    public struct MapConfig
    {
        public INoise Noise { get; private set; }

        public Vector2Int Size { get; internal set; }
        public Vector2 Offset { get; internal set; }
        public float Scale { get; private set; }

        public int Seed { get; private set; }
        public int Octaves { get; private set; }
        public float Persistence { get; private set; }
        public float Lacunarity { get; private set; }


        public MapConfig(INoise noise, Vector2Int size, Vector2 offset, float scale, int seed, int octaves, float persistence, float lacunarity)
        {
            Noise = noise;
            Size = size;
            Offset = offset;
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