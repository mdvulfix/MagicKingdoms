using System;
using UnityEngine;

namespace Core
{

    public abstract class NoiseModel
    {

        public virtual float[,] GetHeightMap2D(int width, int length, float widthOffset, float lengthOffset, float scale, int seed = 0, int octave = 4, float persistence = 0.5f, float lacunarity = 2f)
        {
            float[,] map = new float[width, length];


            var random = new System.Random(seed);
            var offsets = new Vector2[octave];
            var xo = 0.0f;
            var yo = 0.0f;

            for (int i = 0; i < octave; i++)
            {
                xo = random.Next(-100000, 100000) + widthOffset;
                yo = random.Next(-100000, 100000) + lengthOffset;
                offsets[i] = new Vector2(xo, yo);
            }

            var minHeight = float.MaxValue;
            var maxHeight = float.MinValue;



            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var xv = 0.0f;
                    var yv = 0.0f;
                    var height = 0.0f;

                    var amplitude = 1.0f;
                    var frequency = 1.0f;


                    for (int i = 0; i < octave; i++)
                    {
                        xv = ((x - width / 2) / scale + offsets[i].x) * frequency;
                        yv = ((y - length / 2) / scale + offsets[i].y) * frequency;


                        height += Noise2D(xv, yv) * amplitude;

                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }

                    if (height > maxHeight)
                        maxHeight = height;
                    else if (height < minHeight)
                        minHeight = height;


                    map[x, y] = height;
                }
            }

            for (int y = 0; y < length; y++)
                for (int x = 0; x < width; x++)
                    map[x, y] = Mathf.InverseLerp(minHeight, maxHeight, map[x, y]);


            return map;
        }


        public virtual float Noise2D(float x, float y) { return 0.0f; }
        public virtual float Noise3D(float x, float y, float z) { return 0.0f; }
        public virtual float Noise4D(float x, float y, float z, float a) { return 0.0f; }
    }
}