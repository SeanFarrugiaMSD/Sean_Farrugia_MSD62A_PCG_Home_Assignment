using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBehaviour : MonoBehaviour
{
    //Starting Position
    public Vector3 startPosition = new Vector3(17f, 0f, 8f);

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.AddComponent<PyramidGenerator>();
        this.gameObject.GetComponent<PyramidGenerator>().ShapeColour = new Color(0f, 1f, 0f);
        this.gameObject.GetComponent<PyramidGenerator>().Size = new Vector3(1f, 1f, 1f) * 2;

        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f) * 4;
        this.gameObject.GetComponent<BoxCollider>().center = new Vector3(0f, 2f, 0f);

        this.transform.position = startPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "playerObject")
        {
            
        }
    }
}
