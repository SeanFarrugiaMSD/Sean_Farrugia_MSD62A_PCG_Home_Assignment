using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Speed of Player
    [SerializeField] float moveSpeed = 15f;

    //Starting Position
    public Vector3 startPosition = new Vector3(5f, 1f, 5f);

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<CubeGenerator>();
        this.gameObject.GetComponent<CubeGenerator>().ShapeColour = new Color(0.5f, 0f, 1f);

        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f) * 2;

        this.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
