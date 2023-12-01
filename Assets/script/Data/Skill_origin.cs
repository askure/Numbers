using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "Skill_origin", menuName = "SKILL_ORIGIN")]
public class Skill_origin : ScriptableObject
{   
    public enum Skill_priority
    {
        Buff,
        Debuff,
        Heal,
        Attack,
        numIncrease

    }

    public enum Skill_type
    {
        constantAttack, /*敵の定数攻撃*/
        referenceAttack, /*敵の攻撃力参照攻撃*/
        NumDamage,/*数値バリアの値を減らす*/
        Heal_Hp, /*HP回復*/
        Heal_num,/*数値バリア回復*/
        damage,/*自傷ダメージ*/
        IncreaseAttack,/*攻撃力アップ*/
        IncreaseDefence,/*防御力アップ*/
        IncreaseNum, /*数値アップ*/
        IncreaseHp, /*HPアップ*/
        decreaseDefence,/*防御力ダウン*/
        decreaseAttack,/*攻撃力ダウン*/
        Pursuit, /*追撃*/
        NumUp, /*数値加算*/
        partydecreaseDefence,/*味方への防御力デバフ*/
        decreaseNum,/*味方への数値デバフ*/
        partydecreaseAttack,/*味方への攻撃力デバフ*/
        Length
    }

 

    public enum Magic_condition_kind
    {
        sum_up, /*合計数値以上*/
        sum_down, /*合計数値未満*/
        multi,/*倍数*/
        divisor,/*約数*/
        prime,/*素数*/
        Hp_up,/*HP以上*/
        Hp_down, /*HP未満*/
        none,/*条件なし*/
        specificNum,/*特定の数値*/
        Num_up, /*数値バリア以上*/
        Num_down, /*未満*/
        Length

    }

    public enum MagicKind
    {
        add,//加算
        multi, //乗算
        difference,// 減算
        Length
    }

    public string skill_name = "";

    public string skill_infomatin = "";

    public Skill_priority _Priority = Skill_priority.Attack;
    [System.Serializable]
    public class magic_conditon_origin
    {
        
        public Skill_type type = Skill_type.constantAttack;
        public Magic_condition_kind condition_kind; //倍数等の条件種類
        public int condition_num; //2の倍数等の数字誓約
        public int conditons; //〇体以上の条件 0は条件なし
        public MagicKind  magic_kind;
        public double effect_size;
        public int effect_turn; //バフ系のみ

        
    }

    public List<magic_conditon_origin> magic_Conditon_Origins;

    




}
