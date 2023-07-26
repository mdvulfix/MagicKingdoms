using System;
using UnityEngine;

namespace Core
{

    public abstract class NoiseModel : ScriptableObject
    {

        public virtual float[,] GetMatrix2D(int width, int height, float scale, Vector2 offset, int octave, float persistence, float lacunarity, int seed)
        {
            float[,] matrix = new float[width, height];


            var random = new System.Random(seed);
            var octaveOffset = new Vector2[octave];
            var xOffset = 0.0f;
            var yOffset = 0.0f;

            for (int i = 0; i < octave; i++)
            {
                xOffset = random.Next(-100000, 100000) + offset.x;
                yOffset = random.Next(-100000, 100000) + offset.y;
                octaveOffset[i] = new Vector2(xOffset, yOffset);
            }



            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var xValue = 0.0f;
                    var yValue = 0.0f;
                    var noiseValue = 0.0f;

                    var amplitude = 1.0f;
                    var frequency = 1.0f;
                    var contrast = 0.15f;

                    for (int i = 0; i < octave; i++)
                    {
                        xValue = (x - width / 2) / scale * frequency + octaveOffset[i].x;
                        yValue = (y - height / 2) / scale * frequency + octaveOffset[i].y;

                        noiseValue += (Noise2D(xValue, yValue) + contrast) * amplitude;

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