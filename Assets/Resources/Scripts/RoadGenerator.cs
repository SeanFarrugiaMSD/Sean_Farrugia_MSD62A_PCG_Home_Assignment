using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Adding these components to the object
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class RoadGenerator : MonoBehaviour
{
    //To Store the road's radius
    [SerializeField] float roadRadius = 30f;
    public float RoadRadius
    {
        get { return roadRadius; }
        set { roadRadius = value; }
    }

    //Amount of segments the road will be divided into
    [SerializeField] float roadSegments = 300f;
    public float RoadSegments
    {
        get { return roadSegments; }
        set { roadSegments = value; }
    }

    //The middle white line width
    [SerializeField] float midLineWidth = 0.3f;

    //The width of the road
    [SerializeField] float roadWidth = 8f;

    //The width of the edge barrier 
    [SerializeField] float barrierWidth = 1f;

    //The height of the edge barrier
    [SerializeField] float barrierHeight = 1f;

    //To Store the amount of submeshes in the mesh
    [SerializeField] int submeshCount = 3;

    //Determines how wavy the road is
    [SerializeField] float roadWaviness = 5f;
    public float RoadWaviness
    {
        get { return roadWaviness; }
        set { roadWaviness = value; }
    }

    //Determines the size of the wave
    [SerializeField] float waveScale = 0.1f;
    public float WaveScale
    {
        get { return waveScale; }
        set { waveScale = value; }
    }

    //The Wave starting point
    [SerializeField] Vector2 waveOffset;
    public Vector2 WaveOffset
    {
        get { return waveOffset; }
        set { waveOffset = value; }
    }

    //How the wave steps from a wave to another
    [SerializeField] Vector2 waveStep = new Vector2(0.01f, 0.01f);
    public Vector2 WaveStep
    {
        get { return waveStep; }
        set { waveStep = value; }
    }

    //Used to make the barrier have alternating materials
    [SerializeField] bool stripeCheck = true;

    //The Components of the GameObject
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    MeshGenerator meshGenerator;

    //To Store the materials of the mesh
    List<Material> materialList;

    //To know if the mesh has been created or not
    bool isCreated = false;

    //Storing the vertices representing each segment;
    List<Vector3> segmentVertices;
    public List<Vector3> SegmentVertices
    {
        get { return segmentVertices; }
    }

    

    // Start is called before the first frame update
    void Start()
    {
        //Initialising the components
        meshFilter = this.GetComponent<MeshFilter>();
        meshRenderer = this.GetComponent<MeshRenderer>();
        meshCollider = this.GetComponent<MeshCollider>();

        //Generate initial mesh
        GenerateRoad();
        CreateMaterials();

        isCreated = true;
    }

    // Called when a value in the inspector is changed to update the Mesh
    void OnValidate()
    {
        //Only update mesh if it has been created
        if (isCreated)
        {
            GenerateRoad();
        }
    }

    void GenerateRoad()
    {
        //Instantiate a new meshGenerator with the amount of the given submesh
        meshGenerator = new MeshGenerator(submeshCount);

        //Determine the angle between each segment of the circle
        float segmentDegrees = 360f / roadSegments;

        //To Store the vertices representing each segment
        segmentVertices = new List<Vector3>();

        //Locate the segments of the circle, create a vertex representing it and add it to the vertex list
        for (float degrees = 0; degrees < 360f; degrees += segmentDegrees)
        {
            Vector3 segmentVertex = Quaternion.AngleAxis(degrees, Vector3.up) * Vector3.forward * roadRadius;

            segmentVertices.Add(segmentVertex);
        }

        //Applying noise to each segment point

        //Starting position
        Vector2 wave = waveOffset;

        //Loop through each point and apply noise
        for (int i = 0; i < segmentVertices.Count; i++)
        {
            wave += waveStep;

            //The direction from the center to the vertex
            Vector3 centreDirection = segmentVertices[i].normalized;

            //Creating the noise and multiplying it by the waviness to scale it accordingly
            float vertexNoise = Mathf.PerlinNoise(wave.x * waveScale, wave.y * waveScale) * roadWaviness;
            
            //To Smooth out the noise between each segment so that they connect correctly
            float vertexControl = Mathf.PingPong(i, segmentVertices.Count / 2f) / (segmentVertices.Count / 2f);

            //Applying the noise to the segment vertex and translate it away or closer to the center
            segmentVertices[i] += centreDirection * vertexNoise * vertexControl;
        }


        //Generate shape on each vertex in segmentVertices to form the segments and the roads
        for (int index = 1; index < segmentVertices.Count + 1; index++)
        {
            Vector3 vPrev = segmentVertices[index - 1];
            Vector3 vCurr = segmentVertices[index % segmentVertices.Count]; // Maybe needs to add modulus
            Vector3 vNext = segmentVertices[(index + 1) % segmentVertices.Count];

            AddSegmentToRoad(vPrev, vCurr, vNext);
        }

        //Create the final mesh with the given triangles and set it to the meshFilter
        meshFilter.mesh = meshGenerator.GenerateMesh();

        //Set collider's shape to that of the generated mesh
        meshCollider.sharedMesh = meshFilter.mesh;
    }

    void AddSegmentToRoad(Vector3 vPrev, Vector3 vCurr, Vector3 vNext)
    {
        //Generating Road's Middle White Line
        Vector3 startingPartPos = Vector3.zero;
        Vector3 partWidth = Vector3.forward * midLineWidth;
        GenerateSegmentPart(vPrev, vCurr, vNext, startingPartPos, partWidth, 0);

        //Generating the Road
        startingPartPos += partWidth;
        partWidth = Vector3.forward * roadWidth;
        GenerateSegmentPart(vPrev, vCurr, vNext, startingPartPos, partWidth, 1);

        int stripeSubmesh = 2;

        if (stripeCheck)
        {
            stripeSubmesh = 0;
        }

        stripeCheck = !stripeCheck;

        //Generating the Barrier - Vertical
        startingPartPos += partWidth;
        partWidth = Vector3.up * barrierHeight;
        GenerateSegmentPart(vPrev, vCurr, vNext, startingPartPos, partWidth, stripeSubmesh);

        //Generating the Barrier - Horizontal
        startingPartPos += partWidth;
        partWidth = Vector3.forward * barrierWidth;
        GenerateSegmentPart(vPrev, vCurr, vNext, startingPartPos, partWidth, stripeSubmesh);

        //Generating the Barrier - Back - Vertical
        startingPartPos += partWidth;
        partWidth = Vector3.down * barrierHeight;
        GenerateSegmentPart(vPrev, vCurr, vNext, startingPartPos, partWidth, stripeSubmesh);
    }

    void GenerateSegmentPart(Vector3 pPrev, Vector3 pCurr, Vector3 pNext, Vector3 startingPartPos, Vector3 partWidth, int submesh)
    {
        //The direction the segment should face to make sense in the road
        Vector3 roadDirection = (pNext - pCurr).normalized;
        //The previous segment's direction
        Vector3 prevRoadDirection = (pCurr - pPrev).normalized;
        
        int[] directionsArray = new int[2] { 1, -1 };

        foreach(int directionScale in directionsArray)
        {
            //How much the segment should be rotated to face the road direction
            Quaternion segmentAngle = Quaternion.LookRotation(Vector3.Cross(directionScale * roadDirection, Vector3.up));
            //The previous segment's rotation
            Quaternion prevSegmentAngle = Quaternion.LookRotation(Vector3.Cross(directionScale * prevRoadDirection, Vector3.up));

            //Defining the quad making up the segment part
            Vector3 v0 = pCurr + (prevSegmentAngle * startingPartPos);
            Vector3 v1 = pCurr + (prevSegmentAngle * (startingPartPos + partWidth));
            Vector3 v2 = pNext + (segmentAngle * (startingPartPos + partWidth));
            Vector3 v3 = pNext + (segmentAngle * startingPartPos);

            //Creating the triangles making up the quad
            if (directionScale == 1)
            {
                meshGenerator.AddTriangleToMesh(v0, v1, v2, submesh);
                meshGenerator.AddTriangleToMesh(v0, v2, v3, submesh);
            }
            else if (directionScale == -1)
            {
                meshGenerator.AddTriangleToMesh(v0, v2, v1, submesh);
                meshGenerator.AddTriangleToMesh(v0, v3, v2, submesh);
            }

        }

    }

    void CreateMaterials()
    {
        materialList = new List<Material>();

        //Adding new materials to the materialList
        AddMaterials(new Color(0.85f, 0.85f, 0.85f));
        AddMaterials(new Color(0.1f, 0.1f, 0.1f));
        AddMaterials(new Color(0.85f, 0.1f, 0.1f));

        //Initialise meshRenderer and assign materialList to the mesh renderer's materials list
        meshRenderer.materials = materialList.ToArray();
    }

    //Defining new materials and assigning the Specular Shader and a colour to it
    void AddMaterials(Color matColor)
    {
        Material newMaterial = new Material(Shader.Find("Specular"));
        newMaterial.color = matColor;
        materialList.Add(newMaterial);
    }
}
