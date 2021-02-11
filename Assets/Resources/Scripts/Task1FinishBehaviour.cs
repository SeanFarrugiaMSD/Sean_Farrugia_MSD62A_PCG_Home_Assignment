using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Task1FinishBehaviour : MonoBehaviour
{
    Task1RandomSpawner myRandSpawn;
    bool hasHitWallAlready;

    // Start is called before the first frame update
    void Start()
    {
        hasHitWallAlready = false;

        //Adding the necessary Components and their values
        this.gameObject.AddComponent<PyramidGenerator>();
        this.gameObject.GetComponent<PyramidGenerator>().ShapeColour = new Color(1f, 0f, 0f);
        this.gameObject.GetComponent<PyramidGenerator>().Size = new Vector3(1f, 1f, 1f) * 2;

        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f) * 4;
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0f, 2f, 0f);

        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //If when spawned the finish object is clipping through a wall, spawn another finish object and destroy the current one
        if (other.gameObject.tag == "mazeWall" && !hasHitWallAlready)
        {
            hasHitWallAlready = true;

            myRandSpawn = GameObject.FindObjectOfType<Task1RandomSpawner>();
            myRandSpawn.SpawnFinish();

            Destroy(this.gameObject);
        }

        //If Player collides with this object, the game ends
        if (other.gameObject.tag == "playerObject")
        {
            SceneManager.LoadScene("Win_Scene");
        }

    }


}
