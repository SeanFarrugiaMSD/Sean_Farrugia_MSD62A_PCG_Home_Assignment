using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    GameObject playerPrefab;
    GameObject finishPrefab;
    GameObject playerObject;

    MazeGenerator mazeObject;

    // Start is called before the first frame update
    void Start()
    {
        //To Store the Player and Finish Prefabs to Instantiate
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player_Object");
        finishPrefab = Resources.Load<GameObject>("Prefabs/Finish_Object");

        //Get MazeObject to spawn in it
        mazeObject = GameObject.Find("Maze_Object").GetComponent<MazeGenerator>();

        SpawnPlayer();
        SpawnFinish();
    }

    void SpawnPlayer()
    {
        //Generate Random Position to spawn
        float randX = Random.Range(2, mazeObject.mazeSize - 2 * mazeObject.mazeScale.x);
        float randZ = Random.Range(2, mazeObject.mazeSize - 2 * mazeObject.mazeScale.z);

        //Spawn Player
        playerObject = Instantiate(playerPrefab, new Vector3(randX, 1, randZ), Quaternion.identity);
    }

    public void SpawnFinish()
    {
        //Generate Random Position to spawn
        float randX = Random.Range(3, mazeObject.mazeSize - 3 * mazeObject.mazeScale.x);
        float randZ = Random.Range(3, mazeObject.mazeSize - 3 * mazeObject.mazeScale.z);

        //Check whether the random position is far enough from the player
        if(Vector3.Distance(new Vector3(randX, 0, randZ), playerObject.transform.position) > (mazeObject.mazeSize - 6) / 2)
        {
            Instantiate(finishPrefab, new Vector3(randX, 0, randZ), Quaternion.identity);
            
        }
        //If not, respawn
        else
        {
            SpawnFinish();
        }        
    }
}
