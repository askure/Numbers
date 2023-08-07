using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUp : MonoBehaviour
{
    GameManger gameManger;
    string filepath;

    public void Statusup()
    {
        gameManger = new GameManger();
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        gameManger.Dataload(filepath);
        var id = CharacterManager.SerctedCard.model.cardID;
        var card = gameManger.cardLvs[id];
        var status = Random.Range(0, 3);
        string statusName = "";
        var beforeBuf = 0f;
        var afterBuf = 0f;
        var needStone = (card.atbuf + card.dfbuf + card.hpbuf) * 4 + 1;
        if(GameManger.Stone < needStone)
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<Text>().text = "ÉXÉgÅ[ÉìÇ™ë´ÇËÇ‹ÇπÇÒ";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }
        GameManger.Stone -= needStone;
        
        switch (status)
        {
            case 0:
                beforeBuf = 1 + card.atbuf * 0.05f;
                card.atbuf++;
                afterBuf = 1 + card.atbuf * 0.05f;
                statusName  = "çUåÇóÕ";

                break;
            case 1:
                beforeBuf = 1 + card.dfbuf * 0.05f;
                card.dfbuf++;
                afterBuf = 1 + card.dfbuf * 0.05f;
                statusName = "ñhå‰óÕ";
                break;
            case 2:
                beforeBuf = 1 + card.hpbuf * 0.05f;
                card.hpbuf++;
                afterBuf = 1 + card.hpbuf * 0.05f;
                statusName = "HP";
                break;

        }
        gameManger.Datasave(filepath);
        CharacterManager character = new CharacterManager();
        var massage = statusName + "Ç™ã≠âªÇ≥ÇÍÇΩ\n" + beforeBuf.ToString("F2") + "î{Å®" + afterBuf.ToString("F2") + "î{"  ;
        var Object = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUpResult");
        var canvas = GameObject.Find("Canvas").transform;
        var panel = Instantiate(Object, canvas);
        panel.transform.GetChild(0).GetComponent<Text>().text = massage;
        character.SetText();
        
        Destroy(gameObject); 
    }
}
