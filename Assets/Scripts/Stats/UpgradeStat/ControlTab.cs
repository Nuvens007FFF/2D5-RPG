using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTab : MonoBehaviour
{
    [SerializeField] GameObject[] tabs;
    public void SelectTab(int index)
    {
        for (int i = 0;i < tabs.Length; i++)
        {
            if (i == index)
            {
                tabs[i].SetActive(true);
            }
            else tabs[i].SetActive(false);
        }
    }

}
