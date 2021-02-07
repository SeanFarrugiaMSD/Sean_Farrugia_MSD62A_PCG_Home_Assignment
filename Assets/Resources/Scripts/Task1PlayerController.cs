using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task1PlayerController : MonoBehaviour
{
    //Movement Speed of Player
    [SerializeField] float moveSpeed = 15f;

    // Start is called before the first frame update
    void Start()
    {
        //Adding the necessary components and their values
        this.gameObject.tag = "playerObject";

        this.gameObject.AddComponent<CubeGenerator>();
        this.gameObject.GetComponent<CubeGenerator>().ShapeColour = new Color(0.5f, 0f, 1f);

        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f) * 2;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        //If Escape is press, the game stops
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;

            #else
                Application.Quit();

            #endif
        }

    }

    private void Move()
    {
        //Movement with Arrow Keys
        var deltaX = Input.GetAxis("Horizontal") * moveSpeed;
        var deltaZ = Input.GetAxis("Vertical") * moveSpeed;

        //Apply Velocity to the direction the player is pressing
        this.GetComponent<Rigidbody>().velocity = new Vector3(deltaX, 0, deltaZ);
    }
}
