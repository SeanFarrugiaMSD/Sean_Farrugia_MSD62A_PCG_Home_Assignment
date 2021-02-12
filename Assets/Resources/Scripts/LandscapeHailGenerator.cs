using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeHailGenerator : MonoBehaviour
{
    GameObject sHailPrefab;
    GameObject mHailPrefab;
    GameObject hHailPrefab;
    GameObject hailObject;

    TerrainData terrainData;

    [SerializeField] int maxHail;

    int randX;
    int randZ;

    [SerializeField] float hailHeight;

    // Start is called before the first frame update
    void Start()
    {
        float currentHeight;

        maxHail = 25;

        GameObject hailObjects = new GameObject("Hail_Objects");
        hailObjects.transform.SetParent(this.transform);

        for (int i = 0; i < maxHail; i++)
        {
            terrainData = GetComponent<Terrain>().terrainData;
            sHailPrefab = Resources.Load<GameObject>("Prefabs/Hail_s_Prefab");
            mHailPrefab = Resources.Load<GameObject>("Prefabs/Hail_m_Prefab");
            hHailPrefab = Resources.Load<GameObject>("Prefabs/Hail_h_Prefab");

            randX = Random.Range(0, (int)terrainData.size.x);
            randZ = Random.Range(0, (int)terrainData.size.z);

            currentHeight = terrainData.GetHeight(randX, randZ) / terrainData.size.y;

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

            hailObject.transform.position = this.transform.position + new Vector3(randX, hailHeight * terrainData.size.y, randZ);

            hailObject.transform.SetParent(hailObjects.transform);
        }
    }
}
