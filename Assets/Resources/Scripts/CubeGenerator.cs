using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : ParentShapeGenerator
{
    // Start is called before the first frame update
    protected override void Start()
    {
        submeshCount = 6;
        base.Start();
    }

    //To Generate the mesh
    protected override void GenerateMesh()
    {
        base.GenerateMesh();

        //Instantiate a new meshGenerator with the amount of the given submesh
        meshGenerator = new MeshGenerator(submeshCount);

        //Bottom vertices
        Vector3 b0 = new Vector3(-1 * size.x, -1 * size.y, -1 * size.z); //bottom left vertex
        Vector3 b1 = new Vector3(-1 * size.x, -1 * size.y, 1 * size.z); //top left vertex
        Vector3 b2 = new Vector3(1 * size.x, -1 * size.y, 1 * size.z); //top right vertex
        Vector3 b3 = new Vector3(1 * size.x, -1 * size.y, -1 * size.z); //bottom right vertex

        //Top vertices
        Vector3 t0 = new Vector3(-1 * size.x, 1 * size.y, -1 * size.z); //bottom left vertex
        Vector3 t1 = new Vector3(-1 * size.x, 1 * size.y, 1 * size.z); //top left vertex
        Vector3 t2 = new Vector3(1 * size.x, 1 * size.y, 1 * size.z); //top right vertex
        Vector3 t3 = new Vector3(1 * size.x, 1 * size.y, -1 * size.z); //bottom right vertex

        //Creating BOTTOM face by adding triangles to the mesh
        meshGenerator.AddTriangleToMesh(b0, b3, b2, 0);
        meshGenerator.AddTriangleToMesh(b0, b2, b1, 0);

        //Creating FRONT face by adding triangles to the mesh
        meshGenerator.AddTriangleToMesh(t0, t3, b3, 1);
        meshGenerator.AddTriangleToMesh(t0, b3, b0, 1);

        //Creating TOP face by adding triangles to the mesh
        meshGenerator.AddTriangleToMesh(t0, t1, t2, 2);
        meshGenerator.AddTriangleToMesh(t0, t2, t3, 2);        

        //Creating BACK face by adding triangles to the mesh
        meshGenerator.AddTriangleToMesh(t1, b2, t2, 3);
        meshGenerator.AddTriangleToMesh(t1, b1, b2, 3);

        //Creating LEFT face by adding triangles to the mesh
        meshGenerator.AddTriangleToMesh(t0, b1, t1, 4);
        meshGenerator.AddTriangleToMesh(t0, b0, b1, 4);

        //Creating RIGHT face by adding triangles to the mesh
        meshGenerator.AddTriangleToMesh(t3, t2, b2, 5);
        meshGenerator.AddTriangleToMesh(t3, b2, b3, 5);

        //Create the final mesh with the given triangles and set it to the meshFilter
        meshFilter.mesh = meshGenerator.GenerateMesh();
    }
}
