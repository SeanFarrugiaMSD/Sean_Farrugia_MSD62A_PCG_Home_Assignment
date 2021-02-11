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
public class TreeGenerator : MonoBehaviour
{
    [SerializeField]
    private List<TreeData> treeDataList;

    [SerializeField]
    private int maxTrees = 15000;

    [SerializeField]
    private int treeSpacing = 15;

    [SerializeField]
    private float randomXRange = 5.0f;

    [SerializeField]
    private float randomZRange = 5.0f;

    [SerializeField]
    private int terrainLayerIndex = 8;

    private Terrain terrain;
    private TerrainData terrainData;

    bool isCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        terrain = this.gameObject.GetComponent<Terrain>();
        terrainData = terrain.terrainData;

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
                            float randomX = (x + Random.Range(-randomXRange, randomXRange)) / terrainData.size.x;
                            float randomZ = (z + Random.Range(-randomZRange, randomZRange)) / terrainData.size.z;

                            TreeInstance treeInstance = new TreeInstance();

                            treeInstance.position = new Vector3(randomX, currentHeight, randomZ);

                            Vector3 treePosition = new Vector3(treeInstance.position.x * terrainData.size.x, treeInstance.position.y * terrainData.size.y, treeInstance.position.z * terrainData.size.z) + this.transform.position;

                            RaycastHit raycastHit;
                            int layerMask = 1 << terrainLayerIndex;

                            if (Physics.Raycast(treePosition, Vector3.down, out raycastHit, 100, layerMask) || Physics.Raycast(treePosition, Vector3.up, out raycastHit, 100, layerMask))
                            {
                                float treeHeight = (raycastHit.point.y - this.transform.position.y) / terrainData.size.y;

                                treeInstance.position = new Vector3(treeInstance.position.x, treeHeight, treeInstance.position.z);
                                treeInstance.rotation = Random.Range(0, 360);
                                treeInstance.prototypeIndex = treePrototypeIndex;
                                treeInstance.color = Color.white;
                                treeInstance.lightmapColor = Color.white;
                                treeInstance.heightScale = 0.95f;
                                treeInstance.widthScale = 0.95f;

                                treeInstanceList.Add(treeInstance);
                            }
                        }
                    }
                }
            }
        }

        terrainData.treeInstances = treeInstanceList.ToArray();
    }
}
