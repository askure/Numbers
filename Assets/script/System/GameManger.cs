using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using TMPro;

public class GameManger : MonoBehaviour

{

    [SerializeField] Transform Canvaspos;
    [SerializeField] CardController Card;
    [SerializeField] Transform HandPos;
    [SerializeField] EnemyContoller Enemy;
    [SerializeField] Transform Enemys;
    [SerializeField] PartyDfStatusManager pstatusdf;
    [SerializeField] PartyAtManager pstatusat;
    [SerializeField] GameView GameView;
    [SerializeField] GameObject BattleNumText;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] DamageAnimation damageAnimation;
    [SerializeField] Animator enemyturnAnimation;
    [SerializeField] Animator partyturnAnimation;
    [SerializeField] SkillNameAnimation skillnameAnimation;
    [SerializeField] SpriteRenderer back;
    [SerializeField] AudioClip[] DefaultAudioClipIntro;
    [SerializeField] AudioClip[] DefaultAudioClipLoop;
    [SerializeField] GameObject OptionPanel;
    [SerializeField] SettingPanel SoundSettingPanel;
    [SerializeField] List<Sprite> tutorial,bounus_tutorial;
    [SerializeField] StatusListManager statusManager;
    [SerializeField] int debugenemy;
    [SerializeField] AudioClip skillse,damageLow,damageMid,damageHigh;

    BGMManager BGMManager;

    //System
    private const  int HANDNUM = 6;//手札上限
    public  static int SumNum; //合計数値
    public  static int DecideNum;//選択カード数
    public  static int TurnNum = 0; //ターン数
    public  static int PartyHp;//現在パーティHP
    public  static int PartyDf = 0;// 現在パーティDf
    public  static int MAX_HP;//パーティ最大HP

    //playerstatus
    private int PrimeLv; //素数ボーナスレベル
    private int DivisorLv;//約数ボーナスレベル
    private int MultiLv;//倍数ボーナスレベル
    private int AllCharacter;
    private CharacterData.CardLv[] CardLvs; //カードデータ
    private List<CharacterData.DeckCard> DeckList; //デッキデータ
    private float Volume;
   
    public List<CardController> HandCard = new List<CardController>();
    public List<int> DeckCard;
    public List<CardController> UpCard = new List<CardController>();
    public List<GameObject> UpCardObj = new List<GameObject>(); 
    public static List<string> LogText = new List<string>();
    
    public CardController ReaderCard;
    public int SortiePartyNum;
    bool OnlyOneReaderSkill;
    //staic 
    public static bool Myturn = true;
    static int BattleNum = 0;
    public static GameManger instnce;
    public static int MaxDamage;
    public static int MaxNum;
    public static int AveTurn;
    public static int EnemysExp;
    public static bool IsFinish;
    public static bool IsHandChange = false;

    //Enemy
    EnemyContoller EnemyCon;
    List<Skill_origin> EnemyUsedSkill = new List<Skill_origin>();
    [SerializeField] SkillViewNode viewNode;
    [SerializeField] GameObject SkillViewPanel,SkillViewButton, SkillViewBeforeButton, SkillViewAfterButton;
    [SerializeField] Transform SkillListPos;
    int SKillViewPage = 0;
    
    private void Awake()
    {
        if (instnce == null)
        {
            instnce = this;
        }
        DeckList = new List<CharacterData.DeckCard>();
        

    }
    void Start()
    {
        Dataload();
        BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        OptionPanel.SetActive(false);
       
        StartGame();
        StatusNumReset();



    }

  
    void StartGame()
    {
        //cardSetUp
        TurnNum = 1;
        SumNum = 0;
        DeckCard = DeckList[SortiePartyNum].cardId;
        List<int> tmp = new List<int>();
        foreach (int x in DeckCard)
        {
            tmp.Add(x);
        }
        
        for (int i = 1; i < DeckCard.Count; i++)
        {   
            int x = Random.Range(1, DeckCard.Count);
            int y = Random.Range(1, DeckCard.Count);
            (DeckCard[y], DeckCard[x]) = (DeckCard[x], DeckCard[y]);
        }
        var handCount = 0;
        for (int i = 0; i < HANDNUM; i++)
        {
            if (DeckCard.Count == 0) break;
            HandCard.Add(CardCreate(DeckCard[0]));

            DeckCard.RemoveAt(0);
            handCount++;

        }

        ReaderCard = HandCard[0];
        FieldEffectParty();
        BufApplication();
        //


        //enemySetUp
        if (SelectMapManager.enemy != null)
            EnemyCon = EnemyCreate(SelectMapManager.enemy[BattleNum].EnemyId, Enemys);
        else EnemyCon = EnemyCreate(debugenemy, Enemys);

        FieldEffectEnemy();
        SKillViewPage = 0;
        //

        //
        //BackGround
        back.sprite = SelectMapManager.BackGrounds;



        DecideNum = 0;
        IsFinish = false;
        BattleNum++;
        Myturn = true;
        OnlyOneReaderSkill = false;
        
        BattleNumText.GetComponent<TextMeshProUGUI>().text = "Battle " + BattleNum.ToString();
        if (SelectMapManager.enemy != null && BattleNum != SelectMapManager.enemy.Count)
        {
           // BattleNumText.GetComponent<Animator>().enabled = true;
          
            var t = GameObject.Find("Enemys").GetComponent<Animator>();
            t.enabled = true;
            if (SelectMapManager.stage != null)
            {
                if (SelectMapManager.stage.stageid == 0)
                {
                    StartCoroutine(Tutorial(2, tutorial));
                    DataManager.Battle_tutorial = true;
                }
                if (SelectMapManager.stage.stageid == 1)
                {
                    StartCoroutine(Tutorial(2, bounus_tutorial));
                }

            }
            if (BattleNum == 1)
            {   
                if(SelectMapManager.intro[0] == null || SelectMapManager.loop[0] == null)
                    BGMManager.SetBGM(DefaultAudioClipIntro[0], DefaultAudioClipLoop[0], Volume);
                else 
                    BGMManager.SetBGM(SelectMapManager.intro[0], SelectMapManager.loop[0], Volume);
                //BGMManager.Play();
            }

           
        }
        else
        {
            
            var t = GameObject.Find("BossAnimation").GetComponent<Animator>();
            t.enabled = true;
            if (SelectMapManager.intro == null  || SelectMapManager.intro[1] == null || SelectMapManager.loop[1] == null)
                BGMManager.SetBGM(DefaultAudioClipIntro[1], DefaultAudioClipLoop[1], Volume);
            else
                BGMManager.SetBGM(SelectMapManager.intro[1], SelectMapManager.loop[1], Volume);
            // BGMManager.Play();
            if (SelectMapManager.stage != null)
            {
                if (SelectMapManager.stage.stageid == 0)
                {
                    StartCoroutine(Tutorial(5, tutorial));
                    DataManager.Battle_tutorial = true;
                }
                if (SelectMapManager.stage.stageid == 1)
                {
                    StartCoroutine(Tutorial(5, bounus_tutorial));
                }
            }
           

        }
        //初めての戦闘(Battle1)なら、初期化
        if(BattleNum == 1)
        {
            MaxDamage = 0;
            MaxNum = 0;
            AveTurn = 0;
            EnemysExp = 0;
            
            
            CardModel controllerInstance = new CardModel();
            List<int> hp = new List<int>();
            List<int> num = new List<int>();
            LogText = new List<string>();
            foreach (int id in tmp)
            {

                hp.Add((int)controllerInstance.CardHp(id, CardLvs[id].Lv, CardLvs[id].hpbuf));
                num.Add( controllerInstance.CardNum(id));
                
            }
            if(SelectMapManager.stage != null) MAX_HP = SystemParam(num, hp,ReaderCard, SelectMapManager.stage.fieldEffects);
            else MAX_HP = SystemParam(num, hp, ReaderCard, null);
            PartyHp = MAX_HP;
            
        }
        var j = (MAX_HP- PartyHp) * 0.5;
        PartyHp += (int) j ;
        if (PartyHp > MAX_HP) PartyHp = MAX_HP;
        ReaderSkill();
        Df();
        GameView.Init();
        CardShowUpdate();


        //
        //log
        LogTextView("Battle:" + BattleNum);
        LogTextView("Turn:" + TurnNum.ToString());
        //
       
        



    }

    IEnumerator Tutorial(int time,List<Sprite> tutorial)
    {
        yield return new WaitForSeconds(time);
        var tuto = Resources.Load<GameObject>("tutorial");
        var canva = GameObject.Find("Canvas").transform;
        var g = Instantiate(tuto, canva);
        g.GetComponent<Tutorial>().SetUpTutorial(tutorial);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (OptionPanel.activeInHierarchy)
            {
                CloseOptionPanel();
            }
            else
            {
                OpenOptionPanel();
            }
        }

        
    }

    public void OpenOptionPanel()
    {
        if (OptionPanel.activeInHierarchy) return;
        OptionPanel.SetActive(true);
    }
    public void CloseOptionPanel()
    {
        if (!OptionPanel.activeInHierarchy) return;
        OptionPanel.SetActive(false);

    }
    void FieldEffectParty()
    {
        if (SelectMapManager.stage == null) return;
        foreach(StageEntity.FieldEffect fieldEffect in SelectMapManager.stage.fieldEffects)
        {
            if (fieldEffect.EnemyOrParty == StageEntity.EnemyOrParty.Party)
            {
                foreach (CardController card in HandCard)
                {
                    if (fieldEffect.effectCondition == 0 || Multi_check(card.model.num, fieldEffect.effectCondition))
                    {
                        
                        if(fieldEffect.buff == StageEntity.Buff.at)
                        {
                            var temp = card.model.at * fieldEffect.effectSize;
                            card.model.at = (int)temp;
                        }
                        if(fieldEffect.buff== StageEntity.Buff.df)
                        {
                            var temp = card.model.df * fieldEffect.effectSize;
                            card.model.df = (int)temp;
                        }
                        if(fieldEffect.buff == StageEntity.Buff.hp)
                        {
                            var temp = card.model.Hp * fieldEffect.effectSize;
                            card.model.Hp = (int)temp;
                        }
                        if (fieldEffect.buff == StageEntity.Buff.num)
                        {
                            var temp = card.model.num + fieldEffect.effectSize;
                            card.model.num = (int)temp;
                        }
                        if(fieldEffect.buff == StageEntity.Buff.all_not_num)
                        {
                            var temp = card.model.at * fieldEffect.effectSize;
                                card.model.at = (int)temp;

                                temp = card.model.df * fieldEffect.effectSize;
                                card.model.df = (int)temp;

                                temp = card.model.Hp * fieldEffect.effectSize;
                                card.model.Hp = (int)temp;
                        }


                    }
                    card.model.CopyStatus();
                    
                }
            }
            
        }        
    }

    void FieldEffectEnemy()
    {
        if (SelectMapManager.stage == null)
            return;
        foreach (StageEntity.FieldEffect fieldEffect in SelectMapManager.stage.fieldEffects)
        {   
            if (fieldEffect.EnemyOrParty == StageEntity.EnemyOrParty.Enemy)
            {

                if (BattleNum != fieldEffect.ApplicationNum) continue;

                 var effect = fieldEffect.effectSize;
                 var atStatus = new EnemyAttackStatus(effect, 999, Skill_origin.MagicKind.multi);
                EnemyCon.AddAttackStatus(atStatus);


                 var dfStatus = new EnemyDfStatus(effect,999,Skill_origin.MagicKind.multi);
                EnemyCon.AddDefenceStatus(dfStatus);
                 
                 var tempHp = EnemyCon.GetHp() * fieldEffect.effectSize;
                EnemyCon.SetMaxHp((int)tempHp);
                 
                 var temp = EnemyCon.GetNum() + fieldEffect.effectSize * 4;
                EnemyCon.SetMaxNum((int) temp);
                  
                
                
            }

        }
    }
  
    public void Dataload()
    {
        CharacterDataManager.DataLoad();
        DataManager.DataLoad();
        DivisorLv = DataManager.divisor_lv;
        MultiLv = DataManager.multi_lv;
        PrimeLv = DataManager.prime_lv;
        AllCharacter = CharacterDataManager.ALLCHARCTOR;
        CardLvs = new CharacterData.CardLv[AllCharacter];
        for (int i = 0; i < AllCharacter; i++)
        {  
          if (i >= CharacterDataManager.cardLvs.Length)
          {
                CardLvs[i] = new CharacterData.CardLv();
                CardLvs[i].Set(i, 1, 0, false,0,0,0,0);
          }
          else
          {
                var card = CharacterDataManager.cardLvs[i];
                CardLvs[i] = new CharacterData.CardLv();
                CardLvs[i].Set(card.Id, card.Lv, card.expSum, card.pos,card.atbuf,card.dfbuf,card.hpbuf,card.convex);
           }

        }
        for (int i = 0; i < 7; i++)
        {
            DeckList.Add(CharacterDataManager.deck[i]);
        }
        SortiePartyNum = CharacterDataManager.sortiePartyNum;
        Volume = DataManager.volume;
    }
    
    //BattleSystem
    CardController CardCreate(int Cardid)
    {
        CardController card = Instantiate(Card, HandPos);
        card.Init(Cardid, CardLvs[Cardid].Lv);
        return card;

    }
    EnemyContoller EnemyCreate(int Enemyid, Transform place)
    {   
        EnemyContoller enemy = Instantiate(Enemy, place);
        
        enemy.Init(Enemyid);
        return enemy;
    }

    public int[] Sum(List<CardController> cardlist)
    {   
        Bounus bounus = new Bounus();
        SumNum = 0;
        var prime_bounuse_check = true;
        var divisors = 0;
        var multis = 0;
        foreach (CardController card in cardlist)
        {
            if (card.model.decided == true)
            {
                SumNum += card.model.num;
                if (!Prime_number_check(card.model.num) || !prime_bounuse_check) prime_bounuse_check = false;
                if (Divisor_check(card.model.num, EnemyCon.GetNum())) divisors++;
                if (Multi_check(card.model.num, EnemyCon.GetNum())) multis++;
            }
        }

        if (cardlist.Count < 4) prime_bounuse_check = false;
        int multi_bounuses = (int)bounus.Multi_bounus(multis, MultiLv);
        int divisor_bounuses = (int)bounus.Divisor_bounus(divisors, DivisorLv);

        int prime_bounuse = 0;
        if (prime_bounuse_check) prime_bounuse = bounus.Prime_bounus(PrimeLv);

        SumNum += multi_bounuses + divisor_bounuses + prime_bounuse;

        int[] x = new int[2];

        x[0] = SumNum;
        x[1] = multi_bounuses + divisor_bounuses + prime_bounuse;
        return x;



    }

    bool Divisor_check(int card, int enemynum)
    {
        if (enemynum % card == 0) return true;
        return false;
    }

    bool Multi_check(int card, int enemynum)
    {   
        for (int i = 1; i <= card; i++) if (enemynum * i == card) return true;
        return false;
    }

    bool Prime_number_check(int card_num)
    {

        if (card_num <= 1) return false;
        if (card_num == 2) return true;
        for (int i = 2; i < card_num; i++) if (card_num % i == 0) return false;
        return true;
    }

    void Df() 
    {
        double df = 0.0;
        foreach(var hand in HandCard)
        {
            df += hand.model.df;
        }
        PartyDf = (int)df;

    }

   
    void CardShowUpdate()
    {
        foreach (CardController card in HandCard)
        {
            card.view.Show_Updte(card.model);
        }
    }
    public void SetUpCard(CardController card)
    {
        UpCard.Add(card);
        var x = Sum(UpCard);
        GameView.NumPowerText(x);

    }
    public void SetUpCardObj(GameObject card)
    {
        UpCardObj.Add(card);
    }
    public List<CardController> GetUpCard()
    {
        return UpCard;
    }
    public void RemoveCard(CardController card)
    {
        UpCard.Remove(card);
        var x = Sum(UpCard);
        GameView.NumPowerText(x);
    }
    public void RemoveCardObj(GameObject card)
    {
        UpCardObj.Remove(card);
    }
    public void InitUpcard()
    {
        UpCard.Clear();
        int[] x = { 0, 0 };
        GameView.NumPowerText(x);
    }
    private void LogTextView(string s)
    {
        AddLogText(s);
        if(LogText.Count > 25)
        {
            LogText.RemoveRange(0, LogText.Count - 25);
        }

        GameView.LogTextView(LogText.Skip(LogText.Count-5).ToList());
        

    }
    private void AddLogText(string s)
    {
        LogText.Add(s);
        
    }


    void  DeckSheffle()
    {
        for (int i = 0; i < DeckCard.Count; i++)
        {
            int x = Random.Range(0, DeckCard.Count);
            int y = Random.Range(0, DeckCard.Count);
            (DeckCard[y], DeckCard[x]) = (DeckCard[x], DeckCard[y]);
        }
    }
     void  HandChange()
    {

        List<CardController> temp = new List<CardController>();


        foreach (CardController card in HandCard)
        {   
            
            if (card.model.decided == true)
            {
                temp.Add(card);
                DeckCard.Add(card.model.cardID);
                Destroy(card.gbj);


            }

        }
        DeckSheffle();
        for (int i = 0; i < temp.Count; i++)
        {
           HandCard.Remove(temp[i]);
        }
        int changeNum = HANDNUM - HandCard.Count;
        for (int i = 0; i < changeNum; i++)
        {
            if (DeckCard.Count == 0) break;
            CardController card = Instantiate(Card, HandPos);
            card.Init(DeckCard[0], CardLvs[DeckCard[0]].Lv);
            DeckCard.RemoveAt(0);
            HandCard.Add(card);
        }

        DecideNum = 0;
        IsHandChange = true;
    }


    string Enemyattack(List<AnimationType> animations,ref List<int>  partydamage,ref int enemyHeal,ref int healNum,ref int enemydamage)
    {
        
        string skillname = "";
        var skillTable = EnemyCon.GetSKillTable();
        var skillTableLength = skillTable.Count;
        var skilllist = EnemyCon.GetSKillList();
     
            int index = TurnNum;
            if (skillTableLength == 0)
            {
                Debug.LogError("No skill table");
            }
            if (index <= skillTableLength)
            {

                index = TurnNum - 1;
            }
            else
            {
                while (index > skillTableLength)
                {
                    index -=skillTableLength;
                }
                index--;

            }
            var skillId = skillTable[index];
            skillname = skilllist[skillId].skill_name;
            animations.Add(AnimationType.skill);
            EnemySkill(animations, skillId, ref partydamage, ref enemyHeal,ref healNum,ref enemydamage);      
            return skillname;

    }

    private void EnemySkill(List<AnimationType> animations, int skillid, ref List<int> PartyDamage, ref int enemyHeal,ref int healNum,ref int enemydamage)

    {
        var skilllist = EnemyCon.GetSKillList();
        if (skilllist.Count < skillid + 1) return;
        var Skill = skilllist[skillid];
        if(!EnemyUsedSkill.Contains(Skill))
            EnemyUsedSkill.Add(Skill);

        for (int i = 0,len = Skill.magic_Conditon_Origins.Count; i < len; i++)
        {
            var _Origin = Skill.magic_Conditon_Origins[i];
            var effect = _Origin.effect_size;
            bool check = EnemySkillCheck(_Origin);
            var magicKind = _Origin.magic_kind;
            
            if (!check) continue;
             switch (_Origin.type)
            {
                case Skill_origin.Skill_type.constantAttack:
                     
                    PartyDamage.Add((int) effect);
                    animations.Add(AnimationType.damage);
                    break;


                case Skill_origin.Skill_type.referenceAttack:              
                    double damage = effect * EnemyCon.GetAt();
                    if (damage < PartyDf) PartyDamage.Add(1);
                    else PartyDamage.Add((int)damage - PartyDf);
                    animations.Add(AnimationType.damage);
                    break;
                
                case Skill_origin.Skill_type.Heal_Hp:
                    var x = effect * EnemyCon.GetAt();
                    animations.Add(AnimationType.enemyHeal);
                    enemyHeal = (int)x;
                  break;

                case Skill_origin.Skill_type.Heal_num:
                    animations.Add(AnimationType.healNum);
                    healNum = (int)effect;
                    break;


                case Skill_origin.Skill_type.damage:
                    enemydamage += (int)effect;
                    animations.Add(AnimationType.enemydamage);
                    break;


                case Skill_origin.Skill_type.IncreaseAttack:
                    animations.Add(AnimationType.enemyIncreaseAt);
                    {
                        var atStatus = new EnemyAttackStatus(effect, _Origin.effect_turn, _Origin.magic_kind);
                        EnemyCon.AddAttackStatus(atStatus);
                    }
                    break;

                    
                case Skill_origin.Skill_type.IncreaseDefence:
                    animations.Add(AnimationType.enemyIncreaseDf);
                    {
                        var dfStatus = new EnemyDfStatus(effect, _Origin.effect_turn, _Origin.magic_kind);
                        EnemyCon.AddDefenceStatus(dfStatus);
                    }
                    break;


                case Skill_origin.Skill_type.NumDamage:

                    EnemyCon.NumDamage(effect);
                    break;

                case Skill_origin.Skill_type.partydecreaseDefence:
                    if (magicKind == Skill_origin.MagicKind.add)
                    {
                        var g = Instantiate(pstatusdf);
                        g.SetStatusDf(effect,_Origin.effect_turn, "Add", EnemyCon.GetName());
                        g.name = "DfManager";
                    }
                    else if (magicKind == Skill_origin.MagicKind.multi)
                    {
                        var g = Instantiate(pstatusdf);
                        g.SetStatusDf(effect, _Origin.effect_turn, "Multi", EnemyCon.GetName());
                        g.name = "DfManager";
                    }
                    animations.Add(AnimationType.partydecrease);
                    break;

                case Skill_origin.Skill_type.partydecreaseAttack:
                    
                    if (magicKind == Skill_origin.MagicKind.add)
                    {
                        var g = Instantiate(pstatusat);
                        g.SetStatusAt(effect, _Origin.effect_turn, "Add", EnemyCon.GetName());
                        g.name = "AtManager";
                    }
                    else if (magicKind == Skill_origin.MagicKind.multi)
                    {
                        var g = Instantiate(pstatusat);
                        g.SetStatusAt(effect, _Origin.effect_turn, "Multi", EnemyCon.GetName());
                        g.name = "AtManager";
                    }
                    animations.Add(AnimationType.partydecrease);
                    break;


                case Skill_origin.Skill_type.decreaseDefence:
                    animations.Add(AnimationType.enemydecreasedf);
                    {
                        var dfStatus = new EnemyDfStatus(effect, _Origin.effect_turn, _Origin.magic_kind);
                        EnemyCon.AddDefenceStatus(dfStatus);
                    }
                    break;

                case Skill_origin.Skill_type.decreaseAttack:
                    animations.Add(AnimationType.enemydecreaseat);
                    {
                        var atStatus = new EnemyAttackStatus(effect, _Origin.effect_turn, _Origin.magic_kind);
                        EnemyCon.AddAttackStatus(atStatus);
                    }
                    break;


            }
        }

    }

    bool EnemySkillCheck(Skill_origin.magic_conditon_origin _Origin)
    {
        var conditonKind = _Origin.condition_kind;
        var conditionNum = _Origin.condition_num;
        var enemyNum = EnemyCon.GetNum();
        var enemyHp = EnemyCon.GetHp();
        switch (conditonKind)
        {
            case Skill_origin.Magic_condition_kind.Hp_up :
                if (enemyHp < conditionNum) return false;
                break;
            case Skill_origin.Magic_condition_kind.Hp_down:
                if (enemyHp >= conditionNum) return false;
                break;
            case Skill_origin.Magic_condition_kind.none:

                break;
            case Skill_origin.Magic_condition_kind.Num_up:
                if (enemyNum <  conditionNum) return false;

                break;
            case Skill_origin.Magic_condition_kind.Num_down:
                if (enemyNum >= conditionNum) return false;
                break;
            default: return false;
        }
        return true;
        
    }
    
    void  BufApplication()
    {
        List<CardController> temp = new List<CardController>();
        foreach(CardController card in HandCard)
        {
            if (card.model.onBuf) {
                temp.Add(card);
                continue;
            } 
            var id = card.model.cardID;
            //attackBuf
            var tenv = card.model.at * (1 + (CardLvs[id].atbuf*0.1f) );
            card.model.at = (int)tenv;
            //dfBuf
            tenv = card.model.df * (1 + (CardLvs[id].dfbuf * 0.1f));
            card.model.df = (int)tenv;
            card.model.onBuf = true;
            temp.Add(card);
        }
        HandCard = temp;
    }

    void ReaderSkill()
    {   
     
        for (int i = 0; i < ReaderCard.model.ReaderSkill.magic_Conditon_Origins.Count; i++)
        {
            var x = ReaderCard.model.ReaderSkill.magic_Conditon_Origins[i];
            
            switch ((int)x.type)
            {

                case 5:
                    if (OnlyOneReaderSkill) break;
                    double damge = PartyHp * x.effect_size;
                    OnlyOneReaderSkill = true;

                    PartyHp -= (int) damge;
                    break;

                case 6:
                    foreach (CardController hand in HandCard)
                    {
                        if (hand.model.onRedaerskill == false)
                        {

                            
                            hand.model.at = OutPutReaderskillEffect(hand.model.at, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num,MAX_HP);
                        }
                    }
                    break;

                case 7:
                    foreach (CardController hand in HandCard)
                    {
                        if (hand.model.onRedaerskill == false)
                        {

                           
                            hand.model.df = OutPutReaderskillEffect(hand.model.df, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num, MAX_HP);
                        }
                    }
                    break;
                case 8:
                    foreach (CardController hand in HandCard)
                    {
                        if (hand.model.onRedaerskill == false)
                        {


                            hand.model.num = OutPutReaderskillEffect(hand.model.num, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num, MAX_HP);
                        }
                    }
                    break;
                case 9:
                    foreach (CardController hand in HandCard)
                    {
                        if (hand.model.onRedaerskill == false)
                        {


                            hand.model.Hp = OutPutReaderskillEffect(hand.model.Hp, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num, MAX_HP);
                        }
                    }

                    break;


            }
        }
        foreach(CardController hand in HandCard)
        {
           
            hand.model.onRedaerskill = true;
        }

    }
    int SystemParam(List<int> num, List<int> hp,CardController readerCard, List<StageEntity.FieldEffect> fieldEffects)
    {
        
        foreach(int n in num)
        {
            var h = hp[0];
            
            hp.RemoveAt(0);
            foreach(Skill_origin.magic_conditon_origin x in readerCard.model.ReaderSkill.magic_Conditon_Origins)
            {   
               if(x.type == Skill_origin.Skill_type.IncreaseHp) 
                     h =  OutPutReaderskillEffect(h, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, n, MAX_HP);
            }
            if(fieldEffects != null)
            foreach (StageEntity.FieldEffect fieldEffect in fieldEffects)
            {   
                
                if (fieldEffect.EnemyOrParty == StageEntity.EnemyOrParty.Party)
                {
                    
                        if (fieldEffect.effectCondition == 0 || Multi_check(n, fieldEffect.effectCondition))
                        {
                            if(fieldEffect.buff == StageEntity.Buff.hp || fieldEffect.buff == StageEntity.Buff.all_not_num)
                            {
                                var temp = h * fieldEffect.effectSize;
                                h = (int)temp;
                            }
                           
                        }

                }

            }

            hp.Add(h);
        }
        int hpSum = 0;
        foreach(int h in hp)
        {
            hpSum += h;
        }
        return hpSum;
    }
    int OutPutReaderskillEffect(int param, Skill_origin.MagicKind magicKind, double effrct, Skill_origin.Magic_condition_kind condition_Kind, int conditionNum, int handNum,int hpSum)
    {
        switch ((int)condition_Kind)
        {
            case 2:
                if (Multi_check(handNum, conditionNum))
                {
                    break;
                }
                return param;

            case 3:
                if (Divisor_check(handNum, conditionNum))
                {
                    break;
                }
                return param;
            case 4:
                if (Prime_number_check(handNum))
                {
                    break;
                }
                return param;
            case 5:
                if (hpSum >= conditionNum) break;
                return param;
            case 6:
                if (hpSum < conditionNum) break;
                return param;
            case 7:
                break;
            case 8:
                if (handNum == conditionNum) break;
                return param;
            default:
                break;
                
        }
        switch ((int)magicKind)
        {
            case 0:
                return param + (int)effrct;
            case 1:
                double x = param * effrct;
                return (int)x  ;
            case 2:
                return  param-(int)effrct;


        }
        return 0;
    }

    void AutoSkill(List<AnimationType> animations,List<CardController> cards,ref double persuit,ref int teamHeal)
    {   
        List<int> nums = new List<int>();
        foreach(CardController card in cards)
        {
            nums.Add(card.model.num);
        }
        cards = SortAutoSkill(cards);
        foreach (CardController card in cards)
        {
            
            var auto = card.model.PublicSkill;
            var cardNum = card.model.num;
            var skillOrigin = auto.magic_Conditon_Origins;
           
            bool autoInvocation = false; 
            foreach (Skill_origin.magic_conditon_origin _Origin in skillOrigin)
            {
                var type = _Origin.type;
                var magicKind = _Origin.magic_kind;
                var conditonKind = _Origin.condition_kind;
                var conditionNum = _Origin.condition_num;
                var conditons = _Origin.conditons;
                var effect = _Origin.effect_size;
                var effectturn = _Origin.effect_turn;
                autoInvocation = AutoSkillCheck(conditonKind, conditionNum, conditons, nums);
                if (!autoInvocation) continue;
                switch (type)
                {

                    case Skill_origin.Skill_type.NumDamage: // NumDamage
                        if(magicKind == Skill_origin.MagicKind.add)
                        {
                            EnemyCon.NumDamage(effect);
                        }
                        break;

                    case Skill_origin.Skill_type.Heal_Hp: // Heal_Hp
                        var heal =  effect * Mathf.Log(card.model.at,8)/3;
                        teamHeal = (int)heal;
                        if(animations.IndexOf(AnimationType.heal) == -1)animations.Add(AnimationType.heal);
                        break;

                    case Skill_origin.Skill_type.IncreaseAttack: //IncreaceAttack
                        double at = card.model.at;
                        card.model.at = (int)at;
                        if (magicKind == Skill_origin.MagicKind.add)
                        {
                            var g = Instantiate(pstatusat);
                            g.SetStatusAt(effect, effectturn, "Add", card.model.name);
                            g.name = "AtManager";
                            g.SetStatusList();
                        }
                        else if (magicKind == Skill_origin.MagicKind.multi)
                        {
                            var g = Instantiate(pstatusat);
                            g.SetStatusAt(effect, effectturn, "Multi", card.model.name);
                            g.name = "AtManager";
                            g.SetStatusList();
                        }

                        break;

                    case Skill_origin.Skill_type.IncreaseDefence: // IncreaceDefence
                        if (magicKind == Skill_origin.MagicKind.add)
                        {
                            var g = Instantiate(pstatusdf);
                            g.SetStatusDf(effect, effectturn, "Add",card.model.name);
                            g.name = "DfManager";
                            g.SetStatusList();
                        }
                        else if (magicKind == Skill_origin.MagicKind.multi)
                        {
                            var g = Instantiate(pstatusdf);
                            g.SetStatusDf(effect, effectturn, "Multi",card.model.name);
                            g.name = "DfManager";
                            g.SetStatusList();
                        }
                        break;

                    case Skill_origin.Skill_type.Pursuit: //Pursuit
                        persuit += effect;
                        break;
                    case Skill_origin.Skill_type.NumUp: //Num up
                        if (magicKind == Skill_origin.MagicKind.add)
                        {
                            SumNum += (int)effect;
                        }
                        else if (magicKind == Skill_origin.MagicKind.multi)
                        {
                            double temp = SumNum * effect;
                            SumNum = (int)temp;
                        }
                        break;

                    case Skill_origin.Skill_type.decreaseDefence :  
                        var dfStatus = new EnemyDfStatus(effect, _Origin.effect_turn, _Origin.magic_kind);
                        EnemyCon.AddDefenceStatus(dfStatus);
                        break;


                    case Skill_origin.Skill_type.decreaseAttack:
                        {
                            var atStatus = new EnemyAttackStatus(effect, _Origin.effect_turn, _Origin.magic_kind);
                            EnemyCon.AddAttackStatus(atStatus);
                        }
                        break;

                    default:
                        break;
                }

               
            }
            

        }





    }
    List<CardController> SortAutoSkill(List<CardController> cards)
    {
        List<CardController> temps = new List<CardController>();
        List<CardController> heal = new List<CardController>();
        List<CardController> buff = new List<CardController>();
        List<CardController> attack = new List<CardController>();
        List<CardController> increase = new List<CardController>();
        foreach (CardController card in cards){
            if (card.model.PublicSkill._Priority == Skill_origin.Skill_priority.Buff ||
               card.model.PublicSkill._Priority == Skill_origin.Skill_priority.Debuff)
                buff.Add(card);
            if (card.model.PublicSkill._Priority == Skill_origin.Skill_priority.Attack)
                attack.Add(card);
            if (card.model.PublicSkill._Priority == Skill_origin.Skill_priority.Heal)
                heal.Add(card);
            if (card.model.PublicSkill._Priority == Skill_origin.Skill_priority.numIncrease)
                increase.Add(card);

        }
        temps.AddRange(increase);
        temps.AddRange(buff);
        temps.AddRange(heal);
        temps.AddRange(attack);
        return temps;
    }
    public bool AutoSkillCheck( Skill_origin.Magic_condition_kind condition_Kind, int conditionNum,int conditons, List<int> cardNums)
    {
        int x = 0;
       
        if (conditons == 0)
        {   
            switch ((int)condition_Kind)
            {   
                
                case (int)Skill_origin.Magic_condition_kind.sum_up:
                    if (SumNum < conditionNum) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.sum_down:
                    if (SumNum >= conditionNum) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.multi:
                    foreach (int cardNum in cardNums)
                        if (!Multi_check(cardNum, conditionNum))
                            x++;
                    if (x == 0) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.divisor:
                    foreach (int cardNum in cardNums)
                        if (!Divisor_check(cardNum, conditionNum))
                            x++;
                    if (x == 0) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.prime:
                    foreach (int cardNum in cardNums)
                        if (!Prime_number_check(cardNum))
                            x++;
                    if (x == 0) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.Hp_down:
                    if (PartyHp >= conditionNum) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.Hp_up:
                    if (PartyHp < conditionNum) return false;
                    break;

                case (int)Skill_origin.Magic_condition_kind.none: break;
                default:
                    return false;
            }
        }
        else
        {
            
            switch((int)condition_Kind)
            {
                
                case (int)Skill_origin.Magic_condition_kind.multi:
                    foreach (int cardNum in cardNums)
                        if (Multi_check(cardNum, conditionNum))
                            x++;
                    break;
                case (int)Skill_origin.Magic_condition_kind.divisor:
                    foreach (int cardNum in cardNums)
                        if (Divisor_check(cardNum, conditionNum))
                            x++;
                    break;
                case (int)Skill_origin.Magic_condition_kind.prime:
                    foreach (int cardNum in cardNums)
                        if (Prime_number_check(cardNum))
                            x++;
                    break;
           
            }
            if (x < conditons) return false;
        }
        
        return true;
    }
    
    
    public void OpenSKillViewPanel()
    {
        SkillViewPanel.SetActive(true);
        SkillViewButton.SetActive(false);
        SkillViewBeforeButton.SetActive(true);
        SkillViewAfterButton.SetActive(true);
        for (int i=0,len=SkillListPos.childCount; i<len;i++) { 
            Destroy(SkillListPos.GetChild(i).gameObject);
        }
        int length = EnemyUsedSkill.Count;
        if (SKillViewPage == 0)
            SkillViewBeforeButton.SetActive(false);
        if (((SKillViewPage + 1) * 6 + 1) > length)
            SkillViewAfterButton.SetActive(false);
            for (int i = 6*SKillViewPage; i<6*SKillViewPage + 6; i++)
        {
            if (i >= length)
                break;
            var origin = EnemyUsedSkill[i];
            var skill = Instantiate(viewNode, SkillListPos);
            skill.InitSkillViewNode(origin.skill_name, origin.skill_infomatin);
        }
        
    }

    public　void NextPage()
    {
        if (((SKillViewPage+1) * 6+1) >EnemyUsedSkill.Count)
            return;
        SKillViewPage++;
        OpenSKillViewPanel();

    }
    public void BeforePage()
    {
        if (SKillViewPage == 0)
            return;
        SKillViewPage--;
        OpenSKillViewPanel();
    }
    public void CloseViewPanel()
    {
        SkillViewPanel.SetActive(false);
        SkillViewButton.SetActive(true);
    }
    
    //BattleMain

    public void Battle(List<CardController> cardlist)
    {
        
        List<AnimationType> animations = new List<AnimationType>();
        double pesuit = 0;
        List<int> damage = new List<int>();
        int teamHeal = 0;
        List<int> teamDamage = new List<int>();
        int enemyHeal = 0;
        int healNum = 0;
        int enemydamage = 0;
        int HP;

        AutoSkill(animations,cardlist, ref pesuit,ref teamHeal);
        Df();
        string skillName = "";
        Myturn = false;

        if (!EnemyCon.NumDamage(SumNum))
        {
           
            animations.Add(AnimationType.block); //block
            if (pesuit != 0) animations.Add(AnimationType.persuit); //persuit

            if (EnemyCon.IsDeath((int)pesuit))
            {
                AveTurn += TurnNum;
                EnemysExp += EnemyCon.GetExp();
                if (SelectMapManager.enemy != null &&BattleNum != SelectMapManager.enemy.Count)
                {
                    animations.Add(AnimationType.nextStage); //NextStage
                    StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                    return;
                }
                else
                {
                    BattleNum = 0;
                    animations.Add(AnimationType.win); //WIN
                    StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal,healNum,enemydamage));
                    return;
                }

            }

            animations.Add(AnimationType.enemyturn);
            skillName = Enemyattack(animations, ref teamDamage,ref enemyHeal,ref healNum,ref enemydamage);
            Debug.Log(teamDamage.Sum());
            
            if (PartyHp + teamHeal > MAX_HP)
                HP = MAX_HP;
            else 
                HP = PartyHp + teamHeal;    
            if (HP - teamDamage.Sum() <=0)
            {
                //hpSum = 0;
              
                BattleNum = 0;
                IsFinish = true;
                animations.Add(AnimationType.gameover); //gameover
                StartCoroutine(AnimationList(animations, (int)pesuit,damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                return;
            }
            CardShowUpdate();
                       
            
            animations.Add(AnimationType.playerturn);
            StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
            return;
        }

        double bounus = 1 + 0.05 * EnemyCon.GetOverNum(SumNum);
        foreach (CardController card in cardlist)
        {
            if (card.model.decided == true)
            {
                double tmp = card.model.at * bounus - EnemyCon.GetDf();
                if (tmp < 1)
                    tmp = 1;
                damage.Add((int)tmp);
                AddLogText(card.model.name+ "が<color=red>" + (int)tmp + "</color>ダメージ");
                animations.Add(AnimationType.attack); //attack

            }
                

        }
       
 
        if (MaxDamage < damage.Max()) MaxDamage = damage.Max();
        if (MaxNum < SumNum) MaxNum = SumNum;      
        if (pesuit != 0) animations.Add(AnimationType.persuit); //persuit
        if (!EnemyCon.IsDeath(damage.Sum() + (int)pesuit))
        {
            animations.Add(AnimationType.enemyturn);
            animations.Add(AnimationType.numProtect);
            skillName = Enemyattack(animations, ref teamDamage, ref enemyHeal, ref healNum, ref enemydamage);
          
        }
        else
        {
            AveTurn += TurnNum;
            EnemysExp += EnemyCon.GetExp();
            if ((SelectMapManager.enemy != null) && BattleNum != SelectMapManager.enemy.Count)
            {
                animations.Add(AnimationType.nextStage); //NextStage
                StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                return;
            }
            else
            {
                BattleNum = 0;
                animations.Add(AnimationType.win); //WIN
                StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                return;
            }

        }
        if (PartyHp + teamHeal > MAX_HP)
            HP = MAX_HP;
        else
            HP = PartyHp + teamHeal;
        if (HP - teamDamage.Sum() <=0)
        {           
            BattleNum = 0;
            IsFinish = true;
            animations.Add(AnimationType.gameover);
            StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
            return;
        }
        CardShowUpdate();
        animations.Add(AnimationType.playerturn);
        StartCoroutine(AnimationList(animations, (int)pesuit,damage,skillName,teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
        
       

    }


    public void OpenSoundSetting()
    {

        Transform canvas = GameObject.Find("Canvas").transform;
        var g = Instantiate(SoundSettingPanel, canvas);
        g.SetUpPanel();
    }
   
   
    //Animation

    public void StatusNumReset()
    {
        PartyDfStatusManager.statusNum = 0;
        PartyDfStatusManager.statusSum = 0;
        PartyAtManager.statusSum = 0;
        PartyAtManager.statusNum = 0;
    }
    public void EnemyEntryAnimation()
    {
        GameObject.Find("Enemys").GetComponent<Animator>().enabled = true;
    }

    enum AnimationType
    {
        /*味方*/
        playerturn,/*味方のターン*/
        heal,/*味方の回復*/
        attack,/*敵へのダメージ*/
        persuit,/*敵への追撃*/
        block,/*ブレイク失敗*/

        /*敵行動*/
        enemyturn,/*敵のターン*/
        damage,/*味方へのダメージ*/
        numProtect,/*数値バリア復活*/
        skill,/*敵のスキル発動*/
        enemyIncreaseAt,/*敵の攻撃力アップ*/
        enemyIncreaseDf,/*敵の防御力ダウン*/
        enemydecreasedf,/*敵の防御力ダウン*/
        enemydecreaseat,/*敵の攻撃力ダウン*/
        enemyHeal,/*敵の回復*/
        healNum,/*敵の数値回復*/
        partydecrease,/*味方へのデバフ付与*/
        enemydamage, /*自傷*/

        /*System*/
        gameover,/*ゲームオーバー*/
        win,/*勝利*/
        nextStage,/*次バトル移行*/

    }

    IEnumerator AnimationList(List<AnimationType> vs,int persuit,List<int> damage,string skillname,List<int> teamDamage,int teamHeal,int enemyHeal,int healNum,int enemydamage)
    {
        yield return new WaitForSeconds(1f); /*SE遅延*/
        foreach (AnimationType i in vs)
        {
            switch (i)
            {
               
                case AnimationType.damage:
                    int tmp = teamDamage[0];
                    AddLogText("味方に<color=red>" + (int)teamDamage[0] + "</color>ダメージ");
                    teamDamage.RemoveAt(0);     
                    yield return StartCoroutine(DamageAnimation(tmp));
                    break;
                case AnimationType.heal:
                    yield return new WaitForSeconds(0.7f);
                    PartyHp += teamHeal;
                    AddLogText("味方が<color=green>" + (int)teamHeal + "</color>回復");
                    if (PartyHp > MAX_HP) PartyHp = MAX_HP;

                    break;
                case AnimationType.attack:
                    var g = UpCardObj[0];
                    var x = g.transform.position.x;
                    var y = g.transform.position.y;
                    g.transform.position = new Vector3(x, y +40, 0);
                    NotificationBattle.GetInstance().PutInQueue(damage[0].ToString());
                    if (BGMManager == null)
                        BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
                    if(damage[0] >= 1 && damage[0] < EnemyCon.GetHp() * 0.01)
                    {
                        BGMManager.PlaySE(damageLow,7f);
                    }
                    else if(damage[0] < EnemyCon.GetHp() * 0.5)
                    {
                        BGMManager.PlaySE(damageMid, 2f);
                    }
                    else
                    {
                        BGMManager.PlaySE(damageHigh, 2f);
                    }
                    yield return new WaitForSeconds(0.2f);
                    x = g.transform.position.x;
                    y = g.transform.position.y;
                    g.transform.position = new Vector3(x, y - 40, 0);
                    UpCardObj.RemoveAt(0);
                    EnemyCon.HpDamage(damage[0]);   
                    damage.RemoveAt(0);
                    yield return new WaitForSeconds(0.4f);
                    break;
                case AnimationType.enemydamage:
                    NotificationBattle.GetInstance().PutInQueue(enemydamage.ToString());
                    EnemyCon.HpDamage(enemydamage);
                    AddLogText(EnemyCon.GetName()  + enemydamage + "の自傷");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.persuit:    
                    NotificationBattle.GetInstance().PutInQueue("<color=black>" + persuit.ToString()+ "</color>");
                    EnemyCon.HpDamage(persuit);
                    AddLogText("敵に" + persuit + "の追撃");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemydecreasedf:
                    NotificationBattle.GetInstance().PutInQueue("<color=blue>" + "防御力ダウン" + "</color>");
                    AddLogText(EnemyCon.GetName() + "が防御力ダウン");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemydecreaseat:
                    NotificationBattle.GetInstance().PutInQueue("<color=blue>" + "攻撃力力ダウン" + "</color>");
                    AddLogText(EnemyCon.GetName() + "が攻撃力ダウン");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.skill:
                    yield return StartCoroutine(SkillAnimation(skillname));
                    AddLogText("敵のスキル:" + skillname);
                    break;
              
                case AnimationType.gameover:
                    PartyHp = 0;
                    yield return StartCoroutine(GameOverAnimation());
                    DataManager.DataSave();
                    AddLogText("GAME OVER");
                    break;
                case AnimationType.win:
                    Destroy(EnemyCon.gbj);
                    BGMManager.FadeOut();
                    yield return new WaitForSeconds(1f);
                    DataManager.DataSave();
                    AddLogText("YOU WIN");
                    SceneManager.LoadScene("Result");
                    break;
                case AnimationType.block:
                    AddLogText("数値バリアを破壊できなかった");
                   
                    NotificationBattle.GetInstance().PutInQueue("BLOCK");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.nextStage:
                    if ((BattleNum+1) == SelectMapManager.enemy.Count)
                    {
                        BGMManager.FadeOut();
                    }
                    yield return new WaitForSeconds(1f);
                    DataManager.DataSave();
                    AddLogText("YOU WIN");
                    SceneManager.LoadScene("Battle");
                    break;
                case AnimationType.numProtect:
                    EnemyCon.RevivalNum();
                    yield return new WaitForSeconds(0.7f);
                    AddLogText(EnemyCon.GetName()  + "の数値バリアが" + EnemyCon.GetNum() + "に回復");
                    break;

                case  AnimationType.enemyturn:
                    yield return StartCoroutine(EnemyTurnAnimation());
                    AddLogText("-EnemyTurn-");

                    break;
                case AnimationType.playerturn:
                        yield return StartCoroutine(ParyTurnAnimation());
                        AddLogText("-PlayerTurn-");
                    break;
                case AnimationType.healNum:
                    NotificationBattle.GetInstance().PutInQueue("<color=blue>" + healNum.ToString() + "</color>");
                    yield return new WaitForSeconds(0.7f);
                    AddLogText(EnemyCon.GetName() +"が数値バリアを" + healNum + "回復");
                    EnemyCon.HealNum(healNum);
                    break;
                case AnimationType.enemyHeal:
                    NotificationBattle.GetInstance().PutInQueue("<color=green>" + enemyHeal.ToString() + "</color>");
                    EnemyCon.HealHp(enemyHeal);
                    AddLogText(EnemyCon.GetName() + "がHPを" + enemyHeal + "回復");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemyIncreaseAt:
                    NotificationBattle.GetInstance().PutInQueue("<color=red>" + "攻撃力アップ!" + "</color>");
                    AddLogText(EnemyCon.GetName() + "が攻撃アップ");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemyIncreaseDf:
                    NotificationBattle.GetInstance().PutInQueue("<color=red>" + "防御力アップ!" + "</color>");
                    AddLogText(EnemyCon.GetName() + "が防御力アップ");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.partydecrease:
                    statusManager.SetGameObject();
                    Df();
                    yield return new WaitForSeconds(0.7f);
                    break;


            }

        }
        UpCardObj.Clear();
        HandChange();
        FieldEffectParty();
        BufApplication();
        ReaderSkill();
        Myturn = true;
        if(!vs.Contains(AnimationType.nextStage))
        {
            TurnNum++;
            LogTextView("Turn:" + TurnNum.ToString());
            EnemyCon.CheckStatus();
        }
    }
    IEnumerator SkillAnimation(string skillname)
    {
        SkillNameAnimation e = Instantiate(skillnameAnimation, Canvaspos);
        int s = e.StartAniamtion(skillname);
        yield return new WaitForSeconds(s);
        Destroy(e.gameObject);
    }
    IEnumerator DamageAnimation(int damage)
    {

        DamageAnimation e = Instantiate(damageAnimation, Canvaspos);
        int s = e.startAnimation(damage);
        yield return new WaitForSeconds(s); 

        Destroy(e.gameObject);


    }
    IEnumerator EnemyTurnAnimation()
    {
        Animator e = Instantiate(enemyturnAnimation, Canvaspos);
        e.enabled = true;
        e.Play(0);
        var s = e.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(s);
        Destroy(e.gameObject);


    }
    IEnumerator ParyTurnAnimation()
    {
        Animator e = Instantiate(partyturnAnimation, Canvaspos);
        e.enabled = true;
        e.Play(0);
        var s = e.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(s );
        Destroy(e.gameObject);


    }

    IEnumerator GameOverAnimation()
    {
        
        BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        BGMManager.FadeOut();
        var panel = Instantiate(GameOverPanel, Canvaspos);
        panel.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(panel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
    }

    public void ENDAnimation()
    {
       // BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        BGMManager.FadeOut();
        BattleNum = 0;
        var panel = Instantiate(GameOverPanel, Canvaspos);
        panel.GetComponent<Animator>().enabled = true;
    }

 
}
