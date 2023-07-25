using System;
using UnityEngine;

using Core;
using Core.Map;





namespace App.Map
{

    [Serializable]
    [Cached]
    public class MapController : MonoBehaviour
    {


        [Header("Config")]
        private MapConfig m_Config;

        [SerializeField] private Map m_Map;


        private void Setup()
        {
            var noise = new Perlin();
            var width = 100;
            var height = 100;
            var scale = 100;
            var octaves = 4;
            var persistence = 0.5f;
            var lacunarity = 2f;
            var seed = 0;

            m_Config = new MapConfig(noise, width, height, scale, seed, octaves, persistence, lacunarity);


            m_Map = m_Map ?? throw new Exception("Map is not assigned!");
            m_Map.Init(m_Config);
        }



        public void Generate()
        {
            m_Map.Draw();
            m_Map.Display();

        }



    }

}