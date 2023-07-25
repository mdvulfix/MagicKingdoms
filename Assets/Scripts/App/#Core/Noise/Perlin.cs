using UnityEngine;

namespace Core.Map
{
    public class Perlin : NoiseModel, INoise
    {

        public override float[,] GetMatrix(int width, int height, float scale, int octaves = 1, float persistence = 1, float lacunarity = 1, int seed = 1)
        {
            float[,] matrix = new float[width, height];

            if (scale <= 0)
                scale = 0.0001f;


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    float value = Mathf.PerlinNoise(sampleX, sampleY);
                    matrix[x, y] = value;
                }
            }

            return matrix;
        }
    }
}