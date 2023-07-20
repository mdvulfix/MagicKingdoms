using UnityEngine;


namespace Core.Map
{
    public class Simplex : NoiseModel, INoise
    {

        public override float[,] GetMatrix(int width, int height, float scale, int octaves = 1, float persistence = 1, float lacunarity = 1, int seed = 1)
        {
            float[,] matrix = new float[width, height];


            if (scale <= 0)
                scale = 0.0001f;


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float amplitude = 1f;
                    float frequency = 1f;
                    float noiseHeight = 0f;

                    for (int o = 0; o < octaves; o++)
                    {
                        float sampleX = (float)x / scale * frequency + seed;
                        float sampleY = (float)y / scale * frequency + seed;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    matrix[x, y] = noiseHeight;
                }
            }

            return matrix;
        }
    }
}