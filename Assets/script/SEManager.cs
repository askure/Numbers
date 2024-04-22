using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool isLoad = false;
    private static string filepath;
    private void Awake()
    {
        if (isLoad)
        { // ���łɃ��[�h����Ă�����
            Destroy(this.gameObject); // �������g��j�����ďI��
            return;
        }
        isLoad = true; // ���[�h����Ă��Ȃ�������A�t���O�����[�h�ς݂ɐݒ肷��
        DontDestroyOnLoad(this.gameObject);
    }


    public void PlaySE(AudioClip se)
    {   
        AudioSource audio = GetComponent<AudioSource>();
        DataManager.DataLoad();
        audio.volume = DataManager.volume;
        audio.PlayOneShot(se);
        
    }
}
