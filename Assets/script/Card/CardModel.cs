using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour
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
    public new string name;
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
        var tenav = cardEntity.Hp * (double)lv / 100 + (100 * 30 - lv * 30);
        BeforeHp =  Hp = (int)tenav;
        tenav = cardEntity.at * (double)lv / 100 + (100 * 8 - lv * 8);
        BeforeAt = at = (int)tenav;
        tenav = cardEntity.df * (double)lv / 100 + (100 * 10 - lv * 10);
        BeforeDf =  df = (int) tenav;
        num = cardEntity.num;
        name = cardEntity.name;
        icon = cardEntity.icon;
        rare = cardEntity.rare;
        Lv = lv;
        advent = cardEntity.advent;
        stageNum = cardEntity.stageNum;
        firstExp = cardEntity.firstExp;
        ReaderSkill = cardEntity.ReaderSkill;
        PublicSkill = cardEntity.AutoSkill;
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
        return (cardEntity.Hp * (double)lv / 99 + (99 * 30 - lv * 30) ) * (1 + hpuf * 0.05f);
    }

    public int CardNum(int cardId)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        return cardEntity.num;
    }
    public double CardDf(int cardId,int lv)
    {
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + cardId);
        return cardEntity.df * (double)lv / 99 + (99 * 10 - lv * 10);
    }
}
