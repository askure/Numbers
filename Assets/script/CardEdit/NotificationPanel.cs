using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    public void SetText(string text)
    {
        Text t = transform.GetChild(0).GetComponent<Text>();
        t.text = text;
    }
}
