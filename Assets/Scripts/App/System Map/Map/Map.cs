using System;
using UnityEngine;
using URandom = UnityEngine.Random;

using Core;
using Core.Map;

namespace App.Map
{

    [Serializable]
    [RequireComponent(typeof(MeshRenderer))]
    public class Map : MapModel
    {

        public int Width => m_Width;
        public int Height => m_Height;

        public GameObject Obj => gameObject;


        [Header("Terrain")]
        [SerializeField] private TerrainInfo[] m_Regions;


        [Header("Noise")]
        [SerializeField] private NoiseModel m_NoiseDefault;
        private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
        private INoise m_Noise;

        [SerializeField] private AnimationCurve m_Affector;

        private int m_Width = 100;
        private int m_Height = 100;
        private Vector2 m_Offset;
        private int m_Seed;
        private float m_Scale;
        private int m_Octaves;
        private float m_Persistence;
        private float m_Lacunarity;

        [Header("Texture")]
        [SerializeField] private MeshRenderer m_Renderer;
        private MeshRenderer Renderer => m_Renderer != null ? m_Renderer : Obj.GetComponent<MeshRenderer>();
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

            m_Width = m_Config.Width;
            m_Height = m_Config.Height;
            m_Scale = m_Config.Scale;
            m_Seed = m_Config.Seed;
            m_Octaves = m_Config.Octaves;
            m_Persistence = m_Config.Persistence;
            m_Lacunarity = m_Config.Lacunarity;
            m_Offset = m_Config.Offset;

        }


        public override void DrawTexture()
        {
            m_Texture = new Texture2D(m_Width, m_Height);

            var noiseHeights = Noise.GetMatrix2D(m_Width, m_Height, m_Scale, m_Offset, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
                for (int x = 0; x < m_Width; x++)
                    colourMatrix[y * m_Width + x] = Color.Lerp(Color.black, Color.white, noiseHeights[x, y]);


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

            m_Texture = new Texture2D(m_Width, m_Height);

            var noiseHeights = Noise.GetMatrix2D(m_Width, m_Height, m_Scale, m_Offset, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
                for (int x = 0; x < m_Width; x++)
                    colourMatrix[y * m_Width + x] = Color.Lerp(Color.black, Color.white, noiseHeights[x, y]);


            m_Texture.SetPixels(colourMatrix);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;


            Renderer.sharedMaterial.mainTexture = m_Texture;
            Obj.transform.localScale = new Vector3(m_Width, 1, m_Height);
            Renderer.enabled = true;
        }

        private void GenerateColorMap()
        {

            m_Texture = new Texture2D(m_Width, m_Height);

            var noiseHeights = Noise.GetMatrix2D(m_Width, m_Height, m_Scale, m_Offset, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    foreach (var r in m_Regions)
                    {
                        if (noiseHeights[x, y] <= r.Height)
                        {
                            colourMatrix[y * m_Width + x] = r.Color;
                            break;
                        }
                    }
                }
            }

            m_Texture.SetPixels(colourMatrix);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;

            Renderer.sharedMaterial.mainTexture = m_Texture;
            Obj.transform.localScale = new Vector3(m_Width, 1, m_Height);
            Renderer.enabled = true;
        }

        private void GenerateMesh()
        {
            Renderer.enabled = false;


            /*
            var terrainData = new TerrainData();
            m_Resolution = terrainData.heightmapResolution;

            terrainData.size = new Vector3(m_Width, 100, m_Height);
            terrainData.SetHeights(0, 0, GetHights());



            if (m_Terrarian == null)
            {
                m_Terrarian = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
                m_Terrarian.transform.parent = gameObject.transform;
                m_Terrarian.transform.position = Vector3.zero;
            }
            else
                m_Terrarian.terrainData = terrainData;

            Terrarian.gameObject.SetActive(true);

            */
            //m_terrainHeights = new float[m_heightMapSize, m_heightMapSize];

        }



    }


    [Serializable]
    public struct TerrainInfo
    {
        public string Label;
        public float Height;
        public Color Color;

        public TerrainInfo(string label, float height, Color color)
        {
            Label = label;
            Height = height;
            Color = color;
        }
    }

}