using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtleLog : MonoBehaviour
{
    public void OpenLog()
    {
        var g = Resources.Load<GameObject>("BattlePrehub/LogTrigger"); 
        var text = g.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        text.text = "";
        foreach (string s in GameManger.LogText)
        {
            text.text += s + "\n";
        }
        var canvas = GameObject.Find("Canvas").transform;
        Instantiate(g, canvas);
    }

    public void  CloseLog()
    {
        Destroy(gameObject);
    }
}
