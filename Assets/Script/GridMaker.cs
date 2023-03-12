
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
        //현재 퍼즐id에 해당하는 퍼즐데이터를 반환/ 없으면 id=-1인 퍼즐데이터 반환
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
    }//그리드가 자신의 위치와 모양정보에 따라 피스를 만들게 하는 함수
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
    }//PuzzleData에 넣을 gridData(각그리드의 모양정보 및 피스의 고정여부)를 생성
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
    }//단순 그리드 생성
    public void InitRandomGridsShapeInfo()
    {
        Cw dirCw=Cw.north;
        Vector2Int gridPos = new Vector2Int(0, 0);
        while(true){
            //상하좌우봐서 그리드가 없으면(테두리)평평/있는데 안정한거면 랜덤/있고 정해진거면 그거 반대로
            for (int i = 0; i < 4; i++)//0~3을 북동남서로 봄
            {
                Vector2Int dirGridPos = CwToVector(i) + gridPos; //체크할 해당방향 그리드위치
                if (dirGridPos.x >= boardSize || dirGridPos.x < 0 || dirGridPos.y >= boardSize||dirGridPos.y<0)//테두리인지
                    grids[gridPos.x, gridPos.y].SetGridShape(i, GridShape.plane);
                else{
                    if (grids[dirGridPos.x, dirGridPos.y].IsHaveShapeInfo) //주변에 모양정보를 가진 그리드가 있을 때
                    {
                        if (grids[dirGridPos.x, dirGridPos.y].IsOutsidPart((i + 2) % 4))// 해당 그리드의 인접한 부분이 튀어나왔는지
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
            //이동 이동하려는 위치에 그리드가 없거나 있는데 정해졌으면 dir+=1 dir%=4하고 이동 또 이동 못하면 끝
            Vector2Int gridTomove =gridPos+ CwToVector((int)dirCw);
            if (gridTomove.x>=boardSize ||gridTomove.x <0 || gridTomove.y >= boardSize|| gridTomove.y <0
                || grids[gridTomove.x,gridTomove.y].IsHaveShapeInfo){
                dirCw = (Cw)(((int)dirCw + 1) % 4);
            }
            gridPos = gridPos + CwToVector((int)dirCw);
            if (grids[gridPos.x,gridPos.y].IsHaveShapeInfo)
                break;
        }
    }//랜덤하게 퍼즐 모양을 정해줌
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
    }//0~3의 방향값을 받고 크기 1인 vector2로 변환
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
