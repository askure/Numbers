using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PartyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CardController deckCard;
    [SerializeField] GameObject newDeck,go,toquest;
    [SerializeField] Transform lineUp, lineDown;
    [SerializeField] Text mapname, stagename, rskill, rskillinfo,pagetext;
    CharacterDataManager manager;
    bool first = false;
    string filepath;
    private void Awake()
    {
        
    }
    void Start()
    {
        var filepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        manager = new CharacterDataManager(filepath);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string mapname,string stagename)
    {
    
        ALLDestory();
        toquest.SetActive(false);
        if (!first)
        {
           filepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
            manager = new CharacterDataManager(filepath);
            first = true;
        }
        this.mapname.text = mapname;
        this.stagename.text = stagename;
        var cardLv = manager.cardLvs;
        int index = manager.sortiePartyNum;
        pagetext.text = (index + 1).ToString() + "/" + manager.deck.Count;
        if(manager.deck[index].cardId.Count == 0)
        {
            Instantiate(newDeck, lineUp);
            rskill.text = "";
            rskillinfo.text = "";
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            var deck = manager.deck[index];
            var deckId = deck.cardId;
            for(int  i=0; i<6; i++)
            {
                if (i >= deckId.Count) break;
                CardController card = Instantiate(deckCard, lineUp);

                var  id= deckId[i];

                card.DeckEdiInit(id, cardLv[id].Lv);
                card.gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                if(i == 0)
                {
                    rskill.text = card.model.ReaderSkill.skill_name;
                    rskillinfo.text = card.model.ReaderSkill.skill_infomatin;
                }
            }
            for(int i = 6; i<12; i++)
            {
                if (i >= deckId.Count) break;
                CardController card = Instantiate(deckCard, lineDown);

                var id = deckId[i];

                card.DeckEdiInit(id, cardLv[id].Lv);
                card.gameObject.transform.localScale = new Vector3(1f, 1f, 1);
            }
        }
    }

    public void NextParty()
    {
        manager.sortiePartyNum++;
        if (manager.sortiePartyNum >=manager.deck.Count)
            manager.sortiePartyNum = 0;
        Init(mapname.text, stagename.text);

    }

    public void BackParty()
    {
        manager.sortiePartyNum--;
        if (manager.sortiePartyNum < 0)
            manager.sortiePartyNum = manager.deck.Count-1;

        Init(mapname.text, stagename.text);

    }

    private void ALLDestory()
    {
        for(int i=0; i< lineUp.childCount; i++)
        {
            Destroy(lineUp.GetChild(i).gameObject);
        }
        for (int i = 0; i < lineDown.childCount; i++)
        {
            Destroy(lineDown.GetChild(i).gameObject);
        }
    }
    public void StartStage()
    {
        manager.Datasave(filepath);
        GameObject.Find("QuestManager").GetComponent<CrectmapManager>().StartStage();
        CardEditManager.toqest = false;
        Destroy(gameObject);
    }
    public void OpenToQuestPanel()
    {
        manager.Datasave(filepath);
        CardEditManager.toqest = true;
        toquest.SetActive(true);
    }

    public void LoadDeckEdit()
    {
        
        SceneManager.LoadSceneAsync("DeckEdit");
    }
    public void CloseToQuestPanel()
    {
        CardEditManager.toqest = false;
        toquest.SetActive(false);
    }

    public void ClosePanel()
    {
        CardEditManager.toqest = false;
        Destroy(gameObject);
    }
}
