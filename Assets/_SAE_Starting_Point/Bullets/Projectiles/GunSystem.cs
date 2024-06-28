using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    [SerializeField] float rateOfFire = 1f;
    [SerializeField] private Rigidbody Projectile;
    private int ProjectileSpeed = 100;
    private float FireRate = 2;  // The number of bullets fired per second
    private float lastfired;      // The value of Time.time at the last firing moment

    private void Start()
    {

    }
    private void Update()
    {

    }
    public float GetRateOffFire()
    {
        return rateOfFire;
    }
    public void Fire()
    {
        if (Time.time - lastfired > 1 / FireRate)
        {
            lastfired = Time.time;
            Rigidbody clone;
            clone = Instantiate(Projectile, transform.position, transform.rotation);
            clone.velocity = transform.TransformDirection(Vector3.forward * ProjectileSpeed);
        }
   
    }
}