using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleRope : MonoBehaviour
{
    public float SegmentDistance = 1;
    public List<Transform> Segments;
    public float RopeStiffness = 50;
    public float RopeGravity = 10;
    public float RopeSpeed = 5;
    public float MovementRange = 45;
    public bool Reverse;
    public bool EnableGravity;
    private float _speed;

    void OnValidate()
    {
        if (Reverse)
        {
            Reverse = false;
            ReverseRope();
        }
    }
    public void ReverseRope()
    {
        Segments.Reverse();
    }
    public void CreateSegments(int count)
    {
        Segments = new List<Transform>();
        for (int i = 0; i < count; i++)
        {
            var segment = new GameObject("Segment " + i);
            segment.transform.parent = transform;
            segment.transform.localPosition = new Vector3(0, -i * SegmentDistance, 0);
            Segments.Add(segment.transform);
        }
    }
    void FixedUpdate()
    {
        if(!GameManager.Instance.IsPlaying)return;
        for (int i = 0; i < Segments.Count; i++)
        {
            var segment = Segments[i];
            if (i > 0)
            {
                var targetPos = Segments[i - 1].position - Segments[i - 1].up * SegmentDistance;
                segment.position = Vector3.Lerp(segment.position, targetPos, Time.fixedDeltaTime * RopeSpeed);
                segment.rotation = Quaternion.Lerp(segment.rotation, Segments[i - 1].rotation, Time.fixedDeltaTime * RopeStiffness);
            }
            else
            {
                var input = -VirtualJoystick.Instance.GetJoystickDirection().x;
                segment.rotation = Quaternion.Lerp(segment.rotation, Quaternion.Euler(0,0,input * MovementRange), Time.fixedDeltaTime * RopeStiffness);
                if (EnableGravity)
                {
                    _speed += RopeGravity * Time.fixedDeltaTime;
                    segment.position += Vector3.down * _speed * Time.fixedDeltaTime;
                }
            }
        }
    }

    public void AddSegment()
    {
        var i = Segments.Count;
        var segment = new GameObject("Segment " + i);
        segment.transform.parent = transform;
        segment.transform.localPosition = Segments[i - 1].position - Segments[i - 1].up * SegmentDistance;
        Segments.Add(segment.transform);
    }
}
