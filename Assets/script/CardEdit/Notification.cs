using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] NotificationPanel notificationPanelPrefab;
    NotificationPanel currentPanel;

    [SerializeField] List<string> texts;
    bool IsWaiting;

    static Notification instance;

    // インスタンスを取得
    public static Notification GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        texts = new List<string>();
    }

    // テキストをリストに入れてポップアップを表示
    public void PutInQueue(string text)
    {
        texts.Add(text);
        StartNext();
    }

    void StartNext()
    {
        // 待機中でなくてテキストが残っていれば
        if (!IsWaiting && texts.Count != 0)
        {
            IsWaiting = true; // 待機中にする
            // ポップアップウィンドウを作る
            currentPanel = Instantiate(notificationPanelPrefab.gameObject, canvas.transform).GetComponent<NotificationPanel>();
            currentPanel.SetText(texts[0]); // テキストを入れる
        }
    }

    // アニメーションが終わったら
    public void AnimationEnd()
    {
        // リストから削除
        texts.RemoveAt(0);
        IsWaiting = false; // 待機中でなくする
        StartNext(); // 次のポップアップを表示
    }
}
