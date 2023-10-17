using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusListManager : MonoBehaviour
{
    // Start is called before the first frame update
    static private List<GameObject> status;
    [SerializeField] private float changeTime;
    [SerializeField] private List<GameObject> test;
    private float time;
    private int page;
    Transform pos;
    void Start()
    {
        status = new List<GameObject>();
        page = 0;
        pos = GameObject.Find("StatusList").transform;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > changeTime)
        {   

            time = 0;

            if (status.Count <= page * 4)
                page = 0;

            for (int i = 0; i < pos.childCount; i++)
            {
                var g = gameObject.transform.GetChild(i);
                Destroy(g.gameObject);
            }
            for (int i = page * 4; i < page * 4 + 4; i++)
            {
                if (i == status.Count) break;
                Instantiate(status[i], pos);
            }
            page++;
            

        }
    }

    public static void Add(GameObject g)
    {
        status.Add(g);
    }
    public static void Remove(GameObject g)
    {
        int index = status.IndexOf(g);
        if (index == -1) return;
        status.RemoveAt(index);
    }
    public static int Count()
    {
        return status.Count;
    }
}
