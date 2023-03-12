
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridMaker : MonoBehaviour
{
    [SerializeField]
    GameObject gridPrefab;
    [SerializeField]
    private int boardSize;
    public int BoardSize { get { return boardSize; } }
    public const int pieceSize = 156;
    
    Grid[,] grids;
    public Grid[,] Grids { get { return grids; } }
    // Start is called before the first frame update
    void Start()
    {
        boardSize = GameManager.Instance.boardSize;
        GetComponent<GridLayoutGroup>().cellSize = new(1000 / boardSize, 1000 / boardSize);
        MakeGrids();
        Piece.count = boardSize * boardSize;
        Piece.fixCount = boardSize * boardSize;
        //���� ����id�� �ش��ϴ� �������͸� ��ȯ/ ������ id=-1�� �������� ��ȯ
        PuzzleData puzzleData = DataManager.Instance.GetPuzzleData();
        if (puzzleData.puzzleId != -1){
            GameManager.Instance.isSoundOn = false;
            for (int i = 0; i < boardSize; i++){
                for (int j = 0; j < boardSize; j++){
                    grids[i, j].ApplyGridDataAndMakeOwnPiece(puzzleData.gridDatas[i + boardSize * j]);
                }
            }
            GameManager.Instance.isSoundOn = true;
        }
        else{
            InitRandomGridsShapeInfo();
            InitGridDatas();
            InitPieces();
        }
    }
    public void InitPieces()
    {
        foreach(var grid in grids)
        {
            grid.MakeOwnPiece();
        }
    }//�׸��尡 �ڽ��� ��ġ�� ��������� ���� �ǽ��� ����� �ϴ� �Լ�
    public void InitGridDatas()
    {
        GridData[] gridDatas= new GridData[boardSize*boardSize];
        for(int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                gridDatas[i+j * boardSize] = new GridData();
                gridDatas[i + j * boardSize].shapeInfo = grids[i, j].ShapeInfosCw;
            }
        }
        DataManager.Instance.AddPuzzleData(new PuzzleData( 0, gridDatas));
    }//PuzzleData�� ���� gridData(���׸����� ������� �� �ǽ��� ��������)�� ����
    public void MakeGrids()
    {
        grids = new Grid[boardSize,boardSize];
        for (int j = 0; j < boardSize; j++)
        {
            for (int i = 0; i < boardSize; i++)
            {
                Grid grid = Instantiate(gridPrefab, transform).GetComponent<Grid>();
                grid.SetGridPos(i, j);
                grids[i,j] = grid;
            }
        }
    }//�ܼ� �׸��� ����
    public void InitRandomGridsShapeInfo()
    {
        Cw dirCw=Cw.north;
        Vector2Int gridPos = new Vector2Int(0, 0);
        while(true){
            //�����¿���� �׸��尡 ������(�׵θ�)����/�ִµ� �����ѰŸ� ����/�ְ� �������Ÿ� �װ� �ݴ��
            for (int i = 0; i < 4; i++)//0~3�� �ϵ������� ��
            {
                Vector2Int dirGridPos = CwToVector(i) + gridPos; //üũ�� �ش���� �׸�����ġ
                if (dirGridPos.x >= boardSize || dirGridPos.x < 0 || dirGridPos.y >= boardSize||dirGridPos.y<0)//�׵θ�����
                    grids[gridPos.x, gridPos.y].SetGridShape(i, GridShape.plane);
                else{
                    if (grids[dirGridPos.x, dirGridPos.y].IsHaveShapeInfo) //�ֺ��� ��������� ���� �׸��尡 ���� ��
                    {
                        if (grids[dirGridPos.x, dirGridPos.y].IsOutsidPart((i + 2) % 4))// �ش� �׸����� ������ �κ��� Ƣ��Դ���
                            grids[gridPos.x, gridPos.y].SetGridShape(i, GridShape.inside);
                        else
                            grids[gridPos.x, gridPos.y].SetGridShape(i, GridShape.outside);
                    }
                    else{
                        int shapeNum = Random.Range(1, 3);
                        grids[gridPos.x, gridPos.y].SetGridShape(i, (GridShape)shapeNum);
                    }
                }
            }
            //�̵� �̵��Ϸ��� ��ġ�� �׸��尡 ���ų� �ִµ� ���������� dir+=1 dir%=4�ϰ� �̵� �� �̵� ���ϸ� ��
            Vector2Int gridTomove =gridPos+ CwToVector((int)dirCw);
            if (gridTomove.x>=boardSize ||gridTomove.x <0 || gridTomove.y >= boardSize|| gridTomove.y <0
                || grids[gridTomove.x,gridTomove.y].IsHaveShapeInfo){
                dirCw = (Cw)(((int)dirCw + 1) % 4);
            }
            gridPos = gridPos + CwToVector((int)dirCw);
            if (grids[gridPos.x,gridPos.y].IsHaveShapeInfo)
                break;
        }
    }//�����ϰ� ���� ����� ������
    public static Vector2Int CwToVector(int clockWise)
    {
        clockWise %= 4;
        if(clockWise%2==0)
        {
            return new Vector2Int(0, -clockWise + 1);
        }
        else
        {
            return new Vector2Int(-clockWise+2, 0);
        }
    }//0~3�� ���Ⱚ�� �ް� ũ�� 1�� vector2�� ��ȯ
    public static int VectorToCw(Vector2Int vector)
    {
        if(vector.x==0)
        {
            return 1 - vector.y;
        }
        else
        {
            return 2 - vector.x;
        }
    }

   
}
