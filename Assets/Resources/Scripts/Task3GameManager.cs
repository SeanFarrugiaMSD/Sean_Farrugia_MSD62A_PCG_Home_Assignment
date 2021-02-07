using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Task3GameManager : MonoBehaviour
{
    Task3RandomSpawner myRandSpawn;

    Text checkpointText;
    Text levelText;

    int checkpointCount;

    string currentScene;
    public int CheckpointCount
    {
        get { return checkpointCount; }
        set { checkpointCount = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        myRandSpawn = GameObject.FindObjectOfType<Task3RandomSpawner>();
        checkpointText = GameObject.Find("CheckPoints_Text").GetComponent<Text>();
        levelText = GameObject.Find("Level_Text").GetComponent<Text>();

        currentScene = SceneManager.GetActiveScene().name;

        checkpointCount = 0;

        UpdateUI();
        StartCoroutine(WaitForLevel());
    }

    // Update is called once per frame
    void Update()
    {
        //If Escape is press, the game stops
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;

            #else
                Application.Quit();

            #endif
        }
    }

    IEnumerator WaitForLevel()
    {
        yield return new WaitForSeconds(0.5f);
        myRandSpawn.StartLevel();
    }

    public void ChangeCheckpointCount(int num)
    {
        checkpointCount += 1;
        UpdateUI();
    }

    void UpdateUI()
    {
        checkpointText.text = "Checkpoints: " + checkpointCount + " / 3";

        if (currentScene == "Task_3_LVL1")
        {
            levelText.text = "Level 1";
        }
        else if (currentScene == "Task_3_LVL2")
        {
            levelText.text = "Level 2";
        }
        else if (currentScene == "Task_3_LVL3")
        {
            levelText.text = "Level 3";
        }

    }

    public void WonLevel()
    {
        if(currentScene == "Task_3_LVL1")
        {
            SceneManager.LoadScene("Task_3_LVL2");
        }
        else if (currentScene == "Task_3_LVL2")
        {
            SceneManager.LoadScene("Task_3_LVL3");
        }
        else if (currentScene == "Task_3_LVL3")
        {
            SceneManager.LoadScene("Win_Scene");
        }
    }
}
