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
        [SerializeField] private MapDisplayMode m_DisplayMode = MapDisplayMode.Noise;
        private MapDisplayMode DisplayMode =>
            m_DisplayMode ==
            MapDisplayMode.None ?
            MapDisplayMode.Noise :
            m_DisplayMode;


        [Header("Resolution")]
        [SerializeField] private Vector2Int m_Size;
        [SerializeField] private Vector2 m_Offset = Vector2.one;

        [Header("Noise")]
        [SerializeField] private NoiseModel m_Noise;
        [SerializeField] private int m_Seed = 0;

        [Range(1, 256)]
        [SerializeField] private float m_Scale = 20f;

        [Range(1, 6)]
        [SerializeField] private int m_Octaves = 4;
        [Range(0, 1)]
        [SerializeField] private float m_Persistence = 0.5f;
        [Range(0, 4)]
        [SerializeField] private float m_Lacunarity = 2f;





        public bool AutoUpdate = true;


        public void Setup()
        {
            //OnValidate();

            m_Config = new MapConfig((INoise)m_Noise, m_Size, m_Offset, m_Scale, m_Seed, m_Octaves, m_Persistence, m_Lacunarity);

            m_Map = m_Map ??= GetComponent<MapDefault>();
            m_Map.Init(m_Config);
        }


        public void MapDisplay()
            => m_Map.Display(m_DisplayMode);




        public void OnValidate()
        {
            if (m_Size.x < 1) m_Size.x = 1;
            if (m_Size.y < 1) m_Size.y = 1;
            if (m_Octaves < 1) m_Octaves = 1;
            if (m_Scale < 0) m_Scale = 0.001f;


        }

        private void Update()
        {
            MapDisplay();
        }


    }

}