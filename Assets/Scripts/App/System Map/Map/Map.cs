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

        private MeshRenderer Renderer => m_Renderer != null ? m_Renderer : Obj.GetComponent<MeshRenderer>();
        private MeshRenderer m_Renderer;

        private Texture2D m_Texture;

        private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
        private INoise m_Noise;

        [SerializeField] private NoiseModel m_NoiseDefault;

        private int m_Width = 100;
        private int m_Height = 100;
        private float m_Scale = 1;
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
            m_Offset = m_Config.Offset;
            m_Octaves = m_Config.Octaves;
            m_Persistence = m_Config.Persistence;
            m_Lacunarity = m_Config.Lacunarity;

        }


        public override void Draw()
        {
            m_Texture = new Texture2D(m_Width, m_Height);


            var hightMatrix = Noise.GetMatrix2D(m_Width, m_Height, m_Scale, m_Offset, m_Octaves, m_Persistence, m_Lacunarity, m_Seed);
            var colourMatrix = new Color[m_Width * m_Height];

            for (int y = 0; y < m_Height; y++)
                for (int x = 0; x < m_Width; x++)
                    colourMatrix[y * m_Width + x] = Color.Lerp(Color.black, Color.white, hightMatrix[x, y]);


            m_Texture.SetPixels(colourMatrix);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;
        }


        public override void Display()
        {
            Renderer.sharedMaterial.mainTexture = m_Texture;
            Obj.transform.localScale = new Vector3(m_Width, 1, m_Height);
            Obj.SetActive(true);
        }

        public override void Close()
        {
            Obj.SetActive(false);
        }

    }

    public struct MapConfig
    {
        public INoise Noise { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public float Scale { get; private set; }
        public Vector2 Offset { get; internal set; }
        public int Seed { get; private set; }
        public int Octaves { get; private set; }
        public float Persistence { get; private set; }
        public float Lacunarity { get; private set; }


        public MapConfig(INoise noise, int width, int height, float scale, Vector2 offset, int seed, int octaves, float persistence, float lacunarity)
        {
            Noise = noise;
            Width = width;
            Height = height;
            Scale = scale;
            Offset = offset;
            Seed = seed;
            Octaves = octaves;
            Persistence = persistence;
            Lacunarity = lacunarity;
        }




    }
}