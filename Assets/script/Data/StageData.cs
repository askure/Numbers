using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class StageData 
{
    public Stage[] stage;
    public int allstage;
    [System.Serializable]
    public class Stage
    {
       public int stageid;
        public bool clear;
        public int Hiscore;
    }
}
