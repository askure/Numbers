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

    // �C���X�^���X���擾
    public static Notification GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
        texts = new List<string>();
    }

    // �e�L�X�g�����X�g�ɓ���ă|�b�v�A�b�v��\��
    public void PutInQueue(string text)
    {
        texts.Add(text);
        StartNext();
    }

    void StartNext()
    {
        // �ҋ@���łȂ��ăe�L�X�g���c���Ă����
        if (!IsWaiting && texts.Count != 0)
        {
            IsWaiting = true; // �ҋ@���ɂ���
            // �|�b�v�A�b�v�E�B���h�E�����
            currentPanel = Instantiate(notificationPanelPrefab.gameObject, canvas.transform).GetComponent<NotificationPanel>();
            currentPanel.SetText(texts[0]); // �e�L�X�g������
        }
    }

    // �A�j���[�V�������I�������
    public void AnimationEnd()
    {
        // ���X�g����폜
        texts.RemoveAt(0);
        IsWaiting = false; // �ҋ@���łȂ�����
        StartNext(); // ���̃|�b�v�A�b�v��\��
    }
}
