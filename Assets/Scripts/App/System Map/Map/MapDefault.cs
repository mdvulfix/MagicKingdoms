using System;
using UnityEngine;
using URandom = UnityEngine.Random;


using Core;
using Core.Map;
using System.Collections.Generic;

namespace App.Map
{

    [Serializable]
    [ExecuteAlways]
    [RequireComponent(typeof(MeshRenderer))]
    public class MapDefault : MapModel
    {

        [Header("Terrain")]

        [SerializeField] private int m_Width;
        [SerializeField] private int m_Length;
        [SerializeField] private float m_WidthOffset;
        [SerializeField] private float m_LengthOffset;

        private INoise m_Noise;

        [SerializeField] private int m_Seed;
        [SerializeField] private float m_Scale;
        [SerializeField] private int m_Octaves;
        [SerializeField] private float m_Persistence;
        [SerializeField] private float m_Lacunarity;

        [SerializeField] private AnimationCurve m_Curve;
        [SerializeField] private float m_HeightFactor;
        [SerializeField] private float m_FalloffStrength;

        private RegionInfo[] m_Regions;
        [SerializeField] private Color m_Water;
        [SerializeField] private Color m_Sand;
        [SerializeField] private Color m_Grass;
        [SerializeField] private Color m_Rock;
        [SerializeField] private Color m_Ice;


        private MeshFilter m_MeshFilter;
        private MeshRenderer m_MeshRenderer;
        private Texture2D m_Texture;


        private MapConfig m_Config;

        public override void Init(params object[] args)
        {

            m_MeshFilter = GetComponent<MeshFilter>();
            m_MeshRenderer = GetComponent<MeshRenderer>();

            // DEFAULT MAP SETTINGS //
            m_Width = 100;
            m_Length = 100;

            m_Noise = new Simplex();

            m_Scale = 20f;
            m_Octaves = 4;
            m_Persistence = 0.5f;
            m_Lacunarity = 2f;

            var keys = new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) };
            m_Curve = new AnimationCurve(keys);

            m_HeightFactor = 3;

            m_Regions = new RegionInfo[]
            {
                new RegionInfo("Water", 0.4f, m_Water),
                new RegionInfo("Sand", 0.5f, m_Sand),
                new RegionInfo("Grass", 0.8f, m_Grass),
                new RegionInfo("Rock", 0.9f, m_Rock),
                new RegionInfo("Ice", 1.0f, m_Ice)
            };


            // CONFIG MAP SETTINGS //

            foreach (var arg in args)
            {
                if (arg is MapConfig)
                {
                    m_Config = (MapConfig)arg;


                    m_Width = m_Config.Width;
                    m_Length = m_Config.Length;
                    m_WidthOffset = m_Config.WidthOffset;
                    m_LengthOffset = m_Config.LengthOffset;

                    m_Noise = m_Config.Noise;

                    m_Scale = m_Config.Scale;
                    m_Seed = m_Config.Seed;
                    m_Octaves = m_Config.Octaves;
                    m_Persistence = m_Config.Persistence;
                    m_Lacunarity = m_Config.Lacunarity;

                    m_HeightFactor = m_Config.HeightFactor;
                    m_Curve = m_Config.Curve;


                    break;
                }
            }
        }


        public override void DisplayNoiseMap()
        {
            m_MeshRenderer.sharedMaterial.mainTexture = GetNoiseMap();
            transform.localScale = new Vector3(transform.localScale.x, m_HeightFactor / 2, transform.localScale.z);
            m_MeshRenderer.enabled = true;
        }

        public override void DisplayColorMap()
        {
            m_MeshRenderer.sharedMaterial.mainTexture = GetColorMap();
            transform.localScale = new Vector3(transform.localScale.x, m_HeightFactor / 2, transform.localScale.z);
            m_MeshRenderer.enabled = true;
        }

        public override void DisplayMesh()
        {
            m_MeshRenderer.enabled = true;

            var heightMap = GetHeightMap();

            var mesh = MeshHandler.CreateMesh(heightMap, m_Curve, m_HeightFactor);
            m_MeshFilter.sharedMesh?.Clear();
            m_MeshFilter.sharedMesh = mesh;


            DisplayColorMap();

        }

        public override void Close()
        {
            gameObject.SetActive(false);
        }


        private float[,] GetHeightMap()
            => m_Noise.GetHeightMap2D(m_Width,
                                      m_Length,
                                      m_WidthOffset,
                                      m_LengthOffset,
                                      m_Scale,
                                      m_Seed,
                                      m_Octaves,
                                      m_Persistence,
                                      m_Lacunarity);

        private float[,] GetFalloffMap()
        {
            float[,] map = new float[m_Width, m_Length];

            for (int y = 0; y < m_Length; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    var normalizedX = (x / (float)m_Width) * 2 - 1;
                    var normalizedY = (y / (float)m_Length) * 2 - 1;
                    var heght = Mathf.Max(Mathf.Abs(normalizedX), Mathf.Abs(normalizedY));
                    heght = Mathf.Pow(heght, m_FalloffStrength);
                    heght = 1 - heght;

                    map[x, y] = heght;
                }
            }

            return map;
        }

        private Texture2D GetNoiseMap()
        {
            var heightMap = GetHeightMap();
            var falloffMap = GetFalloffMap();
            var resultMap = new float[m_Width, m_Length];
            var map = new Color[m_Width * m_Length];

            for (int y = 0; y < m_Length; y++)
                for (int x = 0; x < m_Width; x++)
                    resultMap[x, y] = heightMap[x, y] * falloffMap[x, y];

            for (int y = 0; y < m_Length; y++)
                for (int x = 0; x < m_Width; x++)
                    map[y * m_Width + x] = Color.Lerp(Color.black, Color.white, resultMap[x, y]);


            var texture = new Texture2D(m_Width, m_Length);
            texture.SetPixels(map);
            texture.Apply();
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            return texture;
        }

        private Texture2D GetColorMap()
        {
            var heightMap = GetHeightMap();
            var falloffMap = GetFalloffMap();
            var resultMap = new float[m_Width, m_Length];
            var map = new Color[m_Width * m_Length];

            for (int y = 0; y < m_Length; y++)
                for (int x = 0; x < m_Width; x++)
                    resultMap[x, y] = heightMap[x, y] * falloffMap[x, y];

            for (int y = 0; y < m_Length; y++)
                for (int x = 0; x < m_Width; x++)
                    foreach (var region in m_Regions)
                        if (resultMap[x, y] <= region.Height) { map[y * m_Width + x] = region.Color; break; }


            var texture = new Texture2D(m_Width, m_Length);
            texture.SetPixels(map);
            texture.Apply();
            texture.filterMode = FilterMode.Point;
            texture.wrapMode = TextureWrapMode.Clamp;

            return texture;

        }


    }

    [Serializable]
    public struct RegionInfo
    {
        public string Label;
        public float Height;
        public Color Color;

        public RegionInfo(string label, float height, Color color)
        {
            Label = label;
            Height = height;
            Color = color;
        }
    }
}