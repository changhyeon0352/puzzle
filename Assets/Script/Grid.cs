using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GridShape { unKnown=0,inside,outside,plane, }
public enum Cw {north=0,east,south,west }

[System.Serializable]
public class Grid : MonoBehaviour
{
    Piece piece;
    [SerializeField]
    Vector2Int gridPos;
    public Vector2Int GridPos { get { return gridPos; } }
    [SerializeField]
    int[] shapeInfosCw = new int[4];
    public int[] ShapeInfosCw { get { return shapeInfosCw; } }
    [SerializeField]
    GameObject piecePrefab;
   
    public bool IsHaveShapeInfo { get { return shapeInfosCw[0]!=0?true:false; } }

    public void SetGridPos(int x,int y)
    {
        gridPos = new Vector2Int(x,y);
    }
    public void SetGridShape(int cw,GridShape shape)
    {
        shapeInfosCw[cw] = (int)shape;
    }
    public void MakeOwnPiece()
    {
        piece =Instantiate(piecePrefab, transform.position, Quaternion.identity).GetComponent<Piece>();
        piece.InitPiece(shapeInfosCw, gridPos);
    }
    
    public bool IsOutsidPart(int clockWiseNum )
    {

        return shapeInfosCw[clockWiseNum]==(int)GridShape.outside;
    }
    public void SetPiece(Piece piece)
    {
        this.piece=piece;
    }
    public void ApplyGridDataAndMakeOwnPiece(GridData gridData)
    {
        shapeInfosCw = gridData.shapeInfo;
        MakeOwnPiece();
        if(gridData.isFixed)
            piece.FixPieceInGrid(this);
    }
}
