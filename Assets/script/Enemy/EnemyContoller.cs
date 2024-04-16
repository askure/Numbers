using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyContoller : MonoBehaviour
{
    public  EnemyView view;
    public EnemyModel model;
    public GameObject gbj;




    private void Awake()
    {
        view = GetComponent<EnemyView>();
        gbj = this.gameObject;
    }
    public void Update()
    {
        Show_update(model.numba.ToString(),model.Hp,model.MaxHp);
    }



    public void Init(int EnemyID)
    {
        model = new EnemyModel(EnemyID);
        view.Show(model);
    }
    public void Show_update( string s,int hp,int maxHp)
    {
        view.Show_update(s,hp,maxHp);
    }
    public double SumAttackStatus()
    {
        double effect  =1.0;
        foreach(var status in model.attackStatuses)
        {
            effect *= status.effect;
        }
        return effect;
    }
    public double SumDefenceStatus()
    {
        double effect = 1.0;
        foreach(var status in model.dfStatuses)
        {
            effect *= status.effect;
        }
        return effect;
    }
    public void AddAttackStatus(EnemyAttackStatus status)
    {
        model.attackStatuses.Add(status);
    }
    public void AddDefenceStatus(EnemyDfStatus status)
    {
        model.dfStatuses.Add(status);
    }

    public void SetMaxHp(int hp)
    {
        model.Hp = model.MaxHp = hp;
    }

    public void SetMaxNum(int num)
    {
       model.numba=  model.maxNumba = num;
    }


    public int GetAt()
    {
        double at = model.at * SumAttackStatus();
        return (int) at;
    }
    public int GetDf()
    {
        double df = model.df * SumDefenceStatus();
        return (int)df;
    }
    public int GetExp()
    {
        return model._exp;
    }
    
    public int GetHp()
    {
        return model.Hp;
    }
    public bool IsDeath(int damage)
    {
        return model.Hp <=damage;
    }
    public void HpDamage(int damage)
    {
        model.Hp -= damage;
        if (model.Hp < 0)
            model.Hp = 0;
        
    }
    public int GetNum()
    {
        return model.numba;
    }
    public List<Skill_origin> GetSKillList()
    {
        return model.skilllist;
    }

    public List<int> GetSKillTable()
    {
        return model.skillTable;
    }

    public bool NumDamage(int damage)
    {

        if (model.numba <= damage)
          return true;

        model.numba -= damage;
        return false;
    }
    
    public int GetOverNum(int sum)
    {
        int over = model.numba - sum;
        model.numba = 0;
        return -over;
    }

    public void NumDamage(double damage)
    {
        model.numba -= (int)damage;
        if(model.numba <=0)
            model.numba = 1;
    }
    public string GetName()
    {
        return model.name;
    }
    public void HealHp(double effect)
    {
        double heal = effect * model.at;
        model.Hp += (int) heal;
        if(model.MaxHp > model.Hp)
            model.Hp = model.MaxHp;

    }
    public void HealNum(int effect)
    {
        model.numba +=effect;
    }

    public void RevivalNum()
    {
        double newnumba = model.maxNumba * model.Hp / model.MaxHp;
        if (newnumba < 1) newnumba = 1;
        model.numba = (int) newnumba;
    }

    public void CheckStatus()
    {

        for (int i = 0, len = model.attackStatuses.Count; i < len; i++)
        {
            if (model.attackStatuses[i].FinishTurn <= GameManger.TurnNum)
            {
                model.attackStatuses[i] = null;
            }
        }
        if(model.attackStatuses.Contains(null))
           model.attackStatuses.RemoveAll(tmp => tmp == null);

        for (int i = 0, len = model.dfStatuses.Count; i < len; i++)
        {
            if (model.dfStatuses[i].FinishTurn <= GameManger.TurnNum)
            {
                model.dfStatuses[i] = null;
            }
        }
        if(model.dfStatuses.Contains(null))
           model.dfStatuses.RemoveAll(tmp => tmp == null);


    }

}
