using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardEditManager : MonoBehaviour
{
    [SerializeField] static  private Transform lineUp;
    [SerializeField] static private Transform lineDown;
    [SerializeField] static private Transform cardListUp;
    [SerializeField] static private Transform cardListDown;
    [SerializeField] Transform Canvas;
    [SerializeField] private GameObject newDeck;
    [SerializeField] private CardController deckCard;
    [SerializeField] private CardController cardList;
    [SerializeField] private CardController cardNothave;
    [SerializeField] private Text partyName;
    [SerializeField] static private Text partyHpText,partyCombatText;
    [SerializeField] static private GameObject editButton, saveButton, deleateButton, stopButton, stopCheckpanel, deleteChackPanel, nextButton, BackButton, sortieButton,nextCardListButton,beforeCardListButton;


    static  private CharacterData.CardLv[] cardLv;
    static public  List<CardController> nowDeckCard;
    static CharacterDataManager cmanager;
    static DataManager dmanager;
    public static bool toqest;
    PopupCardView popup;
   
    string cfilepath,dfilepath;
    static public CardController popupCard;
    static public List<int> decklistTemp;
    static public bool Edit;
    static int partyNum = 0;
    static int cardListNum=0;
    public static CardModel UnlockCardId;
    // Start is called before the first frame update
    void Start()
    {
        UnlockCardId = null;
       
        nowDeckCard = new List<CardController>();
        decklistTemp = new List<int>();
        cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json"; 
        dfilepath = Application.persistentDataPath + "/" + ".savedata.json";
        cmanager = new CharacterDataManager(cfilepath);
        dmanager = new DataManager(dfilepath);
        string  mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        dmanager.StageDataLoad(mapfilepath);
        cardLv = cmanager.cardLvs;

        partyNum = cmanager.sortiePartyNum;
        if (partyNum < 0 || partyNum > 7) partyNum = 0;
       // partyNum = 0;
       // cardListNum = 0;
        Setupobject();
        CreateDeck(partyNum);
        CreateCardList(cardListNum);
        stopCheckpanel.SetActive(false);
        deleteChackPanel.SetActive(false);
        Edit = false;
        if (toqest)
        {
            EditButton();
        }

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
        cardListUp = GameObject.Find("CardList").transform;
        cardListDown = GameObject.Find("CardList2").transform;
        sortieButton = GameObject.Find("Sortie");
        beforeCardListButton = GameObject.Find("CardListBefore ");
        nextCardListButton = GameObject.Find("CardListNext");
    }

    public void EditButton()
    {
        Edit = true;
        saveButton.SetActive(true);
        editButton.SetActive(false);
        deleateButton.SetActive(false);
        stopButton.SetActive(true);
        sortieButton.SetActive(false);
        nextButton.SetActive(false);
        BackButton.SetActive(false);
        beforeCardListButton.SetActive(true);
        nextCardListButton.SetActive(true);
    }
    public void DeleteButton()
    {
        if (nowDeckCard.Count == 0) return;
        deleteChackPanel.SetActive(true);
        nextCardListButton.SetActive(false);
        beforeCardListButton.SetActive(false);
        nextButton.SetActive(false);
        BackButton.SetActive(false);
        Edit = false;

    }
    public void  DeleteDeck()
    {
        InitDaeckData();
        var index = partyNum;
        var cardid = decklistTemp;
        cmanager.deck[index].cardId = cardid;
        cmanager.deck[index].deckName = partyName.text;
        cmanager.Datasave(cfilepath);
        var tempPartyNum = partyNum;
        cmanager.Dataload(cfilepath);
        while (cmanager.deck[tempPartyNum].cardId.Count == 0)
        {
            tempPartyNum++;
            if (tempPartyNum == cmanager.deck.Count) tempPartyNum = 0;
            if (tempPartyNum == partyNum)
            {
                tempPartyNum = -1;
                break;
            }
        }
        cmanager.sortiePartyNum = tempPartyNum;
        cmanager.Datasave(cfilepath);
        CreateDeck(partyNum);
        CheckSerected();
        deleteChackPanel.SetActive(false);
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
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
        CheckSerected();


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

        var index = partyNum;
        var cardid = decklistTemp;
        cmanager.deck[index].cardId = cardid;
        cmanager.deck[index].deckName = partyName.text;
        cmanager.Datasave(cfilepath);
        InitDaeckData();
        cmanager = new CharacterDataManager(cfilepath);
        CreateDeck(partyNum);
        CheckSerected();
        Notification.GetInstance().PutInQueue("保存しました");
        Edit = false;
        if (toqest)
        {
            SceneManager.LoadSceneAsync("Quest");
        }

    }
    public void NextPage()
    {
        
        if (partyNum == 6) partyNum = 0;
        else partyNum++;
        InitDaeckData();
        CreateDeck(partyNum);
        CheckSerected();



    }
    public void  BaxckPage()
    {         
        if (partyNum == 0) partyNum = 6;
        else partyNum--;
        InitDaeckData();
        CreateDeck(partyNum);
        CheckSerected();

    }
    public void SetSortieParty()
    {
        cmanager.sortiePartyNum = partyNum;
        cmanager.Datasave(cfilepath);
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
        if (!dmanager.stages[cardEntity.stageNum].clear) return;
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
    void CreateDeck(int partyNum)
    {

        if (partyNum < 0 || partyNum >= 7) partyNum = 0; 

        var x =  cmanager.deck[partyNum];
        
        if (x.deckName == "") partyName.text = "パーティ" + (partyNum + 1).ToString();
        else  partyName.text = x.deckName;
        var deck = x.cardId;

       
        if (deck == null)return;
        
        if(deck.Count == 0)
        {
           
            Instantiate(newDeck, lineUp);
            PartyHpUpdate(nowDeckCard);
            saveButton.SetActive(false);
            editButton.SetActive(false);
            deleateButton.SetActive(false);
            stopButton.SetActive(false);
            sortieButton.SetActive(false);
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
            var hp = card.model.Hp * (1 + cmanager.cardLvs[card.model.cardID].hpbuf*0.1f);
            var at = card.model.at * (1+ cmanager.cardLvs[card.model.cardID].atbuf*0.1f);
            var df = card.model.df * (1+ cmanager.cardLvs[card.model.cardID].dfbuf*0.1f);
            hpSum += (int)hp;
            combat += (int)(hp + at / 100 + df);
        }
        partyHpText.text = "合計Hp:" + hpSum.ToString();
        partyCombatText.text = "戦闘力:" + combat.ToString();
        
    }

    void CreateCardList(int pageNum)
    {
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
        if (pageNum == 0)
        {
            beforeCardListButton.SetActive(false);
        }
        var x = pageNum * 16;
        for(int i = x; i< x+ 8; i++)
        {
            if (i >= cardLv.Length) {
                nextCardListButton.SetActive(false);
                break;
            }
            CardController card;
            
            if (cardLv[i].pos == false)
            {
                card = Instantiate(cardNothave, cardListUp);
                card.CardlistInit(i,0);
                if(decklistTemp.Contains(card.model.cardID))
                   card.transform.Find("Serected").gameObject.SetActive(true);
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
                
                }
            }

            else
            {
                card = Instantiate(cardList, cardListUp);
                card.DeckEdiInit(i, cardLv[i].Lv);
                if (decklistTemp.Contains(card.model.cardID))
                    card.transform.Find("Serected").gameObject.SetActive(true);
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
                   
                }
            }
           


        }
        for(int i= x + 8; i < x + 16; i++)
        {
            if (i >= cardLv.Length) {
                nextCardListButton.SetActive(false);
                break;
            }
            CardController card;
            if (cardLv[i].pos == false)
            {
                card = Instantiate(cardNothave, cardListDown);
                card.CardlistInit(i,0);
                if (decklistTemp.Contains(card.model.cardID))
                    card.transform.Find("Serected").gameObject.SetActive(true);
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
                }
            }
            else {
                card = Instantiate(cardList, cardListDown);
                card.DeckEdiInit(i, cardLv[i].Lv);
                if (decklistTemp.Contains(card.model.cardID))
                    card.transform.Find("Serected").gameObject.SetActive(true);
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
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
        var x = cardListNum * 16;
        if (decklistTemp.Contains(cardId))
        {
            if (cardId < (x+16) && cardId >=x)
            {
                var index = cardId - x;
                if(index< 8)
                {
                    cardListUp.transform.GetChild(index).transform.Find("Serected").gameObject.SetActive(false);
                }
                else
                {
                    cardListDown.transform.GetChild(index-8).transform.Find("Serected").gameObject.SetActive(false);
                }
            }

        }
        decklistTemp.Remove(cardId);
        nowDeckCard.Remove(controller);
        
        PartyHpUpdate(nowDeckCard);
        
    }
    
    void CheckSerected()
    {
        var x = cardListNum * 16;
        for(int i=0; i<16; i++)
        {
            var cardId = i + x;
            
            if (i >= cardListUp.childCount && i<8) continue;
            else if ((i - 8) > cardListDown.childCount) continue;
            if (decklistTemp.Contains(cardId))
            {
                if (i < 8)
                {
                    cardListUp.transform.GetChild(i).transform.Find("Serected").gameObject.SetActive(true);
                }
                else
                {
                    cardListDown.transform.GetChild(i - 8).transform.Find("Serected").gameObject.SetActive(true);
                }
            }
            else
            {
                if (i < 8)
                {
                    cardListUp.transform.GetChild(i).transform.Find("Serected").gameObject.SetActive(false);
                }
                else
                {
                    cardListDown.transform.GetChild(i - 8).transform.Find("Serected").gameObject.SetActive(false);
                }
            }
        }
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
            if(partyNum == cmanager.sortiePartyNum)
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
        if (partyNum == cmanager.sortiePartyNum)
        {
            sortieButton.SetActive(false);
        }
    }

    


   
}
