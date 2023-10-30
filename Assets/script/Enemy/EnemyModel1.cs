using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel 
{
    public int enemyID;
    public int Hp;
    public int at;
    public int df;
    public int initHp;
    public int initAt;
    public int initDf;
    public int numba;
    public int _exp;
    public string name;
    public Sprite icon;
    public List<Skill_origin> skilllist;
    public List<int> skillTable;
    

    public EnemyModel(int EnemyId)
    {
        EnemyEntity cardEntity = Resources.Load<EnemyEntity>("EnemyEntityList/Enemy" + EnemyId);
        enemyID = cardEntity.EnemyId;
        initHp = Hp = cardEntity.Hp;
        initAt  =at = cardEntity.at;
        initDf  = df = cardEntity.df;
        numba = cardEntity.numba;
        _exp = cardEntity._exp;
        name = cardEntity.name;
        icon = cardEntity.icon;
        skilllist = cardEntity.skilllist;
        skillTable = cardEntity.SkillTable;
    }
}
