using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneAnimation : MonoBehaviour  
        
{
    private string _SceneName;
    // Start is called before the first frame update

    
     public void LoadScneEvent()
    {
        SceneManager.LoadScene(_SceneName);
    }
     public void LoadScene(string sceneName,string mapname, string stagename)
    {
        var mapName = transform.GetChild(0).GetComponent<Text>();
        var stageName = transform.GetChild(1).GetComponent<Text>();
        mapName.text = mapname;
        stageName.text = stagename;
        GetComponent<Animator>().enabled = true;
        _SceneName = sceneName;
    }
}
