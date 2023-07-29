using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float scale = 20.0f;
    public float heightScale = 10.0f;


    public Texture2D Texture;

    void Start()
    {
        // Создаем текстуру на основе алгоритма шума Перлина
        Texture = new Texture2D(width, height);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float noise = Mathf.PerlinNoise(xCoord, yCoord);
                Texture.SetPixel(x, y, new Color(noise, noise, noise));
            }
        }
        Texture.Apply();

        Texture.filterMode = FilterMode.Point;
        Texture.wrapMode = TextureWrapMode.Clamp;

        var renderer = GetComponent<MeshRenderer>();
        renderer.sharedMaterial.mainTexture = Texture;
        transform.localScale = new Vector3(width, 1, height);


        // Создаем Terrain на основе текстуры
        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, heightScale, height);

        float[,] heights = new float[width, height];
        Color[] pixels = Texture.GetPixels();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = pixels[x * height + y];
                heights[x, y] = pixel.grayscale * heightScale;
            }
        }

        terrainData.SetHeights(0, 0, GenHights());

        GameObject terrainObject = Terrain.CreateTerrainGameObject(terrainData);
        terrainObject.transform.position = new Vector3(0, 0, 0);



    }

    private float[,] GenHights()
    {
        var heights = new float[width, height];
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                heights[x, y] = CalcHights(x, y);

        return heights;
    }




    private float CalcHights(int x, int y)
    {
        var xVal = (float)x / width * heightScale;
        var yVal = (float)x / height * heightScale;

        return Mathf.PerlinNoise(xVal, yVal);
    }

}