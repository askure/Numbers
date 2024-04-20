using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationEffect : MonoBehaviour
{
    public void SetText(string text)
    {
        TextMeshProUGUI t = GetComponent<TextMeshProUGUI>();
        t.text = text;
    }
}
