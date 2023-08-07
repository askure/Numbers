using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text mapNameText,stageNameText,expText, MaxDamageText, MaxNumText, AveTurnText, ScoreText,GiftText,rankExpText;
    [SerializeField] GameObject rankup, newRecord;
    string filepath,mapfilepath;
    GameManger gameManger;
    void Start()
    {
        gameManger = new GameManger();
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        rankup.SetActive(false);
        newRecord.SetActive(false);
        gameManger.Dataload(filepath);
        gameManger.StageDataLoad(mapfilepath);
        SetText();
        gameManger.Datasave(filepath);
        gameManger.StageDataSave(mapfilepath);
 
        
    }
    void SetText()
    {
        mapNameText.text = CrectmapManager.MapManager.stageName;
        stageNameText.text = CrectmapManager.stage.stageName;
        expText.text = "経験値:"+GameManger.enemysexp.ToString("N0");
        GameManger.Exp += GameManger.enemysexp;
        MaxDamageText.text = "最大ダメージ:"+GameManger.maxDamage.ToString("N0");
        MaxNumText.text = "最大数値:"+GameManger.maxNum.ToString("N0");
        var averageTurn = (GameManger.aveTurn / CrectmapManager.enemy.Count);
        AveTurnText.text = "平均ターン:"+ averageTurn.ToString("N0");
        var score = GameManger.maxDamage / 10 + GameManger.maxNum * averageTurn * 1000;
        ScoreText.text = "スコア:"+ score.ToString("N0");
       
        if (CrectmapManager.Gift != null)
            GiftText.text = GiftTostring(CrectmapManager.Gift);
        
        else GiftText.text = "";
        if (gameManger.stages[CrectmapManager.stage.stageid].Hiscore < score) {
            gameManger.stages[CrectmapManager.stage.stageid].Hiscore = score;
            newRecord.SetActive(true);
        }

        
        gameManger.stages[CrectmapManager.stage.stageid].clear = true;
        CheckRankUp(score / 100);
        int rankExp = (gameManger.rank + 1) * (gameManger.rank + 1) * 100 - gameManger.rankExp;
        rankExpText.text = "次のランクまで" + rankExp.ToString("N0");

    }
    private string GiftTostring(List<StageEntity.Gifts> gifts)
    {
        string s = "";
        foreach (StageEntity.Gifts gift in gifts)
        {
            switch ((int)gift.Gift)
            {
                case 0:
                    s += "ストーン*" + gift.giftNum + "\n";
                    GameManger.Stone += gift.giftNum;
                    break;
                case 1:
                    s += "Item*" + gift.giftNum + "\n";
                    break;
                case 2:
                    var x = Random.Range(0, 1001);
                    
                    if (x > gift.drop) break;
                    if (gameManger.cardLvs[gift.card.cardID].pos) break;
                    s += gift.card.name + "\n";
                    gameManger.SetGachaData(gift.card.cardID, true);
                    break;
                default:
                    break;
            }
        }

        return s;
    }

    void RankUp()
    {
        gameManger.rank++;
        rankup.SetActive(true);
    }
    void CheckRankUp(int exp)
    {
        while (true)
        {
            int needExp = (gameManger.rank + 1) * (gameManger.rank + 1) * 100;
            if (gameManger.rankExp + exp < needExp)
            {
                gameManger.rankExp += exp;
                return;
            }
            exp -= needExp;
            RankUp();
            if(exp < 0)
            {
               gameManger.rankExp =  exp + needExp;
                return;
            }
        }
       
       
    }
}
