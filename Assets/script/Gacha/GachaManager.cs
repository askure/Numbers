using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;

public class GachaManager : MonoBehaviour
{
    [SerializeField] GameObject ShortagePanel, CheckPanel,OneMoerButton,StopButton,SkipButton,OneGachaButton,TenGachaButton,ToHomeButton;
    [SerializeField] GachaListEntity Gacha;
    [SerializeField] TextMeshProUGUI GachaName,StoneNum;
    [SerializeField] Transform CardUp, CardDown;
    [SerializeField] GachaCardView GachaCard;
    List<CardEntity> GachaResulttmp ;
    bool isten = false;
    Coroutine coroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        DataManager.DataLoad();
        CharacterDataManager.DataLoad();
        ShortagePanel.SetActive(false);
        CheckPanel.SetActive(false);
        OneMoerButton.SetActive(false);
        StopButton.SetActive(false);
        SkipButton.SetActive(false);
        OneGachaButton.SetActive(true);
        TenGachaButton.SetActive(true);
        ToHomeButton.SetActive(true);
        GachaName.text = Gacha.GachaName;
    }

    public void Update()
    {
        StoneNum.text = DataManager.Stone.ToString();
    }
    public void OneGacha()
    {
        if (isten)
            return;
        if (DataManager.Stone < 100)
        {
            OpenShortagePanel();
            return;
        }
        OneGachaButton.SetActive(false);
        TenGachaButton.SetActive(false);
        ToHomeButton.SetActive(false);
        CheckPanel.SetActive(false);
        SkipButton.SetActive(true);
        AllDestroy();
        List<GachaCardView> list = new List<GachaCardView>();
        GachaResulttmp = new List<CardEntity>();
        var choicecard = ChoiceCard(false);
        GachaResulttmp.Add(choicecard);
       
        var geted = CharacterDataManager.cardLvs[choicecard.cardID].pos;
        var gachacard = Instantiate(GachaCard, CardUp);
        gachacard.Init(choicecard, geted);
        list.Add(gachacard);
        if (geted)
        {
            int limitConvex = LimitConvex(choicecard.rare);
            if(limitConvex > CharacterDataManager.cardLvs[choicecard.cardID].convex)
            {
                CharacterDataManager.cardLvs[choicecard.cardID].convex++;
            }
        }
        else
        {
            CharacterDataManager.cardLvs[choicecard.cardID].pos = true;
            CharacterDataManager.cardLvs[choicecard.cardID].Lv = 1;
        }
        DataManager.Stone -= 100;

        CharacterDataManager.DataSave(false);
        DataManager.DataSave();
        coroutine = StartCoroutine(GachaAnimation(list));

    }

    private void OpenShortagePanel()
    {
        ShortagePanel.SetActive(true);
        ShortagePanel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text= DataManager.Stone.ToString();
    }
    public void ClosePanel()
    {
        ShortagePanel.SetActive(false);
        CheckPanel.SetActive(false);
        OneMoerButton.SetActive(false);
        StopButton.SetActive(false);
        SkipButton.SetActive(false);
        OneGachaButton.SetActive(true);
        TenGachaButton.SetActive(true);
        ToHomeButton.SetActive(true);
    }
    private int LimitConvex(string rare)
    {
        return rare switch
        {
            "A" => 8,
            "S" => 6,
            "SS" => 4,
            _ => 0,
        };
    }

    public void OpenGachaPanel(bool isTen)
    {
        this.isten = isTen;
       
        if (!this.isten)
        {
            if (DataManager.Stone < 100)
            {
                OpenShortagePanel();
                return;
            }
            CheckPanel.SetActive(true);
            CheckPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ストーンを100個消費して、ガチャを1回引きます。";
            CheckPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.Stone.ToString() +  "→" + (DataManager.Stone - 100).ToString();


        }
        else
        {
            if (DataManager.Stone < 1000)
            {
                OpenShortagePanel();
                return;
            }
            CheckPanel.SetActive(true);
            CheckPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ストーンを1000個消費して、ガチャを10回引きます。";
            CheckPanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = DataManager.Stone.ToString() + "→" + (DataManager.Stone - 1000).ToString();
        }
       
    }
    public void TenGacha()
    {
        if (!isten)
            return;

        if (DataManager.Stone < 1000)
        {
            OpenShortagePanel();
            return;
        }

        OneGachaButton.SetActive(false);
        TenGachaButton.SetActive(false);
        ToHomeButton.SetActive(false);
        CheckPanel.SetActive(false);
        AllDestroy();
        SkipButton.SetActive(true);
        List<GachaCardView> list = new List<GachaCardView>();
        GachaResulttmp = new List<CardEntity>();
        for(int i = 0; i < 5; i++)
        {
            var choicecard = ChoiceCard(false);
            GachaResulttmp.Add(choicecard);
           var geted = CharacterDataManager.cardLvs[choicecard.cardID].pos;
            var gachacard = Instantiate(GachaCard, CardUp);
            gachacard.Init(choicecard, geted);
            if (geted)
            {
                int limitConvex = LimitConvex(choicecard.rare);
                if (limitConvex > CharacterDataManager.cardLvs[choicecard.cardID].convex)
                {
                    CharacterDataManager.cardLvs[choicecard.cardID].convex++;
                }
            }
            else
            {
                CharacterDataManager.cardLvs[choicecard.cardID].pos = true;
                CharacterDataManager.cardLvs[choicecard.cardID].Lv = 1;
            }
            list.Add(gachacard);
        }
        for(int i = 0;i < 5; i++)
        {
            CardEntity choicecard;
            if (i == 4)
                choicecard = ChoiceCard(true);
            else
                choicecard = ChoiceCard(false);
            GachaResulttmp.Add(choicecard);
            var geted = CharacterDataManager.cardLvs[choicecard.cardID].pos;
            var gachacard = Instantiate(GachaCard, CardDown);
            gachacard.Init(choicecard, geted);
            if (geted)
            {
                int limitConvex = LimitConvex(choicecard.rare);
                if (limitConvex > CharacterDataManager.cardLvs[choicecard.cardID].convex)
                {
                    CharacterDataManager.cardLvs[choicecard.cardID].convex++;
                }
            }
            else
            {
                CharacterDataManager.cardLvs[choicecard.cardID].pos = true;
                CharacterDataManager.cardLvs[choicecard.cardID].Lv = 1;
            }
            list.Add(gachacard);
        }

        DataManager.Stone -= 1000;

        CharacterDataManager.DataSave(false);
        DataManager.DataSave();
        coroutine = StartCoroutine(GachaAnimation(list));
    }

    private CardEntity ChoiceCard(bool last)
    {   
        int r = Random.Range(0, 1000);
        CardEntity card;
        if (!last)
        {
            if (r <= Gacha.ProbabilitySS * 10)
            {
                //SS
                bool PickUp = Random.Range(0, 100) <= 4;
                if (PickUp && Gacha.PickUpCards.Count != 0)
                {
                    int index = Random.Range(0, Gacha.PickUpCards.Count);
                    card = Gacha.PickUpCards[index];
                }
                else
                {
                    int index = Random.Range(0, Gacha.SS.cards.Count);
                    card = Gacha.SS.cards[index];
                }
            }
            else if (r <= Gacha.ProbabilityS * 10)
            {
                //S
                int index = Random.Range(0, Gacha.S.cards.Count);
                card = Gacha.S.cards[index];
            }
            else
            {
                //A
                int index = Random.Range(0, Gacha.A.cards.Count);
                card = Gacha.A.cards[index];
            }
        }
        else
        {
            if (r <= Gacha.ProbabilitySS * 10)
            {
               
                //SS
                bool PickUp = Random.Range(0, 100) <= 4;
                if (PickUp && Gacha.PickUpCards.Count != 0)
                {
                    int index = Random.Range(0, Gacha.PickUpCards.Count);
                    card = Gacha.PickUpCards[index];
                }
                else
                {
                    int index = Random.Range(0, Gacha.SS.cards.Count);
                    card = Gacha.SS.cards[index];
                }
            }
            else
            {
                //S
                int index = Random.Range(0, Gacha.S.cards.Count);
                card = Gacha.S.cards[index];
            }
        }
        
        

        return card; 
    }

    public void Skip()
    {
        AllDestroy();
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        for (int i = 0, len = GachaResulttmp.Count;  i < len; i++)
        {   
            var choicecard = GachaResulttmp[i];
            var geted = CharacterDataManager.cardLvs[choicecard.cardID].pos;
            if (i < 5)
            {
                var gachacard = Instantiate(GachaCard, CardUp);
                gachacard.Init(choicecard, geted);
                gachacard.Skip();
            }
            else
            {
                var gachacard = Instantiate(GachaCard, CardDown);
                gachacard.Init(choicecard, geted);
                gachacard.Skip();
            }
        }
        SkipButton.SetActive(false);
        OneMoerButton.SetActive(true);
        StopButton.SetActive(true);
        ToHomeButton.SetActive(true);
    }

    private void AllDestroy()
    {
        for(int i= 0,len = CardUp.childCount;i<len;i++)
        {

            var obj = CardUp.GetChild(i).gameObject;
            Destroy(obj);
        }
        for (int i = 0, len = CardDown.childCount; i < len; i++)
        {

            var obj = CardDown.GetChild(i).gameObject;
            Destroy(obj);
        }
    }

    IEnumerator GachaAnimation(List<GachaCardView> gachaCards)
    {   
        for(int i= 0; i < gachaCards.Count; i++)
        {
           
            yield return new WaitForSeconds(gachaCards[i].StartAnimation());
        }
        Debug.Log("End Animation");
        OneMoerButton.SetActive(true);
        StopButton.SetActive(true);
        SkipButton.SetActive(false);
        ToHomeButton.SetActive(true);

    }


}
