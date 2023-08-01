using System;
using UnityEngine;

namespace Core
{

    public abstract class NoiseModel : ScriptableObject
    {

        public virtual float[,] GetMatrix2D(Vector2Int size, Vector2 offset, float scale, int octave, float persistence, float lacunarity, int seed)
        {
            float[,] matrix = new float[size.x, size.y];


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

            var minHeight = float.MaxValue;
            var maxHeight = float.MinValue;



            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    var xCoord = 0.0f;
                    var yCoord = 0.0f;
                    var height = 0.0f;

                    var amplitude = 1.0f;
                    var frequency = 1.0f;


                    for (int i = 0; i < octave; i++)
                    {
                        xCoord = (x - size.x / 2) / scale * frequency + octaveOffset[i].x;
                        yCoord = (y - size.y / 2) / scale * frequency + octaveOffset[i].y;

                        height += (Noise2D(xCoord, yCoord) * 2 - 1) * amplitude;

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


        protected virtual float Noise2D(float x, float y) { return 0.0f; }
        protected virtual float Noise3D(float x, float y, float z) { return 0.0f; }
        protected virtual float Noise4D(float x, float y, float z, float a) { return 0.0f; }
    }
}