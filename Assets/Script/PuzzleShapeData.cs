using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = " puzzleShape Data", menuName = "Scriptable Object/PuzzleShape Data", order = int.MaxValue)]
public class PuzzleShapeData : ScriptableObject
{
    [SerializeField]
    Sprite sprite;
    public Sprite Sprite { get { return sprite; } }
    [SerializeField]
    string shapeInfoStr;
    public string ShapeInfoStr { get { return shapeInfoStr; } }


}
