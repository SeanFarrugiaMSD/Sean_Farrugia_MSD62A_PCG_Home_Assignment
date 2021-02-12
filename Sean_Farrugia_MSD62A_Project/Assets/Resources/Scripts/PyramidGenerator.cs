using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidGenerator : ParentShapeGenerator
{
    // Start is called before the first frame update
    protected override void Start()
    {
        submeshCount = 5;
        base.Start();
    }

    //To Generate the mesh
    protected override void GenerateMesh()
    {
        base.GenerateMesh();

        //Instantiate a new meshGenerator with the amount of the given submesh
        meshGenerator = new MeshGenerator(submeshCount);

        Vector3 t0 = new Vector3(0, size.y * 2, 0);

        //rotation of 0f so angle 0, 
        Vector3 b0 = Quaternion.AngleAxis(0f, Vector3.up) * new Vector3(-size.x, 0, size.z);
        Vector3 b1 = Quaternion.AngleAxis(0f, Vector3.up) * new Vector3(size.x, 0, size.z);
        Vector3 b2 = Quaternion.AngleAxis(180f, Vector3.up) * new Vector3(-size.x, 0, size.z);
        Vector3 b3 = Quaternion.AngleAxis(180f, Vector3.up) * new Vector3(size.x, 0, size.z);

        //Creating BOTTOM face
        meshGenerator.AddTriangleToMesh(b0, b3, b2, 0);
        meshGenerator.AddTriangleToMesh(b0, b2, b1, 0);

        //Creating FRONT face
        meshGenerator.AddTriangleToMesh(b3, t0, b2, 1);

        //Creating BACK face
        meshGenerator.AddTriangleToMesh(b1, t0, b0, 2);

        //Creating LEFT face
        meshGenerator.AddTriangleToMesh(b0, t0, b3, 3);

        //Creating RIGHT face
        meshGenerator.AddTriangleToMesh(b2, t0, b1, 4);

        //Create the final mesh with the given triangles and set it to the meshFilter
        meshFilter.mesh = meshGenerator.GenerateMesh();
    }
}