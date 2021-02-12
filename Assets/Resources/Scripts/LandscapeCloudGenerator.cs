using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeCloudGenerator : MonoBehaviour
{
    GameObject cloudPrefab;
    GameObject cloudObject;

    TerrainData terrainData;

    [SerializeField] int maxClouds;

    float cloudHeight;
    float randX;
    float randZ;

    Vector3 randSize;

    // Start is called before the first frame update
    void Start()
    {
        maxClouds = 120;

        GameObject cloudObjects = new GameObject("Cloud_Objects");
        cloudObjects.transform.SetParent(this.transform);

        for(int i = 0; i < maxClouds; i++)
        {
            terrainData = GetComponent<Terrain>().terrainData;
            cloudPrefab = Resources.Load<GameObject>("Prefabs/Cloud_Prefab");

            randX = Random.Range(0, terrainData.size.x * 2f);
            randZ = Random.Range(0, terrainData.size.z * 2f);
            cloudHeight = Random.Range(0.7f, 1.5f) * terrainData.size.y;
            randSize = new Vector3(Random.Range(0.5f, 3f), Random.Range(0.5f, 3f), Random.Range(0.5f, 3f));

            cloudObject = Instantiate(cloudPrefab, this.transform.position, this.transform.rotation);
            cloudObject.name = "Cloud_Object";
            cloudObject.transform.position = this.transform.position + new Vector3(randX - terrainData.size.x / 2f, cloudHeight, randZ - terrainData.size.z / 2f);
            cloudObject.transform.localScale = randSize;
            cloudObject.transform.SetParent(cloudObjects.transform);
        }
    }
}
