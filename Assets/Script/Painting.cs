using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Painting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite=SpriteStorage.Instance.Sprites[GameManager.Instance.PuzzleId];
    }

    
}
