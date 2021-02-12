using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //If button is pressed, quit the application
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;

            #else
                Application.Quit();

            #endif
        });
    }
}
