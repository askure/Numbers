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
        constantAttack,//Only Enemy
        referenceAttack, //Only Enemy
        NumDamage,
        Heal_Hp,
        Heal_num,//only enemy
        damage,
        IncreaseAttack,
        IncreaseDefence,
        IncreaseNum, // Only ReaderSkill
        IncreaseHp, //  Only ReaderSkill
        Pursuit, // Only MagicKind add
        NumUp, // Only MagicKind add
        decreaseDefence,
        decreaseNum,//Only Enemy
        Length
    }

 

    public enum Magic_condition_kind
    {
        sum_up, //sum >=conditon_num
        sum_down, // sum < condition_num
        multi,
        divisor,
        prime,
        Hp_up, // Hp >= condition_num
        Hp_down, //hp < conditon_num
        none,//NorReaderskill
        specificNum,
        Num_up, //Only Enemy
        Num_down, // OnlyEnmey
        Length

    }

    public enum MagicKind
    {
        add,//‰ÁŽZ
        multi, //æŽZ
        difference,// Œ¸ŽZ
        Length
    }

    public string skill_name = "";

    public string skill_infomatin = "";

    public Skill_priority _Priority = Skill_priority.Attack;
    [System.Serializable]
    public class magic_conditon_origin
    {
        
        public Skill_type type = Skill_type.constantAttack;
        public Magic_condition_kind condition_kind; //”{”“™‚ÌðŒŽí—Þ
        public int condition_num; //2‚Ì”{”“™‚Ì”Žš¾–ñ
        public int conditons; //Z‘ÌˆÈã‚ÌðŒ 0‚ÍðŒ‚È‚µ
        public MagicKind  magic_kind;
        public double effect_size;
        public int effect_turn; //ƒoƒtŒn‚Ì‚Ý

        
    }

    public List<magic_conditon_origin> magic_Conditon_Origins;

    




}
