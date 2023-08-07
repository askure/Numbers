using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusReset : MonoBehaviour
{
    GameManger gameManger;
    public void PanelView()
    {
        string filepath = Application.persistentDataPath + "/" + ".savedata.json";
        gameManger = new GameManger();
        gameManger.Dataload(filepath);
       
        var  card = gameManger.cardLvs[CharacterManager.SerctedCard.model.cardID];
        var  bufSum = card.atbuf +
                 card.dfbuf +
                 card.hpbuf;
        var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusReset");
        var canvas = GameObject.Find("Canvas").transform;
        var panel = Instantiate(gameObject, canvas);
        panel.transform.GetChild(0).GetComponent<Text>().text += "\n\nï‘ä“Ç≥ÇÍÇÈÉXÉgÅ[Éìêî:" + ( ((bufSum * 4) + 1) / 3  ) + "å¬";
    }

    public void StatusResetScript()
    {
        string filepath = Application.persistentDataPath + "/" + ".savedata.json";
        gameManger = new GameManger();
        gameManger.Dataload(filepath);
        var card = gameManger.cardLvs[CharacterManager.SerctedCard.model.cardID];

        var bufSum = card.atbuf +
                 card.dfbuf +
                 card.hpbuf;


        GameManger.Stone += ( (bufSum*4) + 1) / 3;
      
        card.hpbuf = 0;
        card.atbuf = 0;
        card.dfbuf = 0;
        gameManger.Datasave(filepath);
        CharacterManager character = new CharacterManager();
        character.SetText();

        
        Destroy(gameObject);

    }
}
