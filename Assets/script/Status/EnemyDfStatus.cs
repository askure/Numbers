using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDfStatus : MonoBehaviour
{// Start is called before the first frame update
    int EffectTurn = 3;
    int FinishTurn;
    int effect;
    EnemyModel model;

    // Update is called once per frame
    void Update()
    {

        if (GameManger.TurnNum >= FinishTurn)
        {
            StatusReset();
        }
    }

    //–hŒä—ÍƒAƒbƒv(ˆø”‚ÍŒø‰Ê—Ê)
    public void SetStatus(int effect, int turn)
    {
        var p = GameObject.Find("Enemys").transform.GetChild(0);
        if (p == null) return;
        model = p.GetComponent<EnemyContoller>().model;
        this.effect = effect;
        model.df += effect;
        transform.parent = p;
        EffectTurn = turn;
        FinishTurn = GameManger.TurnNum + EffectTurn;
    }
    public void StatusReset()
    {
        model.df -= effect;
        Destroy(gameObject);
    }
}
