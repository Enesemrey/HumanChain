using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float MovementRange = 5;
    public float MovementTime = 3;
    public float WaitTime = 1;
    public Transform Exc;
    public bool InPlace;

    public GameObject AttackParticle;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitUntil(() => GameManager.Instance.IsPlaying);
        while (true)
        {
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Animator>().SetBool("Walking",true);
            transform.DOMoveZ(-MovementRange, MovementTime).SetRelative().SetEase(Ease.Linear);
            transform.DORotate(new Vector3(0, 180, 0), 0.2f);
            yield return new WaitForSeconds(MovementTime);
            Exc.gameObject.SetActive(true);
            Exc.DOScale(Vector3.one, 0.5f).From(Vector3.zero).SetEase(Ease.OutElastic);
            transform.DOMoveZ(MovementRange, MovementTime).SetRelative().SetEase(Ease.Linear);
            transform.DORotate(Vector3.zero, 0.2f);
            yield return new WaitForSeconds(MovementTime);
            Exc.DOScale(Vector3.zero, 0.25f);
            GetComponentInChildren<Animator>().SetBool("Walking", false);
            InPlace = true;
            GetComponent<Collider>().enabled = true;
            yield return new WaitForSeconds(WaitTime);
            GetComponent<Collider>().enabled = false;
            InPlace = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!InPlace) return;
        if (HumanChainController.Instance.IsFalling) return;

        var chain = other.GetComponentInParent<HumanChainPiece>();
        if(chain.GetPositionFromHip(other.transform.position) > 2) return;
        if (chain != null && chain.IsLinked)
        {
            AttackParticle.SetActive(true);
            GetComponentInChildren<Animator>().SetTrigger("Attack");
            HumanChainController.Instance.BreakChain();
        }
    }
}
