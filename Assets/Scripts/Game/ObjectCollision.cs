using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectCollision : MonoBehaviour
{
    public bool isFinish;
    public GameManager gm;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (isFinish)
            {
                gm.StartNewLevel();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }

}
