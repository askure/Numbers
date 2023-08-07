using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    public void LoadSecne()
    {

        SceneManager.LoadScene(sceneName);
    }
}
