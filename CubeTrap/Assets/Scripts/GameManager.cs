using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int winScreenWidth, winScrennHeigth;

    public GameObject backButton;
    public int currentLevel = 1;
    public float startTime;
    public Color defaultColorTimer;
    public Color warningColorTimer;
    public GameObject tokenParent;
    public GUISkin skin;

    public int tokenCount;
    private float highestScore;
    private float currentScore;
    private int numberOfLevels = 5;
    private float startTimeAtBegin;    
    private string currentTime;
    private int totalTokenCount;
    private int tokensLeft;
    private bool showWinScreen, showGameWinScreen;
    private AudioSource audio;
    private bool clokIsTicking = false;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();

        audio.clip = Resources.Load<AudioClip>("Sounds/level_music");
        audio.Play();
        GameObject backBtn = GameObject.Instantiate(backButton, new Vector3(70f, -60f + Screen.height, 0f), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
        Button bB = GameObject.FindGameObjectWithTag("BB").GetComponent<Button>();
        bB.onClick.AddListener(ReturnToMainMenu);


        startTimeAtBegin = startTime;
        totalTokenCount = tokenParent.transform.childCount;

        if(PlayerPrefs.GetInt("Level Completed") > 0)
        {
            currentLevel = PlayerPrefs.GetInt("Level Completed");
        }
        else
        {
            PlayerPrefs.SetInt("Level Completed", 1);
            currentLevel = 1;
        }
        //DontDestroyOnLoad(gameObject);

    }
    void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!showWinScreen)
        {
            startTime -= Time.deltaTime;
            currentTime = string.Format("{0:0.0}", startTime);
            tokensLeft = totalTokenCount - tokenCount;
            if (startTime <= 0)
            {
                startTime = 0;
                Destroy(gameObject);
                SceneManager.LoadScene(0);
            }
        }

        if(startTime < 5.5f && !clokIsTicking)
        {
            clokIsTicking = true;
            audio.Stop();
            audio.clip = Resources.Load<AudioClip>("Sounds/clock");
            audio.Play();
        }
       
    }
    public void CompleteLevel()
    {
        showWinScreen = true;
        audio.Stop();
        
        
    }
    void SaveGame()
    {
        PlayerPrefs.SetInt("Level Completed", currentLevel);
        PlayerPrefs.SetFloat("Level " + currentLevel.ToString() + " Score", currentScore);
    }

    void LoadNextLevel()
    {
        Time.timeScale = 1f;
        if (currentLevel < numberOfLevels)
        {
            showGameWinScreen = false;
            currentLevel += 1;
            SaveGame();
            SceneManager.LoadScene(currentLevel);         
        }
        else
        {
            showWinScreen = false;
            showGameWinScreen = true;
        }

    }

    public void AddToken()
    {
        tokenCount += 1;
    }

    private void OnGUI()
    {
        GUI.skin = skin;
        if(startTime <= 5f){
            skin.GetStyle("Timer").normal.textColor = warningColorTimer;
            
        }
        else{
            skin.GetStyle("Timer").normal.textColor = defaultColorTimer;
        }
        GUI.Label(new Rect(30, Screen.height - 100, 100, 100), "Level " + currentLevel.ToString(), skin.GetStyle("LevelLabel"));
        GUI.Label(new Rect(Screen.width - 200,110,400,400), currentTime + " s", skin.GetStyle("Timer"));
        GUI.Label(new Rect(30, 110, 100,100), tokensLeft.ToString() + " Tokens Left", skin.GetStyle("Tokens"));

        if (showWinScreen && !showGameWinScreen)
        {
            CalculateScore();
            Rect winScreenRect = new Rect(Screen.width / 2 - (Screen.width * 0.5f / 2), Screen.height / 2 - (Screen.height * 0.35f / 2), Screen.width*0.5f, Screen.height*0.35f);
            GUI.Box(winScreenRect, "Level Completed", skin.GetStyle("box"));
            if(GUI.Button(new Rect(winScreenRect.x + winScreenRect.width - 200, winScreenRect.y + winScreenRect.height - 80, 180, 60), "Next"))
            {
                LoadNextLevel();
            }
            if (GUI.Button(new Rect(winScreenRect.x + 20, winScreenRect.y + winScreenRect.height - 80, 210, 60), "Main Menu"))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            }

          
            GUI.Label(new Rect(winScreenRect.x + 20, winScreenRect.y + 80, 300, 50), "Your Score: " + string.Format("{0:0.0}", currentScore), skin.GetStyle("BoxText"));
            GUI.Label(new Rect(winScreenRect.x + 20, winScreenRect.y + 130, 300, 50), "Highest Score: " + string.Format("{0:0.0}", highestScore), skin.GetStyle("BoxText"));

        }
        if (showGameWinScreen && !showWinScreen)
        {
            Rect winScreenRect = new Rect(Screen.width / 2 - (Screen.width * 0.5f / 2), Screen.height / 2 - (Screen.height * 0.25f / 2), Screen.width * 0.5f, Screen.height * 0.25f);
            GUI.Box(winScreenRect, "Congratulations!\nGame is Complete!", skin.GetStyle("box"));
            if (GUI.Button(new Rect(winScreenRect.x + (winScreenRect.width/2) - 105, winScreenRect.y + winScreenRect.height - 80, 210, 60), "Main Menu"))
            {
                Time.timeScale = 1f;
                PlayerPrefs.SetInt("Level Completed", 1);
                SceneManager.LoadScene(0);
            }

        }
    }

    private void CalculateScore()
    {
        float expectedTimeToFinish = 0, timeDifferece = 0, bonusScore = 0, fullScore = 0;
        int percentageForEncrease = 0;
        if (currentLevel == 1)
        {
            expectedTimeToFinish = 10f;
            timeDifferece = Mathf.Abs(expectedTimeToFinish - startTime);
            fullScore = 250f;
            switch (tokenCount)
            {
                case 2:
                    percentageForEncrease = 30;
                    break;
                case 3:
                    percentageForEncrease = 40;
                    break;
                default:
                    percentageForEncrease = 0;
                    break;
            }
        }else if (currentLevel == 2)
        {
            expectedTimeToFinish = 5f;
            timeDifferece = Mathf.Abs(expectedTimeToFinish - startTime);
            fullScore = 400f;
            switch (tokenCount)
            {
                case 2:
                    percentageForEncrease = 20;
                    break;
                case 3:
                    percentageForEncrease = 40;
                    break;
                default:
                    percentageForEncrease = 0;
                    break;
            }
        }else if (currentLevel == 3)
        {
            expectedTimeToFinish = 10f;
            timeDifferece = Mathf.Abs(expectedTimeToFinish - startTime);
            fullScore = 600f;
            switch (tokenCount)
            {
                case 2:
                    percentageForEncrease = 5;
                    break;
                case 3:
                    percentageForEncrease = 10;
                    break;
                case 4:
                    percentageForEncrease = 15;
                    break;
                case 5:
                    percentageForEncrease = 40;
                    break;
                default:
                    percentageForEncrease = 0;
                    break;
            }
        } else if (currentLevel == 4)
        {
            expectedTimeToFinish = 5f;
            timeDifferece = Mathf.Abs(expectedTimeToFinish - startTime);
            fullScore = 850f;
            switch (tokenCount)
            {
                case 1:
                    percentageForEncrease = 2;
                    break;
                case 2:
                    percentageForEncrease =  5;
                    break;
                case 3:
                    percentageForEncrease = 10;
                    break;
                case 4:
                    percentageForEncrease = 20;
                    break;
                case 5:
                    percentageForEncrease = 30;
                    break;
                case 6:
                    percentageForEncrease = 40;
                    break;
                case 7:
                    percentageForEncrease = 45;
                    break;
                default:
                    percentageForEncrease = 0;
                    break;
            }
        }else if (currentLevel == 5)
        {
            expectedTimeToFinish = 5f;
            timeDifferece = Mathf.Abs(expectedTimeToFinish - startTime);
            fullScore = 850f;
            switch (tokenCount)
            {
                case 1:
                    percentageForEncrease = 2;
                    break;
                case 2:
                    percentageForEncrease = 4;
                    break;
                case 3:
                    percentageForEncrease = 6;
                    break;
                case 4:
                    percentageForEncrease = 10;
                    break;
                case 5:
                    percentageForEncrease = 15;
                    break;
                case 6:
                    percentageForEncrease = 20;
                    break;
                case 7:
                    percentageForEncrease = 30;
                    break;
                case 8:
                    percentageForEncrease = 35;
                    break;
                case 9:
                    percentageForEncrease = 40;
                    break;
                case 10:
                    percentageForEncrease = 45;
                    break;
                default:
                    percentageForEncrease = 0;
                    break;
            }
        }
        bonusScore = (timeDifferece * fullScore) / expectedTimeToFinish;
        if (startTime - expectedTimeToFinish > 0)
            currentScore = bonusScore + fullScore;
        else
        {
            currentScore = fullScore - bonusScore;
        }
        if(percentageForEncrease != 0)
         currentScore += (percentageForEncrease * fullScore) / 100f;
        highestScore = PlayerPrefs.GetFloat("Level " + currentLevel.ToString() + "Highest Score");
        if (currentScore > highestScore)
        {
            highestScore = currentScore;
            PlayerPrefs.SetFloat("Level " + currentLevel.ToString() + "Highest Score", highestScore);
        }
    }
}
