using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectMapManager : MonoBehaviour
{
    private  GameObject panel;
    private  EventSystem eventSystem;
    [SerializeField] ButtonUi Button;
    [SerializeField] Transform Stagetransform;
    [SerializeField] TextMeshProUGUI MapName;
    [SerializeField] GameObject BeforeStageButton;
    [SerializeField] GameObject AfterStageButton;
    [SerializeField] SpriteRenderer back;
    [SerializeField] PartyManager party;
    [SerializeField] List<Sprite> chartutorial,endtutorial;
    static AudioClip desidese,cancelse;
    static BGMManager bgmobj;
    Animator anime;

    static GameObject sortiePartyPanel,notSortiePartyPanel;
    public static MapManager MapManager;
   [SerializeField] SceneAnimation scene;
    public static StageEntity stage;
    static int Mapid;
    public static List<EnemyEntity> enemy;
    public static List<StageEntity.Gifts> Gift;
    public static Sprite BackGrounds;
    public static AudioClip[] intro;
    public static AudioClip[] loop;
    private AudioClip mapintro,maploop;



    void Start()
    {
        
        sortiePartyPanel = GameObject.Find("SortiePartyPanel");
        notSortiePartyPanel = GameObject.Find("NotSortiePartyPanel");
        DataManager.DataLoad();
        DataManager.StageDataLoad();
        sortiePartyPanel.SetActive(false);
        notSortiePartyPanel.SetActive(false);
        
        InitMap(Mapid);
        if (MapManager == null) return;
        
       
        if (Mapid != 0) BeforeStageButton.SetActive(true);
        else BeforeStageButton.SetActive(false);

        if (!DataManager.enemystatus_tutorial && DataManager.stages[4].clear)
        {
            StartCoroutine(CharTutorial(0));
            DataManager.enemystatus_tutorial = true;
        }

        if(!DataManager.endgame_tutorial && DataManager.stages[29].clear)
        {
            StartCoroutine(EndTutorial(0));
            DataManager.endgame_tutorial = true;
        }
        var flag = false;
        var index = 0;
        foreach (StageEntity stage in MapManager.maps)
        {   
            if(stage.beforestageid != -1 && !DataManager.stages[stage.beforestageid].clear )
            {
                flag = true;
                index++;
                continue;
            }
            
            var map = Instantiate(Button, Stagetransform);
            map.InitButton(stage.stageid);
            if (DataManager.stages[stage.stageid].clear)
            {
                map.GetComponent<Image>().color = Color.yellow;
            }
            index++;
                
        }
        int mapfinalid = MapManager.maps[MapManager.maps.Count-1].stageid;
        if (!flag && DataManager.stages[mapfinalid].clear) AfterStageButton.SetActive(true);
        else AfterStageButton.SetActive(false);
        
        if(bgmobj == null) bgmobj = GameObject.Find("BGM").GetComponent<BGMManager>();
        if ( maploop != null && mapintro != null)
        {
            bgmobj.SetBGM(mapintro,maploop, DataManager.volume);
        }
        if (CardEditManager.toqest)
        {
            CreatePartyList();
        }
        DataManager.DataSave();
    }

    public void Onclick()
    {

        if (desidese == null)
        {
            desidese = Resources.Load<AudioClip>("SE/map選択");
        }
        if (bgmobj != null)
        {

            bgmobj.PlaySE(desidese);
        }
        eventSystem = EventSystem.current;
        panel = GameObject.Find("Panel");
        anime = panel.GetComponent<Animator>();
        var g = eventSystem.currentSelectedGameObject;
        var buttonui = g.GetComponent<ButtonUi>();
        stage = buttonui.GetStage();
        var stagenameText = panel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        stagenameText.text  = stage.stageName;
        panel.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "ハイスコア:" + DataManager.stages[stage.stageid].Hiscore.ToString();
        var stageinfoText = panel.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
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
        


    }

    public void ClosePanel()
    {   
        anime = GameObject.Find("Panel").GetComponent<Animator>();
        anime.SetBool("Infoanimebl", true);
        anime.SetBool("x", false);
        anime.SetBool("flag", false);
        if (cancelse == null)
        {
            cancelse= Resources.Load<AudioClip>("SE/se_cancel01");
        }
        if (bgmobj != null)
        {

            bgmobj.PlaySE(cancelse);
        }

    }
    public void CreatePartyList()
    {
        if (desidese == null)
        {
            desidese = Resources.Load<AudioClip>("SE/map選択");
        }
        if (bgmobj != null)
        {

            bgmobj.PlaySE(desidese);
        }
        var canvas = GameObject.Find("Canvas");
        var g = Instantiate(party, canvas.transform);
        g.Init(MapManager.stageName, stage.stageName);
       

    }
    public void StartStage()
    {
        scene.gameObject.SetActive(true);
        sortiePartyPanel.SetActive(false);
        enemy = stage.enemy;
        Gift = stage.gifts;
        BackGrounds = stage.BackGraunds;
        intro = stage.intro;
        loop = stage.loop;
        AfterStageButton.SetActive(false);
        BeforeStageButton.SetActive(false);
        GameObject.Find("Panel").SetActive(false);
        GameObject.Find("Tohome").SetActive(false);
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
        if(CharacterDataManager.sortiePartyNum == -1)
        {
            notSortiePartyPanel.SetActive(true);
            return;
        }
        else
        {
            sortiePartyPanel.SetActive(true);
            var text = sortiePartyPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = CharacterDataManager.deck[CharacterDataManager.sortiePartyNum].deckName + "で出撃します。";

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
                case 0: s += "ストーン×" + gift.giftNum + "\n";
                    
                    break;
                case 1: s += "Item×" + gift.giftNum + "\n";
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
