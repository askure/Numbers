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
        string chafilepath = Application.persistentDataPath + "/" + ".charactersavedata.json";
        DrawDefaultInspector();
        var dataChanger = target as DataChanger;
        DataManager gameManger = new DataManager(filepath);
        CharacterDataManager characterData = new CharacterDataManager(chafilepath);
        if (GUILayout.Button("�����N�ύX"))
        {
            gameManger.rank = dataChanger.Rank;
            gameManger.DataSave(filepath);
        }
        if (GUILayout.Button("EXP��Stone�ύX"))
        {
            gameManger.Exp = dataChanger.Exp;
            gameManger.Stone = dataChanger.Stone;
            gameManger.DataSave(filepath);
        }

        if (GUILayout.Button("�{�[�i�X�ύX"))
        {
            gameManger.divisor_lv = dataChanger.Diviser;
            gameManger.multi_lv = dataChanger.Multi;
            gameManger.prime_lv = dataChanger.Prime;
            gameManger.DataSave(filepath);
        }

        if (GUILayout.Button("�L�����N�^�[�X�e�[�^�X�ύX"))
        {
           

            foreach (CharacterData.CardLv card in characterData.cardLvs)
            {
                card.Lv = dataChanger.CardLv;
                card.atbuf = dataChanger.Atbuf;
                card.hpbuf = dataChanger.Hpbuf;
                card.dfbuf = dataChanger.Dfbuf;
                card.convex = dataChanger.convex;
                card.pos = dataChanger.pos;
            }
            characterData.Datasave(chafilepath);


        }
        if (GUILayout.Button("MapDataInit"))
        {
            if (dataChanger.allStage <= 0) return;
            gameManger.MapDataInit(mapfilepath,dataChanger.allStage,dataChanger.clear);
        }

        
        Debug.Log("�ύX����");
    }

}
 
