using UnityEngine;

namespace Core
{
    public interface INoise
    {
        float[,] GetMatrix2D(Vector2Int size, Vector2 offset, float scale, int octaves, float persistence, float lacunarity, int seed);
    }
}