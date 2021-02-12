using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class TreeData
{
    public GameObject treeMesh;
    public float minHeight;
    public float maxHeight;
}
public class LandscapeTreeGenerator : MonoBehaviour
{
    //To Store the trees and their values that will be spawned
    [SerializeField] List<TreeData> treeDataList;

    //The maximum amount of trees that can be spawned
    [SerializeField] int maxTrees = 0;

    //The distance between each tree
    [SerializeField] int treeSpacing = 0;

    //To Store the 
    [SerializeField] float randomXRange = 0f;
    [SerializeField] float randomZRange = 0f;

    //The Layer storing the terrain map
    [SerializeField] int terrainLayerIndex = 8;

    //To Store the terrain data
    TerrainData terrainData;

    //To know if the trees have been created or not
    bool isCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        terrainData = terrainData = Terrain.activeTerrain.terrainData;

        //Randomising the values of the trees
        maxTrees = Random.Range(2000, 15000);
        treeSpacing = Random.Range(8, 15);
        randomXRange = Random.Range(1f, 5f);
        randomZRange = Random.Range(1f, 5f);

        AddTree();

        isCreated = true;
    }

    void OnValidate()
    {
        if (isCreated)
        {
            AddTree();
        }
    }

    void AddTree()
    {
        //To store the tree prefabs that will be spawned
        TreePrototype[] trees = new TreePrototype[treeDataList.Count];

        for (int i = 0; i < treeDataList.Count; i++)
        {
            trees[i] = new TreePrototype();
            trees[i].prefab = treeDataList[i].treeMesh;
        }

        terrainData.treePrototypes = trees;

        //To store all of our instantiated trees on the terrain
        List<TreeInstance> treeInstanceList = new List<TreeInstance>();

        for (int z = 0; z < terrainData.size.z; z += treeSpacing)
        {
            for (int x = 0; x < terrainData.size.x; x += treeSpacing)
            {
                //Iterate each tree prefab type that will be used
                for (int treePrototypeIndex = 0; treePrototypeIndex < trees.Length; treePrototypeIndex++)
                {
                    //Spawn if maxTrees Count has not been reached
                    if (treeInstanceList.Count < maxTrees)
                    {
                        //Get the current height between 0 and 1
                        float currentHeight = terrainData.GetHeight(x, z) / terrainData.size.y;

                        //If the current height is within range of the current tree max and min height
                        if (currentHeight >= treeDataList[treePrototypeIndex].minHeight && currentHeight <= treeDataList[treePrototypeIndex].maxHeight)
                        {
                            //randomise x and z slightly so that the trees wont look like a grid
                            float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            TreeInstance treeInstance = new TreeInstance();

                            treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                            Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x, treeInstance.position.y * terrainData.size.y, treeInstance.position.z * terrainData.size.z) + this.transform.position;

                            RaycastHit raycastHit;
                            int layerMask = 1 << terrainLayerIndex;

                            //To prevent floating trees, send a raycast to detect the distance between floating tree and the terrain and reduce that distance.
                            if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) || Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                            {
                                //Correct the tree height
                                float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                //create the tree and set its values
                                treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);
                                treeInstance.rotation = Random.Range(0, 360);
                                treeInstance.prototypeIndex = treePrototypeIndex;
                                treeInstance.color = Color.white;
                                treeInstance.lightmapColor = Color.white;
                                treeInstance.heightScale = 1f;
                                treeInstance.widthScale = 1f;

                                //Add tree to the list of trees that will be spawned
                                treeInstanceList.Add(treeInstance);
                            }
                        }
                    }
                }
            }
        }
        //Setting the tree instances to the terrain
        terrainData.treeInstances = treeInstanceList.ToArray();
    }
}
