using System;
using UnityEngine;
using URandom = UnityEngine.Random;

using Core;
using Core.Map;

namespace App.Map
{



    [Serializable]
    public class Map : MapModel
    {

        public int Width => m_Width;
        public int Height => m_Height;


        private GameObject Obj => gameObject;

        [Header("Noise")]
        [SerializeField] private NoiseModel m_NoiseDefault;
        private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
        private INoise m_Noise;


        [Header("Terrain")]
        [SerializeField] private Terrain m_Terrarian;
        private Terrain Terrarian => m_Terrarian != null ? m_Terrarian : Obj.GetComponentInChildren<Terrain>();

        public float m_landScapeSize = 100;

        public int m_heightMapSize = 513;
        public float m_terrainHeight = 20;
        public float[,] m_terrainHeights;


        private int m_Width = 256;
        private int m_Height = 256;
        private int m_Depth = 20;


        [Header("Texture")]
        [SerializeField] private MeshRenderer m_Renderer;
        private MeshRenderer Renderer => m_Renderer != null ? m_Renderer : Obj.GetComponent<MeshRenderer>();
        private Texture2D m_Texture;






        private float m_Scale = 20f;
        private Vector2 m_Offset = Vector2.zero;
        private int m_Seed = 0;
        private int m_Octaves = 4;
        private float m_Persistence = 0.5f;
        private float m_Lacunarity = 2f;


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


        public override void Draw()
        {
            m_Texture = new Texture2D(m_Width, m_Height);

            var noiseHeights = Noise.GetMatrix2D(m_Width, m_Height, m_Scale, m_Offset, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
            {
                for (int x = 0; x < m_Width; x++)
                {
                    colourMatrix[y * m_Width + x] = Color.Lerp(Color.black, Color.white, noiseHeights[x, y]);
                }
            }

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
                    SetNoiseMap();
                    break;

                case MapDisplayMode.Color:
                    SetColor();
                    break;

                case MapDisplayMode.Terrain:
                    SetMapColorTerrarian();
                    break;
            }


        }

        public override void Close()
        {
            Obj.SetActive(false);
        }

        private void SetNoiseMap()
        {
            Terrarian?.gameObject.SetActive(false);

            Renderer.sharedMaterial.mainTexture = m_Texture;
            Obj.transform.localScale = new Vector3(m_Width, 1, m_Height);
            Renderer.enabled = true;
        }

        private void SetColor()
        {
            Terrarian?.gameObject.SetActive(false);

            Renderer.sharedMaterial.mainTexture = m_Texture;
            Obj.transform.localScale = new Vector3(m_Width, 1, m_Height);
            Renderer.enabled = true;
        }

        private void SetMapColorTerrarian()
        {
            Renderer.enabled = false;

            var terrainData = new TerrainData();
            terrainData.heightmapResolution = m_Width + 1;
            terrainData.size = new Vector3(m_Width, m_Depth, m_Height);
            terrainData.SetHeights(0, 0, GenHights());

            if (m_Terrarian == null)
            {
                m_Terrarian = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
                m_Terrarian.transform.parent = gameObject.transform;
                m_Terrarian.transform.position = Vector3.zero;
            }
            else
                m_Terrarian.terrainData = terrainData;

            Terrarian.gameObject.SetActive(true);


            //m_terrainHeights = new float[m_heightMapSize, m_heightMapSize];



        }


        private float[,] GenHights()
        {
            var heights = new float[m_Width, m_Height];
            for (int y = 0; y < m_Height; y++)
                for (int x = 0; x < m_Width; x++)
                    heights[x, y] = CalcHights(x, y);

            return heights;
        }




        private float CalcHights(int x, int y)
        {
            var xVal = (float)x / m_Width * m_Scale;
            var yVal = (float)x / m_Height * m_Scale;

            return Mathf.PerlinNoise(xVal, yVal);
        }


    }

}