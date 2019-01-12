using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int levelToLoad;
    public GameObject padlock;
    public GUISkin skin;

    private string loadPrompt;
    private bool inRange, canLoadLevel;
    private int completedLevel;
    

    private void Start()
    {
        completedLevel = PlayerPrefs.GetInt("Level Completed");
        canLoadLevel = levelToLoad <= completedLevel ? true : false;
        if (!canLoadLevel)
        {
            Instantiate(padlock, new Vector3(transform.position.x, 2.8f, transform.position.z), Quaternion.identity);
        }
    }
    void OnTriggerStay(Collider other)
    {
        inRange = true;
        if (canLoadLevel)
        {
            loadPrompt = "Press [E] or [space]\nto load level " + levelToLoad.ToString();
        }
        else
        {
            loadPrompt = "Level " + levelToLoad.ToString() + " is locked";
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        loadPrompt = "";
        inRange = false;
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.Label(new Rect(50, Screen.height * 0.9f, 200, 40), loadPrompt, skin.GetStyle("LevelSelectLabel"));
    }
    private void Update()
    {
        if (canLoadLevel && inRange && Input.GetButtonDown("Action"))
        {
            SceneManager.LoadScene("Level" + levelToLoad.ToString());
        }
    }
}
