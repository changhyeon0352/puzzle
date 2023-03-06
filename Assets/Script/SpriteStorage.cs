using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteStorage : Singleton<SpriteStorage>
{
    [SerializeField]
    Sprite[] sprites;
    public Sprite[] Sprites { get { return sprites; } }
}
