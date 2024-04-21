using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupCardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popupNameText;
    [SerializeField] private TextMeshProUGUI popupLvText;
    [SerializeField] private TextMeshProUGUI popupNumText;
    [SerializeField] private TextMeshProUGUI popupSkillinfoText;
    [SerializeField] private TextMeshProUGUI popupRskillnameText;
    [SerializeField] private TextMeshProUGUI popuoRskillinfoText;
    [SerializeField] private TextMeshProUGUI popupSkillLv;
    [SerializeField] private Image cardImage,rareImage;
    [SerializeField] private GameObject autopanel, rpanel;



    public void SetText(CardModel card)
    {
        string name = card.name + "(" + card.rare + ")";
        popupNameText.text = name;
        if(name.Length > 11)
            popupNameText.fontSize = (name.Length - 11) * 3; 
        else
            popupNameText.fontSize = 20;
        if (card.Lv == 0) popupLvText.text = "Lv:-";
        else popupLvText.text = "Lv:" + card.Lv.ToString();
        int skillLv = (card.Lv >= 100) ? 6 : card.Lv / 20 + 1;
        popupNumText.text = card.num.ToString();
        popupSkillinfoText.text = card.PublicSkill.skill_infomatin;
        popupSkillLv.text = "AUTOÉXÉLÉã(Lv:" + skillLv  + ")" ;
        popupRskillnameText.text = card.ReaderSkill.skill_name + "(Lv: " + skillLv  + ")" ;
        popuoRskillinfoText.text = card.ReaderSkill.skill_infomatin;
        cardImage.sprite= card.icon;
        SetPanel(card.rare);
        cardImage.color = new Color(1, 1, 1);
        rpanel.GetComponent<EffectInfoManager>().SetText(card.ReaderSkill,card.at);
        autopanel.GetComponent<EffectInfoManager>().SetText(card.PublicSkill, card.at);
    }

    private void SetPanel(string rare)
    {
        switch (rare)
        {
            case "A":
                rareImage.sprite = Resources.Load<Sprite>("A");

                break;
            case "S":
                rareImage.sprite = Resources.Load<Sprite>("S");
                break;
            case "SS":
                rareImage.sprite = Resources.Load<Sprite>("SS");
                break;
        }
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
