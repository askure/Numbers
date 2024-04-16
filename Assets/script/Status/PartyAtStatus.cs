using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyAtStatus : MonoBehaviour
{

    public int FinishTurn;
    double effect;
    string mode;
    static string path;
    CardModel model;
    public Skill_origin skillOrigin;


    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn)
        {
            StatusReset();
        }

    }
    public PartyAtStatus(double effect,int turn, Skill_origin skillOrigin)
    {
        this.effect = effect;
        FinishTurn = GameManger.TurnNum + turn;
        this.skillOrigin = skillOrigin;
    }

    

    //攻撃力アップ
    public void SetStatus(double effect, int turn, GameObject card, string mode)
    {
        if (card == null) return;
        transform.parent = card.transform;
        model = card.GetComponent<CardController>().model;
        this.effect = effect;
        if(PartyAtManager.statusNum >= 3 && mode == "Multi" && effect >1)
        {
            this.effect =1 +  Mathf.Pow(0.7f, PartyAtManager.statusNum - 2) * (effect - 1);
        }
        this.mode = mode;
        int beforeat = model.at;

        if (mode == "Multi")
        {
            double temp = model.at * this.effect;
            model.at = (int)temp;
        }
        else if (mode == "Add")
        {
            double temp = model.at + effect;
            model.at = (int)temp;
        }
        if (model.at < 1) model.at = 1;
        FinishTurn = turn;
       // Debug.Log(gameObject.name + ":" + model.name + "に" + effect + "の攻撃力に関するステータス変化が適用("  + FinishTurn + ")"+ beforeat + "→" + model.at );
    }
    public void StatusReset()
    {
        int beforeat = model.at;
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
        if (model.at < model.BeforeAt && PartyAtManager.statusNum <= 1) model.at = model.BeforeAt;
        //Debug.Log(gameObject.name + ":" + model.name + "に" + effect + "の攻撃力に関するステータス変化が解除(" + FinishTurn + ")" + beforeat + "→" + model.at);
        Destroy(gameObject);
    }
}
