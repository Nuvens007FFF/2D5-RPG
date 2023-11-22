using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;
using System;

public class LevelSkillManagerBase : MonoBehaviour
{
    public int coinRequired;
    public List<Image> imageList = new List<Image>();
    public int level;
    protected virtual void Awake()
    {
        Image[] objChild = GetComponentsInChildren<Image>();
        imageList.AddRange(objChild);
    }
    protected virtual void SetImageUI(int stt)
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            {
                if (imageList[i] == imageList[stt]) imageList[i].enabled = true;
                else imageList[i].enabled = false;
            }
        }
    }
}
