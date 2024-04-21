using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    public void SetText(string text)
    {
        TextMeshProUGUI t = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        t.text = text;
    }
}
