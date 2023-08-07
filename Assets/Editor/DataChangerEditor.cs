using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataChanger))]
public class DataChangerEditor : Editor {

    public override void OnInspectorGUI()
    {   
        string filepath = Application.persistentDataPath + "/" + ".savedata.json";
        string mapfilepath = Application.persistentDataPath + "/" + ".savemapdata.json";
        DrawDefaultInspector();
        var dataChanger = target as DataChanger;
        GameManger gameManger = new GameManger();
        gameManger.Dataload(filepath);
        if (GUILayout.Button("�����N�ύX"))
        {
            gameManger.rank = dataChanger.Rank;
            gameManger.Datasave(filepath);
        }
        if (GUILayout.Button("EXP��Stone�ύX"))
        {
            GameManger.Exp = dataChanger.Exp;
            GameManger.Stone = dataChanger.Stone;
            gameManger.Datasave(filepath);
        }

        if (GUILayout.Button("�{�[�i�X�ύX"))
        {
            gameManger.divisor_lv = dataChanger.Diviser;
            gameManger.multi_lv = dataChanger.Multi;
            gameManger.prime_lv = dataChanger.Prime;
            gameManger.Datasave(filepath);
        }

        if (GUILayout.Button("�L�����N�^�[�X�e�[�^�X�ύX"))
        {
           

            foreach (Playerstatus.CardLv card in gameManger.cardLvs)
            {
                card.Lv = dataChanger.CardLv;
                card.atbuf = dataChanger.Atbuf;
                card.hpbuf = dataChanger.Hpbuf;
                card.dfbuf = dataChanger.Dfbuf;
                card.convex = dataChanger.convex;
                card.pos = dataChanger.pos;
            }
            gameManger.Datasave(filepath);


        }
        if (GUILayout.Button("MapDataInit"))
        {
            if (dataChanger.allStage <= 0) return;
            gameManger.MapDataInit(mapfilepath,dataChanger.allStage,dataChanger.clear);
        }

        
        Debug.Log("�ύX����");
    }

}
 
