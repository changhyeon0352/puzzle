using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesBox : MonoBehaviour
{
    private bool isOnlySide=false;
    private List<Piece> pieces=new List<Piece>();
    [SerializeField]
    Transform contentsTr;
    Transform tempTr;
    private void Awake()
    {
        tempTr=FindObjectOfType<GridMaker>().transform.parent.Find("temp");
    }
    public void PutAllPiecesInBox()
    {
        Piece[] pieces = FindObjectsOfType<Piece>();
        foreach (var piece in pieces)
        {
            if(!piece.IsFixed)
                PutItInBox(piece);
        }
    }
    public void PutItInBox(Piece piece)
    {
        piece.transform.SetParent(contentsTr);
        piece.ChansePieceSize(0.12f* (float)GameManager.Instance.boardSize);
        piece.isInBox = true;
        pieces.Add(piece);
    }
    public void RemovePieceFromBox(Piece piece)
    {
        pieces.Remove(piece);
        piece.transform.SetParent(tempTr);
        piece.ChansePieceSize(1);
    }
    public void ShowSidePieceToggle()
    {
        isOnlySide= !isOnlySide;
        int size = GameManager.Instance.boardSize-1;
        foreach(Piece piece in pieces)
        {
            if(piece.GridPos.x%size!=0&&piece.GridPos.y%size!=0)
            {
                piece.gameObject.SetActive(!isOnlySide);
            }
        }
    }
}
