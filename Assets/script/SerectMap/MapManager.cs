using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "Create MapManager")]
public class MapManager : ScriptableObject
{
   public int stageNum;
   public int beforeStageid;
   public string stageName;
   public Sprite Back;
   public List<StageEntity> maps;
    public AudioClip _intro, loop;
}
