using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusListManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> status;
    [SerializeField] private float changeTime;
     private List<GameObject> tmp;
    private List<GameObject> beforeAnimationtmp;
    private float time;
    private int page;
    Transform pos;
    void Start()
    {
        status = new List<GameObject>();
        tmp = new List<GameObject>();
        page = 0;
        pos = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (status.Count <= 4) time = 0;
        if(time > changeTime)
        {   
            time = 0;
            InitView();      
            page++;
        }
    }

    public  void Add(GameObject g)
    {   
        if(status.Count < 4)
        {
            var G = Instantiate(g, pos);
            tmp.Add(G);
        }

        status.Add(g);
     
    }
    public  void Remove(GameObject g)
    {
        int index = status.IndexOf(g);
        if (index == -1) return;
        status.RemoveAt(index);
        InitView();
    }
    public  int Count()
    {
        return status.Count;
    }

    void InitView()
    {
        while (tmp.Count != 0)
        {
            var g = tmp[0];
            tmp.RemoveAt(0);
            Destroy(g);
        }
        for (int i = page * 4; i < page * 4 + 4; i++)
        {
            if (i >= status.Count) {
                page = 0;
                break;
            }
            var g = Instantiate(status[i], pos);
            tmp.Add(g);
        }
    }

    public void SetGameObjct(GameObject g)
    {
        if (beforeAnimationtmp == null) beforeAnimationtmp = new List<GameObject>();
        beforeAnimationtmp.Add(g);
    }
    public void SetGameObject()
    {
        for(int i=0; i< beforeAnimationtmp.Count; i++)
        {
            Add(beforeAnimationtmp[i]);
        }
        beforeAnimationtmp.Clear();
    }
}
