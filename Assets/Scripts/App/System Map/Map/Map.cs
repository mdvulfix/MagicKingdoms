using System;
using UnityEngine;
using URandom = UnityEngine.Random;


using Core;
using Core.Map;
using System.Collections.Generic;

namespace App.Map
{

    [Serializable]
    [RequireComponent(typeof(MeshRenderer))]
    public class Map : MapModel
    {

        public GameObject Obj => gameObject;

        [Header("Terrain")]


        [SerializeField] private MeshFilter m_MapMeshFilter;
        private MeshFilter MapMeshFilter => m_MapMeshFilter != null ? m_MapMeshFilter : Obj.GetComponent<MeshFilter>();


        private Vector3[] m_Vertices;
        private int[] m_Triangles;
        private Vector2[] m_UVs;

        [SerializeField] private RegionInfo[] m_Regions;
        [SerializeField] private Color m_Water = Color.red;
        [SerializeField] private Color m_Sand = Color.red;
        [SerializeField] private Color m_Grass = Color.red;
        [SerializeField] private Color m_Rock = Color.red;
        [SerializeField] private Color m_Ice = Color.red;


        [Header("Noise")]
        [SerializeField] private NoiseModel m_NoiseDefault;
        private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
        private INoise m_Noise;

        [SerializeField] private AnimationCurve m_Affector;
        [SerializeField] private float m_HeightMultiplier;


        private Vector2Int m_Size;
        private Vector2 m_Offset;
        private int m_Seed;
        private float m_Scale;
        private int m_Octaves;
        private float m_Persistence;
        private float m_Lacunarity;

        [Header("Texture")]
        [SerializeField] private MeshRenderer m_MapMeshRenderer;
        private MeshRenderer MapMeshRenderer => m_MapMeshRenderer != null ? m_MapMeshRenderer : Obj.GetComponent<MeshRenderer>();
        private Texture2D m_Texture;


        private MapConfig m_Config;

        public override void Init(params object[] args)
        {
            foreach (var arg in args)
                if (arg is MapConfig)
                { m_Config = (MapConfig)arg; break; }
                else
                    throw new Exception($"{this}: config was not found!");

            m_Noise = m_Config.Noise;

            m_Size = m_Config.Size;
            m_Offset = m_Config.Offset;
            m_Scale = m_Config.Scale;
            m_Seed = m_Config.Seed;
            m_Octaves = m_Config.Octaves;
            m_Persistence = m_Config.Persistence;
            m_Lacunarity = m_Config.Lacunarity;


            m_Regions = new RegionInfo[]
            {
                new RegionInfo("Water", 0.4f, m_Water),
                new RegionInfo("Sand", 0.5f, m_Sand),
                new RegionInfo("Grass", 0.8f, m_Grass),
                new RegionInfo("Rock", 0.9f, m_Rock),
                new RegionInfo("Ice", 0.9f, m_Ice)

            };


        }


        public override void DrawTexture()
        {
            m_Texture = new Texture2D(m_Size.x, m_Size.y);

            var noiseHeights = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
                for (int x = 0; x < m_Size.x; x++)
                    colourMatrix[m_Size.x * m_Size.y] = Color.Lerp(Color.black, Color.white, noiseHeights[x, y]);


            m_Texture.SetPixels(colourMatrix);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;
        }

        public override void Display(MapDisplayMode mode)
        {

            switch (mode)
            {
                case MapDisplayMode.Noise:
                    GenerateNoiseMap();
                    break;

                case MapDisplayMode.Color:
                    GenerateColorMap();
                    break;

                case MapDisplayMode.Terrain:
                    GenerateMesh();
                    break;
            }


        }

        public override void Close()
        {
            Obj.SetActive(false);
        }

        private void GenerateNoiseMap()
        {

            MapMeshRenderer.enabled = true;

            var heightMap = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);

            var mesh = MeshHandler.CreateMesh(heightMap, m_HeightMultiplier);
            MapMeshFilter.sharedMesh?.Clear();
            MapMeshFilter.sharedMesh = mesh;


            m_Texture = new Texture2D(m_Size.x, m_Size.y);
            var colorMap = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
                for (int x = 0; x < m_Size.x; x++)
                    colorMap[y * m_Size.x + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);


            m_Texture.SetPixels(colorMap);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;


            MapMeshRenderer.sharedMaterial.mainTexture = m_Texture;
            //Obj.transform.localScale = new Vector3(m_Size.x, 1, m_Size.y);
            MapMeshRenderer.enabled = true;
        }

        private void GenerateColorMap()
        {

            MapMeshRenderer.enabled = true;

            var heightMap = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);

            var mesh = MeshHandler.CreateMesh(heightMap, m_HeightMultiplier);
            MapMeshFilter.sharedMesh?.Clear();
            MapMeshFilter.sharedMesh = mesh;


            m_Texture = new Texture2D(m_Size.x, m_Size.y);
            var colorMap = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
            {
                for (int x = 0; x < m_Size.x; x++)
                {
                    foreach (var r in m_Regions)
                    {
                        if (heightMap[x, y] <= r.Height)
                        {
                            colorMap[y * m_Size.x + x] = r.Color;
                            break;
                        }
                    }
                }
            }


            m_Texture.SetPixels(colorMap);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;


            MapMeshRenderer.sharedMaterial.mainTexture = m_Texture;
            //Obj.transform.localScale = new Vector3(m_Size.x, 1, m_Size.y);
            MapMeshRenderer.enabled = true;
        }

        private void GenerateMesh()
        {
            MapMeshRenderer.enabled = true;

            var heightMap = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);

            var mesh = MeshHandler.CreateMesh(heightMap, m_HeightMultiplier);
            MapMeshFilter.sharedMesh?.Clear();
            MapMeshFilter.sharedMesh = mesh;


            m_Texture = new Texture2D(m_Size.x, m_Size.y);
            var colorMap = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
                for (int x = 0; x < m_Size.x; x++)
                    colorMap[y * m_Size.x + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);


            m_Texture.SetPixels(colorMap);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;


            MapMeshRenderer.sharedMaterial.mainTexture = m_Texture;
            //Obj.transform.localScale = new Vector3(m_Size.x, 1, m_Size.y);
            MapMeshRenderer.enabled = true;

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