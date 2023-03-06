using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = " PuzzleData", menuName = "Scriptable Object/Puzzle Data", order = int.MaxValue)]
public class PuzzleSpriteData:ScriptableObject
{
    [SerializeField]
    private int id;
    public int Id { get { return id; } }
    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get { return sprite; } }
}
