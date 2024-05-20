using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System;
using System.Linq;
using UnityEngine.Networking;
using Goldmetal.UndeadSurvivor;

    



public class DataMiner : MonoBehaviour
{
    public static DataMiner dataMiner;

    public GameManager gameManager;


    StreamWriter file;
    int visuals = 0; // Beau 0, Terne, 1, Laide 2
    int variety = 0; // Yes 0, No 1
    KeyValuePair<int, int> currentTime;
    public static string workerID = "";
    bool dataHasBeenSent = false;
    private static int gameCompleted;
    
  


    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        dataMiner = GameObject.Find("DataMiner").GetComponent<DataMiner>();
    }

    private void Start()
    {
        Debug.Log("Visual: " + visuals);
        Debug.Log("Variety: " + variety);
    }

    public void LogWorkerID(string id)
    {
        workerID = id;
        Debug.Log("Worker ID: " + workerID);
    }
    public void LogData()
{
    string logString = "Visuals: " + visuals + ", Variety: " + variety;
    Debug.Log(logString);
}



    ////Stores timeStamp, modifies adjustedTime to reflect time stamp in seconds
    //void AdjustTimeStamp(ref List<KeyValuePair<int, int>> log)
    //{
    //    currentTime = GameManager.gameManager.TimeStamp();
    //    if (log.Count == 0) log.Add(GameManager.gameManager.TimeStamp());
    //    else
    //    {
    //        int minute = currentTime.Key - log[log.Count - 1].Key;
    //        int second = currentTime.Value - log[log.Count - 1].Value;
    //        log.Add(new KeyValuePair<int, int>(minute, second));
    //    }
    //    int numberOfSeconds = currentTime.Value + (currentTime.Key * 60);
    //    adjustedTimeStamp = numberOfSeconds.ToString();
    //}

    //ID, timeStamp, pointerVersion, logVersion, funnyVersion, totalTime, totalPauseTime, totalDeaths,
    //totalAverageDifficulty, questsCompleted, q1Time, q1Deaths, q1AvgDifficulty, q1Complete, q1NpcInteractions, q1NpcsInteractedWith...
    /*public void logdata()
    {
        string positionlogstring = workerid + "," + datetime.now + ",";
        foreach (vector2 pos in positionlog)
        {
            int tempx = mathf.roundtoint(pos.x);
            int tempy = mathf.roundtoint(pos.y);
            positionlogstring += tempx + ":" + tempy + "|";
        }
        debug.log(positionlogstring);
        if (!datahasbeensent)
        {
            datahasbeensent = true;
            logstring += workerid + "," + datetime.now + "," + gamemanager.gamemanager.questpointerversion() + ","
                + gamemanager.gamemanager.questlogversion() + "," + gamemanager.gamemanager.funnyversion() + ",";
            list<keyvaluepair<int, int>> blank = new list<keyvaluepair<int, int>>();
            adjusttimestamp(ref blank);
                           totaltime 
            logstring += adjustedtimestamp + "," + gamemanager.gamemanager.getpausetime() + "," +
                totaldeaths + "," + gamemanager.gamemanager.getfinaldifficultyaverage() + "," +
                questcompleted + ",";

            for all quests incomplete, adds default values in their place
            if (questlogstrings.count < numberoflogablequests)
            {
                logquestcompletion(false);
                while (questlogstrings.count < numberoflogablequests)
                {
                    questlogstrings.add(-1 + "," + -1 + "," + -1 + "," + "false, " + -1 + "," + -1 + ",");
                }
            }

            adds quest data to final log string
            foreach (string s in questlogstrings) { logstring += s; }

            startcoroutine(writetextviaphp(logstring, "https://gamesux.com/fromunity_aesthetics.php "));
            startcoroutine(writetextviaphp(positionlogstring, "https://gamesux.com/fromunity_aesthetics_location.php"));
        }
    }*/


    IEnumerator WriteTextViaPHP(string data, string destination)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", data);
        UnityWebRequest www = UnityWebRequest.Post(destination, form);
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            www.SetRequestHeader("User-Agent", "Unity 2019");
        www.SendWebRequest();
        yield return www.isDone;
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("good");
        }
    }
}
