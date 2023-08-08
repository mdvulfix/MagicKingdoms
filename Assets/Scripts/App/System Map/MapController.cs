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
        [SerializeField] private MapDefault m_Map;
        [SerializeField] private MapDisplayMode m_DisplayMode = MapDisplayMode.Mesh;



        [Header("Resolution")]
        [SerializeField] private int m_Width;
        [SerializeField] private int m_Length;
        [SerializeField] private float m_WidthOffset;
        [SerializeField] private float m_LengthOffset;

        private INoise m_Noise;

        [Header("Noise")]
        [SerializeField] private int m_Seed = 0;

        [Range(1, 256)]
        [SerializeField] private float m_Scale = 20f;

        [Range(1, 6)]
        [SerializeField] private int m_Octaves = 4;
        [Range(0, 1)]
        [SerializeField] private float m_Persistence = 0.5f;
        [Range(0, 4)]
        [SerializeField] private float m_Lacunarity = 2f;


        [SerializeField] private AnimationCurve m_Curve;
        [SerializeField] private float m_HeightFactor;


        public bool AutoUpdate = true;


        public void Setup()
        {

            var noise = m_Noise ??= new Simplex();

            m_Config = new MapConfig(m_Width,
                                     m_Length,
                                     m_WidthOffset,
                                     m_LengthOffset,
                                     noise,
                                     m_Seed,
                                     m_Scale,
                                     m_Octaves,
                                     m_Persistence,
                                     m_Lacunarity,
                                     m_HeightFactor,
                                     m_Curve);



            m_Map = m_Map ??= GetComponent<MapDefault>();
            m_Map.Init(m_Config);
        }


        public void MapDisplay()
        {
            switch (m_DisplayMode)
            {
                case MapDisplayMode.Noise:
                    m_Map.DisplayNoiseMap();
                    break;

                case MapDisplayMode.Color:
                    m_Map.DisplayColorMap();
                    break;

                case MapDisplayMode.Mesh:
                    m_Map.DisplayMesh();
                    break;
            }
        }





        public void OnValidate()
        {
            if (m_Width < 1) m_Width = 1;
            if (m_Length < 1) m_Length = 1;
            if (m_Octaves < 1) m_Octaves = 1;
            if (m_Scale < 0) m_Scale = 0.001f;

        }

        private void Update()
        {
            MapDisplay();
        }

    }
}

