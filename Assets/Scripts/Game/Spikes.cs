using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(deathParticle, other.transform.position, Quaternion.Euler(0, 0, 0));
            other.gameObject.SetActive(false);
        }
    }
}
