using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeEnvironmentEditor : MonoBehaviour
{
    //To Store the material for the skybox
    Material skyboxMat;

    // Start is called before the first frame update
    void Start()
    {
        //Setting the type of material and the colour of the skybox
        skyboxMat = new Material(Shader.Find("Skybox/Panoramic"));
        skyboxMat.color = new Color(0.46f, 0.5f, 0.6f);
        RenderSettings.skybox = skyboxMat;

        //Creating Fog For the terrain, the colour and its density
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.73f, 0.73f, 0.73f);
        RenderSettings.fogDensity = 0.002f;
    }
}
