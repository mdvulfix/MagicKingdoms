using Core;
using UnityEngine;
namespace Core.Test
{

    [ExecuteAlways]
    [RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
    public class TestMeshConsole : MonoBehaviour
    {

        private GameObject m_Map;
        private GameObject Map => m_Map != null ? m_Map : gameObject;

        [Header("Noise")]
        [SerializeField] private NoiseModel m_NoiseDefault;
        private INoise Noise => m_Noise != null ? m_Noise : (INoise)m_NoiseDefault;
        private INoise m_Noise;

        [SerializeField] private AnimationCurve m_Curve;
        [SerializeField] private float m_HeightMultiplier;

        [SerializeField] private Vector2Int m_Size;
        [SerializeField] private Vector2 m_Offset;
        [SerializeField] private int m_Seed;
        [SerializeField] private float m_Scale;
        [SerializeField] private int m_Octaves;
        [SerializeField] private float m_Persistence;
        [SerializeField] private float m_Lacunarity;


        private MeshRenderer m_MapMeshRenderer;
        private MeshRenderer MapMeshRenderer => m_MapMeshRenderer != null ? m_MapMeshRenderer : m_Map.GetComponent<MeshRenderer>();

        private Texture2D m_Texture;

        private MeshFilter m_MapMeshFilter;
        private MeshFilter MapMeshFilter => m_MapMeshFilter != null ? m_MapMeshFilter : m_Map.GetComponent<MeshFilter>();



        private void OnEnable()
        {

            m_Map = gameObject;

            m_MapMeshFilter = m_Map.GetComponent<MeshFilter>();
            m_MapMeshRenderer = m_Map.GetComponent<MeshRenderer>();
            //m_MapMeshRenderer.sharedMaterial = Resources.Load<Material>("Materials/MapMatUnlit");

        }

        private void Update()
        {
            MapMeshRenderer.enabled = true;

            var heightMap = Noise.GetMatrix2D(m_Size, m_Offset, m_Scale, m_Seed, m_Octaves, m_Persistence, m_Lacunarity);

            var mesh = MeshHandler.CreateMesh(heightMap, m_Curve, m_HeightMultiplier);
            MapMeshFilter.sharedMesh?.Clear();
            MapMeshFilter.sharedMesh = mesh;


            m_Texture = new Texture2D(m_Size.x, m_Size.y);
            var colorMap = new Color[m_Size.x * m_Size.y];

            for (int y = 0; y < m_Size.y; y++)
                for (int x = 0; x < m_Size.x; x++)
                    colorMap[y * m_Size.x + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);


            m_Texture.SetPixels(colorMap);
            m_Texture.Apply();
            m_Texture.filterMode = FilterMode.Point;
            m_Texture.wrapMode = TextureWrapMode.Clamp;


            MapMeshRenderer.sharedMaterial.mainTexture = m_Texture;
            Map.transform.localScale = new Vector3(Map.transform.localScale.x, m_HeightMultiplier / 2, Map.transform.localScale.z);
            MapMeshRenderer.enabled = true;


        }


    }
}