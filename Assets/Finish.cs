using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public GameObject[] Confetties;
    void OnTriggerEnter(Collider other)
    {
        if(!GameManager.Instance.IsPlaying || GameManager.Instance.IsGameOver) return;
        var chainPiece = other.transform.GetComponentInParent<HumanChainPiece>();
        if (chainPiece == null || !chainPiece.IsLinked) return;
        //VCamManager.Instance.PlayConfetti();
        foreach (var confetty in Confetties)
        {
            confetty.SetActive(true);
        }

        GameManager.Instance.FinishCount = Mathf.Max(GameManager.Instance.FinishCount,HumanChainController.Instance.Chain.Count);
        HumanChainController.Instance.FreeFall();
    }
}
