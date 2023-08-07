using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUpReturn : MonoBehaviour
{
    public void StatusReturn()
    {
        CharacterManager characterManager = new CharacterManager();
        characterManager.LevelUp();
        Destroy(gameObject);
    }
}
