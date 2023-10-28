using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixArm : MonoBehaviour
{
    [SerializeField] private Transform _connectionPos;

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_connectionPos == null) return;
        GetComponent<Rigidbody>().AddForce((_connectionPos.transform.position - transform.position) * 4000);
        //transform.position = _connectionPos.position;
        Rigidbody otherBody = _connectionPos.GetComponent<Rigidbody>();
        if (otherBody)
            otherBody.AddForce((_connectionPos.transform.position - transform.position) * -4000);
        //transform.rotation = _connectionPos.rotation;
    }

    public void SetConnectionPos(Transform target)
    {
        _connectionPos = target;
    }

    public void ClearConnection()
    {
        _connectionPos = null;
    }
}
