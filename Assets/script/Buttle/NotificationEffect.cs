using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationEffect : MonoBehaviour
{
    public void SetText(string text)
    {
        Text t = GetComponent<Text>();
        t.text = text;
    }
}
