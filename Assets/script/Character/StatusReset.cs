using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusReset : MonoBehaviour
{
    //GameManger gameManger;
    public void PanelView()
    {
        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        var cmanager = new CharacterDataManager(cfilepath);

        var  card = cmanager.cardLvs[CharacterManager.SerctedCard.model.cardID];
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
        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        var cmanager = new CharacterDataManager(cfilepath);
        var dfilepath = Application.persistentDataPath + "/" + ".savedata.json";
        var dmanager = new DataManager(dfilepath);
        var card = cmanager.cardLvs[CharacterManager.SerctedCard.model.cardID];

        var bufSum = card.atbuf +
                 card.dfbuf +
                 card.hpbuf;


        dmanager.Stone += ( (bufSum*4) + 1) / 3;
      
        card.hpbuf = 0;
        card.atbuf = 0;
        card.dfbuf = 0;
        cmanager.Datasave(cfilepath);
        CharacterManager character = new CharacterManager();
        character.SetText();

        
        Destroy(gameObject);

    }
}
