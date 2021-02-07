using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task3FinishBehaviour : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "playerObject" && myTsk3GameManager.CheckpointCount >= 3)
        {
            myTsk3GameManager.WonLevel();
        }
    }
}
