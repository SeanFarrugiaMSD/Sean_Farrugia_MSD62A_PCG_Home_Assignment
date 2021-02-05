using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : ParentShapeGenerator
{
    //Storing the width of the plane
    [SerializeField] int planeWidth = 24;

    //Storing the height of the plane
    [SerializeField] int planeHeight = 24;

    // Start is called before the first frame update
    protected override void Start()
    {
        submeshCount = 1;
        base.Start();
    }

    //To Generate the mesh
    protected override void GenerateMesh()
    {
        base.GenerateMesh();

        //Instantiate a new meshGenerator with the amount of the given submesh
        meshGenerator = new MeshGenerator(submeshCount);

        //To Store the Plane's vertices
        Vector3[,] planeVertices = new Vector3[planeWidth, planeHeight];

        //Creating the Plane's vertices and storing them
        for (int x = 0; x < planeWidth; x++)
        {
            for (int z = 0; z < planeHeight; z++)
            {
                planeVertices[x, z] = new Vector3(size.x * x, 0, size.z * z);
            }
        }

        //Adding triangles depending on the vertices created
        for (int x = 0; x < planeWidth - 1; x++)
        {
            for (int y = 0; y < planeHeight - 1; y++)
            {
                Vector3 v0 = planeVertices[x, y];
                Vector3 v1 = planeVertices[x + 1, y];
                Vector3 v2 = planeVertices[x, y + 1];
                Vector3 v3 = planeVertices[x + 1, y + 1];

                meshGenerator.AddTriangleToMesh(v0, v2, v3, 0);
                meshGenerator.AddTriangleToMesh(v0, v3, v1, 0);
            }
        }

        //Create the final mesh with the given triangles and set it to the meshFilter
        meshFilter.mesh = meshGenerator.GenerateMesh();
    }
}
