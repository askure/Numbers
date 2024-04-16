using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillViewNode : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI skillName, skillInfo;

    public void InitSkillViewNode(string skillName, string skillInfo)
    {
        this.skillName.SetText(skillName);
        this.skillInfo.SetText(skillInfo);
    }
}
