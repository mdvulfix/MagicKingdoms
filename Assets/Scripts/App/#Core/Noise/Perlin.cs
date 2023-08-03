using UnityEngine;

namespace Core.Map
{

    //[CreateAssetMenu(fileName = "Perlin", menuName = "Noise/Perlin", order = 1)]
    public class Perlin : NoiseModel, INoise
    {
        public override float Noise2D(float x, float y)
            => Mathf.PerlinNoise(x, y);

    }
}