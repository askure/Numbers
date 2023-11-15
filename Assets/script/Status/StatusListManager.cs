using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusListManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private List<GameObject> status;
    [SerializeField] private float changeTime;
     private List<GameObject> tmp;
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

            if (status.Count <= page * 4)
                page = 0;

            while (tmp.Count != 0)
            {
                var g =tmp[0];
                tmp.RemoveAt(0);
                Destroy(g);
            }
            for (int i = page * 4; i < page * 4 + 4; i++)
            {
                if (i == status.Count) break;
                var g = Instantiate(status[i], pos);
                tmp.Add(g);
            }
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
    }
    public  int Count()
    {
        return status.Count;
    }
}
