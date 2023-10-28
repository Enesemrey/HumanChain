using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject ParticleEffect;
    public float FloatForce = 10;
    private List<Rigidbody> _bodies;

    void Awake()
    {
        _bodies = new List<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        var body = other.GetComponent<Rigidbody>();
        if (body != null && !_bodies.Contains(body))
        {
            Instantiate(ParticleEffect, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z),
                ParticleEffect.transform.rotation);
            body.useGravity = false;
            body.drag = 10;
            _bodies.Add(body);
        }
    }

    void FixedUpdate()
    {
        foreach (var body in _bodies)
        {
            body.AddForce(Vector3.up * FloatForce * (transform.position.y - body.transform.position.y));
        }
    }
}
