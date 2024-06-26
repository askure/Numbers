using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CardModel 
{
    public int cardID;
    public bool advent;
    public int Hp;
    public int at;
    public int df;
    public int BeforeHp;
    public int BeforeAt;
    public int BeforeDf;
    public int num;
    public int firstExp;
    public int stageNum;
    public string name;
    public string rare;
    public int Lv;
    public Sprite icon;
    public bool decided = false;
    public bool onRedaerskill = false;
    public bool onBuf = false;
    public Skill_origin ReaderSkill;
    public Skill_origin PublicSkill;

    public CardModel(int cardId,int lv)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        cardID = cardEntity.cardID;
        var tenav = cardEntity.Hp * (double)lv / 99 + (99 * 10 - lv * 10);
        Hp = (int)tenav;
        tenav = cardEntity.at * (double)lv / 99 + (99 * 4 - lv * 4);
        at = (int)tenav;
        tenav = cardEntity.df * (double)lv / 99 + (99 * 5 - lv * 5);
        df = (int) tenav;
        num = cardEntity.num;
        name = cardEntity.name;
        icon = cardEntity.icon;
        rare = cardEntity.rare;
        Lv = lv;
        advent = cardEntity.advent;
        stageNum = cardEntity.stageNum;
        firstExp = cardEntity.firstExp;
        int skillLv = (Lv >=100) ? 5 : Lv / 20;
        ReaderSkill = cardEntity.ReaderSkill[skillLv];
        PublicSkill = cardEntity.AutoSkill[skillLv];

        CopyStatus();
    }
    public CardModel()
    {

    }
    public CardModel(int cardId)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        icon = cardEntity.icon;
        rare = cardEntity.rare;
        num = cardEntity.num;
        name = cardEntity.name;

    }



    public double  CardHp(int cardId,int lv,int hpuf)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        return (cardEntity.Hp * (double)lv / 99 + (99 * 10 - lv * 10) ) * (1 + hpuf * 0.1f);
    }

    public int CardNum(int cardId)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        return cardEntity.num;
    }
    public double CardDf(int cardId,int lv)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        return cardEntity.df * (double)lv / 99 + (99 * 5 - lv * 5);
    }
    public void CopyStatus()
    {
        BeforeAt = at;
        BeforeDf = df;
        BeforeHp = Hp;
    }
}
