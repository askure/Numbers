using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] bool changeBGM;
    [SerializeField] AudioClip se;
    Slider _slider;
    static BGMManager bgm;
    DataManager dataManager;


    public void LoadSecne()
    {
       
        if (bgm == null)
        {
            var SEManager = GameObject.Find("BGM");
            bgm = SEManager.GetComponent<BGMManager>();
        }
       
        bgm.PlaySE(se,2f);
        var gameObject = Resources.Load<GameObject>("LoadPanel");
        var canva = GameObject.Find("Canvas").transform;
        var pane = Instantiate(gameObject, canva);
        _slider = pane.transform.GetChild(1).GetComponent<Slider>();
        StartCoroutine(LoadSceneUI());

    }


    IEnumerator LoadSceneUI()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }
}
