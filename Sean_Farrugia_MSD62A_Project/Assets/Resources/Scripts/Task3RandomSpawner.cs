using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task3RandomSpawner : MonoBehaviour
{
    GameObject carPrefab;
    RoadGenerator myRoadGen;
    

    // Start is called before the first frame update
    void Start()
    {
        carPrefab = Resources.Load<GameObject>("Prefabs/Car");
        RandomGenerateMap();
    }

    public void StartLevel()
    {
        int randSeg = Random.Range(0, myRoadGen.SegmentVertices.Count);

        GameObject playerCar = Instantiate(carPrefab, myRoadGen.SegmentVertices[randSeg], Quaternion.identity);
        playerCar.transform.LookAt(myRoadGen.SegmentVertices[randSeg + 1]);

        GameObject finishObject = new GameObject("Finish_Object");
        finishObject.transform.position = myRoadGen.SegmentVertices[randSeg];
        finishObject.transform.LookAt(myRoadGen.SegmentVertices[(randSeg + 1) % myRoadGen.SegmentVertices.Count]);
        finishObject.AddComponent<CubeGenerator>();
        finishObject.GetComponent<CubeGenerator>().ShapeColour = new Color(0f, 1f, 0f, 0.7f);
        finishObject.GetComponent<CubeGenerator>().MaterialType = "UI/Unlit/Transparent";
        finishObject.GetComponent<CubeGenerator>().Size = new Vector3(9f, 0.4f, 1f);
        finishObject.AddComponent<Task3FinishBehaviour>();
        finishObject.AddComponent<BoxCollider>();
        finishObject.GetComponent<BoxCollider>().size = finishObject.GetComponent<CubeGenerator>().Size * 2;
        finishObject.GetComponent<BoxCollider>().isTrigger = true;

        int segmentsBetweenCheckpoint = myRoadGen.SegmentVertices.Count / 4;

        for(int i = 1; i < 4; i++)
        {
            GameObject checkpointObject = new GameObject("CheckPoint_Object");
            checkpointObject.transform.position = myRoadGen.SegmentVertices[(randSeg + (i * segmentsBetweenCheckpoint)) % myRoadGen.SegmentVertices.Count];
            checkpointObject.transform.LookAt(myRoadGen.SegmentVertices[(((randSeg + (i * segmentsBetweenCheckpoint)) % myRoadGen.SegmentVertices.Count) + 1) % myRoadGen.SegmentVertices.Count]);
            checkpointObject.AddComponent<CubeGenerator>();
            checkpointObject.GetComponent<CubeGenerator>().ShapeColour = new Color(1f, 0f, 0f, 0.3f);
            checkpointObject.GetComponent<CubeGenerator>().MaterialType = "UI/Unlit/Transparent";
            checkpointObject.GetComponent<CubeGenerator>().Size = new Vector3(9f, 8f, 1f);
            checkpointObject.AddComponent <Task3CheckPointBehaviour>();
            checkpointObject.AddComponent<BoxCollider>();
            checkpointObject.GetComponent<BoxCollider>().size = checkpointObject.GetComponent<CubeGenerator>().Size * 2;
            checkpointObject.GetComponent<BoxCollider>().isTrigger = true;
        }

    }

    void RandomGenerateMap()
    {
        GameObject planeObject = new GameObject("Plane Object");
        PlaneGenerator myPlaneGen = planeObject.AddComponent<PlaneGenerator>();
        myPlaneGen.Size = new Vector3(40f,0f, 40f);
        planeObject.transform.position = new Vector3(-450f, -1f, -450f);

        GameObject roadObject = new GameObject("Road_Object");
        myRoadGen = roadObject.AddComponent<RoadGenerator>();

        myRoadGen.RoadRadius = Random.Range(80, 111);
        myRoadGen.RoadSegments = Random.Range(260, 360);
        myRoadGen.RoadWaviness = Random.Range(80, 201);
        myRoadGen.WaveScale = Random.Range(1.6f, 2.4f);
        myRoadGen.WaveOffset = new Vector2(Random.Range(0.65f, 1.65f), Random.Range(1.35f, 2.5f));
        myRoadGen.WaveStep = new Vector2(Random.Range(0.01f, 0.02f), Random.Range(0.01f, 0.03f));
    }
}
