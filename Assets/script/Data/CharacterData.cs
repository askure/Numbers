using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public int allCharactor;
    public CardLv[] cardLvs;
    public DeckCard[] cards;
    public int sortiePartyNum;
    [System.Serializable]
    public class CardLv
    {
        public int Id;
        public int Lv;
        public int expSum;
        public int atbuf;
        public int dfbuf;
        public int hpbuf;
        public int convex;
        public bool pos;

        public void Set(int id, int lv, int exp, bool ps, int at, int df, int hp, int con)
        {
            Id = id;
            Lv = lv;
            pos = ps;
            expSum = exp;
            atbuf = at;
            dfbuf = df;
            hpbuf = hp;
            convex = con;


        }

    }
    [System.Serializable]
    public class DeckCard
    {
        public int deckId;
        public string deckName;
        public List<int> cardId;
    }

}
