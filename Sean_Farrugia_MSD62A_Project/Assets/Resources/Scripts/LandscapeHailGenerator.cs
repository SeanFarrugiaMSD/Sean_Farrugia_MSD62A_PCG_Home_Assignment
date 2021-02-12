using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeHailGenerator : MonoBehaviour
{
    //TO Store the hail that will be spawned
    GameObject sHailPrefab;
    GameObject mHailPrefab;
    GameObject hHailPrefab;
    GameObject hailObject;

    //To Store the terrain data
    TerrainData terrainData;

    //The amount of hail that will be spawned
    [SerializeField] int hailAmount;

    //To Store the random position of the hail
    int randX;
    int randZ;

    //To Store the heigh at which the hail will spawn
    [SerializeField] float hailHeight;

    // Start is called before the first frame update
    void Start()
    {
        float currentHeight;

        //Randomising the amount of hail that will spawn
        hailAmount = Random.Range(20,30);

        GameObject hailObjects = new GameObject("Hail_Objects");
        hailObjects.transform.SetParent(this.transform);

        for (int i = 0; i < hailAmount; i++)
        {
            //Getting the resources and components required to spawn the hail
            terrainData = GetComponent<Terrain>().terrainData;
            sHailPrefab = Resources.Load<GameObject>("Prefabs/Hail_s_Prefab");
            mHailPrefab = Resources.Load<GameObject>("Prefabs/Hail_m_Prefab");
            hHailPrefab = Resources.Load<GameObject>("Prefabs/Hail_h_Prefab");
            
            //Generate Random Position for the hail
            randX = Random.Range(0, (int)terrainData.size.x);
            randZ = Random.Range(0, (int)terrainData.size.z);

            //Get the Terrain Height of the current Random Position
            currentHeight = terrainData.GetHeight(randX, randZ) / terrainData.size.y;

            //Depending on how high the height is, spawn a different type of Hail
            //The higher the height, the denser the hail is
            if(currentHeight <= 0.5f)
            {
                hailObject = Instantiate(sHailPrefab, this.transform.position, this.transform.rotation);
                hailHeight = 0.5f;
            }
            else if(currentHeight <= 0.7f)
            {
                hailObject = Instantiate(mHailPrefab, this.transform.position, this.transform.rotation);
                hailHeight = 0.7f;
            }
            else if(currentHeight <= 1f)
            {
                hailObject = Instantiate(hHailPrefab, this.transform.position, this.transform.rotation);
                hailHeight = 1f;
            }

            //Set the position of the hail
            hailObject.transform.position = this.transform.position + new Vector3(randX, hailHeight * terrainData.size.y, randZ);

            hailObject.transform.SetParent(hailObjects.transform);
        }
    }
}
