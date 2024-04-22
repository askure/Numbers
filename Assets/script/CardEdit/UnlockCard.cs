using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnlockCard : MonoBehaviour
{
    string dfilepath;
    static int  NeedExp = 0;
    public void Unlockcard()
    {
        int cardId = CardEditManager.UnlockCardId.cardID;
        if (cardId == -1) return;
        CharacterDataManager.cardLvs[cardId].pos = true;
        CharacterDataManager.cardLvs[cardId].Lv = 1;
        DataManager.Exp -= NeedExp;
        CharacterDataManager.DataSave(false);
        DataManager.DataSave();
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeckEdit");

    }

    public void UnlockCardPanel()
    {
        if (!Input.GetMouseButtonUp(0))
            return;
        int nowExp = DataManager.Exp;
        var rare = GetComponent<CardController>().model.rare;
        CardEditManager.UnlockCardId = GetComponent<CardController>().model;
        var i = CardEditManager.UnlockCardId.advent;
        switch (rare)
        {
            case "A":
                NeedExp = 100000;
                break;
            case "S":
                NeedExp = 200000;
                break;
            case "SS":
                NeedExp = 400000;
                break;
            default:
                break;
        }
        
        var gameObject = Resources.Load<GameObject>("DeckEditPrehub/CharacterUnlock");
        var canvas = GameObject.Find("Canvas").transform;
        var panel = Instantiate(gameObject,canvas);
        if (i)
        {
            panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "このキャラは解放できません\n\nこのキャラはクエストで入手可能です\n\nクエスト報酬をチェック！";

            var x = panel.transform.GetChild(1).gameObject;
            Destroy(x.transform.GetChild(0).gameObject);
        }else  if (nowExp < NeedExp)
        {
            panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n 消費EXP" + NeedExp + "\n不足しています。\n現在のEXP:" + nowExp;
            var x = panel.transform.GetChild(1).gameObject;
            Destroy(x.transform.GetChild(0).gameObject);
        }
        else panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text += "\n 消費EXP" + NeedExp + "/" + nowExp;


    }

    
}
