using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyDfStatus : MonoBehaviour
{   
   
    [SerializeField]int FinishTurn;
    int beforebuf;
    double effect;
    string mode;
    CardModel model;

    // Update is called once per frame
    void Update()
    {
       
        if(GameManger.TurnNum >=FinishTurn)
        {
            StatusReset();
        }
        
    }

    //–hŒä—ÍƒAƒbƒv(ˆø”‚ÍŒø‰Ê—Ê)
    public void SetStatus(double effect,int turn,GameObject card,string mode)
    {
        if (card == null) return;
        transform.parent = card.transform;
       
        model = card.GetComponent<CardController>().model;
        this.effect = effect;
        this.mode = mode;
        this.beforebuf = model.df;
        if(mode == "Multi")
        {
            double temp = model.df * effect;
            model.df = (int)temp;
        }
        else if(mode == "Add")
        {
            double temp = model.df + effect;
            model.df = (int)temp;
        }
        if (model.df < 1) model.df = 1;
        FinishTurn = turn;
    }
    public void StatusReset()
    {
       
        
        if (mode == "Multi")
        {
            double temp = model.df / effect;
            model.df = (int)temp;
        }
        else if (mode == "Add")
        {
            double temp = model.df - effect;
            model.df = (int)temp;
        }
        if (model.df < model.BeforeDf && PartyDfStatusManager.statusNum <= 1) model.df = model.BeforeDf;
        Destroy(gameObject);
    }
}
