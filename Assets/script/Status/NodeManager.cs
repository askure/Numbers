using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string info)
    {
        text.text = info;
    }
}
