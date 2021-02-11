using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    GameObject waterPrefab;
    GameObject waterObject;

    TerrainData terrainData;

    // Start is called before the first frame update
    void Start()
    {
        terrainData = GetComponent<Terrain>().terrainData;
        waterPrefab = Resources.Load<GameObject>("Prefabs/Water_Prefab");

        waterObject = Instantiate(waterPrefab, this.transform.position, this.transform.rotation);
        waterObject.name = "Water_Object";
        waterObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, 0.35f * terrainData.size.y, terrainData.size.z / 2);
        waterObject.transform.localScale = new Vector3(terrainData.size.x / 10, terrainData.size.y, terrainData.size.z / 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
