using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Onclick : MonoBehaviour
{
    // Start is called before the first frame update
    private CanvasGroup Color_obj;
    private GameObject info_obj;
    private CardController cardcontroll;
    [SerializeField] GameObject trigger;
    [SerializeField] GameObject TurnText;
    [SerializeField] GameObject Button;
    void Start()
    {   
        if(gameObject.name == "DECIDE" ) return;
        if (gameObject.name == "AutoSkillListButton") return;
        if (gameObject.name == "Trigger") return;
        
        cardcontroll = GetComponent<CardController>();
        Color_obj = transform.Find("Color").gameObject.GetComponent<CanvasGroup>();
        info_obj = transform.Find("Infomation").gameObject;
        Color_obj.alpha = 0f;
        info_obj.SetActive(false);
        
       






    }

    public void UpCard()
    {
       
        if(Color_obj.alpha == 0f)
        {
            if (GameManger.decide_num == 4) return;
            var x =transform.position.x;
            var y = transform.position.y;
            transform.position = new Vector3(x, y + 70, 0);
            Color_obj.alpha = 0.5f;
            cardcontroll.model.decided = true;
            var card = GetComponent<CardController>();
            GameManger.instnce.SetUpCard(card);
            GameManger.decide_num++;
           
        }
        else
        {
            var x = transform.position.x;
            var y = transform.position.y;
            transform.position = new Vector3(x, y - 70, 0);
            Color_obj.alpha = 0f;
            cardcontroll.model.decided = false;
            var card = GetComponent<CardController>();
            GameManger.instnce.RemoveCard(card);
            GameManger.decide_num--;
           
        }

        
    }

    public void InfomationUp()
    {
        Debug.Log("ポインタon");
    }

    public void InfomationDown()
    {
        Debug.Log("ポインタoff");
    }
    public void TriggerClick()
    {
        TurnText.SetActive(true);
        Button.SetActive(true);
        gameObject.SetActive(false);

    }
    public void Decide() {
       
        if (GameManger.Myturn == false) return;

        /*var cards = GameObject.FindGameObjectsWithTag("card");
        List<CardController> cardlist = new List<CardController>();
        if (cards.Length > 6) return;
        
        foreach(GameObject card in cards)
            cardlist.Add(card.GetComponent<CardController>());
          */
        var cardlist = GameManger.instnce.GetUpCard();
        
        GameManger.instnce.Sum(cardlist);
        GameManger.instnce.Buttle(cardlist);
        GameManger.instnce.InitUpcard();
        


       
    }
    public void AutoSKillListButton()
    {
        
        trigger.SetActive(true);
        var Panel = trigger.transform.GetChild(0);
        var hand = GameObject.Find("Hand");
        List<GameObject> _hand = new List<GameObject>();
        for(int i =0;i < hand.transform.childCount; i++)
        {
            var card = hand.transform.GetChild(i).gameObject;
            _hand.Add(card);
        }
        int index = 0;
        var sum = GameManger.sum;
        foreach (GameObject card in _hand)
        {
            var cardText = Panel.transform.GetChild(index);
            var color = card.transform.GetChild(2).GetComponent<CanvasGroup>().alpha;
            var Text = cardText.transform.GetChild(0).GetComponent<Text>();
            Text.text = card.GetComponent<CardController>().model.name;
            var auto = AutoSKillFlag(GameManger.instnce.GetUpCard(), card.GetComponent<CardController>());

            if (color == 0.5f && auto) Text.color = new Color(255, 0, 0);
            else if(color == 0.5f && !auto) Text.color = new Color(0, 255, 0);
            else Text.color = new Color(0, 0, 0);
            Text = cardText.transform.GetChild(1).GetComponent<Text>();
            Text.text = card.GetComponent<CardController>().model.PublicSkill.skill_infomatin;
            if (color == 0.5f && auto) Text.color = new Color(255, 0, 0);
            else if (color == 0.5f && !auto) Text.color = new Color(0, 255, 0);
            else Text.color = new Color(0, 0, 0);
            index++;
        }
        
        gameObject.SetActive(false);
        TurnText.SetActive(false);
    }

    bool AutoSKillFlag(List<CardController> cards,CardController card)
    {
        List<int> nums = new List<int>();
        GameManger gameManger = new GameManger();

        foreach (CardController x in cards)
        {
            nums.Add(x.model.num);
        }
        var auto = card.model.PublicSkill;
        var cardNum = card.model.num;
        var skillOrigin = auto.magic_Conditon_Origins;
        var SkillLength = skillOrigin.Count;
        if (SkillLength == 0) return false;
        bool[] autoInvocation = new bool[SkillLength];
        int index = 0;
        foreach (Skill_origin.magic_conditon_origin _Origin in skillOrigin)
        {

            var magicKind = _Origin.magic_kind;
            var conditonKind = _Origin.condition_kind;
            var conditionNum = _Origin.condition_num;
            var conditons = _Origin.conditons;
            var effect = _Origin.effect_size;
            autoInvocation[index] = gameManger.OutPutAutoSkillEffect(ref effect, magicKind, 0, conditonKind, conditionNum, conditons, nums, GameManger.hpSum, GameManger.sum);
            index++;

        }

        foreach(bool b in autoInvocation)
        {
            
            if (b) return true;
        }
        return false;
    }
}
