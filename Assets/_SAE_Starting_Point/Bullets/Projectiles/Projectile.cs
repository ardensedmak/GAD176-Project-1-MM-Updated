using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 9000f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Impulse();
        Destroy(gameObject, 1);
    }
    private void Impulse()
    {
        //rb.AddForce(transform.forward * projectileSpeed, ForceMode.Impulse);
    }
}
