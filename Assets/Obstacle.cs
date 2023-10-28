using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject[] ObstacleObjects;
    public ObstacleTypes ObstacleType;
    public Ease TweenMode = Ease.InOutQuart;
    public float MovementRange = 5;
    public float MovementTime = 1;
    public float TimeOffset = 0.25f;
    public GameObject ClashParticle;
    public enum ObstacleTypes
    {
        small,medium,large
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.IsPlaying);
        yield return new WaitForSeconds(TimeOffset);
        ObstacleObjects[(int) ObstacleType].transform.DOMoveZ(-MovementRange, MovementTime)
            .SetEase(TweenMode)
            .SetLoops(10000, loopType: LoopType.Yoyo);
    }
    void OnValidate()
    {
        for (var i = 0; i < ObstacleObjects.Length; i++)
        {
            var obstacle = ObstacleObjects[i];
            obstacle.SetActive((int)ObstacleType == i);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(HumanChainController.Instance.IsFalling) return;
        var chain = other.transform.GetComponentInParent<HumanChainPiece>();
        if (chain.GetPositionFromHip(other.transform.position) > 2) return;

        ClashParticle.SetActive(true);
        HumanChainController.Instance.BreakChain();
    }
}
