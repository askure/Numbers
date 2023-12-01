using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CrectmapManager : MonoBehaviour
{
    private  GameObject panel;
    private  EventSystem eventSystem;
    [SerializeField] ButtonUi Button;
    [SerializeField] Transform Stagetransform;
    [SerializeField] Text MapName;
    [SerializeField] GameObject BeforeStageButton;
    [SerializeField] GameObject AfterStageButton;
    [SerializeField] SpriteRenderer back;
    [SerializeField] PartyManager party;
    [SerializeField] List<Sprite> chartutorial,endtutorial;

    Animator anime;
    GameObject gameobj;
    static GameObject sortiePartyPanel,notSortiePartyPanel;
    public static MapManager MapManager;
    SceneAnimation scene;
    public static StageEntity stage;
    static int Mapid;
    string filepath,mapfilepath;
    public static List<EnemyEntity> enemy;
    public static List<StageEntity.Gifts> Gift;
    public static Sprite BackGrounds;
    public static AudioClip[] intro;
    public static AudioClip[] loop;
    private AudioClip mapintro,maploop;



    void Start()
    {
        
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        sortiePartyPanel = GameObject.Find("SortiePartyPanel");
        notSortiePartyPanel = GameObject.Find("NotSortiePartyPanel");
        DataManager manger = new DataManager(filepath);
        manger.StageDataLoad(mapfilepath);
        sortiePartyPanel.SetActive(false);
        notSortiePartyPanel.SetActive(false);
        
        InitMap(Mapid);
        if (MapManager == null) return;
        
       
        if (Mapid != 0) BeforeStageButton.SetActive(true);
        else BeforeStageButton.SetActive(false);

        if (!manger.enemystatus_tutorial && manger.stages[4].clear)
        {
            StartCoroutine(CharTutorial(0));
            manger.enemystatus_tutorial = true;
        }

        if(!manger.endgame_tutorial && manger.stages[29].clear)
        {
            StartCoroutine(EndTutorial(0));
            manger.endgame_tutorial = true;
        }
        var flag = false;
        var index = 0;
        foreach (StageEntity stage in MapManager.maps)
        {   
            if(stage.beforestageid != -1 && !manger.stages[stage.beforestageid].clear )
            {
                flag = true;
                index++;
                continue;
            }
            
            var map = Instantiate(Button, Stagetransform);
            map.InitButton(stage.stageid);
            if (manger.stages[stage.stageid].clear)
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            index++;
                
        }

        if (!flag && manger.stages[MapManager.maps.Count-1].clear) AfterStageButton.SetActive(true);
        else AfterStageButton.SetActive(false);
        var bgmobj = GameObject.Find("BGM");
        if (bgmobj != null && maploop != null && mapintro != null)
        {
            bgmobj.GetComponent<BGMManager>().SetBGM(mapintro,maploop, manger.volume);
            //bgmobj.GetComponent<BGMManager>().Play();

        }
        if (CardEditManager.toqest)
        {
            CreatePartyList();
        }
        manger.DataSave(filepath);
    }

    public void Onclick()
    {
        filepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        DataManager manger = new DataManager();
        manger.StageDataLoad(filepath);
        eventSystem = EventSystem.current;
        panel = GameObject.Find("Panel");
        anime = panel.GetComponent<Animator>();
        var g = eventSystem.currentSelectedGameObject;
        var buttonui = g.GetComponent<ButtonUi>();
        stage = buttonui.GetStage();
        var stagenameText = panel.transform.GetChild(1).GetComponent<Text>();
        panel.transform.GetChild(1).GetComponent<Text>().text = "ハイスコア:"+ manger.stages[stage.stageid].Hiscore.ToString();
        var stageinfoText = panel.transform.GetChild(3).GetComponent<Text>();
        stagenameText.text = stage.stageName;
        var gifts = stage.gifts;
        var giftText = GiftTostring(gifts);
        var stageinfo = "";
        foreach(string s in stage.stageinfo)
        {
            stageinfo += s + "\n";
        }
        stageinfoText.text =    "[推奨キャラレベル] Lv:" + stage.RecoLevel_Chatactor.ToString() + "\n[推奨ボーナスレベル] Lv:" + stage.RecoLevel_Bounus.ToString() + "\n[バトル数]" + stage.buttleNum.ToString() + "\n[情報]\n" + stageinfo +  "[報酬]\n"  + giftText + "\n" ;
        anime.SetBool("flag", true);
        anime.SetBool("Infoanimebl", false);
        /*if (gameobj == g)
        {
            anime.SetBool("Infoanimebl", true);
            anime.SetBool("x", false);
            anime.SetBool("flag", false);
            gameobj = null;
      
        }
        else if(gameobj == null)
        {
            anime.SetBool("flag", true);
            anime.SetBool("Infoanimebl", false);
            gameobj = g;

        }
        else
        {
            anime.SetBool("Infoanimebl", true);
            anime.SetBool("x", false);
            anime.SetBool("Infoanimebl", false);
            gameobj = g;
        }*/

    }

    public void ClosePanel()
    {   
        anime = GameObject.Find("Panel").GetComponent<Animator>();
        anime.SetBool("Infoanimebl", true);
        anime.SetBool("x", false);
        anime.SetBool("flag", false);
    }
    public void CreatePartyList()
    {
        var canvas = GameObject.Find("Canvas");
        var g = Instantiate(party, canvas.transform);
        g.Init(MapManager.stageName, stage.stageName);
    }
    public void StartStage()
    {
        sortiePartyPanel.SetActive(false);
        scene = GameObject.Find("SceneChange").GetComponent<SceneAnimation>();
        enemy = stage.enemy;
        Gift = stage.gifts;
        BackGrounds = stage.BackGraunds;
        intro = stage.intro;
        loop = stage.loop;
        AfterStageButton.SetActive(false);
        BeforeStageButton.SetActive(false);
        GameObject.Find("Panel").SetActive(false);
        GameObject.Find("Tohome").SetActive(false);
        var bgmobj = GameObject.Find("BGM");
        if (bgmobj != null)
        {
            bgmobj.GetComponent<BGMManager>().FadeOut();
        }
        //SceneManager.LoadScene("Battle");
        scene.LoadScene("Battle",MapManager.stageName,stage.stageName);
        Debug.Log("ステージをロードしました");
    }

    public void OpenThePanel()
    {
        eventSystem = EventSystem.current;
        var g = eventSystem.currentSelectedGameObject;
        var buttonui = g.GetComponent<ButtonUi>();
        stage = buttonui.GetStage();       
       
        var filepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        CharacterDataManager manger = new CharacterDataManager(filepath);
        manger.Dataload(filepath);
        if(manger.sortiePartyNum == -1)
        {
            notSortiePartyPanel.SetActive(true);
            return;
        }
        else
        {
            sortiePartyPanel.SetActive(true);
            var text = sortiePartyPanel.transform.GetChild(0).GetComponent<Text>();
            text.text = manger.deck[manger.sortiePartyNum].deckName + "で出撃します。";

        }
            
    }
    void InitMap(int Mapid)
    {
        MapManager = Resources.Load<MapManager>("stage_prehub/Map/" + Mapid.ToString());
        if (MapManager == null) return;
        MapName.text = MapManager.stageName;
        back.sprite = MapManager.Back;
        mapintro = MapManager._intro;
        maploop = MapManager.loop;

    }
    private string GiftTostring(List<StageEntity.Gifts> gifts) {
        string s = "";
        foreach(StageEntity.Gifts gift in gifts){
            switch ((int)gift.Gift)
            {
                case 0: s += "ストーン*" + gift.giftNum + "\n";
                    
                    break;
                case 1: s += "Item*" + gift.giftNum + "\n";
                    break;
                case 2:s += gift.card.name + "\n";
                    break;
                default:
                    break;
            }
        }

        return s;
    }

    public void ToAfterStage()
    {

        if (Mapid == 9) return;
        Mapid++;
        LoadSecne();
    }
    public void ToBeforeStage()
    {
        if (Mapid == 0) return;
        Mapid--;
        LoadSecne();
    }

    public void LoadSecne()
    {

        var gameObject = Resources.Load<GameObject>("LoadPanel");
        var canva = GameObject.Find("Canvas").transform;
        var pane = Instantiate(gameObject, canva);
        var _slider = pane.transform.GetChild(1).GetComponent<Slider>();
        StartCoroutine(LoadSceneUI(_slider));

    }


    IEnumerator LoadSceneUI(Slider _slider)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("Quest");
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }

    IEnumerator CharTutorial(int time)
    {
        yield return new WaitForSeconds(time);
        var tuto = Resources.Load<GameObject>("tutorial");
        var canva = GameObject.Find("Canvas").transform;
         var g  =Instantiate(tuto, canva);
        g.GetComponent<Tutorial>().SetUpTutorial(chartutorial);

    }
    IEnumerator EndTutorial(int time)
    {
        yield return new WaitForSeconds(time);
        var tuto = Resources.Load<GameObject>("tutorial");
        var canva = GameObject.Find("Canvas").transform;
        var g = Instantiate(tuto, canva);
        g.GetComponent<Tutorial>().SetUpTutorial(endtutorial);

    }


}
