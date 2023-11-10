using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static bool isLoad = false;
    private static DataManager dataManager;
    private static string filepath;
    private void Awake()
    {
        if (isLoad)
        { // すでにロードされていたら
            Destroy(this.gameObject); // 自分自身を破棄して終了
            return;
        }
        isLoad = true; // ロードされていなかったら、フラグをロード済みに設定する
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if(dataManager == null)
        {
            dataManager = new DataManager();
        }
        if(filepath == "")
        {
            filepath = Application.persistentDataPath + "/" + ".savedata.json";
        }
    }

    public void PlaySE(AudioClip se)
    {   
        AudioSource audio = GetComponent<AudioSource>();
        dataManager.DataLoad(filepath);
        audio.volume = dataManager.volume;
        audio.PlayOneShot(se);
        
    }
}
