using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexUp : MonoBehaviour
{
    GameManger gameManger;
    string filepath;

    public void  Convex()
    {

        gameManger = new GameManger();
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        gameManger.Dataload(filepath);
        var id = CharacterManager.SerctedCard.model.cardID;
        var card = gameManger.cardLvs[id];
        card.convex++;
        card.atbuf = 0;
        card.dfbuf = 0;
        card.hpbuf = 0;
        gameManger.Datasave(filepath);
        CharacterManager character = new CharacterManager();
        character.SetText();
        Destroy(gameObject);
    }
}
