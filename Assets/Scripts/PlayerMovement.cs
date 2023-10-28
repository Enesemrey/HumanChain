using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float ZSpeed;
    public float XSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }



    public void ApplyMovement(float change)
    {
        var pos = transform.position;
        pos += ZSpeed * Time.deltaTime * Vector3.forward;
        pos.x = Mathf.Clamp(pos.x + change * XSpeed, -4, 4);
        transform.position = pos;
    }
}
