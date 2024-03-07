using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infomation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Tutorial turorial;
    [SerializeField] List<Sprite> phto;
    public void SetUp()
    {
        if (phto.Count == 0) return;
        Transform canvas = GameObject.Find("Canvas").transform;
        Tutorial obj =  Instantiate(turorial, canvas);
        obj.SetUpTutorial(phto);

    }
}
