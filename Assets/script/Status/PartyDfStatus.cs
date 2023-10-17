using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyDfStatus : MonoBehaviour
{int EffectTurn = 3;
    int FinishTurn;
    int effect;
    int index;
    CardModel model;

    // Update is called once per frame
    void Update()
    {
       
        if(transform.childCount > index)
        {
            Debug.Log(transform.childCount - index + "Œ¸­");
        }
        
    }

    //–hŒä—ÍƒAƒbƒv(ˆø”‚ÍŒø‰Ê—Ê)
    public void SetStatus(int effect, int turn)
    {
        var p = transform.parent;
        if (p == null) return;
        model = p.GetComponent<CardController>().model;
        this.effect = effect;
        model.df += effect;
        EffectTurn = turn;
        FinishTurn = GameManger.TurnNum + EffectTurn;
    }
    public void StatusReset()
    {
        model.df -= effect;
        Destroy(gameObject);
    }
}
