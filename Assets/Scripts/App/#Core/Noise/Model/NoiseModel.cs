using System;
using UnityEngine;

namespace Core
{

    public abstract class NoiseModel : ScriptableObject
    {

        public virtual float[,] GetMatrix2D(Vector2Int size, Vector2 offset, float scale, int seed, int octave, float persistence, float lacunarity)
        {
            float[,] matrix = new float[size.x, size.y];


            var random = new System.Random(seed);
            var offsets = new Vector2[octave];
            var xo = 0.0f;
            var yo = 0.0f;

            for (int i = 0; i < octave; i++)
            {
                xo = random.Next(-100000, 100000) + offset.x;
                yo = random.Next(-100000, 100000) + offset.y;
                offsets[i] = new Vector2(xo, yo);
            }

            var minHeight = float.MaxValue;
            var maxHeight = float.MinValue;



            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    var xv = 0.0f;
                    var yv = 0.0f;
                    var height = 0.0f;

                    var amplitude = 1.0f;
                    var frequency = 1.0f;


                    for (int i = 0; i < octave; i++)
                    {
                        xv = ((x - size.x / 2) / scale + offsets[i].x) * frequency;
                        yv = ((y - size.y / 2) / scale + offsets[i].y) * frequency;


                        height += Noise2D(xv, yv) * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (height > maxHeight)
                        maxHeight = height;
                    else if (height < minHeight)
                        minHeight = height;


                    matrix[x, y] = height;
                }
            }

            for (int y = 0; y < size.y; y++)
                for (int x = 0; x < size.x; x++)
                    matrix[x, y] = Mathf.InverseLerp(minHeight, maxHeight, matrix[x, y]);


            return matrix;
        }


        public virtual float Noise2D(float x, float y) { return 0.0f; }
        public virtual float Noise3D(float x, float y, float z) { return 0.0f; }
        public virtual float Noise4D(float x, float y, float z, float a) { return 0.0f; }
    }
}