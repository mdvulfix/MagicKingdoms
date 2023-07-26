using System;
using UnityEngine;

namespace Core
{

    public abstract class NoiseModel : ScriptableObject
    {

        public virtual float[,] GetMatrix2D(int width, int height, float scale, Vector2 offset, int octave, float persistence, float lacunarity, int seed)
        {

            float[,] matrix = new float[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var xValue = 0.0f;
                    var yValue = 0.0f;
                    var noiseValue = 0.0f;

                    var amplitude = 1.0f;
                    var frequency = 1.0f;

                    for (int i = 0; i < octave; i++)
                    {
                        xValue = (float)x / scale * frequency;
                        yValue = (float)y / scale * frequency;

                        noiseValue += Noise2D(xValue, yValue) * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }


                    matrix[x, y] = noiseValue;
                }
            }


            return matrix;
        }



        protected virtual float Noise2D(float x, float y) { return 0.0f; }
        protected virtual float Noise3D(float x, float y, float z) { return 0.0f; }
        protected virtual float Noise4D(float x, float y, float z, float a) { return 0.0f; }
    }
}

/*
Vector2 v3 = new Vector3(width, width);
float val = 0;
for (int i = 0; i < octaves; i++)
{
    val += Noise(v3.x, v3.y) / scale * amplitude;
    v3 *= lacunarity;
    amplitude *= persistence;
}
*/