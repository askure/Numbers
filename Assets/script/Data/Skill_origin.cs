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
        constantAttack, /*�G�̒萔�U��*/
        referenceAttack, /*�G�̍U���͎Q�ƍU��*/
        NumDamage,/*���l�o���A�̒l�����炷*/
        Heal_Hp, /*HP��*/
        Heal_num,/*���l�o���A��*/
        damage,/*�����_���[�W*/
        IncreaseAttack,/*�U���̓A�b�v*/
        IncreaseDefence,/*�h��̓A�b�v*/
        IncreaseNum, /*���l�A�b�v*/
        IncreaseHp, /*HP�A�b�v*/
        decreaseDefence,/*�h��̓_�E��*/
        decreaseAttack,/*�U���̓_�E��*/
        Pursuit, /*�ǌ�*/
        NumUp, /*���l���Z*/
        partydecreaseDefence,/*�����ւ̖h��̓f�o�t*/
        decreaseNum,/*�����ւ̐��l�f�o�t*/
        partydecreaseAttack,/*�����ւ̍U���̓f�o�t*/
        Length
    }

 

    public enum Magic_condition_kind
    {
        sum_up, /*���v���l�ȏ�*/
        sum_down, /*���v���l����*/
        multi,/*�{��*/
        divisor,/*��*/
        prime,/*�f��*/
        Hp_up,/*HP�ȏ�*/
        Hp_down, /*HP����*/
        none,/*�����Ȃ�*/
        specificNum,/*����̐��l*/
        Num_up, /*���l�o���A�ȏ�*/
        Num_down, /*����*/
        Length

    }

    public enum MagicKind
    {
        add,//���Z
        multi, //��Z
        difference,// ���Z
        Length
    }

    public string skill_name = "";

    public string skill_infomatin = "";

    public Skill_priority _Priority = Skill_priority.Attack;
    [System.Serializable]
    public class magic_conditon_origin
    {
        
        public Skill_type type = Skill_type.constantAttack;
        public Magic_condition_kind condition_kind; //�{�����̏������
        public int condition_num; //2�̔{�����̐�������
        public int conditons; //�Z�̈ȏ�̏��� 0�͏����Ȃ�
        public MagicKind  magic_kind;
        public double effect_size;
        public int effect_turn; //�o�t�n�̂�

        
    }

    public List<magic_conditon_origin> magic_Conditon_Origins;

    




}
