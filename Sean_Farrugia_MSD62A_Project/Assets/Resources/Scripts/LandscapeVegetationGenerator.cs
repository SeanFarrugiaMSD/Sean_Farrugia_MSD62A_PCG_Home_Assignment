using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class VegetationData
{
    public Texture2D vegetationTexture;
    public float minHeight;
    public float maxHeight;
}
public class LandscapeVegetationGenerator : MonoBehaviour
{
    //To Store the vegetation and their values that will be spawned
    [SerializeField] List<VegetationData> vegetationDataList;

    //The maximum amount of vegetation that can be spawned
    [SerializeField] int maxVegetation = 0;

    //The distance between each vegetation
    [SerializeField] int vegetationSpacing = 0;

    //To Store the 
    [SerializeField] float randomXRange = 0f;
    [SerializeField] float randomZRange = 0f;

    //To Store the terrain data
    TerrainData terrainData;

    //To know if the vegetation have been created or not
    bool isCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        terrainData = terrainData = Terrain.activeTerrain.terrainData;

        //Randomising the values of the vegetation
        maxVegetation = Random.Range(2000, 5000);
        vegetationSpacing = Random.Range(8, 15);
        randomXRange = Random.Range(1f, 5f);
        randomZRange = Random.Range(1f, 5f);

        AddVegetation();

        isCreated = true;
    }

    void OnValidate()
    {
        if (isCreated)
        {
            AddVegetation();
        }
    }

    void AddVegetation()
    {
        //To store the vegetation that will be spawned
        DetailPrototype[] vegetation = new DetailPrototype[vegetationDataList.Count];

        for (int i = 0; i < vegetationDataList.Count; i++)
        {
            vegetation[i] = new DetailPrototype();
            vegetation[i].prototypeTexture = vegetationDataList[i].vegetationTexture;
        }

        terrainData.detailPrototypes = vegetation;

        //To store all of our instantiated vegetation on the terrain
        List<DetailPrototype> vegetationList = new List<DetailPrototype>();

        for (int z = 0; z < terrainData.size.z; z += vegetationSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += vegetationSpacing)
            {
                //Iterate each vegetation texture type that will be used
                for (int vegetationIndex = 0; vegetationIndex < vegetation.Length; vegetationIndex++)
                {
                    //Spawn if maxVegetation Count has not been reached
                    if (vegetationList.Count < maxVegetation)
                    {
                        //Get the current height between 0 and 1
                        float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                        //If the current height is within range of the current vegetation max and min height
                        if (currentHeight >= vegetationDataList[vegetationIndex].minHeight && currentHeight <= vegetationList[vegetationIndex].maxHeight)
                        {
                            //randomise x and z slightly so that the vegetation wont look like a grid
                            float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            DetailPrototype vegetationInstance = new DetailPrototype();

                            vegetationInstance.prototypeTexture = vegetationDataList[vegetationIndex].vegetationTexture;
                            vegetationInstance.prototype.transform.position = new Vector3(randomX, currentHeight, randomZ);
                            vegetationInstance.healthyColor = Color.white;
                            vegetationInstance.dryColor = Color.green + new Color(0.5f, 0f, 0.5f);

                            //Add tree to the list of trees that will be spawned
                            vegetationList.Add(vegetationInstance);
                        }
                    }
                }
            }
        }
        //Setting the detail prototypes to the terrain data
        terrainData.detailPrototypes = vegetationList.ToArray();
    }
}
