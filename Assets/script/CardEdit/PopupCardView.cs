using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupCardView : MonoBehaviour
{
    [SerializeField] private Text popupNameText;
    [SerializeField] private Text popupLvText;
    [SerializeField] private Text popupNumText;
    [SerializeField] private Text popupSkillinfoText;
    [SerializeField] private Text popupRskillnameText;
    [SerializeField] private Text popuoRskillinfoText;
    [SerializeField] private Text popupSkillLv;
    [SerializeField] private Image cardImage;
    [SerializeField] private GameObject autopanel, rpanel;



    public void SetText(CardModel card)
    {
        popupNameText.text = card.name;
        if(card.Lv == 0) popupLvText.text = "Lv:-";
        else popupLvText.text = "Lv:" + card.Lv.ToString();
        int skillLv = (card.Lv >= 100) ? 6 : card.Lv / 20 + 1;
        popupNumText.text = card.num.ToString();
        popupSkillinfoText.text = card.PublicSkill.skill_infomatin;
        popupSkillLv.text = "AUTOÉXÉLÉã(Lv:" + skillLv  + ")" ;
        popupRskillnameText.text = card.ReaderSkill.skill_name + "(Lv: " + skillLv  + ")" ;
        popuoRskillinfoText.text = card.ReaderSkill.skill_infomatin;
        cardImage.sprite= card.icon;
        cardImage.color = new Color(1, 1, 1);
        rpanel.GetComponent<EffectInfoManager>().SetText(card.ReaderSkill,card.at);
        autopanel.GetComponent<EffectInfoManager>().SetText(card.PublicSkill, card.at);
    }

    private void SetPanel()
    {

    }

    public void OpenAutoPanel()
    {
        autopanel.SetActive(true);
    }
    public void CloseAutoPanel()
    {
        autopanel.SetActive(false);
    }
    public void OpenRPanel()
    {
        rpanel.SetActive(true);
    }

    public void CloseRPanel()
    {
        rpanel.SetActive(false);
    }
}
