using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class GameManger : MonoBehaviour

{
    [SerializeField] Transform canvaspos;
    [SerializeField] CardController Card;
    [SerializeField] Transform Hand;
    [SerializeField] EnemyContoller Enemy;
    [SerializeField] Transform Enemys;
    [SerializeField] EnemyAttackStatus estatusat;
    [SerializeField] EnemyDfStatus estatusdf;
    [SerializeField] PartyDfStatusManager pstatusdf;
    [SerializeField] PartyAtManager pstatusat;
    [SerializeField] GameView gameView;
    [SerializeField] GameObject buttleNumText;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] Transform gameOverPanelTrance;
    [SerializeField] DamageAnimation damageAnimation;
    [SerializeField] Animator enemyturnAnimation;
    [SerializeField] Animator partyturnAnimation;
    [SerializeField] SkillNameAnimation skillnameAnimation;
    [SerializeField] SpriteRenderer back;
    [SerializeField] AudioClip[] DefaultAudioClipIntro;
    [SerializeField] AudioClip[] DefaultAudioClipLoop;
    [SerializeField] GameObject OptionPanel;
    [SerializeField] GameObject SoundSettingPanel;
    [SerializeField] List<Sprite> tutorial,bounus_tutorial;
    [SerializeField] StatusListManager statusManager;
    [SerializeField] int debugenemy;
    [SerializeField] AudioClip skillse;

    BGMManager BGMManager;
    Slider volumeslider;
    
     //numbers
    public static int sum;
    public static int decide_num;
    private bool prime_bounuse_check = true;
    private int prime_bounuse = 0;
    private int divisor_bounuses = 0;
    private int multi_bounuses = 0;
    private int divisors = 0;
    private int multis = 0;
    static public int TurnNum = 0;
    
    

    //playerstatus
    private int prime_lv;
    private int divisor_lv;
    private int multi_lv;
    private int ALLCHARCTOR;
    private CharacterData.CardLv[] cardLvs;
    private List<CharacterData.DeckCard> deck;
    private bool buttle_tutorial;
    public static int hpSum;
    public static int partyDf = 0;
    static  private int MAX_HP;
    private float volume;
    EnemyContoller _enemy;
    public List<CardController> _hand = new List<CardController>();
    public List<int> decklist;
    public List<CardController> UpCard = new List<CardController>();
    public List<GameObject> UpCardObj = new List<GameObject>(); 
    static public List<string> logText = new List<string>();
    
   // public List<int> decklist;
    public CardController ReaderCard;
    public int sortiePartyNum;
    bool OnlyOneReaderSkill;
    //stage
    public int allStage;
    public StageData.Stage[] stages;
    // Enemystatus
    private int MAX_ENEMYHP;
    private int MAX_NUMBA;

    //skill
    private List<Skill_origin> Enemy_skilllist;
    private List<int> Enemy_skilllistTable;


    //Savedata
    string filepath,cfilepath;
    DataManager dmanager;
    CharacterDataManager cmanager;
    //staic 
    public static bool Myturn = true;
    static int ButtleNum = 0;
    public static GameManger instnce;
    public static int maxDamage;
    public static int maxNum;
    public static int aveTurn;
    public static int enemysexp;
    public static bool finish;
    public static bool handchange = false;


    //debug
    double debugAt;
    double debugBounus;
     
    

    //StartGame
    public GameManger()
    {
        
        cardLvs = new CharacterData.CardLv[ALLCHARCTOR];
        deck = new List<CharacterData.DeckCard>();

    }
    private void Awake()
    {
        if (instnce == null)
        {
            instnce = this;
        }
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        cfilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
      
        deck = new List<CharacterData.DeckCard>();
        

    }
    void Start()
    {
        Dataload();
        // DataInit(Application.persistentDataPath + "/" + ".savemapdata.json");

        BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        OptionPanel.SetActive(false);
        SoundSettingPanel.SetActive(false);
 
        StartGame();
        StatusNumReset();



    }

  
    void StartGame()
    {
        //cardSetUp
        TurnNum = 1;
        debugAt = 0;
        debugBounus = 0;
        decklist = deck[sortiePartyNum].cardId;
        List<int> tmp = new List<int>();
        foreach (int x in decklist)
        {
            tmp.Add(x);
        }
        for (int i = 1; i < decklist.Count; i++)
        {
            int x = Random.Range(1, decklist.Count);
            int y = Random.Range(1, decklist.Count);
            int _temp = decklist[x];
            decklist[x] = decklist[y];
            decklist[y] = _temp;

        }
        var handCount = 0;
        for (int i = 0; i < 6; i++)
        {
            if (decklist.Count == 0) break;
            _hand.Add(CardCreate(decklist[0], Hand));

            decklist.RemoveAt(0);
            handCount++;

        }

        ReaderCard = _hand[0];
       if(CrectmapManager.stage != null) FieldEffectParty(_hand, CrectmapManager.stage.fieldEffects);
        _hand = BufApplication(_hand);
        //


        //enemySetUp
        if (CrectmapManager.enemy != null)
            _enemy = EnemyCreate(CrectmapManager.enemy[ButtleNum].EnemyId, Enemys);
        else _enemy = EnemyCreate(debugenemy, Enemys);
        if (CrectmapManager.stage != null) FieldEffectEnemy(ref _enemy, CrectmapManager.stage.fieldEffects);
        
        MAX_NUMBA = _enemy.model.numba;
        MAX_ENEMYHP = _enemy.model.Hp;
        Enemy_skilllist = _enemy.model.skilllist;
        Enemy_skilllistTable = _enemy.model.skillTable;
       // _enemy.Show_update(_enemy.model.numba.ToString(), _enemy.model.Hp,MAX_ENEMYHP);

        //

        //
        //BackGround
        back.sprite = CrectmapManager.BackGrounds;



        //SystemSetUp
        /*var trigger = GameObject.Find("Trigger");
        trigger.SetActive(false);*/
        decide_num = 0;
        finish = false;
        ButtleNum++;
        Myturn = true;
        OnlyOneReaderSkill = false;
        
        //buttleAnimatoin.SetActive(false);
        buttleNumText.GetComponent<Text>().text = "Battle " + ButtleNum.ToString();
        if (CrectmapManager.enemy != null && ButtleNum != CrectmapManager.enemy.Count)
        {
           // buttleNumText.GetComponent<Animator>().enabled = true;
          
            var t = GameObject.Find("Enemys").GetComponent<Animator>();
            t.enabled = true;
            if (CrectmapManager.stage != null)
            {
                if (CrectmapManager.stage.stageid == 0)
                {
                    StartCoroutine(Tutorial(2, tutorial));
                    dmanager.buttle_tutorial = true;
                }
                if (CrectmapManager.stage.stageid == 1)
                {
                    StartCoroutine(Tutorial(2, bounus_tutorial));
                }

            }



            if (ButtleNum == 1)
            {   
                if(CrectmapManager.intro[0] == null || CrectmapManager.loop[0] == null)
                    BGMManager.SetBGM(DefaultAudioClipIntro[0], DefaultAudioClipLoop[0],volume);
                else 
                    BGMManager.SetBGM(CrectmapManager.intro[0], CrectmapManager.loop[0], volume);
                //BGMManager.Play();
            }

           
        }
        else
        {
            
            var t = GameObject.Find("BossAnimation").GetComponent<Animator>();
            t.enabled = true;
            if (CrectmapManager.intro == null  || CrectmapManager.intro[1] == null || CrectmapManager.loop[1] == null)
                BGMManager.SetBGM(DefaultAudioClipIntro[1], DefaultAudioClipLoop[1], volume);
            else
                BGMManager.SetBGM(CrectmapManager.intro[1], CrectmapManager.loop[1], volume);
            // BGMManager.Play();
            if (CrectmapManager.stage != null)
            {
                if (CrectmapManager.stage.stageid == 0)
                {
                    StartCoroutine(Tutorial(5, tutorial));
                    dmanager.buttle_tutorial = true;
                }
                if (CrectmapManager.stage.stageid == 1)
                {
                    StartCoroutine(Tutorial(5, bounus_tutorial));
                }
            }
           

        }
        if(ButtleNum == 1)
        {
            maxDamage = 0;
            maxNum = 0;
            aveTurn = 0;
            enemysexp = 0;
            
            
            CardModel controllerInstance = new CardModel();
            List<int> hp = new List<int>();
            List<int> num = new List<int>();
            logText = new List<string>();
            foreach (int id in tmp)
            {

                hp.Add((int)controllerInstance.CardHp(id, cardLvs[id].Lv,cardLvs[id].hpbuf));
                num.Add( controllerInstance.CardNum(id));
                
            }
            if(CrectmapManager.stage != null) MAX_HP = SystemParam(num,hp,ReaderCard, CrectmapManager.stage.fieldEffects);
            else MAX_HP = SystemParam(num, hp, ReaderCard, null);
            hpSum = MAX_HP;
            
        }
        var j = hpSum * 0.3;
        hpSum +=(int) j ;
        if (hpSum > MAX_HP) hpSum = MAX_HP;
        ReaderSkill(ReaderCard, _hand);
        partyDf = Df(_hand);
        gameView.Init(MAX_HP, hpSum,partyDf);
        CardShowUpdate(_hand);


        //
        //log
        LogTextView("Battle:" + ButtleNum);
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
                OptionPanel.SetActive(false);
                SoundSettingPanel.SetActive(false);
            }
            else
            {
                OptionPanel.SetActive(true);
            }
        }

        if (SoundSettingPanel.activeInHierarchy)
        {
            volume = volumeslider.value;
            BGMManager.ChangeVolume(volume);
            dmanager.volume = volume;
        }
    }
    void FieldEffectParty( List<CardController> hand,  List<StageEntity.FieldEffect> fieldEffects )
    {
       
        foreach(StageEntity.FieldEffect fieldEffect in fieldEffects)
        {
            if (fieldEffect.EnemyOrParty == StageEntity.EnemyOrParty.Party)
            {
                foreach (CardController card in hand)
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

    void FieldEffectEnemy(ref EnemyContoller enemy , List<StageEntity.FieldEffect> fieldEffects)
    {   
        
        foreach (StageEntity.FieldEffect fieldEffect in fieldEffects)
        {   
            if (fieldEffect.EnemyOrParty == StageEntity.EnemyOrParty.Enemy)
            {

                if (ButtleNum != fieldEffect.ApplicationNum) continue;
                 
                 var temp = enemy.model.at * fieldEffect.effectSize;
                 enemy.model.at = (int)temp;

                 temp = enemy.model.df * fieldEffect.effectSize;
                 enemy.model.df = (int)temp;

                 temp = enemy.model.Hp * fieldEffect.effectSize;
                 enemy.model.initHp =  enemy.model.Hp = (int)temp;

                temp = enemy.model.numba + fieldEffect.effectSize * 4;
                enemy.model.numba = (int)temp;
                  
                
                
            }

        }
    }
  
    public void Dataload()
    {
        dmanager = new DataManager(filepath);
        cmanager = new CharacterDataManager(cfilepath);    
        divisor_lv = dmanager.divisor_lv;
        multi_lv = dmanager.multi_lv;
        prime_lv = dmanager.prime_lv;
        ALLCHARCTOR = cmanager.ALLCHARCTOR;
        cardLvs = new CharacterData.CardLv[ALLCHARCTOR];
        for (int i = 0; i < ALLCHARCTOR; i++)
        {  
          if (i >= cmanager.cardLvs.Length)
            {
               cardLvs[i] = new CharacterData.CardLv();
               cardLvs[i].Set(i, 1, 0, false,0,0,0,0);
             }
          else
             {
                var card = cmanager.cardLvs[i];
                cardLvs[i] = new CharacterData.CardLv();
                cardLvs[i].Set(card.Id, card.Lv, card.expSum, card.pos,card.atbuf,card.dfbuf,card.hpbuf,card.convex);
               }

        }
        for (int i = 0; i < 7; i++)
        {
           deck.Add(cmanager.deck[i]);
        }
        sortiePartyNum = cmanager.sortiePartyNum;
         volume = dmanager.volume;
        buttle_tutorial = dmanager.buttle_tutorial;
    }
    

   


    //ButtleSystem
    CardController CardCreate(int Cardid, Transform place)
    {
        CardController card = Instantiate(Card, place);
        card.Init(Cardid, cardLvs[Cardid].Lv);
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
        sum = 0;
        prime_bounuse_check = true;
        prime_bounuse = 0;
        divisor_bounuses = 0;
        multi_bounuses = 0;
        divisors = 0;
        multis = 0;



        foreach (CardController card in cardlist)
        {
            if (card.model.decided == true)
            {

                sum += card.model.num;


                if (!Prime_number_check(card.model.num) || !prime_bounuse_check) prime_bounuse_check = false;
                if (Divisor_check(card.model.num, _enemy.model.numba)) divisors++;
                if (Multi_check(card.model.num, _enemy.model.numba)) multis++;




            }



        }

        if (cardlist.Count < 4) prime_bounuse_check = false;
        multi_bounuses = (int)Multi_bounus(multis, multi_lv);
        divisor_bounuses = (int)Divisor_bounus(divisors, divisor_lv);

        double correction = (34 - prime_lv) * 0.1;
        if (prime_bounuse_check) prime_bounuse = (int)(1 + prime_lv *1.05 + correction);
        else prime_bounuse = 0;

        sum += multi_bounuses + divisor_bounuses + prime_bounuse;

        int[] x = new int[2];

        x[0] = sum;
        x[1] = multi_bounuses + divisor_bounuses + prime_bounuse;
        return x;



    }

    double Divisor_bounus(int divisors, int lv)
    {
        switch (divisors)
        {
            case 1: return lv * 0.54 + 1;
            case 2: return lv * 0.59 + 2;
            case 3: return lv * 0.67 + 3;
            case 4: return lv * 0.9+ 4;

            default: return 0;
        }
    }

    double Multi_bounus(int multis, int lv)
    {
        switch (multis)
        {
            case 1: return lv * 0.64 + 1;
            case 2: return lv * 0.73 + 2;
            case 3: return lv * 0.82 + 3;
            case 4: return lv * 0.93 + 4;

            default: return 0;
        }
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

    int Df(List<CardController> hands) 
    {
        List<int> dfs = new List<int>();
        double df = 0.0;
        foreach(CardController hand in hands)
        {
            dfs.Add(hand.model.df);
        }
        for(int i = 0; i< dfs.Count; i++)
        {
            for(int j = i+1 ; j < dfs.Count; j++)
            {
                if(dfs[i] > dfs[j])
                {
                    int temp = dfs[j];
                    dfs[j] = dfs[i];
                    dfs[i] = temp;
                }
            }
        }
        for(int i =0; i < dfs.Count; i++)
        {
            //df += dfs[i] * (i*0.85+1);
            df += dfs[i];
        }
        Debug.Log("Df:" + df);
        return (int)df;

    }

   
    void CardShowUpdate(List<CardController> cardmodel)
    {
        foreach (CardController card in cardmodel)
        {
            card.view.Show_Updte(card.model);
        }
    }
    public void SetUpCard(CardController card)
    {
        UpCard.Add(card);
        var x = Sum(UpCard);
        gameView.NumPowerText(x);

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
        gameView.NumPowerText(x);
    }
    public void RemoveCardObj(GameObject card)
    {
        UpCardObj.Remove(card);
    }
    public void InitUpcard()
    {
        UpCard.Clear();
        int[] x = { 0, 0 };
        gameView.NumPowerText(x);
    }
    private void LogTextView(string s)
    {
        AddLogText(s);
        if(logText.Count > 25)
        {
            logText.RemoveRange(0, logText.Count - 25);
        }
  
        gameView.LogTextView(logText.Skip(logText.Count-5).ToList());
        

    }
    public List<string> GetLog()
    {
        return logText;
    }
    private void AddLogText(string s)
    {   
        logText.Add(s);
        
    }


    List<int> DeckSheffle(List<int> deck)
    {
        List<int> _deck = deck;
        for (int i = 0; i < _deck.Count; i++)
        {
            int x = Random.Range(0, _deck.Count);
            int y = Random.Range(0, _deck.Count);
            int _temp = _deck[x];
            _deck[x] = _deck[y];
            _deck[y] = _temp;

        }
        return _deck;
    }
    List<CardController> HandChange(List<CardController> _card, Transform place)
    {

        List<CardController> temp = new List<CardController>();


        foreach (CardController card in _card)
        {
            if (card.model.decided == true)
            {
                temp.Add(card);
                decklist.Add(card.model.cardID);
                Destroy(card.gbj);


            }

        }
        decklist = DeckSheffle(decklist);



        for (int i = 0; i < temp.Count; i++)
        {


            int x = _card.IndexOf(temp[i]);


            _card.RemoveAt(x);


        }
        int j = 6 - _card.Count;
        for (int i = 0; i < j; i++)
        {
            if (decklist.Count == 0) break;
            CardController card = Instantiate(Card, place);
            card.Init(decklist[0], cardLvs[decklist[0]].Lv);
            decklist.RemoveAt(0);
            _card.Add(card);
        }

        decide_num = 0;
        handchange = true;
        return _card;
    }


    string Enemyattack(List<AnimationType> animations,bool skill,ref List<int>  partydamage,ref int enemyHeal,ref int healNum,ref int enemydamage)
    {
        
        string skillname = "";
        if (skill)
        {
            int index = TurnNum;
            if (Enemy_skilllistTable.Count == 0)
            {
                Debug.LogError("No skill table");
            }
            if (index <= Enemy_skilllistTable.Count)
            {

                index = TurnNum - 1;
            }
            else
            {
                while (index > Enemy_skilllistTable.Count)
                {
                    index = index - Enemy_skilllistTable.Count;
                }
                index--;

            }
            var i = Enemy_skilllistTable[index];
            skillname = Enemy_skilllist[i].skill_name;
            animations.Add(AnimationType.skill);
            EnemySkill(animations,Enemy_skilllist, Enemy_skilllistTable[index], ref partydamage, ref _enemy.model.Hp, ref enemyHeal, ref partyDf, ref _enemy.model.at,ref _enemy.model.numba,ref healNum,ref enemydamage);
           

        }
        else
        {
           
            animations.Add(AnimationType.numProtect);



        }
        //_enemy.Show_update(_enemy.model.numba.ToString(), _enemy.model.Hp);
        return skillname;

    }

    private void EnemySkill(List<AnimationType> animations,List<Skill_origin> skilllist, int skillid, ref List<int> PartyDamage, ref int enemyHp, ref int enemyHeal, ref int partydf, ref int enemyat,ref int enemyNum,ref int healNum,ref int enemydamage)

    {
       
        if (skilllist.Count < skillid + 1) return;
        var Skill = skilllist[skillid];
        for (int i = 0; i < Skill.magic_Conditon_Origins.Count; i++)
        {
            var _Origin = Skill.magic_Conditon_Origins[i];
            var effect = _Origin.effect_size;
            bool check = EnemySkillCheck(_Origin, enemyat, effect, _Origin.condition_num, enemyHp, enemyNum);
            var magicKind = _Origin.magic_kind;
            
            if (!check) continue;
             switch (_Origin.type)
            {
                case Skill_origin.Skill_type.constantAttack:
                     
                   PartyDamage.Add((int) effect);
                    animations.Add(AnimationType.damage);
                    break;


                case Skill_origin.Skill_type.referenceAttack:

                    double damage = effect * enemyat;
                    if (damage < partydf) PartyDamage.Add( 1);
                    else PartyDamage.Add((int)damage - partydf);
                    animations.Add(AnimationType.damage);
                    break;
                
                case Skill_origin.Skill_type.Heal_Hp:
                    var x = effect * enemyat;
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
                    var s = Instantiate(estatusat);
                    if (_Origin.magic_kind == Skill_origin.MagicKind.add)
                    {
                        s.SetStatus(effect, _Origin.effect_turn, "Add");
                    }
                    else if (_Origin.magic_kind == Skill_origin.MagicKind.multi)
                    {
                        s.SetStatus(effect, _Origin.effect_turn, "Multi");
                    }


                    break;

                    
                case Skill_origin.Skill_type.IncreaseDefence:
                    animations.Add(AnimationType.enemyIncreaseDf);
                    var df = Instantiate(estatusdf);
                    if(_Origin.magic_kind == Skill_origin.MagicKind.add)
                    {
                        df.SetStatus(effect, _Origin.effect_turn,"Add");
                    }
                    else if(_Origin.magic_kind == Skill_origin.MagicKind.multi)
                    {
                        df.SetStatus(effect, _Origin.effect_turn, "Multi");
                    }
                    
                    break;


                case Skill_origin.Skill_type.NumDamage:

                    enemyNum -= (int)effect;
                    if (enemyNum <= 0) enemyNum = 1;
                    break;

                case Skill_origin.Skill_type.partydecreaseDefence:
                    if (magicKind == Skill_origin.MagicKind.add)
                    {
                        var g = Instantiate(pstatusdf);
                        g.SetStatusDf(effect,_Origin.effect_turn, "Add", _enemy.model.name);
                        g.name = "DfManager";
                    }
                    else if (magicKind == Skill_origin.MagicKind.multi)
                    {
                        var g = Instantiate(pstatusdf);
                        g.SetStatusDf(effect, _Origin.effect_turn, "Multi", _enemy.model.name);
                        g.name = "DfManager";
                    }
                    animations.Add(AnimationType.partydecrease);
                    break;

                case Skill_origin.Skill_type.partydecreaseAttack:
                    
                    if (magicKind == Skill_origin.MagicKind.add)
                    {
                        var g = Instantiate(pstatusat);
                        g.SetStatusAt(effect, _Origin.effect_turn, "Add", _enemy.model.name);
                        g.name = "AtManager";
                    }
                    else if (magicKind == Skill_origin.MagicKind.multi)
                    {
                        var g = Instantiate(pstatusat);
                        g.SetStatusAt(effect, _Origin.effect_turn, "Multi", _enemy.model.name);
                        g.name = "AtManager";
                    }
                    animations.Add(AnimationType.partydecrease);
                    break;


                case Skill_origin.Skill_type.decreaseDefence:
                    var statusdf = Instantiate(estatusdf);
                    animations.Add(AnimationType.enemydecreasedf);
                    if (_Origin.magic_kind == Skill_origin.MagicKind.add)
                    {
                        statusdf.SetStatus(effect, _Origin.effect_turn, "Add");
                    }
                    else if (_Origin.magic_kind == Skill_origin.MagicKind.multi)
                    {
                        statusdf.SetStatus(effect, _Origin.effect_turn, "Multi");
                    }
                    break;

                case Skill_origin.Skill_type.decreaseAttack:
                    animations.Add(AnimationType.enemydecreaseat);
                    var statusAt = Instantiate(estatusat);
                    if (_Origin.magic_kind == Skill_origin.MagicKind.add)
                    {
                        statusAt.SetStatus(effect, _Origin.effect_turn, "Add");
                    }
                    else if (_Origin.magic_kind == Skill_origin.MagicKind.multi)
                    {
                        statusAt.SetStatus(effect, _Origin.effect_turn, "Multi");
                    }
                    break;


            }
        }

    }

    bool EnemySkillCheck(Skill_origin.magic_conditon_origin _Origin, int param,double effect,int conditionNum,int enemyHp,int enemyNum)
    {
        var conditonKind = _Origin.condition_kind;
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
    
    List<CardController> BufApplication(List<CardController> cards)
    {
        List<CardController> temp = new List<CardController>();
        foreach(CardController card in cards)
        {
            if (card.model.onBuf) {
                temp.Add(card);
                continue;
            } 
            var id = card.model.cardID;
            //attackBuf
            var tenv = card.model.at * (1 + (cardLvs[id].atbuf*0.1f) );
            card.model.at = (int)tenv;
            //dfBuf
            tenv = card.model.df * (1 + (cardLvs[id].dfbuf * 0.1f));
            card.model.df = (int)tenv;
            card.model.onBuf = true;
            temp.Add(card);
        }
        return temp;
    }

    void ReaderSkill(CardController card, List<CardController> hands)
    {   
     
        for (int i = 0; i < card.model.ReaderSkill.magic_Conditon_Origins.Count; i++)
        {
            var x = card.model.ReaderSkill.magic_Conditon_Origins[i];
            switch ((int)x.type)
            {

                case 5:
                    if (OnlyOneReaderSkill) break;
                    double damge = hpSum * x.effect_size;
                    OnlyOneReaderSkill = true;
                    
                    hpSum -=(int) damge;
                    break;

                case 6:
                    foreach (CardController hand in hands)
                    {
                        if (hand.model.onRedaerskill == false)
                        {

                            
                            hand.model.at = OutPutReaderskillEffect(hand.model.at, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num,MAX_HP);
                        }
                    }
                    break;

                case 7:
                    foreach (CardController hand in hands)
                    {
                        if (hand.model.onRedaerskill == false)
                        {

                           
                            hand.model.df = OutPutReaderskillEffect(hand.model.df, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num, MAX_HP);
                        }
                    }
                    break;
                case 8:
                    foreach (CardController hand in hands)
                    {
                        if (hand.model.onRedaerskill == false)
                        {


                            hand.model.num = OutPutReaderskillEffect(hand.model.num, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num, MAX_HP);
                        }
                    }
                    break;
                case 9:
                    foreach (CardController hand in hands)
                    {
                        if (hand.model.onRedaerskill == false)
                        {


                            hand.model.Hp = OutPutReaderskillEffect(hand.model.Hp, x.magic_kind, x.effect_size, x.condition_kind, x.condition_num, hand.model.num, MAX_HP);
                        }
                    }

                    break;


            }
        }
        foreach(CardController hand in hands)
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

    void AutoSkill(List<AnimationType> animations,List<CardController> cards,ref double persuit,ref EnemyModel enemy,ref int teamHeal)
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
                autoInvocation = AutoSkillCheck(conditonKind, conditionNum, conditons, nums, hpSum, sum);
                if (!autoInvocation) continue;
                switch (type)
                {

                    case Skill_origin.Skill_type.NumDamage: // NumDamage
                        if(magicKind == Skill_origin.MagicKind.add)
                        {
                            enemy.numba -= (int)effect;
                        }
                        else if(magicKind == Skill_origin.MagicKind.multi)
                        {
                            double temp = enemy.numba * effect;
                            enemy.numba = (int)temp;
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
                            sum += (int)effect;
                        }
                        else if (magicKind == Skill_origin.MagicKind.multi)
                        {
                            double temp = sum * effect;
                            sum = (int)temp;
                        }
                        break;

                    case Skill_origin.Skill_type.decreaseDefence :
                        if (magicKind == Skill_origin.MagicKind.add)
                        {
                            Instantiate(estatusdf).SetStatus(effect, effectturn,"Add");
                        }
                        else if (magicKind == Skill_origin.MagicKind.multi)
                        {
                            Instantiate(estatusdf).SetStatus(effect, effectturn,"Multi");
                        }
                        break;


                    case Skill_origin.Skill_type.decreaseAttack:
                        if (magicKind == Skill_origin.MagicKind.add)
                        {
                            Instantiate(estatusat).SetStatus(effect, effectturn, "Add");
                        }
                        else if (magicKind == Skill_origin.MagicKind.multi)
                        {
                            Instantiate(estatusat).SetStatus(effect, effectturn, "Multi");
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
    public bool AutoSkillCheck( Skill_origin.Magic_condition_kind condition_Kind, int conditionNum,int conditons, List<int> cardNums, int hpSum,int numSum)
    {
        int x = 0;
       
        if (conditons == 0)
        {   
            switch ((int)condition_Kind)
            {   
                
                case (int)Skill_origin.Magic_condition_kind.sum_up:
                    if (numSum < conditionNum) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.sum_down:
                    if (numSum >= conditionNum) return false;
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
                    if (hpSum >= conditionNum) return false;
                    break;
                case (int)Skill_origin.Magic_condition_kind.Hp_up:
                    if (hpSum < conditionNum) return false;
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
    //ButtleMain

    public void Buttle(List<CardController> cardlist)
    {
        
        List<AnimationType> animations = new List<AnimationType>();
        double pesuit = 0;
        double enemyDf = _enemy.model.df;
        List<int> damage = new List<int>();
        int teamHeal = 0;
        List<int> teamDamage = new List<int>();
        int enemyHeal = 0;
        int healNum = 0;
        int enemydamage = 0;
       
        AutoSkill(animations,cardlist, ref pesuit,ref _enemy.model,ref teamHeal);
        partyDf = Df(_hand);
        //_enemy.model.numba = (int)enemyNum;
       // _enemy.model.Hp -= (int)pesuit;
        _enemy.model.df = (int)enemyDf;
        string skillName = "";
        Myturn = false;

        if (_enemy.model.numba - sum > 0)
        {
           
            animations.Add(AnimationType.block); //block
            _enemy.model.numba -= sum;
            if (pesuit != 0) animations.Add(AnimationType.persuit); //persuit

            if (_enemy.model.Hp - pesuit < 0)
            {
                _enemy.model.Hp = 0;
                
                aveTurn += TurnNum;
                enemysexp += _enemy.model._exp;
                if (ButtleNum != CrectmapManager.enemy.Count)
                {
                    animations.Add(AnimationType.nextStage); //NextStage
                    StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                    return;
                }
                else
                {
                    ButtleNum = 0;
                    animations.Add(AnimationType.win); //WIN
                    StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal,healNum,enemydamage));
                    return;
                }

            }

            animations.Add(AnimationType.enemyturn);
            skillName = Enemyattack(animations,true, ref teamDamage,ref enemyHeal,ref healNum,ref enemydamage);
           // _hand = HandChange(_hand, Hand);
            if (CrectmapManager.stage != null)
                FieldEffectParty(_hand, CrectmapManager.stage.fieldEffects);
            ReaderSkill(ReaderCard, _hand);
            if (hpSum + teamHeal - teamDamage.Sum() <=0)
            {
                //hpSum = 0;
              
                ButtleNum = 0;
                finish = true;
                animations.Add(AnimationType.gameover); //gameover
                StartCoroutine(AnimationList(animations, (int)pesuit,damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                return;
            }
            CardShowUpdate(_hand);
                       
            
            animations.Add(AnimationType.playerturn);
            StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
            return;
        }

        double bounus = 1 + 0.05 * (sum - _enemy.model.numba);
        if (sum - _enemy.model.numba >= 20)
        {
            bounus = 2 * Mathf.Log(sum - _enemy.model.numba, 20);
        }

        foreach (CardController card in cardlist)
        {
            if (card.model.decided == true)
            {
                int tmp = (int)card.model.at - _enemy.model.df;
                if (tmp <=0) tmp = 1;
                double tmpBounus = tmp * bounus;
                damage.Add((int)tmpBounus);
                AddLogText( card.model.name+ "が<color=red>" + (int)tmpBounus + "</color>ダメージ");
                animations.Add(AnimationType.attack); //attack

            }
                

        }
       
       
        if (maxDamage < damage.Max()) maxDamage = damage.Max();

        if (maxNum < sum) maxNum = sum;
        _enemy.model.numba = 0;       
        if (pesuit != 0) animations.Add(AnimationType.persuit); //persuit
        if (_enemy.model.Hp - damage.Sum() - pesuit > 0)
        {
           
            animations.Add(AnimationType.enemyturn);
            Enemyattack(animations, false, ref teamDamage, ref enemyHeal,ref healNum, ref enemydamage);
            skillName = Enemyattack(animations,true, ref teamDamage, ref enemyHeal, ref healNum, ref enemydamage);
          
        }
        else
        {

           
            aveTurn += TurnNum;
            enemysexp += _enemy.model._exp;
            if ((CrectmapManager.enemy != null) && ButtleNum != CrectmapManager.enemy.Count)
            {
                animations.Add(AnimationType.nextStage); //NextStage
                StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                return;
            }
            else
            {
                ButtleNum = 0;
                animations.Add(AnimationType.win); //WIN
                StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
                return;
            }

        }

        
        if(CrectmapManager.stage != null) FieldEffectParty(_hand, CrectmapManager.stage.fieldEffects);
        ReaderSkill(ReaderCard, _hand);
        if (hpSum + teamHeal - teamDamage.Sum() <=0)
        {
           // hpSum = 0;
          
            ButtleNum = 0;
            finish = true;
            animations.Add(AnimationType.gameover);
            StartCoroutine(AnimationList(animations, (int)pesuit, damage, skillName, teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
            return;
        }
        CardShowUpdate(_hand);
        animations.Add(AnimationType.playerturn);
        StartCoroutine(AnimationList(animations, (int)pesuit,damage,skillName,teamDamage, teamHeal, enemyHeal, healNum, enemydamage));
        
       

    }


    public void OpenSoundSetting()
    {
        
        SoundSettingPanel.SetActive(true);
        if (volumeslider == null) volumeslider = SoundSettingPanel.transform.GetChild(1).GetComponent<Slider>();
        volumeslider.value = volume;
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
                    hpSum += teamHeal;
                    AddLogText("味方が<color=green>" + (int)teamHeal + "</color>回復");
                    if (hpSum > MAX_HP) hpSum = MAX_HP;

                    break;
                case AnimationType.attack:
                    var g = UpCardObj[0];
                    var x = g.transform.position.x;
                    var y = g.transform.position.y;
                    g.transform.position = new Vector3(x, y +40, 0);
                    NotificationButtle.GetInstance().PutInQueue(damage[0].ToString());
                    yield return new WaitForSeconds(0.2f);
                    x = g.transform.position.x;
                    y = g.transform.position.y;
                    g.transform.position = new Vector3(x, y - 40, 0);
                    UpCardObj.RemoveAt(0);
                   
                    if (_enemy.model.Hp - damage[0] < 0)
                     {
                            _enemy.model.Hp = 0;
                            

                     }
                     else _enemy.model.Hp -= damage[0];
                    
                    damage.RemoveAt(0);
                    yield return new WaitForSeconds(0.4f);
                    break;
                case AnimationType.enemydamage:
                    NotificationButtle.GetInstance().PutInQueue(enemydamage.ToString());
                    if (_enemy.model.Hp - enemydamage < 0)
                    {
                        _enemy.model.Hp = 0;

                    }
                    else _enemy.model.Hp -= enemydamage;
                    AddLogText(_enemy.model.name  + enemydamage + "の自傷");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.persuit:    
                    NotificationButtle.GetInstance().PutInQueue("<color=black>" + persuit.ToString()+ "</color>");
                    if (_enemy.model.Hp - persuit < 0)
                    {
                        _enemy.model.Hp = 0;

                    }
                    else _enemy.model.Hp -= persuit;
                    AddLogText("敵に" + persuit + "の追撃");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemydecreasedf:
                    NotificationButtle.GetInstance().PutInQueue("<color=blue>" + "防御力ダウン" + "</color>");
                    AddLogText(_enemy.model.name + "が防御力ダウン");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemydecreaseat:
                    NotificationButtle.GetInstance().PutInQueue("<color=blue>" + "攻撃力力ダウン" + "</color>");
                    AddLogText(_enemy.model.name + "が攻撃力ダウン");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.skill:
                    yield return StartCoroutine(SkillAnimation(skillname));
                    AddLogText("敵のスキル:" + skillname);
                    break;
              
                case AnimationType.gameover:
                    hpSum = 0;
                    yield return StartCoroutine(GameOverAnimation());
                    dmanager.DataSave(filepath);
                    AddLogText("GAME OVER");
                    break;
                case AnimationType.win:
                    Destroy(_enemy.gbj);
                    BGMManager.FadeOut();
                    yield return new WaitForSeconds(1f);
                    dmanager.DataSave(filepath);
                    AddLogText("YOU WIN");
                    Debug.Log("Ave_at(before):" + debugAt / debugBounus);
                    Debug.Log("Ave_bounus:" + debugBounus / TurnNum);
                    Debug.Log("Ave_at(after):" + debugAt / TurnNum);
                    SceneManager.LoadScene("Result");
                    break;
                case AnimationType.block:
                    AddLogText("数値バリアを破壊できなかった");
                   
                    NotificationButtle.GetInstance().PutInQueue("BLOCK");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.nextStage:
                    if ((ButtleNum+1) == CrectmapManager.enemy.Count)
                    {
                        BGMManager.FadeOut();
                    }
                    yield return new WaitForSeconds(1f);
                    dmanager.DataSave(filepath);
                    SceneManager.LoadScene("Battle");
                    break;
                case AnimationType.numProtect:
                    double newnumba = MAX_NUMBA * _enemy.model.Hp / MAX_ENEMYHP;
                    if ((int)newnumba < 1) _enemy.model.numba = 1;
                    else _enemy.model.numba = (int)newnumba;
                    yield return new WaitForSeconds(0.7f);
                    AddLogText(_enemy.model.name + "の数値バリアが" + _enemy.model.numba + "に回復");
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
                    NotificationButtle.GetInstance().PutInQueue("<color=blue>" + healNum.ToString() + "</color>");
                    yield return new WaitForSeconds(0.7f);
                    AddLogText(_enemy.model.name + "が数値バリアを" + healNum + "回復");
                    _enemy.model.numba += healNum;
                    break;
                case AnimationType.enemyHeal:
                    NotificationButtle.GetInstance().PutInQueue("<color=green>" + enemyHeal.ToString() + "</color>");
               
                    if (_enemy.model.Hp + enemyHeal > MAX_ENEMYHP)
                    {
                        _enemy.model.Hp = MAX_ENEMYHP;
                    }
                    else
                    {
                        _enemy.model.Hp += enemyHeal;
                    }
                    AddLogText(_enemy.model.name + "がHPを" + enemyHeal + "回復");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemyIncreaseAt:
                    NotificationButtle.GetInstance().PutInQueue("<color=red>" + "攻撃力アップ!" + "</color>");
                    AddLogText(_enemy.model.name + "が攻撃アップ");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.enemyIncreaseDf:
                    NotificationButtle.GetInstance().PutInQueue("<color=red>" + "防御力アップ!" + "</color>");
                    AddLogText(_enemy.model.name + "が防御力アップ");
                    yield return new WaitForSeconds(0.7f);
                    break;
                case AnimationType.partydecrease:
                    statusManager.SetGameObject();
                    partyDf = Df(_hand);
                    yield return new WaitForSeconds(0.7f);
                    break;


            }

        }
        UpCardObj.Clear();
        _hand = HandChange(_hand, Hand);
        Myturn = true;
        if(!vs.Contains(AnimationType.nextStage))
        {
            TurnNum++;
            LogTextView("Turn:" + TurnNum.ToString());
        }
    }
    IEnumerator SkillAnimation(string skillname)
    {
        SkillNameAnimation e = Instantiate(skillnameAnimation, canvaspos);
        int s = e.StartAniamtion(skillname);
        yield return new WaitForSeconds(s);
        Destroy(e.gameObject);
    }
    IEnumerator DamageAnimation(int damage)
    {

        DamageAnimation e = Instantiate(damageAnimation, canvaspos);
        int s = e.startAnimation(damage);
        yield return new WaitForSeconds(s);
        Destroy(e.gameObject);


    }
    IEnumerator EnemyTurnAnimation()
    {
        Animator e = Instantiate(enemyturnAnimation,canvaspos);
        e.enabled = true;
        e.Play(0);
        var s = e.GetCurrentAnimatorClipInfo(0).Length;
        yield return new WaitForSeconds(s);
        Destroy(e.gameObject);


    }
    IEnumerator ParyTurnAnimation()
    {
        Animator e = Instantiate(partyturnAnimation, canvaspos);
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
        var panel = Instantiate(gameOverPanel, gameOverPanelTrance);
        panel.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(panel.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
    }

    public void ENDAnimation()
    {
       // BGMManager = GameObject.Find("BGM").GetComponent<BGMManager>();
        BGMManager.FadeOut();
        ButtleNum = 0;
        var panel = Instantiate(gameOverPanel, gameOverPanelTrance);
        panel.GetComponent<Animator>().enabled = true;
    }

 
}
