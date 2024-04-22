using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterDataManager 
{
    static public CharacterData.CardLv[] cardLvs { set; get; } = null;
    static public List<CharacterData.DeckCard> deck { set; get; } = new List<CharacterData.DeckCard>();
    static public int sortiePartyNum { set; get; } = 0;
    static public int ALLCHARCTOR { set; get; } = 0;
    static readonly string Filepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
    static  bool IsDataLoad = false;


    public static void DataLoad()
    {
        if (File.Exists(Filepath))
        {

            StreamReader streamReader;
            streamReader = new StreamReader(Filepath);
            string data = streamReader.ReadToEnd();
            streamReader.Close();
            var playerstatus_save = JsonUtility.FromJson<CharacterData>(data);
            if (!IsDataLoad) 
            {
                ALLCHARCTOR = playerstatus_save.allCharactor;
                cardLvs = new CharacterData.CardLv[ALLCHARCTOR];
                for (int i = 0; i < ALLCHARCTOR; i++)
                {
                    if (i >= playerstatus_save.cardLvs.Length)
                    {
                        cardLvs[i] = new CharacterData.CardLv();
                        cardLvs[i].Set(i, 1, 0, false, 0, 0, 0, 0);
                    }
                    else
                    {
                        var card = playerstatus_save.cardLvs[i];
                        cardLvs[i] = new CharacterData.CardLv();
                        cardLvs[i].Set(card.Id, card.Lv, card.expSum, card.pos, card.atbuf, card.dfbuf, card.hpbuf, card.convex);
                    }

                }
                sortiePartyNum = playerstatus_save.sortiePartyNum;
            }

            deck = new List<CharacterData.DeckCard>();
            for (int i = 0; i < playerstatus_save.cards.Length; i++)
            {

                deck.Add(playerstatus_save.cards[i]);
            }
            
            
        }
        else
        {
            DataInit();
        }
        IsDataLoad = true;

    }


   

    public static void DataSave(bool deckSave)
    {
        Debug.Log("Start CharacterDataSave....");
        TextAsset textAsset = Resources.Load<TextAsset>("CardData");
        StringReader reader = new StringReader(textAsset.text);
        var ac = -1;
        while (reader.Peek() != -1)
        {
            ac++;
            reader.ReadLine();
        }
        var playerstatus_save = new CharacterData
        {
            allCharactor = ac,
            cardLvs = new CharacterData.CardLv[ac],
            cards = new CharacterData.DeckCard[7],
            sortiePartyNum = sortiePartyNum
        };

        for (int i = 0; i < ac; i++)
        {
            playerstatus_save.cardLvs[i] = new CharacterData.CardLv
            {
                Id = i,
                Lv = cardLvs[i].Lv,
                pos = cardLvs[i].pos,
                expSum = cardLvs[i].expSum,
                atbuf = cardLvs[i].atbuf,
                dfbuf = cardLvs[i].dfbuf,
                hpbuf = cardLvs[i].hpbuf,
                convex = cardLvs[i].convex

            };

        }
        for (int i = 0; i < 7; i++)
        {
             playerstatus_save.cards[i] = deck[i];
        }
        
        

        string json = JsonUtility.ToJson(playerstatus_save, true);
        StreamWriter streamWriter = new StreamWriter(Filepath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
        Debug.Log("End CharacterDataSave");
    }


    static private void DataInit()
    {
        Debug.Log("Start DataInit...");
        TextAsset textAsset = Resources.Load<TextAsset>("CardData");
        StringReader reader = new StringReader(textAsset.text);
        var ac = -1;
        while (reader.Peek() != -1)
        {
            ac++;
            reader.ReadLine();
        }
        var playerstatus_save = new CharacterData
        {
            allCharactor = ac,
            cardLvs = new CharacterData.CardLv[ac],
            cards = new CharacterData.DeckCard[7],
            sortiePartyNum = 0
        };

        for (int i = 0; i < 11; i++)
        {
            playerstatus_save.cardLvs[i] = new CharacterData.CardLv
            {
                Id = i,
                Lv = 1,
                pos = true,
                expSum = 0,
                atbuf = 0,
                dfbuf = 0,
                hpbuf = 0,
                convex = 0

            };

        }
        for (int i = 11; i < ac; i++)
        {
            playerstatus_save.cardLvs[i] = new CharacterData.CardLv
            {
                Id = i,
                Lv = 1,
                pos = false,
                expSum = 0,
                atbuf = 0,
                dfbuf = 0,
                hpbuf = 0,
                convex = 0

            };

        }
        for (int i = 0; i < 7; i++)
        {

            playerstatus_save.cards[i] = new CharacterData.DeckCard();
            playerstatus_save.cards[i].deckId = i;
            playerstatus_save.cards[i].deckName = "パーティ" + i;
            if (i == 0)
            {
                playerstatus_save.cards[i].cardId = new List<int>();
                for (int j = 0; j < 11; j++)
                    playerstatus_save.cards[i].cardId.Add(j);
            }
        }


        string json = JsonUtility.ToJson(playerstatus_save, true);
        StreamWriter streamWriter = new StreamWriter(Filepath);
        streamWriter.Write(json); streamWriter.Flush();
        streamWriter.Close();
        Debug.Log("End DataInit");
    }


}
