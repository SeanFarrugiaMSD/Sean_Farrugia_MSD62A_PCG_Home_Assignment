using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeWaterGenerator : MonoBehaviour
{
    GameObject waterPrefab;
    GameObject waterObject;

    TerrainData terrainData;

    float waterHeight;

    // Start is called before the first frame update
    void Start()
    {
        waterHeight = Random.Range(0.3f, 0.4f);

        terrainData = GetComponent<Terrain>().terrainData;
        waterPrefab = Resources.Load<GameObject>("Prefabs/Water_Prefab");

        waterObject = Instantiate(waterPrefab, this.transform.position, this.transform.rotation);
        waterObject.name = "Water_Object";
        waterObject.transform.position = this.transform.position + new Vector3(terrainData.size.x / 2, waterHeight * terrainData.size.y, terrainData.size.z / 2);
        waterObject.transform.localScale = new Vector3(terrainData.size.x / 10, terrainData.size.y, terrainData.size.z / 10);
        waterObject.transform.SetParent(this.gameObject.transform);
    }
}
