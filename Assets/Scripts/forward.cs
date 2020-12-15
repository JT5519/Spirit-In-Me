using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forward : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public int vel;
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(transform.forward*vel);
    }
}
