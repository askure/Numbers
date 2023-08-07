using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<int> trans;
    
    void Start()
    {
        
        StartCoroutine(Test(trans));
    }

    IEnumerator Test(List<int> trans)
    {
        Animator anime = GetComponent<Animator>();
        foreach (int t in trans)
        {
            anime.SetInteger("Trigger", t);
            int seconds = anime.GetCurrentAnimatorClipInfo(0).Length;
            yield return new WaitForSeconds(seconds + 1);
        }

        anime.SetInteger("Trigger", -1);
    }
    

}
