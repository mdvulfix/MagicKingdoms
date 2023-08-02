using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public class TestNoiseConsole : MonoBehaviour
{

    private GameObject m_Plane;
    private Texture2D m_Texture;




    [Header("Noise")]
    [SerializeField] private NoiseModel m_NoiseDefault;
    private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
    private INoise m_Noise;

    [SerializeField] private Vector2Int m_Size;

    [SerializeField] private int m_SizeX;
    [SerializeField] private int m_SizeZ;


    [SerializeField] private Vector2 m_Offset;
    [SerializeField] private int m_Seed;
    [SerializeField] private float m_Scale;
    [SerializeField] private int m_Octaves;
    [SerializeField] private float m_Persistence;
    [SerializeField] private float m_Lacunarity;


    private MeshFilter m_PlaneMeshFilter;
    private MeshRenderer m_PlaneMeshRenderer;

    private void OnEnable()
    {

        m_Plane = gameObject;

        m_PlaneMeshFilter = m_Plane.GetComponent<MeshFilter>();
        m_PlaneMeshRenderer = m_Plane.GetComponent<MeshRenderer>();
        m_PlaneMeshRenderer.sharedMaterial = Resources.Load<Material>("Materials/MapMatUnlit");

    }

    private void Update()
    {

        m_Size = new Vector2Int(m_Size.x, m_Size.y);
        m_Texture = new Texture2D(m_SizeX, m_SizeZ);

        var heightMap = new float[m_SizeX, m_SizeZ];


        var colorMap = new Color[m_SizeX * m_SizeZ];


        var random = new System.Random(m_Seed);
        var offsets = new Vector2[m_Octaves];
        var xo = 0.0f;
        var yo = 0.0f;

        for (int i = 0; i < m_Octaves; i++)
        {
            xo = random.Next(-100000, 100000) + m_Offset.x;
            yo = random.Next(-100000, 100000) + m_Offset.y;
            offsets[i] = new Vector2(xo, yo);
        }




        for (int y = 0; y < m_SizeZ; y++)
        {
            for (int x = 0; x < m_SizeX; x++)
            {
                var height = 0.0f;
                var amplitude = 2.0f;
                var frequency = 1.0f;
                var xv = 0.0f;
                var yv = 0.0f;

                for (int i = 0; i < m_Octaves; i++)
                {
                    //xCoord = (x - size.x / 2) / scale * frequency + octaveOffset[i].x;
                    //yCoord = (y - size.y / 2) / scale * frequency + octaveOffset[i].y;

                    // xv = ((float)x / m_Size.x * m_Scale + offsets[i].x) * frequency;
                    // yv = ((float)y / m_Size.y * m_Scale + offsets[i].y) * frequency;

                    xv = ((x - m_SizeX / 2) / m_Scale + offsets[i].x) * frequency;
                    yv = ((y - m_SizeZ / 2) / m_Scale + offsets[i].y) * frequency;

                    height += Noise.Noise2D(xv, yv) * amplitude;

                    amplitude *= m_Persistence;
                    frequency *= m_Lacunarity;
                }

                heightMap[x, y] = height;
            }

        }

        for (int y = 0; y < m_SizeZ; y++)
            for (int x = 0; x < m_SizeX; x++)
                colorMap[y * m_SizeX + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);

        m_Texture.SetPixels(colorMap);
        m_Texture.Apply();
        m_Texture.filterMode = FilterMode.Point;
        m_Texture.wrapMode = TextureWrapMode.Clamp;


        m_PlaneMeshRenderer.sharedMaterial.mainTexture = m_Texture;
        transform.localScale = new Vector3(m_SizeX, 1, m_SizeZ);
        m_PlaneMeshRenderer.enabled = true;
    }

    public void OnValidate()
    {
        if (m_Size.x < 1) m_Size.x = 1;
        if (m_Size.y < 1) m_Size.y = 1;
        if (m_Octaves < 1) m_Octaves = 1;
        if (m_Scale < 0) m_Scale = 0.001f;

    }

}
