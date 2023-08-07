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
    [SerializeField] private Image cardImage;



    public void SetText(CardModel card)
    {
        popupNameText.text = card.name;
        if(card.Lv == 0) popupLvText.text = "Lv:-";
        else popupLvText.text = "Lv:" + card.Lv.ToString();
        popupNumText.text = card.num.ToString();
        popupSkillinfoText.text = card.PublicSkill.skill_infomatin;
        popupRskillnameText.text = card.ReaderSkill.skill_name;
        popuoRskillinfoText.text = card.ReaderSkill.skill_infomatin;
        cardImage.sprite= card.icon;
        cardImage.color = new Color(1, 1, 1);
    }

}
