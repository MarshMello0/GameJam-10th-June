using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class RandomPainting : MonoBehaviour
{
    [SerializeField] private GameObject[] paintings;
    private void OnEnable()
    {
        int painting = Random.Range(0, paintings.Length - 1);
        paintings[painting].SetActive(true);
    }
}
