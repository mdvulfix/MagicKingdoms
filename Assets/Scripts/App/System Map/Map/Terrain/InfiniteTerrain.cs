using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteTerrain : MonoBehaviour
{

    //bool terrainIsFlushed = true;

    public static float waterHeight = 50;
    public static float m_landScapeSize = 100;

    public const int m_heightMapSize = 513;
    public const float m_terrainHeight = 1500;
    public static float[,] m_terrainHeights = new float[m_heightMapSize, m_heightMapSize];

    protected const int dim = 1;
    public static Terrain m_terrain;


    void Awake()
    {


        TerrainData terrainData = new TerrainData();

        //terrainData.wavingGrassStrength = m_wavingGrassStrength;
        //terrainData.wavingGrassAmount = m_wavingGrassAmount;
        //terrainData.wavingGrassSpeed = m_wavingGrassSpeed;
        //terrainData.wavingGrassTint = m_wavingGrassTint;


        terrainData.heightmapResolution = m_heightMapSize;
        terrainData.size = new Vector3(m_landScapeSize, m_terrainHeight, m_landScapeSize);
        //terrainData.alphamapResolution = m_alphaMapSize;
        //terrainData.splatPrototypes = m_splatPrototypes;
        //terrainData.treePrototypes = m_treeProtoTypes;

        //terrainData.SetDetailResolution(m_detailMapSize, m_detailResolutionPerPatch);
        //terrainData.detailPrototypes = m_detailProtoTypes;

        m_terrain = Terrain.CreateTerrainGameObject(terrainData).GetComponent<Terrain>();
        m_terrain.transform.parent = gameObject.transform;

        m_terrain.transform.position = Vector3.zero;
        //m_terrain.treeDistance = m_treeDistance;
        //m_terrain.treeBillboardDistance = m_treeBillboardDistance;
        //m_terrain.treeCrossFadeLength = m_treeCrossFadeLength;
        //m_terrain.treeMaximumFullLODCount = m_treeMaximumFullLODCount;

        //m_terrain.detailObjectDensity = m_detailObjectDensity;
        //m_terrain.detailObjectDistance = m_detailObjectDistance;

        //m_terrain.GetComponent<Collider>().enabled = false;
        //m_terrain.basemapDistance = 4000;

    }
}