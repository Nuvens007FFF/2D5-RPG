using DG.Tweening;
using System;
using UnityEngine;

public class AnimationCoin : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private float countRotate;
    [SerializeField] private float timeRotate;
    void Start()
    {
        UiEventBattle.animationCoinEvent += animationCoin;
    }
    private void animationCoin()
    {   
        transform.DORotate(new Vector3(0f, countRotate * 360f , 0f), timeRotate, RotateMode.FastBeyond360);
    }
    private void OnDestroy()
    {
        UiEventBattle.animationCoinEvent -= animationCoin;
    }
}
