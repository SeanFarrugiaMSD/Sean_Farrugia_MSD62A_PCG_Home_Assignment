using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    //To Store the vertices of the mesh
    List<Vector3> vertices = new List<Vector3>();

    //To Store the indices of the vertices
    List<int> verticesIndices = new List<int>();

    //To Store the co-ordinates of the triangle's UV
    List<Vector2> triangleUVs = new List<Vector2>();

    //To Store the submeshes count and allocate the triangles under each one
    List<int>[] submeshIndices = new List<int>[] { };

    //To Store the direction of the vertices
    List<Vector3> normals = new List<Vector3>();

    //Constructor
    public MeshGenerator(int submeshCount)
    {
        submeshIndices = new List<int>[submeshCount];

        for (int i = 0; i < submeshCount; i++)
        {
            submeshIndices[i] = new List<int>();
        }
    }

    //Adding a new triangle and its data to the arrays
    public void AddTriangleToMesh(Vector3 v0, Vector3 v1, Vector3 v2, int subMesh)
    {
        //Creating the normal from the cross product of a line
        Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;

        for (int i = 0; i < 3; i++)
        {
            //Add the index of the current vertex in verticesIndices list
            verticesIndices.Add(vertices.Count + i);

            //Add the triangle to the submesh index it belongs to in submeshIndices list
            submeshIndices[subMesh].Add(vertices.Count + i);

            //Add the normal of the current vertex in normals list
            normals.Add(normal);
        }

        //Add each vertex to the vertices list
        vertices.AddRange(new Vector3[3] { v0, v1, v2 });

        //Add each UV coordinate to the UV list
        triangleUVs.AddRange(new Vector2[3] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) });
    }

    //Build the final Mesh from the triangles
    public Mesh GenerateMesh()
    {
        //Create new mesh and set its values
        Mesh finalMesh = new Mesh();
        finalMesh.vertices = vertices.ToArray();
        finalMesh.triangles = verticesIndices.ToArray();
        finalMesh.normals = normals.ToArray();
        finalMesh.uv = triangleUVs.ToArray();
        finalMesh.subMeshCount = submeshIndices.Length;

        for (int i = 0; i < submeshIndices.Length; i++)
        {
            //Generate a default triangle if there are not enough vertices to create a triangle
            if (submeshIndices[i].Count < 3)
            {
                finalMesh.SetTriangles(new int[3] { 0, 0, 0 }, i);
            }
            else
            {
                finalMesh.SetTriangles(submeshIndices[i].ToArray(), i);
            }
        }

        return finalMesh;
    }
}