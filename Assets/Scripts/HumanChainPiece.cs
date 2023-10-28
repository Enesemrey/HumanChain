using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class HumanChainPiece : MonoBehaviour
{
    public FixArm RightArm;
    public FixArm LeftArm;
    public Transform LinkPosition;
    public bool IsLinked;
    public Animator Animator;
    public GameObject GrabParticle;
    public Transform HipTransform;

    void Start()
    {
        if (Animator != null)
            Animator.SetInteger("Idle", Random.Range(0, 4));
    }
    void OnTriggerStay(Collider other)
    {
        if (HumanChainController.Instance.IsFalling) return;
        var chainController = other.transform.GetComponentInParent<HumanChainController>();
        if (chainController == null) return;
        Animator.enabled = false;
        chainController.AddPiece(this, Random.Range(0, 2) == 0, LinkPosition);
        GetComponent<Collider>().enabled = false;
    }

    public void UnLink()
    {
        RightArm.enabled = false;
        LeftArm.enabled = false;
        IsLinked = false;
        transform.parent = null;
        this.enabled = false;
        foreach (var body in GetComponentsInChildren<Rigidbody>())
        {
            body.AddForce(Vector3.forward,ForceMode.Impulse);
        }
    }

    public float GetPositionFromHip(Vector3 transformPosition)
    {
        return (HipTransform.position - transformPosition).magnitude;
    }
}
