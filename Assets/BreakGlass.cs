using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using TMPro;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    public bool IsBroken;
    public bool IsWin;
    public GameObject Broken;
    public GameObject Normal;
    public bool ShowText;
    public string Text = "x1";
    public GameObject MinusHuman;
    public GameObject Canvas;
    public TextMeshProUGUI TextMesh;
    public Material GlassMat;
    public GameObject GlassBreakParticle;
    void OnValidate()
    {
        Canvas.SetActive(true);
        SetGlobalScale(Canvas.transform,new Vector3(1,1,1));
        MinusHuman.SetActive(!ShowText);
        TextMesh.gameObject.SetActive(ShowText);
        TextMesh.text = Text;
        foreach (var child in Broken.GetComponentsInChildren<Renderer>())
        {
            child.material = GlassMat;
        }
        foreach (var child in Normal.GetComponentsInChildren<Renderer>())
        {
            child.material = GlassMat;
        }

    }
    public static void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }
    void OnCollisionEnter(Collision other)
    {
        if (IsBroken) return;
        var chainPiece = other.transform.GetComponentInParent<HumanChainPiece>();
        if (chainPiece == null || !chainPiece.IsLinked) return;
        if (chainPiece.GetPositionFromHip(other.transform.position) > 2) return;

        Instantiate(GlassBreakParticle,
            new Vector3(other.contacts[0].point.x, transform.position.y, other.contacts[0].point.z),
            GlassBreakParticle.transform.rotation);
        VCamManager.Instance.ScreenShake();
        Normal.SetActive(false);
        Broken.SetActive(true);
        IsBroken = true;
        HumanChainController.Instance.RemoveOne();
        Canvas.transform.DOScale(Vector3.zero, .5f);
        MMVibrationManager.Haptic(HapticTypes.MediumImpact);

        if (IsWin && GameManager.Instance.IsPlaying)
        {
            HumanChainController.Instance.BreakChain(false);
            GameManager.Instance.Win();
        }
    }
}
