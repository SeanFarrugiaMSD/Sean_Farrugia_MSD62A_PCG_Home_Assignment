using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TerrainTextureData
{
    public Texture2D terrainTexture;
    public float minHeight;
    public float maxHeight;
    public Vector2 tileSize = new Vector2(500f,500f);
}

public class LandscapeGenerator : MonoBehaviour
{
    private TerrainData terrainData;

    //Variables for generating terrain using perlin noise
    [SerializeField]
    private float perlNoiseRandX = 0.01f;

    [SerializeField]
    private float perlNoiseRandZ = 0.01f;

    [SerializeField]
    List<Texture2D> heightMapsSamples;

    [SerializeField]
    private Texture2D heightMapImage;

    [SerializeField]
    private Vector3 heightMapScale = new Vector3(1.99f, 0.2f, 1.99f);

    //Variables for adding textures to our texture
    [SerializeField]
    private List<TerrainTextureData> terrainTextureDataList;

    [SerializeField]
    private float terrainTextureBlendOffset = 0.01f;

    bool isCreated = false;


    // Start is called before the first frame update
    void Start()
    {
        //To Store the list of HeightMaps to load onto the terrain
        heightMapsSamples = new List<Texture2D>();
        heightMapsSamples.Add(Resources.Load<Texture2D>("Textures/Terrain_HeightMap_1"));
        heightMapsSamples.Add(Resources.Load<Texture2D>("Textures/Terrain_HeightMap_2"));
        heightMapsSamples.Add(Resources.Load<Texture2D>("Textures/Terrain_HeightMap_3"));

        heightMapImage = heightMapsSamples[Random.Range(0, 3)];
        perlNoiseRandX = Random.Range(0f, 0.008f);
        perlNoiseRandZ = Random.Range(0f, 0.008f);

        terrainData = GetComponent<Terrain>().terrainData;

        GenerateTerrain();

        isCreated = true;
    }

    void OnValidate()
    {
        if (isCreated)
        {
            GenerateTerrain();
        }
    }

    void GenerateTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                float currentHeight = heightMapImage.GetPixel((int)(width * heightMapScale.x), (int)(height * heightMapScale.z)).grayscale * heightMapScale.y;

                heightMap[width, height] = currentHeight;
            }
        }

        terrainData.SetHeights(0, 0, heightMap);

        AddTerrainTexture();
        
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] += Mathf.PerlinNoise(width * perlNoiseRandX, height * perlNoiseRandZ);
            }
        }

        terrainData.SetHeights(0, 0, heightMap);
    }

    void AddTerrainTexture()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {
            terrainLayers[i] = new TerrainLayer();
            terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
            terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
        }

        terrainData.terrainLayers = terrainLayers;

        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1f;
                    }

                }

                //normalise splatmap
                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }

            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }

    void NormaliseSplatMap(float[] splatMap)
    {
        float total = 0;

        for (int i = 0; i < splatMap.Length; i++)
        {
            total += splatMap[i];
        }

        for (int i = 0; i < splatMap.Length; i++)
        {
            splatMap[i] = splatMap[i] / total;
        }
    }
}
