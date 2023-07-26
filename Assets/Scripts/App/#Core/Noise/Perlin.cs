using UnityEngine;

namespace Core.Map
{

    //[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
    [CreateAssetMenu(fileName = "Perlin", menuName = "Noise/Perlin", order = 1)]
    public class Perlin : NoiseModel, INoise
    {

        protected override float Noise2D(float x, float y)
            => Mathf.PerlinNoise(x, y);

    }
}