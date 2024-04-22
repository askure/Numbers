using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class DataManager
{
    static readonly string FilePath = Application.persistentDataPath + "/" + ".savedata.json";
    static readonly string StageDataFilePath = Application.persistentDataPath + "/" + ".savemapdata.json";
    static readonly int AllStages = 40;
    static bool IsDataLoad = false;
    static bool IsStageDataLoaded = false;

    static public bool FirstGame { set; get; } = false;
    static public int prime_lv { set; get; } = 1;
    static public int divisor_lv { set; get; } = 1;
    static public int multi_lv { set; get; } = 1;
    static public int rank { set; get; } = 1;
    static public int rankExp { set; get; } = 0;
    static public int Stone { set; get; } = 0;
    static public int Exp { set; get; } = 0;
    static public float volume { set; get; } = 0.5f;

    static public int allStage { set; get; } = 0;
    static public StageData.Stage[] stages { set; get; }=null;

    static public bool Battle_tutorial { set; get; } = false;

    static public bool enemystatus_tutorial { set; get; } = false;

    static public bool charactor_tutorial { set; get; } = false;

    static public bool endgame_tutorial { set; get; } = false;
    static public bool status_tutorial { set; get; } = false;

    public static void DataLoad()
    {
        if (IsDataLoad) return;
        UnityEngine.Debug.Log("Start UserDataLoad....");
        if (File.Exists(FilePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(FilePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            var playerstatus_save = JsonUtility.FromJson<Playerstatus>(data);
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
        else
        {
            DataInit();
        }
        IsDataLoad = true;
        UnityEngine.Debug.Log("End UserDataLoad");

    }

    public static void StageDataLoad()
    {
        if (IsStageDataLoaded) return;
        UnityEngine.Debug.Log("Start StageDataLoad....");
        if (File.Exists(StageDataFilePath))
        {
            StreamReader streamReader;
            streamReader = new StreamReader(StageDataFilePath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            var stageData = JsonUtility.FromJson<StageData>(data);
            stages = new StageData.Stage[AllStages];

            for (int i = 0; i < stages.Length; i++)
            {
                if (i >= stageData.stage.Length)
                {
                    stages[i] = new StageData.Stage
                    {
                        clear = false,
                        Hiscore = 0,
                        stageid = i
                    };
                }
                else
                {
                    stages[i] = new StageData.Stage();
                    stages[i] = stageData.stage[i];
                }
            }
        }
        else
        {
            StageDataInit(false);
        }
        IsStageDataLoaded = true;
        UnityEngine.Debug.Log("End StageDataLoad");
    }

    public static void DataSave()
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
        StreamWriter streamWriter = new StreamWriter(FilePath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
        UnityEngine.Debug.Log("End UserDataSave");
    }

    public static void StageDataSave()
    {
        UnityEngine.Debug.Log("Start StageDataSave....");
        var stageData = new StageData();
        stageData.allstage = allStage;
        stageData.stage = stages;
        string json = JsonUtility.ToJson(stageData);
        StreamWriter streamWriter = new StreamWriter(StageDataFilePath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
        UnityEngine.Debug.Log("End StageDataSave");
    }

    private static void DataInit()
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
        StreamWriter streamWriter = new StreamWriter(FilePath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
    }

    public static void StageDataInit(bool clear)
    {
        var stageData = new StageData
        {
            allstage = AllStages,
            stage = new StageData.Stage[AllStages]
        };

        for (int i = 0; i < AllStages; i++)
        {
            stageData.stage[i] = new StageData.Stage
            {
                stageid = i,
                clear = clear,
                Hiscore = 0
            };
        }
        string json = JsonUtility.ToJson(stageData, true);
        StreamWriter streamWriter = new StreamWriter(StageDataFilePath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
    }
 
}

