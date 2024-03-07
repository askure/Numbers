using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardInformation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject InfomationObject;
    [SerializeField] Text infoText;
    
    public void InfomationUp()
    {   


        InfomationObject.SetActive(true);

        /*infoText.text += "Hp:" + controller.model.Hp.ToString() + "\n\n";
        infoText.text += "çUåÇóÕ:" + controller.model.at.ToString() + "\n\n";
        infoText.text += "ñhå‰óÕ:" + controller.model.df.ToString() + "\n\n";*/
        SetText();
        
       

    }

    public void SetText()
    {
        var controller = GetComponent<CardController>();
        var name = controller.model.name;
        infoText.color = new Color(0, 0, 0);
        infoText.text = name + "\n";
        infoText.text += controller.model.PublicSkill.skill_infomatin + "\n\n";
        List<CardController> UpCard = GameManger.instnce.GetUpCard();
        int auto = AutoSKillFlag(UpCard, controller);
        int autoSum = controller.model.PublicSkill.magic_Conditon_Origins.Count;
        if (auto == autoSum) infoText.color = new Color(0, 0, 255);
        else if (auto == 0) infoText.color = new Color(255, 0, 0);
        else infoText.color = new Color(0, 0.5f, 0.2f);
    }

    public void InfomationDown()
    {


        InfomationObject.SetActive(false);
    }

    int AutoSKillFlag(List<CardController> cards, CardController card)
    {
        List<int> nums = new List<int>();
        GameManger gameManger = new GameManger();

        foreach (CardController x in cards)
        {
            nums.Add(x.model.num);
        }
        var auto = card.model.PublicSkill;
        var skillOrigin = auto.magic_Conditon_Origins;
        var SkillLength = skillOrigin.Count;
        if (SkillLength == 0) return 0;
        bool[] autoInvocation = new bool[SkillLength];
        int index = 0;
        foreach (Skill_origin.magic_conditon_origin _Origin in skillOrigin)
        {

            var magicKind = _Origin.magic_kind;
            var conditonKind = _Origin.condition_kind;
            var conditionNum = _Origin.condition_num;
            var conditons = _Origin.conditons;
            var effect = _Origin.effect_size;
            autoInvocation[index] = gameManger.AutoSkillCheck(conditonKind, conditionNum, conditons, nums, GameManger.hpSum, GameManger.sum);
            index++;

        }
        int autoNum = 0;
        foreach (bool b in autoInvocation)
        {

            if (b) autoNum++;
        }
        return autoNum;
    }

}
