using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="StageEntity",menuName ="Create StageEntity")]
public class StageEntity : ScriptableObject
{
   public enum gift
   {
        stone,
        item,
        charactor

   }
    public enum EnemyOrParty
    {
        Enemy,
        Party
    }
    public enum Buff
    {
        num,
        at,
        df,
        hp,
        all_not_num
    }
   
   public int stageid;
   public int beforestageid;
   public string stageName;
   public int buttleNum;
   
   public List<EnemyEntity> enemy;
   public List<string> stageinfo;
    [System.Serializable]
    public class FieldEffect
    {
        public EnemyOrParty EnemyOrParty;
        public Buff buff;
        public int ApplicationNum;
        public double effectSize;
        public int effectCondition;
    }
    public List<FieldEffect> fieldEffects;

    [System.Serializable]
   public class Gifts
    {
        public gift Gift;
        public int giftNum;
        public CardEntity card;
        public int drop;

    }
    public List<Gifts> gifts;

    public Sprite BackGraunds;

    public AudioClip[] intro = new AudioClip[2];
    public AudioClip[] loop = new AudioClip[2];

    
  
   
}
