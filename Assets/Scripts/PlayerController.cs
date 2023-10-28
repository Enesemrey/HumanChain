using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _lastFrameMousePos;
    [SerializeField] private Animator _animator;


    public Transform PlayerMesh;

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

        var pos = Input.mousePosition.x / Screen.width;
        if (Input.GetMouseButtonDown(0))
        {
            _lastFrameMousePos = pos;
            GetComponent<PlayerMovement>().ApplyMovement(0);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            GetComponent<PlayerMovement>().ApplyMovement(pos - _lastFrameMousePos);
            _lastFrameMousePos = pos;
        }
        else
        {
            GetComponent<PlayerMovement>().ApplyMovement(0);
        }
    }
}
