using UnityEngine;

namespace Core.Map
{
    public class Perlin : NoiseModel, INoise
    {

        public override float[,] GetMatrix(int width, int height, float scale, Vector2 offset, int octaves = 1, float persistence = 1, float lacunarity = 1, int seed = 1)
        {

            float[,] matrix = new float[width, height];


            float halfWidth = width / 2f;
            float halfHeight = height / 2f;


            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var xVal = ((float)x + offset.x) / scale;
                    var yVal = ((float)y + offset.y) / scale;

                    float noiseVal = Mathf.PerlinNoise(xVal, yVal);
                    matrix[x, y] = noiseVal;
                }
            }


            return matrix;
        }

    }
}