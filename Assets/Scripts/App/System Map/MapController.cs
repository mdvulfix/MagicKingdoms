using System;
using UnityEngine;

using Core;
using Core.Map;


namespace App.Map
{
    [Serializable]
    public class MapController : MonoBehaviour
    {



        [Header("Config")]
        private MapConfig m_Config;

        [Header("Map")]
        [SerializeField] private Map m_Map;
        [SerializeField] private MapDisplayMode m_DisplayMode = MapDisplayMode.Noise;


        [Header("Resolution")]
        [SerializeField] private int m_Width = 100;
        [SerializeField] private int m_Height = 100;
        [SerializeField] private Vector2 m_Offset = Vector2.one;

        [Header("Noise")]
        [SerializeField] private NoiseModel m_Noise;
        [SerializeField] private int m_Seed = 0;

        [Range(1, 100)]
        [SerializeField] private float m_Scale = 20f;

        [Range(1, 6)]
        [SerializeField] private int m_Octaves = 4;
        [Range(0, 1)]
        [SerializeField] private float m_Persistence = 0.5f;
        [Range(0, 4)]
        [SerializeField] private float m_Lacunarity = 2f;





        public bool AutoUpdate = true;

        void Start()
        {

            Setup();
            Generate();

        }




        public void Setup()
        {
            OnValidate();


            m_Config = new MapConfig((INoise)m_Noise, m_Width, m_Height, m_Scale, m_Offset, m_Seed, m_Octaves, m_Persistence, m_Lacunarity);

            m_Map = m_Map ?? throw new Exception("Map is not assigned!");
            m_Map.Init(m_Config);
        }


        public void Generate()
        {
            m_Map.Draw();
            m_Map.Display(m_DisplayMode);
        }



        public void OnValidate()
        {
            if (m_Width < 1) m_Width = 1;
            if (m_Height < 1) m_Height = 1;
            if (m_Octaves < 1) m_Octaves = 1;
            if (m_Scale < 0) m_Scale = 0.001f;

        }


    }

}