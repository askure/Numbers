using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Numanimation : MonoBehaviour
{
    [SerializeField] GameObject nums;
    void Start()
    {
       for(int i = 0; i< nums.transform.childCount; i++)
        {
            var text = nums.transform.GetChild(i).GetComponent<Text>();
            var num = Random.Range(1, 100);
            text.text = num.ToString();
        }
        nums.GetComponent<Animator>().enabled = true;
    }

    public void EndAnimation()
    {
        GameObject.Find("NUMBERS").GetComponent<Animator>().enabled = true;
        Destroy(gameObject);
    }
    
}
