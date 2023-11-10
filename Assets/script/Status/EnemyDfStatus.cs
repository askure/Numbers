using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDfStatus : MonoBehaviour
{// Start is called before the first frame update
    int EffectTurn = 3;
    int FinishTurn;
    double effect;
    string mode;
    int beforbuf;
    EnemyModel model;

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
        this.beforbuf = model.df;
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
        transform.parent = p;
        EffectTurn = turn;
        FinishTurn = GameManger.TurnNum + EffectTurn;
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
        if (model.df < 1) model.df = 1;
        Destroy(gameObject);
    }
}
