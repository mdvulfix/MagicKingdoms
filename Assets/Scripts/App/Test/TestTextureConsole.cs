
using System;
using UnityEngine;

namespace Core.Test
{
    [RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
    public class TestTextureConsole : MonoBehaviour
    {

        private MeshRenderer m_MeshRenderer;
        private MeshFilter m_MeshFilter;

        private INoise m_Noise;

        private Texture2D m_Texture;

        [Header("Texture")]
        [SerializeField] private int m_Width;
        [SerializeField] private int m_Length;
        [SerializeField] private int m_FilterRadius;
        [SerializeField] private TextureShape m_Shape;


        [SerializeField] private bool m_AutoUpdate;

        private void Awake()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();
            m_MeshFilter = GetComponent<MeshFilter>();

            m_Noise = new Simplex();

        }


        private void Start()
        {

            m_Texture = TextureHandler.GetTextureShapeMask(m_Width, m_Length, m_FilterRadius, m_Shape);
            m_MeshRenderer.sharedMaterial.mainTexture = m_Texture;
            gameObject.transform.localScale = new Vector3(m_Width, 0, m_Length);

        }




        [Button]
        public void Click()
        {
            Awake();
            Start();

        }


        private void OnValidate()
        {
            if (m_AutoUpdate)
                Click();
        }

    }
}