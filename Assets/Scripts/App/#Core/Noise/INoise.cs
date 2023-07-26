using UnityEngine;

namespace Core
{
    public interface INoise
    {
        float[,] GetMatrix2D(int width, int height, float scale, Vector2 offset, int octaves, float persistence, float lacunarity, int seed);
    }
}