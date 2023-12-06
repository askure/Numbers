using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
   
    public GameObject ExpShoratage;
    [SerializeField]CardController Card;
    [SerializeField] Transform Transform;
     
    CharacterView characterView;
    public Text expText,pageText,pageMaxText;
    static  string filepath,cfilepath;
    public static CardController SerctedCard;
    //public static Playerstatus.CardLv[] cardLvs;
    static List<CharacterData.CardLv> cards;
    static int pageNum,pageMax;
    List<CardController> temp;
    bool changeText;
    private void Awake()
    {   
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        cards = new List<CharacterData.CardLv>();
        temp = new List<CardController>();
        
    }

    
    private void Start()
    {
        DataManager gameManger = new DataManager();
        gameManger.DataLoad(filepath);
        CharacterDataManager cmanager = new CharacterDataManager(cfilepath);
        CharacterData.CardLv[] cardLvs = cmanager.cardLvs;
        foreach(CharacterData.CardLv lv in cardLvs)
        {
            if (lv.pos == false) continue;
            cards.Add(lv);
           
        }
        pageMax = cards.Count / 5 + 1;
        if (cards.Count != 0 && cards.Count % 5 == 0) pageMax--;
        pageNum = 0;
        SetExpText();
        SetCard(pageNum);
        InitText();
        changeText = false;
    }

    public void SetCard
        (int pageNum)
    {   
        for(int i = pageNum*5; i < (pageNum*5) + 5; i++)
        {
            if (i == cards.Count) break;
            CardController card = Instantiate(Card, Transform);
            temp.Add(card);
            card.CahacterInit(cards[i].Id, cards[i].Lv);
        }
        pageText.text = (pageNum + 1).ToString();
        pageMaxText.text = pageMax.ToString();

    }

    void InitCard()
    {
        foreach(CardController card in temp)
        {
            var x = card.gameObject;
            Destroy(x);
        }
        temp.Clear();
    }

    public void NextPage()
    {

        if ((pageNum + 1) * 5 >= cards.Count) pageNum = 0;
        else pageNum++;
     
        InitCard();
        SetCard(pageNum);
    }
    public void BeforePage()
    {
        if (pageNum == 0) pageNum = pageMax-1;
        else pageNum--;
       
        InitCard();
        SetCard(pageNum);
    }
    public void LevelUp()
    {
        DataManager gameManger = new DataManager();
        gameManger.DataLoad(filepath);
        CharacterDataManager cmanager = new CharacterDataManager(cfilepath);
        if (SerctedCard == null) return;
        
        int id = SerctedCard.model.cardID;
        CharacterData.CardLv[] cardLvs = cmanager.cardLvs;
        var bufSum = cardLvs[id].atbuf+
                     cardLvs[id].dfbuf+
                     cardLvs[id].hpbuf;
        var convex = cardLvs[id].convex;
        
       
          
        
        if (cardLvs[id].Lv < 100 + 10 * convex)
        {
            int expSum = cardLvs[id].expSum;
            int needExp = GetExp(cardLvs[id].Lv + 1, expSum);
            if (needExp > gameManger.Exp)
            {
                var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
                var canva = GameObject.Find("Canvas").transform;
                var pane = Instantiate(gameObject, canva);
                pane.transform.GetChild(0).GetComponent<Text>().text = "EXPが足りません";
                var Button = pane.transform.GetChild(1).transform.GetChild(0).gameObject;
                Destroy(Button);
                return;
            }
            gameManger.Exp -= needExp;
            if (gameManger.Exp < 0) gameManger.Exp = 0;


           cardLvs[id].Lv++;
           if(cardLvs[id].Lv % 20 == 0 && cardLvs[id].Lv <=100) Notification.GetInstance().PutInQueue("スキルアップ!");
            cardLvs[id].expSum += GetExp(cardLvs[id].Lv, cardLvs[id].expSum);
           cmanager.cardLvs[id].expSum = cardLvs[id].expSum;
           cmanager.cardLvs[id].Lv = cardLvs[id].Lv;
           SerctedCard.CahacterInit(id, cardLvs[id].Lv);
           gameManger.DataSave(filepath);
           cmanager.Datasave(cfilepath); 
           SetText();
           SetExpText();

        }
        else if(bufSum < LimitBuf(SerctedCard.model.rare)  +(convex * 5))
        {
            
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canvas = GameObject.Find("Canvas").transform;
            var panel = Instantiate(gameObject, canvas);
            panel.transform.GetChild(0).GetComponent<Text>().text += "\n\n消費量:" + (bufSum * 4+1) +"個(現在:" + gameManger.Stone + "個)\n\nあと" + (LimitBuf(SerctedCard.model.rare) + (convex * 5) - bufSum)+"回強化可能";
        }
        else if(bufSum >= LimitBuf(SerctedCard.model.rare) + (convex*5) && convex != LimitConvex(SerctedCard.model.rare))
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterConvexUp");
            var canvas = GameObject.Find("Canvas").transform;
            var panel = Instantiate(gameObject, canvas);
            panel.transform.GetChild(0).GetComponent<Text>().text += "\n 強化前:" + convex.ToString() + "→強化後: " + (convex+1).ToString();

        }
        else
        {
            var gameObject = Resources.Load<GameObject>("CharacterPrehub/CharacterStatusUp");
            var canvas = GameObject.Find("Canvas").transform;
            var panel = Instantiate(gameObject, canvas);
            panel.transform.GetChild(0).GetComponent<Text>().text ="これ以上は強化できません";
            var Button = panel.transform.GetChild(1).transform.GetChild(0).gameObject;
            Destroy(Button);
        }
    }

    private int LimitBuf(string rare)
    {
        switch (rare)
        {
            case "A":
                return 10;
                
            case "S":
                return 15;
                
            case "SS":
                return 30;
                
            default:
                return 0;
        }
    }
    private int LimitConvex(string rare)
    {
        switch (rare)
        {
            case "A":
                return 8;

            case "S":
                return 6;

            case "SS":
                return 4;

            default:
                return 0;
        }
    }
    void SetExpText()
    {
        DataManager gameManger = new DataManager();
        gameManger.DataLoad(filepath);
        if (gameManger.Exp > 999999999)
        {
            expText.text = "999999999+";
        }
        else
        {
            
                expText.text =gameManger.Exp.ToString();
            
        }
    }
    public  void SetText()
    {

        DataManager gameManger = new DataManager();
        gameManger.DataLoad(filepath);
        var cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        CharacterDataManager cmanager = new CharacterDataManager(cfilepath);
        CharacterData.CardLv[] cardLvs = cmanager.cardLvs;
        
        var card = SerctedCard;
        var id = card.model.cardID;

        var x = new CardModel(id, cardLvs[id].Lv);
        var y = GameObject.Find("ChacterManager");
        characterView = y.GetComponent<CharacterView>();

        int expSum = cardLvs[id].expSum;
        int convex = cardLvs[id].convex;
        if (x.Lv == 1) expSum = x.firstExp;
        int needExp = GetExp( x.Lv + 1, expSum);

        var bufSum = cardLvs[id].atbuf +
                     cardLvs[id].dfbuf +
                     cardLvs[id].hpbuf;

        var ResetButton = GameObject.Find("StatusReset");
        if (ResetButton == null && (bufSum >= 10 && bufSum != LimitBuf(card.model.rare)))
        {
            var Button = Resources.Load<GameObject>("CharacterPrehub/StatusReset");
            var Line = GameObject.Find("ButtonLine").transform;
            var  Object =  Instantiate(Button, Line) as GameObject;
            Object.name = "StatusReset";
        }
        else if (ResetButton != null && (bufSum < 10 || bufSum == LimitBuf(card.model.rare) ))
        {
            Destroy(ResetButton);
        }
        if (x.Lv > 100 + 10 * convex) x.Lv = 100 + 10 * convex;
        if (x.Lv == (100 + 10 * convex) && bufSum < LimitBuf(card.model.rare) + (convex * 5))
        {
            GameObject.Find("Lvup").transform.GetChild(0).GetComponent<Text>().text = "ステータス強化";
        }
        else if(bufSum > LimitBuf(card.model.rare))
        {
            GameObject.Find("Lvup").transform.GetChild(0).GetComponent<Text>().text = "限界突破";

        }
        else
        {
            GameObject.Find("Lvup").transform.GetChild(0).GetComponent<Text>().text = "レベルアップ"; 
        }
        characterView.SetText(x,needExp);
    }

    private void InitText()
    {   
        var y = GameObject.Find("ChacterManager");
        characterView = y.GetComponent<CharacterView>();
        characterView.TextInit();
    }

    public void ChangeText()
    {
        if (SerctedCard == null) return;
        if (changeText)
        {
            SetText();
            changeText =false;

        }
        else
        {
            var cardModel = SerctedCard.model;
            var y = GameObject.Find("ChacterManager");
            characterView = y.GetComponent<CharacterView>();
            characterView.SetText(cardModel);
            changeText = true;
        }
       
    }
    public void RemoveCard()
    {
        if (SerctedCard == null) return;
        InitText();
        SerctedCard = null;
    }
    public CardController GetSerectedCard()
    {
        return SerctedCard;
    }
    
    int GetExp( int lv,int expSum)
    {
        int needExp = (expSum * 6) / 100 + lv * 200 + (lv - 50) * 4;
        if (needExp > 9999999) needExp = 9999999;
        return needExp;
    }




}
