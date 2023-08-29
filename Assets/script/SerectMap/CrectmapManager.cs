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

    Animator anime;
    GameObject gameobj;
    static GameObject sortiePartyPanel,notSortiePartyPanel;
    public static MapManager MapManager;
    SceneAnimation scene;
    public static StageEntity stage;
    static int Mapid;
    string filepath;
    public static List<EnemyEntity> enemy;
    public static List<StageEntity.Gifts> Gift;
    public static Sprite BackGrounds;
    public static AudioClip[] intro;
    public static AudioClip[] loop;




    void Start()
    {
        
        filepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        sortiePartyPanel = GameObject.Find("SortiePartyPanel");
        notSortiePartyPanel = GameObject.Find("NotSortiePartyPanel");
        DataManager manger = new DataManager();
        manger.StageDataLoad(filepath);
        sortiePartyPanel.SetActive(false);
        notSortiePartyPanel.SetActive(false);
        
        InitMap(Mapid);
        if (MapManager == null) return;
        if(manger.stages[MapManager.maps.Count -1].clear) AfterStageButton.SetActive(true);
        else AfterStageButton.SetActive(false);
       
        if (Mapid != 0) BeforeStageButton.SetActive(true);
        else BeforeStageButton.SetActive(false);
        foreach (StageEntity stage in MapManager.maps)
        {   
            if(stage.beforestageid != -1 && !manger.stages[stage.beforestageid].clear )
            {
                continue;
            }
            var map = Instantiate(Button, Stagetransform);
            map.InitButton(stage.stageid);
        }
        
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
        var buttonui = g.transform.parent.GetComponent<ButtonUi>();
        var stage = buttonui.GetStage();
        var stagenameText = panel.transform.GetChild(0).GetComponent<Text>();
        panel.transform.GetChild(1).GetComponent<Text>().text = "ハイスコア:"+ manger.stages[stage.stageid].Hiscore.ToString();
        var stageinfoText = panel.transform.GetChild(2).GetComponent<Text>();
        stagenameText.text = stage.stageName;
        var gifts = stage.gifts;
        var giftText = GiftTostring(gifts);
        var stageinfo = "";
        foreach(string s in stage.stageinfo)
        {
            stageinfo += s + "\n";
        }
        stageinfoText.text =   "[バトル数]" + stage.buttleNum.ToString() + "\n[情報]\n" + stageinfo +  "[報酬]\n"  + giftText + "\n" ;


        if (gameobj == g)
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
        }

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
        GameObject.Find("Tohome").SetActive(false);
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
        MapManager = Resources.Load<MapManager>("stage_prehub/Map/" + (Mapid+1).ToString());
        if(MapManager == null)
        {
            MapManager = Resources.Load<MapManager>("stage_prehub/Map/" + (Mapid).ToString());
            return;
        }
        Mapid++;
        SceneManager.LoadScene("Quest");
    }
    public void ToBeforeStage()
    {
        if (Mapid == 0) return;
        Mapid--;
        SceneManager.LoadScene("Quest");
    }



}
