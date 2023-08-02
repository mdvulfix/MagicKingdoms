using Core;
using UnityEngine;



public class TestMeshConsole : MonoBehaviour
{

    private GameObject m_Plane;
    [SerializeField] private int m_PlaneWidth = 10;
    [SerializeField] private int m_PlaneHeight = 10;


    [SerializeField] private NoiseModel m_NoiseDefault;

    private MeshFilter m_PlaneMeshFilter;
    private MeshRenderer m_PlaneMeshRenderer;


    private void Awake()
    {

        m_Plane = new GameObject("Test plane");
        m_Plane.SetActive(false);
        m_Plane.transform.SetParent(transform);

        m_PlaneMeshFilter = m_Plane.AddComponent<MeshFilter>();
        m_PlaneMeshRenderer = m_Plane.AddComponent<MeshRenderer>();
        m_PlaneMeshRenderer.sharedMaterial = Resources.Load<Material>("Materials/MapMatUnlit");

    }

    private void Start()
    {
        var heightMap = m_NoiseDefault.GetMatrix2D(new Vector2Int(m_PlaneWidth, m_PlaneHeight), Vector2.zero, 20f, 4, 0.5f, 2, 1);

        var mesh = MeshHandler.CreateMesh(heightMap);
        m_PlaneMeshFilter.sharedMesh?.Clear();

        m_PlaneMeshFilter.sharedMesh = mesh;

        m_Plane.SetActive(true);


    }


}
