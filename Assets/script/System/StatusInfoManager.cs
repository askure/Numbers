using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusInfoManager : MonoBehaviour
{
    [SerializeField] GameObject hand;
    [SerializeField] Transform content;
    [SerializeField] NodeManager node;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StatusInfoOpen()
    {
        if (gameObject.activeInHierarchy) return;
        
        gameObject.SetActive(true);
        ALLDesttory();
        for(int i=0;i<hand.transform.childCount; i++)
        {
            var g = hand.transform.GetChild(i).gameObject;
            if (g.CompareTag("status"))
            {
                if(g.name == "AtManager")
                {
                    PartyAtManager atManager = g.GetComponent<PartyAtManager>();
                    string info = atManager.SkillInfo();
                    NodeManager node = Instantiate(this.node, content);
                    node.SetText(info);
                    atManager.GetNode(node.gameObject);

                }
                else if(g.name == "DfManager")
                {
                    PartyDfStatusManager dfManager = g.GetComponent<PartyDfStatusManager>();
                    string info = dfManager.SkillInfo();
                    NodeManager node = Instantiate(this.node, content);
                    node.SetText(info);
                    dfManager.GetNode(node.gameObject);
                }
            }
        }
    }
    public void StatusInfoClose()
    {
      
        if (!gameObject.activeInHierarchy) return;
        gameObject.SetActive(false);
    }
    private void ALLDesttory()
    {
        for(int i=0; i< content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
