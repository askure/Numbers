using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel 
{
    public int enemyID;
    public int Hp;
    public int at;
    public int df;
    public int MaxHp;
    public int numba;
    public int maxNumba;
    public int _exp;
    public string name;
    public Sprite icon;
    public List<Skill_origin> skilllist;
    public List<int> skillTable;
    public List<EnemyAttackStatus> attackStatuses;
    public List<EnemyDfStatus> dfStatuses;
    

    public EnemyModel(int EnemyId)
    {
        EnemyEntity cardEntity = Resources.Load<EnemyEntity>("EnemyEntityList/Enemy" + EnemyId);
        enemyID = cardEntity.EnemyId;
        MaxHp = Hp = cardEntity.Hp;
        at = cardEntity.at;
        df = cardEntity.df;
        maxNumba=  numba = cardEntity.numba;
        _exp = cardEntity._exp;
        name = cardEntity.name;
        icon = cardEntity.icon;
        skilllist = cardEntity.skilllist;
        skillTable = cardEntity.SkillTable;
        attackStatuses = new List<EnemyAttackStatus>();
        dfStatuses = new List<EnemyDfStatus>();
    }
}
