using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //Stores the size of the maze generated
    [SerializeField] public Vector3 mazeScale = Vector3.one;

    public int mazeSize = 52;

    bool isCreated = false;

    List<GameObject> mazeObjectsList;

    // Start is called before the first frame update
    void Start()
    {
        mazeObjectsList = new List<GameObject>();
        GenerateMaze();
        isCreated = true;
    }

    // Called when a value in the inspector is changed to update the Mesh
    void OnValidate()
    {
        //Only update mesh if it has been created
        if (isCreated)
        {
            GenerateMaze();
        }
    }

    //To Generate the mesh
    void GenerateMaze()
    {
        if(mazeObjectsList.Count > 0)
        {
            foreach(GameObject currentMazeObject in mazeObjectsList)
            {
                Destroy(currentMazeObject.gameObject);
            }
        }

        mazeObjectsList = new List<GameObject>();

        //Generate Base Plane
        GenerateBasePlane();

        //Generate Maze Walls
        //Vertical Border
        GenerateWall(mazeSize, mazeSize/2, 1f, mazeSize/2);
        GenerateWall(0, mazeSize / 2, 1f, mazeSize / 2);
        //Horizontal Border
        GenerateWall(mazeSize / 2, 0, mazeSize / 2, 1f);
        GenerateWall(mazeSize / 2, mazeSize, mazeSize / 2, 1f);
        //MazeWalls - Horizontals
        GenerateWall(6f, 12f, 5f, 1f);
        GenerateWall(14f, 22f, 5f, 1f);
        GenerateWall(16f, 32f, 15f, 1f);
        GenerateWall(16f, 42f, 5f, 1f);
        GenerateWall(36f, 42f, 5f, 1f);
        GenerateWall(36f, 22f, 5f, 1f);
        GenerateWall(46f, 12f, 5f, 1f);
        GenerateWall(47f, 32f, 5f, 1f);
        //MazeWalls - Verticals
        GenerateWall(12f, 38f, 1f, 5f);
        GenerateWall(20f, 12f, 1f, 11f);
        GenerateWall(30f, 21f, 1f, 10f);
        GenerateWall(42f, 37f, 1f, 6f);
    }

    void GenerateBasePlane()
    {
        //Generating the base Plane, setting its values and adding the required Components 
        GameObject basePlane = new GameObject("Base_Plane");
        basePlane.AddComponent<PlaneGenerator>();
        basePlane.GetComponent<PlaneGenerator>().Size = mazeScale;
        basePlane.GetComponent<PlaneGenerator>().PlaneHeight = mazeSize;
        basePlane.GetComponent<PlaneGenerator>().PlaneWidth = mazeSize;
        basePlane.GetComponent<PlaneGenerator>().ShapeColour = new Color(0.8f, 0.8f, 0.8f);
        basePlane.AddComponent<BoxCollider>();
        basePlane.GetComponent<BoxCollider>().size = new Vector3(mazeSize, 0.01f, mazeSize);
        basePlane.GetComponent<BoxCollider>().center = new Vector3(mazeSize/2, 0, mazeSize/2);

        basePlane.transform.SetParent(this.transform);

        mazeObjectsList.Add(basePlane);
    }

    void GenerateWall(float cubeX, float cubeZ, float cubeWidth, float cubeHeight)
    {
        //Generating Cube Objects as WallsGenerating Cube Objects as Walls, setting their values and adding the required Components
        GameObject wallObject = new GameObject("Wall");
        wallObject.transform.position = new Vector3(cubeX * mazeScale.x, 0, cubeZ * mazeScale.z);
        wallObject.AddComponent<CubeGenerator>();
        wallObject.GetComponent<CubeGenerator>().Size = new Vector3(cubeWidth * mazeScale.x, 3 * mazeScale.y, cubeHeight * mazeScale.z);
        wallObject.GetComponent<CubeGenerator>().ShapeColour = new Color(0.3f, 0.3f, 0.3f);
        wallObject.AddComponent<BoxCollider>();
        wallObject.GetComponent<BoxCollider>().size = new Vector3(cubeWidth * mazeScale.x, 3 * mazeScale.y, cubeHeight * mazeScale.z) * 2;
        wallObject.transform.SetParent(this.transform);

        wallObject.tag = "mazeWall";

        mazeObjectsList.Add(wallObject);
    }
}
