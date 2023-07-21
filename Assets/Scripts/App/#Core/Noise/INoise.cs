namespace Core.Map
{
    public interface INoise
    {
        float[,] GetMatrix(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed);
    }
}