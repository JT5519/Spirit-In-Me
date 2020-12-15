using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletCollider : MonoBehaviour
{
    public GameObject bulletExplosion;
    private void OnCollisionEnter(Collision collision)
    {
        GameObject bulletHole = Instantiate(bulletExplosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
