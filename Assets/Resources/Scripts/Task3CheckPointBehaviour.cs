using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task3CheckPointBehaviour : MonoBehaviour
{
    Task3GameManager myTsk3GameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        myTsk3GameManager = GameObject.FindObjectOfType<Task3GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        //If Player collides with this object, destroy this object and add 1 to checkpoints
        if (other.gameObject.tag == "playerObject")
        {
            myTsk3GameManager.ChangeCheckpointCount(1);
            Destroy(this.gameObject);
        }

    }
}
