using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mapNameText,stageNameText,expText, MaxDamageText, MaxNumText, AveTurnText, ScoreText,GiftText,rankExpText;
    [SerializeField] GameObject rankup, newRecord;
    
    void Start()
    {
        CharacterDataManager.DataLoad();
        DataManager.DataLoad();
        DataManager.StageDataLoad();
        rankup.SetActive(false);
        newRecord.SetActive(false);
        GameObject.Find("ResultBGM").GetComponent<AudioSource>().volume = DataManager.volume ;
        SetText();
        DataManager.DataSave();
        CharacterDataManager.DataSave(false);
        DataManager.StageDataSave();
 
        
    }
    void SetText()
    {
        if (SelectMapManager.MapManager == null || SelectMapManager.stage == null) return;
        mapNameText.text = SelectMapManager.MapManager.stageName;
        stageNameText.text = SelectMapManager.stage.stageName;
        expText.text = "�o���l:"+GameManger.enemysexp.ToString("N0");
        DataManager.Exp += GameManger.enemysexp;
        MaxDamageText.text = "�ő�_���[�W:"+GameManger.maxDamage.ToString("N0");
        MaxNumText.text = "�ő吔�l:"+GameManger.maxNum.ToString("N0");
        var averageTurn = (GameManger.aveTurn / SelectMapManager.enemy.Count);
        AveTurnText.text = "���σ^�[��:"+ averageTurn.ToString("N0");
        var score = GameManger.maxDamage / 10 + GameManger.maxNum * averageTurn * 1000 + GameManger.enemysexp/10;
        ScoreText.text = "�X�R�A:"+ score.ToString("N0");
       
        if (SelectMapManager.Gift != null)
            GiftText.text = GiftTostring(SelectMapManager.Gift);
        
        else GiftText.text = "";
        if (DataManager.stages[SelectMapManager.stage.stageid].Hiscore < score) {
            DataManager.stages[SelectMapManager.stage.stageid].Hiscore = score;
            newRecord.SetActive(true);
        }

        DataManager.stages[SelectMapManager.stage.stageid].clear = true;
        CheckRankUp(score / 20);

        int rankExp = (DataManager.rank + 1) * (DataManager.rank + 1) * 100 - DataManager.rankExp;
        rankExpText.text = "���̃����N�܂�" + rankExp.ToString("N0");

    }
    private string GiftTostring(List<StageEntity.Gifts> gifts)
    {
        string s = "";
        foreach (StageEntity.Gifts gift in gifts)
        {
            switch ((int)gift.Gift)
            {
                case 0:
                    s += "�X�g�[��*" + gift.giftNum + "\n";
                    DataManager.Stone += gift.giftNum;
                    break;
                case 1:
                    s += "Item*" + gift.giftNum + "\n";
                    break;
                case 2:
                    var x = Random.Range(0, 1001);
                    
                    if (x > gift.drop) break;
                    var cardId = gift.card.cardID;
                    s += gift.card.name;
                    if (CharacterDataManager.cardLvs[gift.card.cardID].pos)
                    {
                        s += "(�����ς�)\n";
                        CharacterDataManager.cardLvs[cardId].Lv += 5;
                        break;
                    }
                    s += "\n";
                    CharacterDataManager.cardLvs[gift.card.cardID].pos = true;
                    CharacterDataManager.cardLvs[cardId].Id = cardId;
                    CharacterDataManager.cardLvs[cardId].Lv = 1;
                    CharacterDataManager.cardLvs[cardId].expSum = 0;
                    break;
                default:
                    break;
            }
        }

        return s;
    }

    void RankUp()
    {
        DataManager.rank++;
        rankup.SetActive(true);
        DataManager.Exp += 100000;
        DataManager.Stone += 10;
    }
    void CheckRankUp(int exp)
    {
        while (true)
        {   

            int needExp = (DataManager.rank + 1) * (DataManager.rank + 1) * 100;
            if (DataManager.rankExp + exp < needExp)
            {
                DataManager.rankExp += exp;
                return;
            }
            exp -= needExp;
            RankUp();
            if(exp < 0)
            {
                DataManager.rankExp =  exp + needExp;
                return;
            }
        }
       
       
    }
}
