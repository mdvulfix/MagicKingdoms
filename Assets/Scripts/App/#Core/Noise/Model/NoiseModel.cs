using UnityEngine;

namespace Core
{
    public abstract class NoiseModel
    {
        public abstract float[,] GetMatrix(int width, int height, float scale, Vector2 offset, int octaves, float persistence, float lacunarity, int seed);
    }
}