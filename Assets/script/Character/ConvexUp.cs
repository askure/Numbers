using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexUp : MonoBehaviour
{
    string filepath;

    public void  Convex()
    {

        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        var cmanager = new CharacterDataManager(cfilepath);
        var id = CharacterManager.SerctedCard.model.cardID;
        var card = cmanager.cardLvs[id];
        card.convex++;
        card.atbuf = 0;
        card.dfbuf = 0;
        card.hpbuf = 0;
        cmanager.Datasave(cfilepath);
        CharacterManager character = new CharacterManager();
        character.SetText();
        Destroy(gameObject);
    }
}
