using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adding these components to the object
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class TriangleGenerator : MonoBehaviour
{
    //Components used to generate and render the mesh
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshGenerator meshGenerator;

    //To Store the materials of the mesh
    List<Material> materialList;

    //Stores the size of the triangle generated
    [SerializeField] Vector3 size = Vector3.one;

    //To know if the mesh has been created or not
    bool isCreated = false;

    // Start is called before the first frame update
    void Start()
    {
        //Initialising the components
        meshFilter = this.GetComponent<MeshFilter>();
        meshRenderer = this.GetComponent<MeshRenderer>();

        //Generate initial mesh
        GenerateMesh();
        AddMaterials();

        isCreated = true;
    }

    //Called when a value in the inspector is changed to update the Mesh
    void OnValidate()
    {
        //Only update mesh if it has been created
        if (isCreated)
        {
            GenerateMesh();
        }
    }

    //To Generate the mesh
    void GenerateMesh()
    {
        //Instantiate a new meshGenerator with 1 submesh
        meshGenerator = new MeshGenerator(1);

        //Setting the vertices of triangle
        Vector3 v0 = new Vector3(0 * size.x, 0 * size.y, 0 * size.z);
        Vector3 v1 = new Vector3(0 * size.x, 0 * size.y, 1 * size.z);
        Vector3 v2 = new Vector3(1 * size.x, 0 * size.y, 0 * size.z);

        //Adding a triangle with its submesh to the meshGenerator
        meshGenerator.AddTriangleToMesh(v0, v1, v2, 0);

        //Create the final mesh with the given triangles and set it to the meshFilter
        meshFilter.mesh = meshGenerator.GenerateMesh();
    }

    //To add materials to the mesh
    void AddMaterials()
    {
        //Defining a new material and assigning the Specular Shader to it
        Material randColMat = new Material(Shader.Find("Specular"));

        //Assigning a random colour to the material
        randColMat.color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        //Add the new material to a new materialList
        materialList = new List<Material>();
        materialList.Add(randColMat);

        //assign materialList to the mesh renderer's materials list
        meshRenderer.materials = materialList.ToArray();
    }
}
