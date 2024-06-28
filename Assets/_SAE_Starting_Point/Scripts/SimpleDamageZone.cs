using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Game;
using UnityEngine;

public class SimpleDamageZone : MonoBehaviour
{
    [SerializeField] float damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Health>() && other.tag == "Player")
        {
            other.GetComponent<Health>().TakeDamage(damage,gameObject);
        }
    }
}
