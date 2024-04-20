using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUp : MonoBehaviour
{
    string filepath;

    public void Statusup()
    {
        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        var cmanager = new CharacterDataManager(cfilepath);
        var dfilepath = Application.persistentDataPath + "/" + ".savedata.json";
        var dmanager = new DataManager(dfilepath);
        var id = CharacterManager.SerctedCard.model.cardID;
        var card = cmanager.cardLvs[id];
        var status = Random.Range(0, 3);
        string statusName = "";
        var beforeBuf = 0f;
        var afterBuf = 0f;
        var needStone = (card.atbuf + card.dfbuf + card.hpbuf) * 4 + 1;
        if(dmanager.Stone < needStone)
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canva = GameObject.Find("Canvas").transform;
            var pane = Instantiate(gameObject, canva);
            pane.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ÉXÉgÅ[ÉìÇ™ë´ÇËÇ‹ÇπÇÒ";
            var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
            return;
        }
        dmanager.Stone -= needStone;
        
        switch (status)
        {
            case 0:
                beforeBuf = 1 + card.atbuf * 0.1f;
                card.atbuf++;
                afterBuf = 1 + card.atbuf * 0.1f;
                statusName  = "çUåÇóÕ";

                break;
            case 1:
                beforeBuf = 1 + card.dfbuf * 0.1f;
                card.dfbuf++;
                afterBuf = 1 + card.dfbuf * 0.1f;
                statusName = "ñhå‰óÕ";
                break;
            case 2:
                beforeBuf = 1 + card.hpbuf * 0.1f;
                card.hpbuf++;
                afterBuf = 1 + card.hpbuf * 0.1f;
                statusName = "HP";
                break;

        }
        dmanager.DataSave(dfilepath);
        cmanager.Datasave(cfilepath);
        CharacterManager character = new CharacterManager();
        var massage = statusName + "Ç™ã≠âªÇ≥ÇÍÇΩ\n" + beforeBuf.ToString("F2") + "î{Å®" + afterBuf.ToString("F2") + "î{"  ;
        var Object = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUpResult");
        var canvas = GameObject.Find("Canvas").transform;
        var panel = Instantiate(Object, canvas);
        panel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = massage;
        character.SetText();
        
        Destroy(gameObject); 
    }
}
