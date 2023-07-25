namespace Core
{
    public abstract class NoiseModel
    {
        public abstract float[,] GetMatrix(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed);
    }
}