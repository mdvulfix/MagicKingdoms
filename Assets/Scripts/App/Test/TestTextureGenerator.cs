using Core;
using UnityEngine;

public class HeightMapGenerator : MonoBehaviour
{
    [SerializeField] private int m_Width = 512;
    [SerializeField] private int m_Height = 512;

    [Range(1, 8)]
    [SerializeField] private float m_FalloffStrength;

    [Range(1, 256)]
    [SerializeField] private float m_Scale = 20f;


    [SerializeField] private string m_Seed;

    [Range(1, 6)]
    [SerializeField] private int m_Octaves = 4;
    [Range(0, 1)]
    [SerializeField] private float m_Persistence = 0.5f;
    [Range(0, 4)]
    [SerializeField] private float m_Lacunarity = 2f;


    [SerializeField] private bool m_AutoUpdate;

    private void Start()
    {
        var heightMap = GenerateHeightMap();
        var falloff = GenerateFalloffMap();
        var falloffShape = GenerateFalloffShape();
        // GenerateHeightMap();
        // GenerateFalloffMapStandart();

        var texture = ApplyFalloffMap(falloff, falloffShape);
        texture = ApplyFalloffMap(heightMap, texture);


        GetComponent<Renderer>().material.mainTexture = texture;
    }

    private Texture2D GenerateHeightMap()
    {
        var texture = new Texture2D(m_Width, m_Height, TextureFormat.RGBA32, false);

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                float sampleX = x / m_Scale;
                float sampleY = y / m_Scale;

                float value = Mathf.PerlinNoise(sampleX, sampleY);

                Color color = new Color(value, value, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private Texture2D GenerateFalloffMap()
    {
        var texture = new Texture2D(m_Width, m_Height, TextureFormat.RGBA32, false);

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                float normalizedX = (x / (float)m_Width) * 2 - 1;
                float normalizedY = (y / (float)m_Height) * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(normalizedX), Mathf.Abs(normalizedY));
                value = Mathf.Pow(value, m_FalloffStrength);
                value = 1 - value;

                Color color = new Color(value, value, value);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private Texture2D GenerateFalloffShape()
    {
        var texture = new Texture2D(m_Width, m_Height, TextureFormat.RGBA32, false);

        var falloffStrength = 3.0f;
        var scale = 25.0f;
        var octaves = 3;
        var persistence = 0.8f;
        var lacunarity = 1.25f;


        var seed = m_Seed != "" ? m_Seed : Time.deltaTime.ToString();
        var random = new System.Random(seed.GetHashCode());
        var offsets = new Vector2[octaves];
        var xo = 0.0f;
        var yo = 0.0f;

        for (int i = 0; i < octaves; i++)
        {
            xo = random.Next(-100000, 100000);
            yo = random.Next(-100000, 100000);
            offsets[i] = new Vector2(xo, yo);
        }

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                var value = 0.0f;
                var amplitude = 1.0f;
                var frequency = 1.0f;


                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (float)x / scale * frequency + offsets[i].x;
                    float sampleY = (float)y / scale * frequency + offsets[i].y;

                    value += Mathf.PerlinNoise(sampleX, sampleY) * amplitude;

                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                value = Mathf.Pow(value, falloffStrength);

                Color color = new Color(value, value, value);
                texture.SetPixel(x, y, color);

            }
        }

        texture.Apply();
        return texture;
    }


    private Texture2D ApplyFalloffMap(Texture2D heightMap, Texture2D falloff)
    {
        var texture = new Texture2D(m_Width, m_Height, TextureFormat.RGBA32, false);

        for (int y = 0; y < m_Height; y++)
        {
            for (int x = 0; x < m_Width; x++)
            {
                Color heightColor = heightMap.GetPixel(x, y);
                Color falloffColor = falloff.GetPixel(x, y);

                float r = heightColor.r * falloffColor.r;
                float g = heightColor.g * falloffColor.g;
                float b = heightColor.b * falloffColor.b;

                Color color = new Color(r, g, b);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;

    }

    [Button("Update")]
    public void OnButtonClick()
    {
        Start();
    }

    public void OnValidate()
    {
        if (m_AutoUpdate)
            Start();
    }
}