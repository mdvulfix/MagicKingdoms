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

        public GameObject Obj => gameObject;

        [Header("Terrain")]


        [SerializeField] private MeshFilter m_Filter;
        private MeshFilter Filter => m_Filter != null ? m_Filter : Obj.GetComponent<MeshFilter>();


        private Vector3[] m_Vertices;
        private int[] m_Triangles;
        private Vector2[] m_UVs;

        [SerializeField] private RegionInfo[] m_Regions;
        [SerializeField] private Color m_Water;
        [SerializeField] private Color m_Sand;
        [SerializeField] private Color m_Grass;
        [SerializeField] private Color m_Rock;
        [SerializeField] private Color m_Ice;


        [Header("Noise")]
        [SerializeField] private NoiseModel m_NoiseDefault;
        private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
        private INoise m_Noise;

        [SerializeField] private AnimationCurve m_Affector;


        private Vector2Int m_Size;
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
                    GenerateNoiseMap();
                    break;
            }


        }

        public override void Close()
        {
            Obj.SetActive(false);
        }

        private void GenerateNoiseMap()
        {


            m_Texture = new Texture2D(m_Size.x, m_Size.y);

            var noiseHeights = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
                for (int x = 0; x < m_Size.x; x++)
                    colourMatrix[y * m_Size.x + x] = Color.Lerp(Color.black, Color.white, noiseHeights[x, y]);


            m_Texture.SetPixels(colourMatrix);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;


            Renderer.sharedMaterial.mainTexture = m_Texture;
            Obj.transform.localScale = new Vector3(m_Size.x, 1, m_Size.y);
            Renderer.enabled = true;
        }

        private void GenerateColorMap()
        {

            m_Texture = new Texture2D(m_Size.x, m_Size.y);

            var noiseHeights = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
            {
                for (int x = 0; x < m_Size.x; x++)
                {
                    foreach (var r in m_Regions)
                    {
                        if (noiseHeights[x, y] <= r.Height)
                        {
                            colourMatrix[y * m_Size.x + x] = r.Color;
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
            Obj.transform.localScale = new Vector3(m_Size.x, 1, m_Size.y);
            Renderer.enabled = true;
        }

        private void GenerateMesh()
        {
            Renderer.enabled = true;

            //Filter.mesh.Clear();
            var noiseHeights = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var width = noiseHeights.GetLength(0);
            var height = noiseHeights.GetLength(1);

            m_Vertices = new Vector3[width * height];
            m_UVs = new Vector2[width * height];
            m_Triangles = new int[(width - 1) * (height - 1) * 6];

            var vert = 0;
            var tris = 0;

            for (int i = 0, z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    m_Vertices[i] = new Vector3(x, noiseHeights[x, z], z);
                    m_UVs[i] = new Vector2(x / (float)width, z / (float)height);
                    i++;

                    if (x < width - 1 && z < height - 1)
                    {
                        m_Triangles[tris + 0] = vert + 0;
                        m_Triangles[tris + 1] = vert + width + 1;
                        m_Triangles[tris + 2] = vert + width;
                        m_Triangles[tris + 3] = vert + width + 1;
                        m_Triangles[tris + 4] = vert + 0;
                        m_Triangles[tris + 5] = vert + width + 1;

                    }

                    vert++;
                    tris += 6;
                }

                vert++;
            }


            var mesh = new Mesh();
            mesh.name = "Custom mesh";
            mesh.vertices = m_Vertices;
            mesh.triangles = m_Triangles;
            mesh.uv = m_UVs;
            mesh.RecalculateNormals();
            Filter.mesh = mesh;


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