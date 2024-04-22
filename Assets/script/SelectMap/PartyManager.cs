using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PartyManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CardController deckCard;
    [SerializeField] GameObject newDeck,go,toquest;
    [SerializeField] Transform lineUp, lineDown;
    [SerializeField] TextMeshProUGUI mapname, stagename, rskill, rskillinfo,pagetext;
    public void Init(string mapname,string stagename)
    {
        CharacterDataManager.DataLoad();
        ALLDestory();
        toquest.SetActive(false);
        this.mapname.text = mapname;
        this.stagename.text = stagename;
        var cardLv = CharacterDataManager.cardLvs;
        int index = CharacterDataManager.sortiePartyNum;
        if(index == -1)
            index = 0;
        pagetext.text = (index + 1).ToString() + "/" + CharacterDataManager.deck.Count;
        if(CharacterDataManager.deck[index].cardId.Count == 0)
        {
            Instantiate(newDeck, lineUp);
            rskill.text = "";
            rskillinfo.text = "";
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            var deck = CharacterDataManager.deck[index];
            var deckId = deck.cardId;
            List<int> numSum = new List<int>();
            for(int  i=0; i<6; i++)
            {
                if (i >= deckId.Count) break;
                CardController card = Instantiate(deckCard, lineUp);

                var  id= deckId[i];

                card.DeckEdiInit(id, cardLv[id].Lv);
                card.gameObject.transform.localScale = new Vector3(1f, 1f, 1);
                numSum.Add(card.model.num);
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
                numSum.Add(card.model.num);
            }
        }
    }

    public void NextParty()
    {
        CharacterDataManager.sortiePartyNum++;
        if (CharacterDataManager.sortiePartyNum >=CharacterDataManager.deck.Count)
            CharacterDataManager.sortiePartyNum = 0;
        Init(mapname.text, stagename.text);

    }

    public void BackParty()
    {
        CharacterDataManager.sortiePartyNum--;
        if (CharacterDataManager.sortiePartyNum < 0)
            CharacterDataManager.sortiePartyNum = CharacterDataManager.deck.Count-1;

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
        CharacterDataManager.DataSave(false);
        GameObject.Find("QuestManager").GetComponent<SelectMapManager>().StartStage();
        CardEditManager.toqest = false;
        Destroy(gameObject);
    }
    public void OpenToQuestPanel()
    {
        CharacterDataManager.DataSave(false);
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
