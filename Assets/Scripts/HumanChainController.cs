using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class HumanChainController : MonoBehaviour
{
    private static HumanChainController _instance;
    public static HumanChainController Instance =>
        _instance != null
            ? _instance
            : _instance = FindObjectOfType<HumanChainController>();
    private InvisibleRope _invisibleRope;

    public HumanChainPiece Prefab;
    public List<HumanChainPiece> Chain;
    //public bool Direction;
    public int StartCount = 3;

    public bool Reverse;
    public bool CreateOnStart;
    public bool IsFalling;
    private bool IsFreeFalling;

    void OnValidate()
    {
        if (Reverse)
        {
            Reverse = false;
            ReverseChain();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        _invisibleRope = GetComponent<InvisibleRope>();
        if (!CreateOnStart) return;
        if (Chain == null) Chain = new List<HumanChainPiece>();
        _invisibleRope.CreateSegments(StartCount + 1);
        int i = 0;
        do
        {
            var direction = Random.Range(0, 2) == 0;
            var piece = Instantiate(Prefab, Vector3.down * i, Quaternion.identity, transform) as HumanChainPiece;
            if (direction)
            {
                piece.RightArm.SetConnectionPos(_invisibleRope.Segments[i]);
                piece.LeftArm.SetConnectionPos(_invisibleRope.Segments[i + 1]);
            }
            else
            {
                piece.RightArm.SetConnectionPos(_invisibleRope.Segments[i + 1]);
                piece.LeftArm.SetConnectionPos(_invisibleRope.Segments[i]);
            }
            foreach (var body in piece.GetComponentsInChildren<Rigidbody>())
            {
                body.isKinematic = false;
            }
            piece.IsLinked = true;
            Chain.Add(piece);
            i++;
        } while (Chain.Count < StartCount);
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying || GameManager.Instance.IsGameOver) return;
    }

    public void AddPiece(HumanChainPiece piece, bool direction = false, Transform linkPosition = null)
    {
        if (IsFalling) return;
        piece.transform.parent = transform;
        var segment1 = _invisibleRope.Segments[_invisibleRope.Segments.Count - 1];
        _invisibleRope.AddSegment();
        var segment2 = _invisibleRope.Segments[_invisibleRope.Segments.Count - 1];
        if (direction)
        {
            piece.RightArm.SetConnectionPos(segment1);
            piece.LeftArm.SetConnectionPos(segment2);
        }
        else
        {
            piece.RightArm.SetConnectionPos(segment2);
            piece.LeftArm.SetConnectionPos(segment1);
        }
        if (piece.GrabParticle != null)
            Instantiate(piece.GrabParticle, segment1.position, piece.GrabParticle.transform.rotation);
        foreach (var body in piece.GetComponentsInChildren<Rigidbody>())
        {
            body.collisionDetectionMode = CollisionDetectionMode.Continuous;
            body.isKinematic = false;
        }
        piece.IsLinked = true;
        Chain.Add(piece);
        ReverseChain(linkPosition);
        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
    }

    public void ReverseChain(Transform connectionPos = null)
    {
        _invisibleRope.ReverseRope();
        Chain.Reverse();
        if (connectionPos != null)
            _invisibleRope.Segments[0].position = connectionPos.position;
        StartCoroutine(SlowFall(1));
    }

    private IEnumerator SlowFall(int duration)
    {
        if (IsFalling) yield return new WaitUntil(() => !IsFalling);
        IsFalling = true;
        var originalStiffness = _invisibleRope.RopeStiffness;
        var originalRotSpeed = _invisibleRope.RopeSpeed;

        for (float i = 0; i < duration; i += 0.02f)
        {
            _invisibleRope.RopeStiffness = Mathf.Lerp(0, originalStiffness, i / duration);
            _invisibleRope.RopeSpeed = Mathf.Lerp(0, originalRotSpeed, i / duration);
            yield return new WaitForSeconds(0.02f);
        }
        //yield return new WaitForSeconds(duration);
        _invisibleRope.RopeStiffness = originalStiffness;
        _invisibleRope.RopeSpeed = originalRotSpeed;
        IsFalling = false;
    }

    public void BreakChain(bool isFail = true)
    {
        if (IsFalling)
        {
            return;
        }
        foreach (var humanChainPiece in Chain)
        {
            humanChainPiece.UnLink();
        }
        if (isFail)
            StartCoroutine(FailRoutine());
    }

    private IEnumerator FailRoutine()
    {
        if (!GameManager.Instance.IsPlaying) yield break;
        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
        GameManager.Instance.IsPlaying = false;
        yield return new WaitForSeconds(3);
        GameManager.Instance.Fail();
    }

    public void FreeFall()
    {
        IsFreeFalling = true;
        _invisibleRope.EnableGravity = true;
    }
    public void RemoveOne()
    {
        if (Chain.Count < 1) return;
        var chainPiece = Chain[Chain.Count - 1];
        chainPiece.UnLink();
        Chain.RemoveAt(Chain.Count - 1);
        _invisibleRope.Segments.RemoveAt(_invisibleRope.Segments.Count - 1);
        if (IsFreeFalling && Chain.Count < 1)
            GameManager.Instance.Win();
    }
}
