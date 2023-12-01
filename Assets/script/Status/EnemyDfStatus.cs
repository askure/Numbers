using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDfStatus : MonoBehaviour
{// Start is called before the first frame update
    int EffectTurn = 3;
   [SerializeField] int FinishTurn;
   [SerializeField] double effect;
    string mode;
    EnemyModel model;
    static int statusNum = 0;

    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn)
        {
            StatusReset();
        }
    }

    //ñhå‰óÕÉAÉbÉv(à¯êîÇÕå¯â ó )
    public void SetStatus(double effect, int turn,string mode)
    {
        var p = GameObject.Find("Enemys").transform.GetChild(0);
        if (p == null) return;
        this.mode = mode;
        model = p.GetComponent<EnemyContoller>().model;
        this.effect = effect;
        Debug.Log("setbefore" + mode + ":" + model.df);
        if (mode == "Multi")
        {
            double temp = model.df * effect;
            model.df = (int)temp;
        }
        else if (mode == "Add")
        {
            double temp = model.df + effect;
            model.df = (int)temp;
        }
        Debug.Log("setafter" + mode + ":" + model.df);
        transform.parent = p;
        EffectTurn = turn;
        FinishTurn = GameManger.TurnNum + EffectTurn;
        statusNum++;
    }
    public void StatusReset()
    {
        Debug.Log("resetbefore" + mode  + ":"+ model.df);
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
        Debug.Log("resetafter:" + mode + ":" + model.df);
        if (model.df < 1) model.df = 1;
        statusNum--;
        if (statusNum == 0 && model.df < model.initDf) model.df = model.initDf;
        
        Destroy(gameObject);
    }
}
