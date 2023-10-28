using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraMovement : MonoBehaviour
{
    public InvisibleRope InvisibleRope;
    private Vector3 _offset;
    void Start()
    {
        _offset = InvisibleRope.Segments[Mathf.Max(0, InvisibleRope.Segments.Count - 3)].position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(InvisibleRope.Segments.Count < 2) return;
        var pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, InvisibleRope.Segments[Mathf.Max(0,InvisibleRope.Segments.Count - 3)].position.y - _offset.y, Time.deltaTime * 10);
        transform.position = pos;
    }
}
