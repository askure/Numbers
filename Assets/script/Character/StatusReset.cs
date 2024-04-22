using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusReset : MonoBehaviour
{
    public void PanelView()
    {
        var  card = CharacterDataManager.cardLvs[CharacterManager.SerctedCard.model.cardID];
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
        var card = CharacterDataManager.cardLvs[CharacterManager.SerctedCard.model.cardID];
        var bufSum = card.atbuf +
                 card.dfbuf +
                 card.hpbuf;


        DataManager.Stone += ( (bufSum*4) + 1) / 3;
      
        card.hpbuf = 0;
        card.atbuf = 0;
        card.dfbuf = 0;
        CharacterDataManager.DataSave(false);
        DataManager.DataSave();
        CharacterManager character = new CharacterManager();  
        character.SetText();
        
        Destroy(gameObject);

    }
}
