using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapePlayerGenerator : MonoBehaviour
{
    //To Store the Cloud Prefab and the instantiated cloud object
    GameObject playerPrefab;

    //To Store the terrain data
    TerrainData terrainData;

    //To Store the location of the cloud
    float playerY;
    int randX;
    int randZ;

    // Start is called before the first frame update
    void Start()
    {
        terrainData = GetComponent<Terrain>().terrainData;
        playerPrefab = Resources.Load<GameObject>("Prefabs/FPSController");

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        while (true)
        {
            //Generate random location
            randX = Random.Range(0, (int)terrainData.size.x);
            randZ = Random.Range(0, (int)terrainData.size.z);

            //Get the Height of the current random location
            playerY = terrainData.GetHeight(randX, randZ);

            //If it is within a walkable range, spawn the player there.
            if(playerY / terrainData.size.y < 0.7f && playerY / terrainData.size.y > 0.4f)
            {
                Instantiate(playerPrefab, new Vector3(randX, playerY + 50f, randZ), Quaternion.identity);
                break;
            }
        }
    }
}
