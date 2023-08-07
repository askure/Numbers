using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DeckData : MonoBehaviour
{
    public DeckCard[] cards;
    [System.Serializable]
    public class DeckCard
    {
        public int deckId;
        public string deckName;
        public int[] cardid = new int[12];
    }


}
