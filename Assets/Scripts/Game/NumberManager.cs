using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;
public class NumberManager : MonoBehaviour
{
    
    [Foldout("Number Sprites")] [SerializeField] private Sprite[] numbers;

    private Sprite GetSpriteNumber(int number)
    {
        if (number == 0 || number >= numbers.Length)
        {
            Debug.LogError("Number is too high for the sprite");
            return null;
        }
        return numbers[number - 1];
    }

    public void SetSpriteFromNumber(int number, SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sprite = GetSpriteNumber(number);
    }
}
