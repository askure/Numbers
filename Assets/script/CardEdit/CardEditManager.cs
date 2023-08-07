using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardEditManager : MonoBehaviour
{
    [SerializeField] static  private Transform lineUp;
    [SerializeField] static private Transform lineDown;
    [SerializeField] private Transform cardListUp;
    [SerializeField] private Transform cardListDown;
    [SerializeField] Transform Canvas;
    [SerializeField] private GameObject newDeck;
    [SerializeField] private CardController deckCard;
    [SerializeField] private CardController cardList;
    [SerializeField] private CardController cardNothave;
    [SerializeField] private Text partyName;
    [SerializeField] static private Text partyHpText,partyCombatText;
    [SerializeField] static private GameObject editButton, saveButton, deleateButton, stopButton, stopCheckpanel, deleteChackPanel, nextButton, BackButton, sortieButton,nextCardListButton,beforeCardListButton;


    static  private Playerstatus.CardLv[] cardLv;
    static public  List<CardController> nowDeckCard;
    static GameManger manger;
    
    PopupCardView popup;
   
    string filepath;
    static public CardController popupCard;
    static public List<int> decklistTemp;
    static public bool Edit;
    static int partyNum;
    static int cardListNum;
    public static CardModel UnlockCardId;
    // Start is called before the first frame update
    void Start()
    {
        UnlockCardId = null;
        manger = new GameManger();
        nowDeckCard = new List<CardController>();
        decklistTemp = new List<int>();
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        manger.Dataload(filepath);
        string  mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        manger.StageDataLoad(mapfilepath);
        cardLv = manger.GetCardLvs();
        cardListNum = 0;
        Setupobject();
        CreateDeck(0);
        CreateCardList(cardListNum);
        stopCheckpanel.SetActive(false);
        deleteChackPanel.SetActive(false);
        Edit = false;
    }

    void Setupobject()
    {
        editButton = GameObject.Find("Edit");
        saveButton = GameObject.Find("Save");

        deleateButton = GameObject.Find("Delete");
        stopButton = GameObject.Find("Stop");
        nextButton = GameObject.Find("NextButton"); 
        BackButton = GameObject.Find("BackButton"); 
        stopCheckpanel = GameObject.Find("StopCheckPanel");
        deleteChackPanel = GameObject.Find("DeleteCheckPanel");
        partyHpText = GameObject.Find("HpText").GetComponent<Text>();
        partyCombatText = GameObject.Find("CombatScoreText").GetComponent<Text>();
        lineUp = GameObject.Find("DeckLine").transform;
        lineDown = GameObject.Find("DeckLine2").transform;
        sortieButton = GameObject.Find("Sortie");
        beforeCardListButton = GameObject.Find("CardListBefore ");
        nextCardListButton = GameObject.Find("CardListNext");
    }

    public void EditButton()
    {
        Edit = true;
        ChangeView(false,false);
    }
    public void DeleteButton()
    {
        if (nowDeckCard.Count == 0) return;
        deleteChackPanel.SetActive(true);
        nextCardListButton.SetActive(false);
        beforeCardListButton.SetActive(false);
        Edit = false;

    }
    public void  DeleteDeck()
    {
        InitDaeckData();
        manger.SetDeck(partyNum, partyName.text, decklistTemp);
        manger.Datasave(filepath);
        var tempPartyNum = partyNum;
        manger.Dataload(filepath);
        while (manger.GetDeck(tempPartyNum).cardId.Count == 0)
        {
            
            tempPartyNum++;
            if (tempPartyNum > 7) tempPartyNum = 0;
            if (tempPartyNum == partyNum)
            {
                tempPartyNum = -1;
                break;
            }
        }
        manger.sortiePartyNum = tempPartyNum;
        manger.Datasave(filepath);
        CreateDeck(partyNum);
        deleteChackPanel.SetActive(false);
        Notification.GetInstance().PutInQueue("削除しました");
    }

    public void StopButton()
    {
        stopCheckpanel.SetActive(true);
        nextCardListButton.SetActive(false);
        beforeCardListButton.SetActive(false);
        Edit = false;
    }
    public void StopEdit()
    {
        InitDaeckData();
        stopCheckpanel.SetActive(false);
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
        CreateDeck(partyNum);
       
    }
    void InitDaeckData()
    {
        decklistTemp.Clear();
        nowDeckCard.Clear();
        
        ALLDelete();
    }
    public void ClosePanel()
    {
        stopCheckpanel.SetActive(false);
        deleteChackPanel.SetActive(false);
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
        Edit = true;
    }
    public void SaveButton()
    {
       
        manger.SetDeck(partyNum, partyName.text,decklistTemp);
        manger.Datasave(filepath);
        InitDaeckData();
        manger = new GameManger();
        manger.Dataload(filepath);
  
        CreateDeck(partyNum);
       
        Notification.GetInstance().PutInQueue("保存しました");
        Edit = false;
    }
    public void NextPage()
    {
        
        if (partyNum == 6) partyNum = 0;
        else partyNum++;
        InitDaeckData();
        CreateDeck(partyNum);
            
        
    }
    public void  BaxckPage()
    {         
        if (partyNum == 0) partyNum = 6;
        else partyNum--;
        InitDaeckData();
        CreateDeck(partyNum);
    }
    public void SetSortieParty()
    {
        manger.sortiePartyNum = partyNum;
        manger.Datasave(filepath);
        sortieButton.SetActive(false);
    }
    void InitCardList()
    {
        for(int i = 0; i< cardListUp.childCount; i++)
        {
            var obj = cardListUp.GetChild(i);
            Destroy(obj.gameObject);
        }
        for (int i = 0; i < cardListDown.childCount; i++)
        {
            var obj = cardListDown.GetChild(i);
            Destroy(obj.gameObject);
        }
    }
    public void CardListNext()
    {
        if ((cardListNum + 1) * 16 >=cardLv.Length) return ;
        int stageNum = (cardListNum + 1) * 16;
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + stageNum);
        if (!manger.stages[cardEntity.stageNum].clear) return;
        cardListNum++;
        InitCardList();
        CreateCardList(cardListNum);
    }
    public void CardListBefore()
    {
        if (cardListNum == 0) return;
        cardListNum--;
        InitCardList();
        CreateCardList(cardListNum);
    }
    void CreateDeck(int patryNum)
    {
      
        if (patryNum < 0 || patryNum >= 7) return;

        var x =  manger.GetDeck(patryNum);
        
        if (x.deckName == "") partyName.text = "パーティ" + (patryNum+1).ToString();
        else  partyName.text = x.deckName;
        var deck = x.cardId;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                

        if (deck == null)return;
        
        if(deck.Count == 0)
        {
           
            Instantiate(newDeck, lineUp);
            PartyHpUpdate(nowDeckCard);
            ChangeView(false, true);
            nextButton.SetActive(true);
            BackButton.SetActive(true);

            return;
        }

        for(int i = 0; i < 6; i++)
        {
            

            if (i >= deck.Count) break;
           
            CardController card = Instantiate(deckCard, lineUp);
            
            var index = deck[i];
           
            card.DeckEdiInit(index,cardLv[index].Lv);
            
            nowDeckCard.Add(card);
            
            decklistTemp.Add(card.model.cardID);
            
           
           
            


        }
        for(int i = 6; i< 12; i++)
        {
           
            if (i >= deck.Count) break;
          
            CardController card = Instantiate(deckCard, lineDown);
            
            var index = deck[i];
            card.DeckEdiInit(index, cardLv[index].Lv);
            nowDeckCard.Add(card);
            decklistTemp.Add(card.model.cardID);
           

        }

        PartyHpUpdate(nowDeckCard);
        ChangeView(true, false);

        

    }

    void PartyHpUpdate(List<CardController> cards)
    {
        int hpSum = 0;
        int combat = 0;
        foreach(CardController card in cards)
        {
            var hp = card.model.Hp * Mathf.Pow(1.1f, manger.cardLvs[card.model.cardID].hpbuf);
            var at = card.model.at * Mathf.Pow(1.1f, manger.cardLvs[card.model.cardID].atbuf);
            var df = card.model.df * Mathf.Pow(1.1f, manger.cardLvs[card.model.cardID].dfbuf);
            hpSum += (int)hp;
            combat += (int)(hp + at / 100 + df);
        }
        partyHpText.text = "合計Hp:" + hpSum.ToString();
        partyCombatText.text = "戦闘力:" + combat.ToString();
        
    }

    void CreateCardList(int pageNum)
    {   
        var x = pageNum * 16;
        for(int i = x; i< x+ 8; i++)
        {
            if (i >= cardLv.Length) break;
            CardController card;
            
            if (cardLv[i].pos == false)
            {
                card = Instantiate(cardNothave, cardListUp);
                card.CardlistInit(i,0);
                if(card.model.stageNum != -1 && !manger.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                }
            }

            else
            {
                card = Instantiate(cardList, cardListUp);
                card.DeckEdiInit(i, cardLv[i].Lv);
                if (card.model.stageNum != -1 && !manger.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                }
            }
           


        }
        for(int i= x + 8; i < x + 16; i++)
        {
            if (i >= cardLv.Length) break;
            CardController card;
            if (cardLv[i].pos == false)
            {
                card = Instantiate(cardNothave, cardListDown);
                card.CardlistInit(i,0);
                if (card.model.stageNum != -1 && !manger.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                }
            }
            else {
                card = Instantiate(cardList, cardListDown);
                card.DeckEdiInit(i, cardLv[i].Lv);
                if (card.model.stageNum != -1 && !manger.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                }
            } 
            

        }
    }

    public void SetPopUpcard()
    {   
        
        var y = GameObject.Find("CardEditManager");
        popup = y.GetComponent<PopupCardView>();
        popup.SetText(popupCard.model);

    }
    public void SetDeckCard(int index,CardController controller)
    {
        if (decklistTemp.Count > 12) return;
        CardController card;
        if(decklistTemp.Count < 6)  card = Instantiate(controller, lineUp);
        else  card = Instantiate(controller, lineDown);
        
        card.DeckEdiInit(index, cardLv[index].Lv);
        
        nowDeckCard.Add(card);
        decklistTemp.Add(card.model.cardID);
       
        PartyHpUpdate(nowDeckCard);
        

    }

    public void DeckCardDelete(int cardId,CardController controller)
    {   
        decklistTemp.Remove(cardId);
        nowDeckCard.Remove(controller);
        PartyHpUpdate(nowDeckCard);
        
    }
    
    void ALLDelete()
    {
        GameObject line = GameObject.Find("DeckLine");
        int x = line.transform.childCount;
        for(int i = 0; i < x; i++)
        {
            Destroy(line.transform.GetChild(i).gameObject);
        }
         line = GameObject.Find("DeckLine2");
         x = line.transform.childCount;
        for (int i = 0; i < x; i++)
        {
            Destroy(line.transform.GetChild(i).gameObject);
        }
    }
    void ChangeView(bool on_off,bool all)
    {   
        if (all)
        {
            saveButton.SetActive(on_off);
            editButton.SetActive(on_off);
            deleateButton.SetActive(on_off);
            stopButton.SetActive(on_off);
            sortieButton.SetActive(on_off);
            if(partyNum == manger.sortiePartyNum)
            {
                sortieButton.SetActive(false);
            }
            return;
        }
        saveButton.SetActive(!on_off);
        editButton.SetActive(on_off);
        deleateButton.SetActive(on_off);
        stopButton.SetActive(!on_off);
        nextButton.SetActive(on_off);
        BackButton.SetActive(on_off);
        
        sortieButton.SetActive(on_off);
        if (partyNum == manger.sortiePartyNum)
        {
            sortieButton.SetActive(false);
        }
    }

    


   
}
