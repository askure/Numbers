using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CardEditManager : MonoBehaviour
{
    [SerializeField] static private Transform lineUp;
    [SerializeField] static private Transform lineDown;
    [SerializeField] static private Transform cardListUp;
    [SerializeField] static private Transform cardListDown;
    [SerializeField] Transform Canvas;
    [SerializeField] private GameObject newDeck;
    [SerializeField] private CardController deckCard;
    [SerializeField] private CardController cardList;
    [SerializeField] private CardController cardNothave;
    [SerializeField] private TextMeshProUGUI partyName;
    [SerializeField] private TextMeshProUGUI partyHpText;
     static private GameObject editButton, saveButton, deleateButton, stopButton, stopCheckpanel,notRcardPanel, deleteChackPanel, nextButton, BackButton, sortieButton, nextCardListButton, beforeCardListButton;
    [SerializeField] GameObject DummyCard;

    static private CharacterData.CardLv[] cardLv;
    static public List<CardController> nowDeckCard;
    static CharacterDataManager cmanager;
    static DataManager dmanager;
    public static bool toqest;
    PopupCardView popup;

    string cfilepath, dfilepath;
    static public CardController popupCard;
    static public List<int> decklistTemp;
    static public bool Edit;
    static int partyNum = 0;
    static int cardListNum = 0;
    private string deckName = "";
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
        string mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        dmanager.StageDataLoad(mapfilepath);
        cardLv = cmanager.cardLvs;

        partyNum = cmanager.sortiePartyNum;
        decklistTemp = cmanager.deck[partyNum].cardId;
        deckName = cmanager.deck[partyNum].deckName;
        if(deckName == "")
            deckName = "パーティー" + (partyNum+1);
        if (partyNum < 0 || partyNum > 7) partyNum = 0;
        Setupobject();
        CreateDeck();
        CreateCardList();
        InitView();
        Edit = false;
        if (toqest)
        {
            EditButton();
        }

    }

    void Setupobject()
    {
        Debug.Log("Start SetUpObcect....");
        editButton = GameObject.Find("Edit");
        saveButton = GameObject.Find("Save");

        deleateButton = GameObject.Find("Delete");
        stopButton = GameObject.Find("Stop");
        nextButton = GameObject.Find("NextButton");
        BackButton = GameObject.Find("BackButton");
        stopCheckpanel = GameObject.Find("StopCheckPanel");
        deleteChackPanel = GameObject.Find("DeleteCheckPanel");
        notRcardPanel = GameObject.Find("NotReaderCardPanel");
        partyHpText = GameObject.Find("HpText").GetComponent<TextMeshProUGUI>();
        lineUp = GameObject.Find("DeckLine").transform;
        lineDown = GameObject.Find("DeckLine2").transform;
        cardListUp = GameObject.Find("CardList").transform;
        cardListDown = GameObject.Find("CardList2").transform;
        sortieButton = GameObject.Find("Sortie");
        beforeCardListButton = GameObject.Find("CardListBefore ");
        nextCardListButton = GameObject.Find("CardListNext");
        Debug.Log("End SetUpObcect....");
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
    public void DeleteDeck()
    {     
        decklistTemp.Clear();
        cmanager.deck[partyNum].cardId = decklistTemp;
        cmanager.deck[partyNum].deckName = deckName;
        cmanager.Datasave(cfilepath);
        cmanager.Datasave(cfilepath);
        CreateDeck();
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
        //beforeCardListButton.SetActive(false);
        Edit = false;
    }
    public void StopEdit()
    {   
        stopCheckpanel.SetActive(false);
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
        CreateDeck();
        CheckSerected();
        toqest = false;

    }
    void InitDeckData()
    {   
        decklistTemp = cmanager.deck[partyNum].cardId;
        deckName = cmanager.deck[partyNum].deckName;
    }
    public void ClosePanel()
    {
        stopCheckpanel.SetActive(false);
        deleteChackPanel.SetActive(false);
        notRcardPanel.SetActive(false);
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
        Edit = true;
    }
    public void SaveButton(bool flag)
    {   
        if(decklistTemp.Count != 0 && decklistTemp[0] == -1 && !flag)
        {
            notRcardPanel.SetActive(true);
            return;
        }
        decklistTemp.RemoveAll(x => x == -1);
        cmanager.deck[partyNum].cardId = decklistTemp;
        cmanager.deck[partyNum].deckName = deckName;
        cmanager.Datasave(cfilepath);
        cmanager = new CharacterDataManager(cfilepath);
        CreateDeck();
        CheckSerected();
        InitView();
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
        CreateDeck();
        CheckSerected();
        InitView();



    }
    public void BaxckPage()
    {
        if (partyNum == 0) partyNum = 6;
        else partyNum--;
        CreateDeck();
        CheckSerected();
        InitView();

    }
    public void SetSortieParty()
    {
        cmanager.sortiePartyNum = partyNum;
        cmanager.Datasave(cfilepath);
        sortieButton.SetActive(false);
    }
    void InitCardList()
    {
        for (int i = 0; i < cardListUp.childCount; i++)
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
        if ((cardListNum + 1) * 16 >= cardLv.Length) return;
        int stageNum = (cardListNum + 1) * 16;
        CardEntity cardEntity = Resources.Load<CardEntity>("CardEntityList/Card " + stageNum);
        if (!dmanager.stages[cardEntity.stageNum].clear) return;
        cardListNum++;
        InitCardList();
        CreateCardList();
    }
    public void CardListBefore()
    {
        if (cardListNum == 0) return;
        cardListNum--;
        InitCardList();
        CreateCardList();
    }
    void CreateDeck()
    {
        Debug.Log("Start CreateDeck....");
        ALLDelete();
        InitDeckData();
        if (partyNum < 0 || partyNum >= 7) partyNum = 0;
        nowDeckCard.Clear();

        partyName.text = deckName;

        if (decklistTemp == null) return;

        if (decklistTemp.Count == 0 && !toqest)
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

        for (int i = 0; i < 6; i++)
        {
            if (i >= decklistTemp.Count) break;
            var index = decklistTemp[i];
            if (index == -1)
                Instantiate(DummyCard, lineUp);
            else
            {
                CardController card = Instantiate(deckCard, lineUp);
                card.DeckEdiInit(index, cardLv[index].Lv);
                nowDeckCard.Add(card);
            }
          
        }
        for (int i = 6; i < 12; i++)
        {

            if (i >= decklistTemp.Count) break;
            var index = decklistTemp[i];
            if (index == -1)
                Instantiate(DummyCard, lineDown);
            else
            {
                CardController card = Instantiate(deckCard, lineDown);
                card.DeckEdiInit(index, cardLv[index].Lv);
                nowDeckCard.Add(card);
            }
        }
        PartyHpUpdate(nowDeckCard);
        Debug.Log("End SetUpObcect....");
    }
    void PartyHpUpdate(List<CardController> cards)
    {
        int hpSum = 0;
        foreach (CardController card in cards)
        {
            hpSum += card.model.Hp;
        }
        partyHpText.text = "合計Hp:" + hpSum.ToString();
    }

    void CreateCardList()
    {
        nextCardListButton.SetActive(true);
        beforeCardListButton.SetActive(true);
        var x = cardListNum * 16;
        for (int i = x; i < x + 8; i++)
        {
            if (i >= cardLv.Length)
            {
                nextCardListButton.SetActive(false);
                break;
            }
            CardController card;

            if (cardLv[i].pos == false)
            {
                
                card = Instantiate(cardNothave, cardListUp);
                card.CardlistInit(i, 0);
               
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
              
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                   
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
                   
                    continue;

                }
                if (decklistTemp.Contains(card.model.cardID))
                {
                   
                    var g = card.transform.Find("Serected");
                    if (g != null) g.gameObject.SetActive(true);
                    card.GetComponent<SelectCard>().isTodeck = false;
                   
                }
                else
                {
                  
                    var g = card.transform.Find("Serected");
                    if (g != null) g.gameObject.SetActive(false);
                    card.GetComponent<SelectCard>().isTodeck = true;
                   
                }
                    
                
            }
        }
        for (int i = x + 8; i < x + 16; i++)
        {
            if (i >= cardLv.Length)
            {
               
                nextCardListButton.SetActive(false);
                break;
            }
            CardController card;
            if (cardLv[i].pos == false)
            {
                
                card = Instantiate(cardNothave, cardListDown);
                card.CardlistInit(i, 0);
                
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
                }
            }
            else
            {
                card = Instantiate(cardList, cardListDown);
                card.DeckEdiInit(i, cardLv[i].Lv);
                if (card.model.stageNum != -1 && !dmanager.stages[card.model.stageNum].clear)
                {
                    Destroy(card.gameObject);
                    nextCardListButton.SetActive(false);
                    continue;
                }
                if (decklistTemp.Contains(card.model.cardID))
                {
                    card.transform.Find("Serected").gameObject.SetActive(true);
                    card.GetComponent<SelectCard>().isTodeck = false;
                }
                else 
                {
                    card.transform.Find("Serected").gameObject.SetActive(false);
                    card.GetComponent<SelectCard>().isTodeck = true;

                }
                    
                
            }


        }
        Debug.Log("End CreateCardList....");
    }

    public void SetPopUpcard()
    {

        var y = GameObject.Find("CardEditManager");
        popup = y.GetComponent<PopupCardView>();
        popup.SetText(popupCard.model);

    }
    public void SetDeckCard(int index)
    {
        CardController card;
        if (!decklistTemp.Contains(-1))
        {
            if (decklistTemp.Count < 6) card = Instantiate(deckCard, lineUp);
            else card = Instantiate(deckCard, lineDown);
            card.DeckEdiInit(index, cardLv[index].Lv);
            decklistTemp.Add(card.model.cardID);
        }
        else
        {
            var id = decklistTemp.IndexOf(-1);
            if(id < 6) card = Instantiate(deckCard, lineUp);
            else card = Instantiate(deckCard, lineDown);
            card.DeckEdiInit(index, cardLv[index].Lv);
            decklistTemp[id] = card.model.cardID;

        }
        CreateDeck();
    }

    public void DeckCardDelete(int cardId)
    {
      
        
        int id = decklistTemp.IndexOf(cardId);
        decklistTemp[id] = -1;
        CheckSerected();
        CreateDeck();

    }

    void CheckSerected()
    {
        var x = cardListNum * 16;
        for (int i = 0; i < 16; i++)
        {
            var cardId = i + x;
            
            if (i >= cardListUp.childCount && i < 8) continue;
            else if ((i - 8) >=cardListDown.childCount) continue;

            if (decklistTemp.Contains(cardId))
            {
                if (i < 8)
                {
                    var card = cardListUp.transform.GetChild(i);
                    var g = card.transform.Find("Serected");
                    if (g != null)g.gameObject.SetActive(true);
                    card.GetComponent<SelectCard>().isTodeck = false;
                }
                else
                {
                    var card = cardListDown.transform.GetChild(i - 8);
                    var g =card.transform.Find("Serected");
                    if(g != null)g.gameObject.SetActive(true);
                    card.GetComponent<SelectCard>().isTodeck = false;
                }
            }
            else
            {
                if (i < 8)
                {
                    var card = cardListUp.transform.GetChild(i);
                    var g = card.transform.Find("Serected");
                    if (g != null) g.gameObject.SetActive(false);
                    card.GetComponent<SelectCard>().isTodeck = true;
                }
                else
                {
                    var card = cardListDown.transform.GetChild(i - 8);
                    var g = card.transform.Find("Serected");
                    if (g != null)g.gameObject.SetActive(false);
                    card.GetComponent<SelectCard>().isTodeck = true;
                }
            }
        }
    }
    void ALLDelete()
    {
        GameObject line = GameObject.Find("DeckLine");
        int x = line.transform.childCount;
        for (int i = 0; i < x; i++)
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


    void InitView()
    {
        saveButton.SetActive(false);
        editButton.SetActive(true);
        deleateButton.SetActive(true);
        stopButton.SetActive(false);
        sortieButton.SetActive(false);
        nextButton.SetActive(true);
        BackButton.SetActive(true);
        stopCheckpanel.SetActive(false);
        deleteChackPanel.SetActive(false);
        notRcardPanel.SetActive(false);
        if (decklistTemp.Count == 0)
        {
            editButton.SetActive(false);
            deleateButton.SetActive(false);
        }
        else if(partyNum != cmanager.sortiePartyNum)
        {
            sortieButton.SetActive(true);
        }
        Debug.Log("End InitView");
    }





}