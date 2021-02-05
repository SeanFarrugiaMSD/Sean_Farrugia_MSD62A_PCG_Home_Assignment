using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adding these components to the object
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ParentShapeGenerator : MonoBehaviour
{
    //Components used to generate and render the mesh
    protected MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    protected MeshGenerator meshGenerator;

    //To Store the materials of the mesh
    protected List<Material> materialList;

    //Stores the size of the pyramid generated
    [SerializeField] protected Vector3 size = Vector3.one;

    //To Store the amount of submeshes in the mesh
    [SerializeField] protected int submeshCount;

    //To know if the mesh has been created or not
    protected bool isCreated = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //Initialising the components
        meshFilter = this.GetComponent<MeshFilter>();
        meshRenderer = this.GetComponent<MeshRenderer>();

        //Generate initial mesh
        GenerateMesh();
        AddMaterials();

        isCreated = true;
    }

    // Called when a value in the inspector is changed to update the Mesh
    protected virtual void OnValidate()
    {
        //Only update mesh if it has been created
        if (isCreated)
        {
            GenerateMesh();
        }
    }

    //To Generate the mesh
    protected virtual void GenerateMesh()
    {
        //Instantiate a new meshGenerator with the amount of the given submesh
        meshGenerator = new MeshGenerator(submeshCount);

        //Child generates as required
    }

    protected virtual void AddMaterials()
    {
        materialList = new List<Material>();

        for (int i = 0; i < submeshCount; i++)
        {
            //Defining a new material and assigning the Specular Shader to it
            Material randColMat = new Material(Shader.Find("Specular"));

            //Assigning a random colour to the material
            randColMat.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            materialList.Add(randColMat);
        }

        //Initialise meshRenderer and assign materialList to the mesh renderer's materials list
        meshRenderer.materials = materialList.ToArray();
    }
}
