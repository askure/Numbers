using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockCard : MonoBehaviour
{
    GameManger manger;
    string filepath;
    static int  NeedExp = 0;
    public void Unlockcard()
    {
       int cardId = CardEditManager.UnlockCardId.cardID;
        if (cardId == -1) return;
        
        manger = new GameManger();
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        manger.Dataload(filepath);
        manger.cardLvs[cardId].pos = true;
        manger.cardLvs[cardId].Lv = 1;
        GameManger.Exp -= NeedExp;
        manger.Datasave(filepath);
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeckEdit");

    }

    public void UnlockCardPanel()
    {
        manger = new GameManger();
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        manger.Dataload(filepath);
        int nowExp = GameManger.Exp;
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
        if (nowExp < NeedExp)
        {
            panel.transform.GetChild(0).GetComponent<Text>().text += "\n è¡îÔEXP" + NeedExp + "\nïsë´ÇµÇƒÇ¢Ç‹Ç∑ÅBåªç›ÇÃEXP:"+nowExp;
            var x = panel.transform.GetChild(1).gameObject;
            Destroy(x.transform.GetChild(0).gameObject);
        }
        else if(i)
        {
            panel.transform.GetChild(0).GetComponent<Text>().text = "Ç±ÇÃÉLÉÉÉâÇÕâï˙Ç≈Ç´Ç‹ÇπÇÒ";
             var x =  panel.transform.GetChild(1).gameObject;
            Destroy(x.transform.GetChild(0).gameObject);
        }
        else panel.transform.GetChild(0).GetComponent<Text>().text += "\n è¡îÔEXP" + NeedExp + "/" + nowExp;


    }

    
}
