using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModifier : MonoBehaviour
{
    RectTransform[] rectTransforms;
    private RectTransform m_RectTransform;
    [SerializeField] private float Width = 80f;
    [SerializeField] private float Height = 80f;
    void Start()
    {   
        m_RectTransform = GetComponent<RectTransform>();
        rectTransforms = GetComponentsInChildren<RectTransform>();
        for(int i = 0; i < rectTransforms.Length; i++)
        {
            if (rectTransforms[i] != null)
            {
                rectTransforms[i].sizeDelta = new Vector2(Width,Height);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
