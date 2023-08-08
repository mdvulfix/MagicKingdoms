using UnityEngine;

namespace Core
{
    public interface INoise
    {
        float[,] GetHeightMap2D(int width, int length, float widthOffset, float lengthOffset, float scale, int seed, int octaves, float persistence, float lacunarity);

        float Noise2D(float x, float y);
        float Noise3D(float x, float y, float z);
        float Noise4D(float x, float y, float z, float a);

    }
}