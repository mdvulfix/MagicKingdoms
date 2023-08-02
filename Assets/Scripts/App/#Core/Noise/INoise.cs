using UnityEngine;

namespace Core
{
    public interface INoise
    {
        float[,] GetMatrix2D(Vector2Int size, Vector2 offset, float scale, int seed, int octaves, float persistence, float lacunarity);

        float Noise2D(float x, float y);
        float Noise3D(float x, float y, float z);
        float Noise4D(float x, float y, float z, float a);

    }
}