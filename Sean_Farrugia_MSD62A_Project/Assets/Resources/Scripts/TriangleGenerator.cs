using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleGenerator : ParentShapeGenerator
{
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

        //Instantiate a new meshGenerator with 1 submesh
        meshGenerator = new MeshGenerator(1);

        //Setting the vertices of triangle
        Vector3 v0 = new Vector3(-1 * size.x, 0 * size.y, -1 * size.z);
        Vector3 v1 = new Vector3(-1 * size.x, 0 * size.y, 1 * size.z);
        Vector3 v2 = new Vector3(1 * size.x, 0 * size.y, -1 * size.z);

        //Adding a triangle with its submesh to the meshGenerator
        meshGenerator.AddTriangleToMesh(v0, v1, v2, 0);

        //Create the final mesh with the given triangles and set it to the meshFilter
        meshFilter.mesh = meshGenerator.GenerateMesh();
    }
}
