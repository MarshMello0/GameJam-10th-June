using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DeathEffect(other.transform.position);
        }
    }

    private void DeathEffect(Vector3 position)
    {
        
    }
}
