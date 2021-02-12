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
    //To Store the terrain data
    TerrainData terrainData;

    //To generate perlin noise for the terrain
    [SerializeField] float perlNoiseRandX = 0.01f;

    //To generate perlin noise for the terrain
    [SerializeField] float perlNoiseRandZ = 0.01f;

    //To Store the Height Map Samples
    [SerializeField] List<Texture2D> heightMapsSamples;

    //To Store the height map that will have its heights loaded onto the terrain
    [SerializeField] Texture2D heightMapImage;

    //The Scaling of the height map
    [SerializeField] Vector3 heightMapScale = new Vector3(1.99f, 0.2f, 1.99f);

    //Variables for adding textures to our texture
    [SerializeField] List<TerrainTextureData> terrainTextureDataList;

    //To Blend in the textures
    [SerializeField] float terrainTextureBlendOffset = 0.01f;

    //To know if the mesh has been created or not
    bool isCreated = false;


    // Start is called before the first frame update
    void Start()
    {
        //To Store the list of HeightMaps to load onto the terrain
        heightMapsSamples = new List<Texture2D>();
        heightMapsSamples.Add(Resources.Load<Texture2D>("Textures/Terrain_HeightMap_1"));
        heightMapsSamples.Add(Resources.Load<Texture2D>("Textures/Terrain_HeightMap_2"));
        heightMapsSamples.Add(Resources.Load<Texture2D>("Textures/Terrain_HeightMap_3"));

        //Loading a random Map Sample to further randomise the map
        heightMapImage = heightMapsSamples[Random.Range(0, 3)];

        //To randomise the Perline Noise and further randomise the map
        perlNoiseRandX = Random.Range(0f, 0.008f);
        perlNoiseRandZ = Random.Range(0f, 0.008f);

        terrainData = GetComponent<Terrain>().terrainData;

        GenerateTerrain();

        isCreated = true;

        this.GetComponent<LandscapePlayerGenerator>().enabled = true;
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
        //Create an array representing the heights of a map as big as the terrain resolution
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        //Reading and Storing the heights of the selected Height Map
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                float currentHeight = heightMapImage.GetPixel((int)(width * heightMapScale.x), (int)(height * heightMapScale.z)).grayscale * heightMapScale.y;

                heightMap[width, height] = currentHeight;
            }
        }

        //Setting the heights of the terrain to that of the selected Height Map
        terrainData.SetHeights(0, 0, heightMap);

        //Add textures to the terrain
        AddTerrainTexture();
        
        //Randomise the terrain further by applying the Perlin Noise to the heights
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] += Mathf.PerlinNoise(width * perlNoiseRandX, height * perlNoiseRandZ);
            }
        }

        //Setting the heights of the terrain to that of the selected Height Map + Perlin Noise
        terrainData.SetHeights(0, 0, heightMap);
    }

    void AddTerrainTexture()
    {
        //To Store the texture layers that will be used to "paint" the terrain
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureDataList.Count];

        //Fetching and storing the textures and their values
        for (int i = 0; i < terrainTextureDataList.Count; i++)
        {
            terrainLayers[i] = new TerrainLayer();
            terrainLayers[i].diffuseTexture = terrainTextureDataList[i].terrainTexture;
            terrainLayers[i].tileSize = terrainTextureDataList[i].tileSize;
        }

        //Applying the texture layers to the terrain
        terrainData.terrainLayers = terrainLayers;

        //Storing the Heights of the map, to apply the textures depending on the height
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        //To Store which texture layer will be visible at a specific co-ordinate
        float[,,] alphaMapList = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int height = 0; height < terrainData.alphamapHeight; height++)
        {
            for (int width = 0; width < terrainData.alphamapWidth; width++)
            {
                //Dectates how visible each texture layer is
                float[] splatmap = new float[terrainData.alphamapLayers];

                for (int i = 0; i < terrainTextureDataList.Count; i++)
                {
                    float minHeight = terrainTextureDataList[i].minHeight - terrainTextureBlendOffset;
                    float maxHeight = terrainTextureDataList[i].maxHeight + terrainTextureBlendOffset;

                    //If the height of the current terrain location is within the given range of the texture, make that texture visible
                    if (heightMap[width, height] >= minHeight && heightMap[width, height] <= maxHeight)
                    {
                        splatmap[i] = 1f;
                    }

                }

                //Normalise splatmap
                NormaliseSplatMap(splatmap);

                for (int j = 0; j < terrainTextureDataList.Count; j++)
                {
                    alphaMapList[width, height, j] = splatmap[j];
                }

            }
        }

        //Setting the Textures and their alpha values to the terrain
        terrainData.SetAlphamaps(0, 0, alphaMapList);
    }

    //Normalising the values of the splatmap list
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
