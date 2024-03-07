using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] Text lvText, nameText,hpText,atText, dfText,expNumText;
    [SerializeField] Image Icon,rareImage;

    public void SetText(CardModel card,int exp)
    {
        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        var cmanager = new CharacterDataManager(cfilepath);
        var Lv = card.Lv;
        var name = card.name;
        var Hp = card.Hp;
        var at = card.at;
        var df = card.df;
        var icon = card.icon;
        var rare = card.rare;
        var id = card.cardID;
        var cardinfo = cmanager.cardLvs[id];
        var atbuf = 1 + cardinfo.atbuf * 0.05f;
        var dfbuf = 1 + cardinfo.dfbuf * 0.05f;
        var hpbuf = 1 + cardinfo.hpbuf * 0.05f;
        var convex = cardinfo.convex;
        var LimitConvex = 0;
        switch (rare)
        {
            case "A":
                LimitConvex = 8;
                break;
            case "S":
                LimitConvex = 6;
                break;
            case "SS":
                LimitConvex = 4;
                break;
        }
        lvText.text = "Lv:" + Lv.ToString();
        nameText.text = name + "\n"+convex.ToString() + "/" + LimitConvex.ToString();
        hpText.text = "Hp:" + Hp.ToString() + "(+" + (hpbuf*Hp - Hp).ToString("F0") + ")";
        atText.text = "çUåÇóÕ:" + at.ToString() + "(+" + (at * atbuf - at).ToString("F0") + ")";
        dfText.text = "ñhå‰óÕ:" + df.ToString() + "(+" + (df * dfbuf - df).ToString("F0") + ")";
        Icon.sprite = icon;
        Icon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        if(Lv ==100 +  10 * (convex))
        {
            expNumText.text = "Max";
        }
        else
        {
            expNumText.text = exp.ToString();
        }
        SetPanel(rare);

    }

    public void TextInit()
    {

        lvText.text = "Lv:";
        nameText.text = "";
        hpText.text = "Hp:";
        atText.text = "Attack:" ;
        dfText.text = "Defence:";
        Icon.sprite = null;
        Icon.color = new Color(1.0f, 1.0f, 1.0f, 0f);
        expNumText.text = "0";
        SetPanel("A");



    }
    public void SetText(CardModel card)
    {
        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        var cmanager = new CharacterDataManager(cfilepath);
        var Hp = card.Hp;
        var at = card.at;
        var df = card.df;
        var id = card.cardID;
        var cardinfo = cmanager.cardLvs[id];
        var atbuf = 1 + cardinfo.atbuf * 0.05f;
        var dfbuf = 1 + cardinfo.dfbuf * 0.05f;
        var hpbuf = 1 + cardinfo.hpbuf * 0.05f;

        hpText.text = "Hp:" + (Hp*hpbuf).ToString("F0") ;
        atText.text = "Attack:" + (at * atbuf).ToString("F0");
        dfText.text = "Defence:" + (df * dfbuf ).ToString("F0");
    }
    void SetPanel(string rare)
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
}
