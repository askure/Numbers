using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardListEntiry", menuName = "Create CardListEntiry")]
public class CardListEntiry : ScriptableObject
{
    public List<CardEntity> cards;
}
