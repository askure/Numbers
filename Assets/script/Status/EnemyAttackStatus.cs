using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackStatus : MonoBehaviour
{
    // Start is called before the first frame update
    int EffectTurn = 3;
    int FinishTurn;
    double effect;
    string mode;
    int beforbuf;
    EnemyModel model;
    static int statusNum = 0;

    // Update is called once per frame
    void Update()
    {
       
        if(GameManger.TurnNum>= FinishTurn )
        {
            StatusReset();
        }
    }

    //UŒ‚—ÍƒAƒbƒv(ˆø”‚ÍŒø‰Ê—Ê)
    public void SetStatus(double effect,int turn,string mode)
    {
        var p = GameObject.Find("Enemys").transform.GetChild(0);
        if (p == null) return;
        model = p.GetComponent<EnemyContoller>().model;
        this.mode = mode;
        this.effect = effect;
        if(statusNum==0) this.beforbuf = model.df;
        if (mode == "Multi")
        {
            double temp = model.at * effect;
            model.at = (int)temp;
        }
        else if (mode == "Add")
        {
            double temp = model.at + effect;
            model.at = (int)temp;
        }
        transform.parent = p;
        EffectTurn = turn;
        FinishTurn = GameManger.TurnNum + EffectTurn;
        statusNum++;
    }
    public void StatusReset()
    {
        
        if (mode == "Multi")
        {
            double temp = model.at / effect;
            model.at = (int)temp;
           
        }
        else if (mode == "Add")
        {
            double temp = model.at - effect;
            model.at = (int)temp;
        }
        if (model.at < 1) model.at = 1;
        statusNum--;
        if (statusNum == 0 && model.at < model.initAt) model.at = model.initAt;
        Destroy(gameObject);
    }


}
