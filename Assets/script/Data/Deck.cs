using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(fileName = "Deck", menuName ="Create Deck")]

public class Deck : ScriptableObject
{
    public List<CardEntity> cardlist;
}
