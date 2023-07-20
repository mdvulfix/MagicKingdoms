using UnityEngine;

using Core.Map;
using System;

namespace App.Map
{
    public class MapController : MonoBehaviour
    {

        [SerializeField] private Map m_Map;

        void Start()
        {
            var noise = new Perlin();
            var width = 100;
            var height = 100;
            var scale = 1;
            var octaves = 4;
            var persistence = 0.5f;
            var lacunarity = 2f;
            var seed = 0;
            var mapConfig = new MapConfig(noise, width, height, scale, seed, octaves, persistence, lacunarity);


            m_Map = m_Map ?? throw new Exception("Map is not assigned!");
            m_Map.Init(mapConfig);
            m_Map.Draw();
            m_Map.Display();

        }



    }

}