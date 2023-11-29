using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text info;
    
    public void SetText(Skill_origin skill,int at)
    {
        info.text = "";
        bool[] flag = new bool[(int)Skill_origin.Skill_type.Length];
        for (int i = 0; i < flag.Length; i++)
            flag[i] = false;
        foreach(Skill_origin.magic_conditon_origin origin in skill.magic_Conditon_Origins)
        {
            Skill_origin.Skill_type type= origin.type;
            double effectsize = origin.effect_size;
            int index = (int)type;
            if (flag[index]) continue;
            switch (type)
            {
                
                case Skill_origin.Skill_type.damage:
                    
                    info.text += "�o�g���J�n���_���[�W���󂯂�\n";
                    
                    break;
                case Skill_origin.Skill_type.Heal_Hp:
                    double heal = effectsize * Mathf.Log(at, 8) / 3;
                    info.text += "HP��" + (int)heal + "�񕜂���\n";
                    break;
                case Skill_origin.Skill_type.Pursuit:
                    info.text += "�G��" + effectsize + "�̒ǌ�\n";
                    break;
                case Skill_origin.Skill_type.IncreaseAttack:
                    info.text +=  "�L�����̍U���̓A�b�v\n";
                    break;
                case Skill_origin.Skill_type.IncreaseDefence:
                    info.text += "�L�����̖h��̓A�b�v\n";
                    break;
                case Skill_origin.Skill_type.IncreaseHp:
                    info.text += "�L������HP�A�b�v\n";
                    break;
                case Skill_origin.Skill_type.IncreaseNum:
                    info.text += "�L�����̐��l���A�b�v\n";
                    break;
                case Skill_origin.Skill_type.NumUp:
                    info.text += "���v���l���A�b�v\n";
                    break;
              
                

            }
            flag[index] = true;

        }
    }
}
