using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Text mapNameText,stageNameText,expText, MaxDamageText, MaxNumText, AveTurnText, ScoreText,GiftText,rankExpText;
    [SerializeField] GameObject rankup, newRecord;
    
    string mapfilepath;
    string cfilepath ;
    string dfilepath;
    CharacterDataManager cmanager ;
    DataManager dmanager ;
    void Start()
    {
        cmanager = new CharacterDataManager(cfilepath);
        dmanager = new DataManager(dfilepath);
        cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        dfilepath = Application.persistentDataPath + "/" + ".savedata.json";
        mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        cmanager = new CharacterDataManager(cfilepath);
        dmanager = new DataManager(dfilepath);

        rankup.SetActive(false);
        newRecord.SetActive(false);
        dmanager.StageDataLoad(mapfilepath);
        GameObject.Find("ResultBGM").GetComponent<AudioSource>().volume =dmanager.volume ;
        SetText();
        dmanager.DataSave(dfilepath);
        dmanager.StageDataSave(mapfilepath);
 
        
    }
    void SetText()
    {
        mapNameText.text = CrectmapManager.MapManager.stageName;
        stageNameText.text = CrectmapManager.stage.stageName;
        expText.text = "経験値:"+GameManger.enemysexp.ToString("N0");
        dmanager.Exp += GameManger.enemysexp;
        MaxDamageText.text = "最大ダメージ:"+GameManger.maxDamage.ToString("N0");
        MaxNumText.text = "最大数値:"+GameManger.maxNum.ToString("N0");
        var averageTurn = (GameManger.aveTurn / CrectmapManager.enemy.Count);
        AveTurnText.text = "平均ターン:"+ averageTurn.ToString("N0");
        var score = GameManger.maxDamage / 10 + GameManger.maxNum * averageTurn * 1000;
        ScoreText.text = "スコア:"+ score.ToString("N0");
       
        if (CrectmapManager.Gift != null)
            GiftText.text = GiftTostring(CrectmapManager.Gift);
        
        else GiftText.text = "";
        if (dmanager.stages[CrectmapManager.stage.stageid].Hiscore < score) {
            dmanager.stages[CrectmapManager.stage.stageid].Hiscore = score;
            newRecord.SetActive(true);
        }

        
        dmanager.stages[CrectmapManager.stage.stageid].clear = true;
        CheckRankUp(score / 100);
        int rankExp = (dmanager.rank + 1) * (dmanager.rank + 1) * 100 - dmanager.rankExp;
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
                    dmanager.Stone += gift.giftNum;
                    break;
                case 1:
                    s += "Item*" + gift.giftNum + "\n";
                    break;
                case 2:
                    var x = Random.Range(0, 1001);
                    
                    if (x > gift.drop) break;
                    if (cmanager.cardLvs[gift.card.cardID].pos) break;
                    s += gift.card.name + "\n";
                    var cardId = gift.card.cardID;
                    cmanager.cardLvs[gift.card.cardID].pos = true;
                    cmanager.cardLvs[cardId].Id = cardId;
                    cmanager.cardLvs[cardId].Lv = 1;
                    cmanager.cardLvs[cardId].expSum = 0;
                    break;
                default:
                    break;
            }
        }

        return s;
    }

    void RankUp()
    {
        dmanager.rank++;
        rankup.SetActive(true);
    }
    void CheckRankUp(int exp)
    {
        while (true)
        {
            int needExp = (dmanager.rank + 1) * (dmanager.rank + 1) * 100;
            if (dmanager.rankExp + exp < needExp)
            {
                dmanager.rankExp += exp;
                return;
            }
            exp -= needExp;
            RankUp();
            if(exp < 0)
            {
               dmanager.rankExp =  exp + needExp;
                return;
            }
        }
       
       
    }
}
