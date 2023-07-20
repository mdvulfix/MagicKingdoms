using System;
using UnityEngine;

using Core.Map;

namespace App.Map
{

    [RequireComponent(typeof(MeshRenderer))]
    public class Map : MonoBehaviour
    {

        public int Width => m_Width;
        public int Height => m_Height;

        private GameObject m_Obj;



        private MeshRenderer m_Renderer;
        private Texture2D m_Texture;

        private INoise m_Noise;

        [SerializeField] private int m_Width = 256;
        [SerializeField] private int m_Height = 256;
        [SerializeField] private int m_Scale = 1;
        [SerializeField] private int m_Seed = 0;
        [SerializeField] private int m_Octaves = 4;
        [SerializeField] private float m_Persistence = 0.5f;
        [SerializeField] private float m_Lacunarity = 2f;


        private float[,] m_Matrix;

        private MapConfig m_Config;

        public void Init(params object[] args)
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

            m_Obj = gameObject;
            m_Obj.transform.localScale = new Vector3(m_Width, 1, m_Height);

            m_Matrix = m_Noise.GetMatrix(m_Width, m_Height, m_Scale, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);

            m_Renderer = m_Obj.GetComponent<MeshRenderer>();
            m_Renderer.sharedMaterial.mainTexture = m_Texture;


        }

        public void Draw()
        {
            m_Texture = new Texture2D(m_Width, m_Height);
            var colourMap = new Color[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
                for (int x = 0; x < m_Width; x++)
                    colourMap[y * m_Width + x] = Color.Lerp(Color.black, Color.white, m_Matrix[x, y]);


            m_Texture.SetPixels(colourMap);
            m_Texture.Apply();

        }


        public void Display()
        {
            m_Obj.SetActive(true);
        }

        public void Close()
        {
            m_Obj.SetActive(false);
        }

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