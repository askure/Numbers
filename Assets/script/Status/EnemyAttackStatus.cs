using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackStatus 
{
    public int FinishTurn;
    public double effect;
    public Skill_origin.MagicKind mode;
    [SerializeField] EnemyContoller EnemyContoller;
    public  EnemyAttackStatus(double effect, int turn, Skill_origin.MagicKind mode)
    {
        this.mode = mode;
        this.effect = effect;
        FinishTurn = GameManger.TurnNum + turn;
    }
    


}
