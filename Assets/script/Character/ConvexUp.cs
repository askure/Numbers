using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvexUp : MonoBehaviour
{
    string filepath;

    public void  Convex()
    {
        var id = CharacterManager.SerctedCard.model.cardID;
        var card = CharacterDataManager.cardLvs[id];
        card.convex++;
        card.atbuf = 0;
        card.dfbuf = 0;
        card.hpbuf = 0;
        CharacterDataManager.DataSave(false);
        CharacterManager character = new CharacterManager();
        character.SetText();
        Destroy(gameObject);
    }
}
