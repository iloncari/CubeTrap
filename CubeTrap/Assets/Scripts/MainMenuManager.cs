using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public GUISkin skin;
    public Texture image;
    public Button infoBtn, soundBtn;
    public Sprite soundOffSprite, soundOnSprite;

    private bool showInfoBox = false;
    private AudioSource audio;

    private void Start()
    {
        infoBtn.onClick.AddListener(ShowInfoBox);
        soundBtn.onClick.AddListener(ChangeSoundStatus);
        audio = GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Sounds/menu_music");
        audio.Play();
    }
    private void ShowInfoBox()
    {
        showInfoBox = !showInfoBox;
    }
    private void ChangeSoundStatus()
    {
        if(soundBtn.GetComponent<Image>().sprite == soundOnSprite)
        {
            audio.mute = true;
            soundBtn.GetComponent<Image>().sprite = soundOffSprite;
        }
        else
        {
            audio.mute = false;
            soundBtn.GetComponent<Image>().sprite = soundOnSprite;
        }
    }
    private void OnGUI()
    {
        GUI.skin = skin;
        GUI.DrawTexture(new Rect(Screen.width / 2 - 250, 50, 500, 120), image);
        if (PlayerPrefs.GetInt("Level Completed") > 0)
        {
            if (GUI.Button(new Rect(Screen.width/2 - 150, 190, 300, 80), "Continue"))
            {
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level Completed"));
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 290, 300, 80), "New Game"))
            {
                PlayerPrefs.SetInt("Level Completed", 1);
                SceneManager.LoadScene(1);            
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 390, 300, 80), "Level Select"))
            {
                SceneManager.LoadScene("level_select");
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 490, 300, 80), "Quit"))
            {
                Application.Quit();
            }
        }else
        {
            PlayerPrefs.SetInt("Level Completed", 1);
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 190, 300, 80), "Continue"))
            {
                SceneManager.LoadScene(1);
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 290, 300, 80), "New Game"))
            {
                SceneManager.LoadScene(1);
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 390, 300, 80), "Level Select"))
            {
                SceneManager.LoadScene("level_select");
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 150, 490, 300, 80), "Quit"))
            {
                Application.Quit();
            }
        }
        if (showInfoBox)
        {
            GUI.Box(new Rect(Screen.width/2 - ((Screen.width / 3)/2), Screen.height/2 - ((Screen.height / 3) / 2), (Screen.width / 3), (Screen.height / 3)), "Made By:\n\nIgor Lončarić\nToni Bahčić\n\n2018", skin.GetStyle("Menu_info_box"));    
        }

    }
}
