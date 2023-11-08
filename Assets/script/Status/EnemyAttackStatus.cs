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
    EnemyModel model;

    // Update is called once per frame
    void Update()
    {
       
        if(GameManger.TurnNum>= FinishTurn )
        {
            StatusReset();
        }
    }

    //�U���̓A�b�v(�����͌��ʗ�)
    public void SetStatus(double effect,int turn,string mode)
    {
        var p = GameObject.Find("Enemys").transform.GetChild(0);
        if (p == null) return;
        model = p.GetComponent<EnemyContoller>().model;
        this.mode = mode;
        this.effect = effect;
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
        Destroy(gameObject);
    }


}