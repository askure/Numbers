using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDfStatus 
{// Start is called before the first frame update
    int EffectTurn = 3;
    public int FinishTurn;
    public double effect;
    Skill_origin.MagicKind mode;
    static int statusNum = 0;

    // Update is called once per frame
    
    public EnemyDfStatus(double effect,int turn, Skill_origin.MagicKind mode)
    {
        this.mode = mode;
        this.effect = effect;
        FinishTurn = GameManger.TurnNum + turn;
    }
}
