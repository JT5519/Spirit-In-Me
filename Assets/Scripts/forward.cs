using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//obsolete script, not required for now.
public class forward : MonoBehaviour
{
    public Rigidbody rb;
    public int vel;
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * vel);
    }
}
