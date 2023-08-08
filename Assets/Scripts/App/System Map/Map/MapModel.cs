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

        public abstract void DisplayNoiseMap();
        public abstract void DisplayColorMap();
        public abstract void DisplayMesh();
        public abstract void Close();


    }

    public enum MapDisplayMode
    {
        None,
        Noise,
        Color,
        Mesh
    }

    public struct MapConfig
    {
        public MapConfig(int width, int length, float widthOffset, float lengthOffset, INoise noise, int seed, float scale, int octaves, float persistence, float lacunarity, float heightFactor, AnimationCurve curve)
        {
            Width = width;
            Length = length;
            WidthOffset = widthOffset;
            LengthOffset = lengthOffset;
            Noise = noise;
            Seed = seed;
            Scale = scale;
            Octaves = octaves;
            Persistence = persistence;
            Lacunarity = lacunarity;
            HeightFactor = heightFactor;
            Curve = curve;
        }

        public int Width { get; private set; }
        public int Length { get; private set; }
        public float WidthOffset { get; private set; }
        public float LengthOffset { get; private set; }

        public INoise Noise { get; private set; }

        public int Seed { get; private set; }
        public float Scale { get; private set; }
        public int Octaves { get; private set; }
        public float Persistence { get; private set; }
        public float Lacunarity { get; private set; }

        public float HeightFactor { get; private set; }
        public AnimationCurve Curve { get; private set; }

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