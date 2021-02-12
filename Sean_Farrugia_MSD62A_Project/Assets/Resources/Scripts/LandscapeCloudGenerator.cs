using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeCloudGenerator : MonoBehaviour
{
    //To Store the Cloud Prefab and the instantiated cloud object
    GameObject cloudPrefab;
    GameObject cloudObject;

    //To Store the terrain data
    TerrainData terrainData;

    //The amount of clouds that will be spawned
    [SerializeField] int cloudAmount;

    //To Store the location of the cloud
    float cloudHeight;
    float randX;
    float randZ;

    //To Store the scale of the cloud
    Vector3 randSize;

    // Start is called before the first frame update
    void Start()
    {
        //Randomise the Amount of clouds that will spawn
        cloudAmount = Random.Range(100,121);

        //Spawn an object representing all of the clouds
        GameObject cloudObjects = new GameObject("Cloud_Objects");
        cloudObjects.transform.SetParent(this.transform);

        //Get the required components to spawn the clouds
        terrainData = GetComponent<Terrain>().terrainData;
        cloudPrefab = Resources.Load<GameObject>("Prefabs/Cloud_Prefab");

        //Spawn clouds until the amount of clouds is reached
        for (int i = 0; i < cloudAmount; i++)
        {
            //Randomise the cloud's position and size
            randX = Random.Range(0, terrainData.size.x * 2f);
            randZ = Random.Range(0, terrainData.size.z * 2f);
            cloudHeight = Random.Range(0.7f, 2f) * terrainData.size.y;
            randSize = new Vector3(Random.Range(0.5f, 3f), Random.Range(0.5f, 3f), Random.Range(0.5f, 3f));

            //Spawn the cloud at the random position and scale it accordingly
            cloudObject = Instantiate(cloudPrefab, this.transform.position, this.transform.rotation);
            cloudObject.name = "Cloud_Object";
            cloudObject.transform.position = this.transform.position + new Vector3(randX - terrainData.size.x / 2f, cloudHeight, randZ - terrainData.size.z / 2f);
            cloudObject.transform.localScale = randSize;
            cloudObject.transform.SetParent(cloudObjects.transform);
        }
    }
}
