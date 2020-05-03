using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.IO;

[Serializable]
public class MazeData
{
    public string size;
    public int minute, second;
}

public class GameManager : MonoBehaviour
{
    //Public Variables
    public GameObject mazeGen, winPanel, gamePanel, pausePanel;
    public Text winText, bestTimeText;
    public string mazeSize;
    public int minute, second;

    //Private Variables
    private float t;
    private string minutes, seconds;
    private bool timerPaused = false;
    private RandomMazeLoader mazeLoader;
    private FreePlayMazeLoader FPmazeLoader;

    // Start is called before the first frame update
    void Start()
    {
        //Check if the active scene is named Freeplay
        if (SceneManager.GetActiveScene().name == "Freeplay")
        {
            //If it is then get the FreeplayMazeLoader component from mazeGen gameObject
            FPmazeLoader = mazeGen.GetComponent<FreePlayMazeLoader>();

            //Set the timeScale of the game to 0.1
            Time.timeScale = 0.1f;
        }
        else if (SceneManager.GetActiveScene().name == "BestTime") //Else check if the active scene is named BestTime
        {
            //If it is then get the RandomMazeLoader component from mazeGen gameObject
            mazeLoader = mazeGen.GetComponent<RandomMazeLoader>();

            //Set the timeScale of the game to 1
            Time.timeScale = 1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the active scene is named Freeplay
        if (SceneManager.GetActiveScene().name == "Freeplay")
        {
            //If it is then set the mazeSize string to the current maze Rows and Columns value
            mazeSize = FPmazeLoader.mazeRows + "x" + FPmazeLoader.mazeColumns;
        }
        else if (SceneManager.GetActiveScene().name == "BestTime") //Else check if the active scene is named BestTime
        {
            //If it is then set the mazeSize string to the current maze Rows and Columns value
            mazeSize = mazeLoader.mazeRows + "x" + mazeLoader.mazeColumns;
        }
        
        //Check if timerPaused is false
        if(!timerPaused)
        {
            //If it is then increment t by deltaTime
            t += Time.deltaTime;
        }

        //Set minute value to t divided by 60
        minute = (int)t / 60;
        //Set second value to t modulus 60
        second = (int)(t % 60);

        //Convert the minute and second values to strings and set in relevant variables
        minutes = minute.ToString("00");
        seconds = second.ToString("00");
    }

    public void Pause()
    {
        //Set pausePanel to active, timesScale to 0 and call the PauseTimer method
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        PauseTimer();
    }

    public void Resume()
    {
        //Set pausePanel to inactive, timesScale to 1 and set timerPaused to false
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        timerPaused = false;
    }

    public void Win()
    {
        gamePanel.SetActive(false);
        winPanel.SetActive(true);

        //Check if the active scene is named BestTime
        if (SceneManager.GetActiveScene().name == "BestTime")
        {
            //If it is then set text of winText to this, call the PauseTimer method and the SaveCompletionStats method
            winText.text = "Completed a " + mazeLoader.mazeRows + " by " + mazeLoader.mazeColumns + " maze in " + minutes + " minute(s) and " + seconds + " second(s).";
            PauseTimer();
            SaveCompletionStats(this);

            //Check if game is running in the Unity editor
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }
        else if(SceneManager.GetActiveScene().name == "Freeplay")//Else check if the active scene is named Freeplay
        {
            //If it is then call the pauseTimer method and set the text of winText to this
            PauseTimer();
            winText.text = "Completed a " + FPmazeLoader.mazeRows + " by " + FPmazeLoader.mazeColumns + " maze.";
        }
    }

    //Not currently used
    public void Lose()
    {
        Time.timeScale = 0;
        PauseTimer();
    }

    public void PauseTimer()
    {
        //Set timerPaused to true
        timerPaused = true;
    }

    public void ResumeTimer()
    {
        //Set timerPaused to false
        timerPaused = false;
    }

    public void ResetTimer()
    {
        //Set t to 0
        t = 0.0f;
    }

    public void SaveCompletionStats(GameManager manager)
    {
        string fileName;
        string dataPath;
        bool betterTime;

        //Create new MazeData variable and set relevant information
        MazeData mData = new MazeData();
        mData.size = manager.mazeSize;
        mData.minute = manager.minute;
        mData.second = manager.second;

        //Set the fileName, adding .json at the end
        fileName = mData.size + ".json";

        //Set the value of betterTime by calling the CheckPlayerTime method
        betterTime = manager.CheckPlayerTime(fileName, mData);

        //Check if betterTime is true
        if(betterTime)
        {
            //If it is then do this

            //Set dataPath depending on what game is being run on
            #if UNITY_EDITOR
            dataPath = "Assets/Resources/";
            #elif UNITY_ANDROID && !UNITY_EDITOR
            dataPath = Application.persistentDataPath + "/";
            #elif UNITY_STANDALONE && !UNITY_EDITOR
            dataPath = Application.persistentDataPath + "/";
            #endif

            //Generate JSON representation of the fields of mData
            string mazeCompletionData = JsonUtility.ToJson(mData);

            using(FileStream fs = new FileStream(dataPath + fileName, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(mazeCompletionData);
                }
            }

            //Show message saying that a new best time has been achieved
            bestTimeText.gameObject.SetActive(true);
            bestTimeText.text = "A new best time for a " + mData.size + " maze has been recorded.";
        }
        else
        {
            //If it isn't then set the baseTimeText gameObject to inactive
            bestTimeText.gameObject.SetActive(false);
        }
    }

    bool CheckPlayerTime(string fileName, MazeData mazeData)
    {
        string json;
        DirectoryInfo levelDirPath;
        MazeData checkData;
        int trueFalse = 0;
        List<string> names = new List<string>();

        //Set levelDirPath depending on what game is being run on
        #if UNITY_EDITOR
        levelDirPath = new DirectoryInfo(Application.dataPath + "/Resources");
        #elif UNITY_ANDROID && !UNITY_EDITOR
        levelDirPath = new DirectoryInfo(Application.persistentDataPath);
        #elif UNITY_STANDALONE && !UNITY_EDITOR
        levelDirPath = new DirectoryInfo(Application.persistentDataPath);
        #endif

        //Get files at the levelDirPath, then create new FileInfo array and set each of the files to this array 
        FileInfo[] fileInfos = levelDirPath.GetFiles();

        foreach(FileInfo file in fileInfos)
        {
            //Check if the file name ends with .json
            if(file.Name.EndsWith(".json"))
            {
                //If it does then add it to the names list
                names.Add(file.Name);
            }
        }

        //Loop through names list
        for(int i = 0; i < names.Count; i++)
        {
            //Check if the fileName is the same as the element i of names
            if(fileName == names[i])
            {
                //If it is then read all the text from the file and set to the json variable
                json = File.ReadAllText(levelDirPath + "/" + names[i]);
                //Deserialize json information and set into checkData variable
                checkData = JsonUtility.FromJson<MazeData>(json);

                //Check if the mazeData minute value is less than or equal to checkData minute value
                if (mazeData.minute <= checkData.minute)
                {
                    //If it is then check if the mazeData second value is less than or equal to checkData second value
                    if(mazeData.second <= checkData.second)
                    {
                        //If it does then set trueFalse to 0
                        trueFalse = 0;
                    }
                    else
                    {
                        //If it isn't then set trueFalse to 1
                        trueFalse = 1;
                    }
                }
                else
                {
                    //Else set trueFalse to 1
                    trueFalse = 1;
                }
            }

            //Check if the fileName doesn't equal the element i of names and that i equals to names.count - 1
            if(fileName != names[i] && i == names.Count - 1)
            {
                //If both conditions are met then set trueFalse to 0
                trueFalse = 0;
            }
        }

        //Check if trueFalse equals 0
        if(trueFalse == 0)
        {
            //If it is then return true
            return true;
        }
        else
        {
            //Else return false
            return false;
        }
    }
}
