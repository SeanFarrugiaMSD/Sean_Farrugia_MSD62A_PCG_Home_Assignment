using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1RandomSpawner : MonoBehaviour
{
    GameObject playerObject;
    GameObject finishObject;

    MazeGenerator mazeObject;

    // Start is called before the first frame update
    void Start()
    {
        //Get MazeObject to spawn in it
        mazeObject = GameObject.Find("Maze_Object").GetComponent<MazeGenerator>();
        playerObject = Resources.Load<GameObject>("Prefabs/Player_Object");

        SpawnPlayer();
        SpawnFinish();
    }

    void SpawnPlayer()
    {
        //Generate Random Position to spawn
        float randX = Random.Range(2, mazeObject.mazeSize - 2 * mazeObject.mazeScale.x);
        float randZ = Random.Range(2, mazeObject.mazeSize - 2 * mazeObject.mazeScale.z);

        //Spawn Player
        Instantiate(playerObject, new Vector3(randX, 0, randZ), Quaternion.identity);

        GameObject startMarker = new GameObject();
        startMarker.AddComponent<PyramidGenerator>();
        startMarker.GetComponent<PyramidGenerator>().ShapeColour = new Color(0f, 0.5f, 0f);
        startMarker.GetComponent<PyramidGenerator>().Size = new Vector3(1f, 1f, 1f) * 2;
        startMarker.transform.position = new Vector3(randX, 0, randZ);
    }

    public void SpawnFinish()
    {
        //Generate Random Position to spawn
        float randX = Random.Range(3, mazeObject.mazeSize - 3 * mazeObject.mazeScale.x);
        float randZ = Random.Range(3, mazeObject.mazeSize - 3 * mazeObject.mazeScale.z);

        //Check whether the random position is far enough from the player
        if(Vector3.Distance(new Vector3(randX, 0, randZ), playerObject.transform.position) > (mazeObject.mazeSize - 6) / 2)
        {
            finishObject = new GameObject();
            finishObject.AddComponent<Task1FinishBehaviour>();
            finishObject.transform.position = new Vector3(randX, 0, randZ);

        }
        //If not, respawn
        else
        {
            SpawnFinish();
        }        
    }
}
