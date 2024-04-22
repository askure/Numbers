using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataChanger))]
public class DataChangerEditor : Editor {

    public override void OnInspectorGUI()
    {   
        DrawDefaultInspector();
        var dataChanger = target as DataChanger;
        DataManager.DataLoad();
        DataManager.StageDataLoad();
        CharacterDataManager.DataLoad();
        if (GUILayout.Button("ランク変更"))
        {
            DataManager.rank = dataChanger.Rank;
            DataManager.DataSave();
        }
        if (GUILayout.Button("EXPとStone変更"))
        {
            DataManager.Exp = dataChanger.Exp;
            DataManager.Stone = dataChanger.Stone;
            DataManager.DataSave();
        }

        if (GUILayout.Button("ボーナス変更"))
        {
            DataManager.divisor_lv = dataChanger.Diviser;
            DataManager.multi_lv = dataChanger.Multi;
            DataManager.prime_lv = dataChanger.Prime;
            DataManager.DataSave();
        }

        if (GUILayout.Button("キャラクターステータス変更"))
        {
           

            foreach (CharacterData.CardLv card in CharacterDataManager.cardLvs)
            {
                card.Lv = dataChanger.CardLv;
                card.atbuf = dataChanger.Atbuf;
                card.hpbuf = dataChanger.Hpbuf;
                card.dfbuf = dataChanger.Dfbuf;
                card.convex = dataChanger.convex;
                card.pos = dataChanger.pos;
            }
            CharacterDataManager.DataSave(false);


        }
        if (GUILayout.Button("MapDataInit"))
        {
            if (dataChanger.allStage <= 0) return;
            DataManager.StageDataInit(dataChanger.clear);
            DataManager.StageDataSave();
        }

        
      
    }

}
 
