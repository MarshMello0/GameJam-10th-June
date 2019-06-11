using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    [Header("Roof Falling")] 
    [SerializeField] private Transform roof;
    [SerializeField] private Animator animator;

    public void PlayDeath()
    {
        MoveRoofDown();
    }

    private void MoveRoofDown()
    {
        animator.Play("Drop");
    }
}
