using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSetting : MonoBehaviour
{
    public Image image;

    
    private void Start()
    {
        image.sprite = SpriteStorage.Instance.Sprites[GameManager.Instance.PuzzleId];
    }
    private void OnEnable()
    {
        image.sprite = SpriteStorage.Instance.Sprites[GameManager.Instance.PuzzleId];
    }
}
