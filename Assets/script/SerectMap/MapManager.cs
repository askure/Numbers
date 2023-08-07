using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapManager", menuName = "Create MapManager")]
public class MapManager : ScriptableObject
{
   public int stageNum;
   public int beforeStageid;
   public string stageName;
   public List<StageEntity> maps;
}
