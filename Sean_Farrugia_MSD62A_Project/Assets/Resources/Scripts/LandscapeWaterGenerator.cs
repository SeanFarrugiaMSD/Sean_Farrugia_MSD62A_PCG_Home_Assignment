using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeWaterGenerator : MonoBehaviour
{
    //To Store the Water Prefab and the instantiated water object
    GameObject waterPrefab;
    GameObject waterObject;

    //To Store the terrain data
    TerrainData terrainData;

    //Dictates the level of water in accordance with the terrain
    float waterHeight;

    // Start is called before the first frame update
    void Start()
    {
        //Randomise the level of the water
        waterHeight = Random.Range(0.3f, 0.4f);

        //Get the required components to instantiate
        terrainData = GetComponent<Terrain>().terrainData;
        waterPrefab = Resources.Load<GameObject>("Prefabs/Water_Prefab");

        //Instantiate the Water Object at the given height, in the middle of the map to cover the whole map
        waterObject = Instantiate(waterPrefab, this.transform.position, this.transform.rotation);
        waterObject.name = "Water_Object";
        waterObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y, terrainData.size.z / 2);
        waterObject.transform.localScale = new Vector3(terrainData.size.x / 10, terrainData.size.y, terrainData.size.z / 10);
        waterObject.transform.SetParent(this.gameObject.transform);
    }
}
