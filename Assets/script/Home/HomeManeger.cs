using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class HomeManeger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text rankeText,expText,stoneText,rankExpText;
    GameManger gameManger;
    string filepath,mapfilepath;
    void Start()
    {
        filepath = Application.persistentDataPath + "/" + ".savedata.json";
        mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        gameManger = new GameManger();
        if (!File.Exists(mapfilepath) || !File.Exists(filepath))
        {
            gameManger.DataInit(filepath);
            gameManger.MapDataInit(mapfilepath, 40, false);
        }
        gameManger.Dataload(filepath);
        rankeText.text = gameManger.rank.ToString();
        expText.text = GameManger.Exp.ToString("N0");
        stoneText.text = GameManger.Stone.ToString();
        int rankExp = (gameManger.rank + 1) * (gameManger.rank + 1) * 100 - gameManger.rankExp;
        rankExpText.text = "ŽŸ‚Ìƒ‰ƒ“ƒN‚Ü‚Å" + rankExp.ToString("N0");
        

    }

    
}
