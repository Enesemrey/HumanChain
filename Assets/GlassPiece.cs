using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassPiece : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-10,10f),0, Random.Range(-10, 10f)));
    }

}
