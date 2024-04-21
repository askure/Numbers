using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class DataManager
{

    public DataManager(string s)
    {
        DataLoad(s);
    }

    public DataManager()
    {

    }
    private void Awake()
    {
        var s = Application.persistentDataPath + "/" + ".savedata.json";
   
    }

    public bool FirstGame { set; get; } = false;
    public int prime_lv { set; get; } = 1;
    public int divisor_lv { set; get; } = 1;
    public int multi_lv { set; get; } = 1;
    public int rank { set; get; } = 1;
    public int rankExp { set; get; } = 0;
    public int Stone { set; get; } = 0;
    public int Exp { set; get; } = 0;
    public float volume { set; get; } = 0.5f;

    public int allStage { set; get; } = 0;
    public StageData.Stage[] stages { set; get; }=null;

    public bool Battle_tutorial { set; get; } = false;

    public bool enemystatus_tutorial { set; get; } = false;

    public bool charactor_tutorial { set; get; } = false;

    public bool endgame_tutorial { set; get; } = false;
    public bool status_tutorial { set; get; } = false;

    public  void DataLoad(string s)
    {
        UnityEngine.Debug.Log("Start UserDataLoad....");
        if (File.Exists(s))
        {

            StreamReader streamReader;
            streamReader = new StreamReader(s);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            var  playerstatus_save = JsonUtility.FromJson<Playerstatus>(data);
            FirstGame = playerstatus_save.FirstGame;
            divisor_lv = playerstatus_save.divisor_lv;
            multi_lv = playerstatus_save.multi_lv;
            prime_lv = playerstatus_save.prime_lv;
            rank = playerstatus_save.rank;
            Stone = playerstatus_save.stone;
            Exp = playerstatus_save.exp;
            rankExp = playerstatus_save.rankexp;
            volume = playerstatus_save.volume;
            enemystatus_tutorial = playerstatus_save.enemystatus_tutorial;
            charactor_tutorial = playerstatus_save.charactor_tutorial;
            endgame_tutorial = playerstatus_save.endgame_tutorial;
            status_tutorial = playerstatus_save.status_tutorial;
        }
        UnityEngine.Debug.Log("End UserDataLoad");

    }

    public void DataSave(string s)
    {
        UnityEngine.Debug.Log("Start UserDataSave....");
        Playerstatus playerstatus_save = new Playerstatus
        {
            FirstGame = FirstGame,
            divisor_lv = divisor_lv,
            multi_lv = multi_lv,
            prime_lv = prime_lv,
            rank = rank,
            stone = Stone,
            exp = Exp,
            rankexp = rankExp,
            volume = volume,
            enemystatus_tutorial = enemystatus_tutorial,
            charactor_tutorial = charactor_tutorial,
            endgame_tutorial = endgame_tutorial,
            status_tutorial = status_tutorial
        };
        string json = JsonUtility.ToJson(playerstatus_save, true);
        StreamWriter streamWriter = new StreamWriter(s);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
        UnityEngine.Debug.Log("End UserDataLoad");
    }

    public void DataInit(string s)
    {
        Playerstatus playerstatus_save = new Playerstatus
        {
            FirstGame = true,
            divisor_lv = 1,
            multi_lv = 1,
            prime_lv = 1,
            rank = 1,
            stone = 0,
            exp = 10000,
            rankexp = 0,
            volume = 0.3f,
            enemystatus_tutorial = false,
            charactor_tutorial = false,
            endgame_tutorial = false,
            status_tutorial = false
        };
        string json = JsonUtility.ToJson(playerstatus_save, true);
        StreamWriter streamWriter = new StreamWriter(s);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
    }
    public void StageDataSave(string s)
    {
        UnityEngine.Debug.Log("Start StageDataSave....");
        var stageData = new StageData();
        stageData.allstage = allStage;
        stageData.stage = stages;
        string json = JsonUtility.ToJson(stageData);
        StreamWriter streamWriter = new StreamWriter(s);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
        UnityEngine.Debug.Log("End StageDataSave");
    }

    public void StageDataLoad(string s)
    {
        UnityEngine.Debug.Log("Start StageDataLoad....");
        if (File.Exists(s))
        {

            StreamReader streamReader;
            streamReader = new StreamReader(s);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            var stageData = JsonUtility.FromJson<StageData>(data);
            allStage = stageData.allstage;
            stages = new StageData.Stage[allStage];

            for (int i = 0; i < stages.Length; i++)
            {
                if (i >= stageData.stage.Length)
                {
                    stages[i] = new StageData.Stage();
                    stages[i].clear = false;
                    stages[i].Hiscore = 0;
                    stages[i].stageid = i;
                }
                else
                {
                    stages[i] = new StageData.Stage();
                    stages[i] = stageData.stage[i];

                }
            }

        }
        UnityEngine.Debug.Log("End StageDataSave");
    }

    public void MapDataInit(string s, int Allstage, bool clear)
    {
        var stageData = new StageData();
        stageData.allstage = Allstage;
        stageData.stage = new StageData.Stage[Allstage];
        for (int i = 0; i < Allstage; i++)
        {
            stageData.stage[i] = new StageData.Stage();
            stageData.stage[i].stageid = i;
            stageData.stage[i].clear = clear;
            stageData.stage[i].Hiscore = 0;
        }
        string json = JsonUtility.ToJson(stageData, true);
        StreamWriter streamWriter = new StreamWriter(s);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
    }
}

