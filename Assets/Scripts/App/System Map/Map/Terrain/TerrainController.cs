using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class TerrainController : MonoBehaviour
{


    private Terrain Terrain => m_Terrain ??= GetComponent<Terrain>();
    [SerializeField] private Terrain m_Terrain;
    private int m_Res;

    [SerializeField] private float m_Scale = 0.1f;
    [SerializeField] private float m_Depth = 1f;

    [SerializeField] private AnimationCurve m_Affector;


    private void OnEnable()
    {

        m_Terrain ??= GetComponent<Terrain>();
        m_Res = m_Terrain.terrainData.heightmapResolution;


        // var meshNew = terrain.terrainData.GetHeights(0, 0, res, res);
        // for (int i = 0; i < 20; i++)
        // {
        //     meshNew[0, i] = 0.01f;
        // }
        // terrain.terrainData.SetHeights(0, 0, meshNew);
    }


    private void Draw()
    {
        var mesh = new float[m_Res, m_Res];

        for (int x = 0; x < m_Res; x++)
        {
            for (int y = 0; y < m_Res; y++)
            {
                mesh[x, y] = m_Affector.Evaluate(Mathf.PerlinNoise(x * m_Scale, y * m_Scale)) * m_Depth;
            }
        }


        Terrain.terrainData.SetHeights(0, 0, mesh);
    }

    private void OnValidate()
    {
        Draw();
    }

}
