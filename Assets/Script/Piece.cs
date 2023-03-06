using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Linq;

[Serializable]
public class Piece : MonoBehaviour,IBeginDragHandler, IEndDragHandler,IDragHandler,IPointerClickHandler,IPointerUpHandler
{
    PointerEventData m_PointerEventData;
    [SerializeField]
    PuzzleShapeData[]   puzzleShapeDatas;
    [SerializeField]
    Transform maskTr;
    [SerializeField]
    Image image;
    public bool isInBox=true;
    [SerializeField]
    private bool isFixed=false;
    public bool IsFixed { get { return isFixed; } }
    //Piece[] nearPieces;
    Vector2Int gridPos;
    int spinToCw = 0;
    int extraSpinToCw = 0; //추가 회전
    Image img;
    public static int fixCount;
    public static int count;
    private CanvasGroup canvasgroup;
    private ScrollRect scrollRect;
    
    private RectTransform rectTr;
    private void Awake()
    {
        canvasgroup = GetComponent<CanvasGroup>();
        img = GetComponentInChildren<Image>();
        rectTr = GetComponent<RectTransform>();
        scrollRect=FindObjectOfType<ScrollRect>();
    }
    private void Start()
    {
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        InitPieceSize();
    }


    public void ChansePieceSize(float scale)
    {
        maskTr.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        
    }
    public void InitPieceSize()
    {
        float scale = 10f / (float)GameManager.Instance.boardSize;
        maskTr.GetComponent<RectTransform>().sizeDelta = new Vector2(GridMaker.pieceSize * scale, GridMaker.pieceSize * scale);
    }
    public Vector2Int GridPos { get { return gridPos; } }
    
    

    public void InitPiece(int[] shapeInfo,Vector2Int gridpos)
    {
        gridPos = gridpos;
        StringBuilder sb = new StringBuilder();
        Array.ForEach(shapeInfo, x => sb.Append(x));
        string str= sb.ToString();
        foreach (var shapeData in puzzleShapeDatas)
        {
            if(shapeData.ShapeInfoStr.Contains(str))
            {
                spinToCw = shapeData.ShapeInfoStr.IndexOf(str);//퍼즐 기본 형태에서 회전정도
                img.sprite = shapeData.Sprite;
                MakePainting(-spinToCw * 90);
                break;
            }
        }
        maskTr.Rotate(0, 0, spinToCw * 90);
        RandomRotate();
        count--;
        if(count==0)
        {
            GameManager.Instance.StartPuzzleGame();
        }
    }
    private void RandomRotate()
    {
        extraSpinToCw = UnityEngine.Random.Range(0, 4);
        transform.Rotate(0, 0, extraSpinToCw * -90);
    }
    public void MakePainting(int rotate)
    {
        int boardSize=(GameManager.Instance.boardSize);
        float x = ((boardSize-1) * 0.5f - gridPos.x) * 1000/boardSize;
        float y = ((boardSize-1) * 0.5f - gridPos.y) * 1000 / boardSize;

        if (rotate==90||rotate==-270)
        {
            float temp = x;
            x = -y;
            y = temp;
        }
        else if(rotate==-90||rotate==270)
        {
            float temp = x;
            x = y;
            y = -temp;
        }
        else if(rotate==180||rotate==-180)
        {
            float temp = x;
            x = -x;
            y = -y;
        }
        image.rectTransform.anchoredPosition = new Vector2(x,y );
        image.rectTransform.Rotate(0,0,rotate);
        image.sprite = SpriteStorage.Instance.Sprites[GameManager.Instance.PuzzleId];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isFixed)
            return;
        if(!isInBox)
            rectTr.SetAsLastSibling();
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
        canvasgroup.blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isFixed)
            return;
        if (isInBox)
        {
            ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.dragHandler);//따로 추가 안하면 실행이 안됨
            m_PointerEventData = new PointerEventData(GameManager.Instance.EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            GameManager.Instance.Raycaster.Raycast(m_PointerEventData, results);
            isInBox = false;
            foreach (RaycastResult result in results)
            {
                if(result.gameObject.CompareTag("PiecesBox"))
                {
                    isInBox = true;
                    return;
                }
            }
            FindObjectOfType<PiecesBox>().RemovePieceFromBox(this);
            rectTr.position = Input.mousePosition;
        }
        else
        {
            if(transform.parent.CompareTag("Combine"))
            {
                foreach(Piece piece in transform.parent.GetComponentsInChildren<Piece>())
                {
                    piece.rectTr.anchoredPosition += eventData.delta;
                }
            }
            else
            {
                rectTr.position = Input.mousePosition;
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasgroup.blocksRaycasts = true;
        if(IsFixed)
            return;
        ExecuteEvents.Execute(scrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
        m_PointerEventData = new PointerEventData(GameManager.Instance.EventSystem);
        if (transform.parent.CompareTag("Combine"))
        {
            List<Piece> pieces = transform.parent.GetComponentsInChildren<Piece>().ToList<Piece>();
            
            for (int i=0; i<pieces.Count;i++)
            {
                if(pieces[i].FixOrCombineOrPutInBox())
                {
                    pieces.RemoveAt(i);
                    i = -1;
                }
            }
        }
        else
        {
            FixOrCombineOrPutInBox();
        }
    }
    //그리드에 고정 | 주변4방향을보고 피스들끼리 결합 | 피스박스에 넣기 
    private bool FixOrCombineOrPutInBox()
    {
        if(m_PointerEventData == null)
        {
            m_PointerEventData = new PointerEventData(GameManager.Instance.EventSystem);
        }
        m_PointerEventData.position = rectTr.position;
        List<RaycastResult> results = new List<RaycastResult>();
        GameManager.Instance.Raycaster.Raycast(m_PointerEventData, results);
        bool isIngrid = false;
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Grid"))
            {
                isIngrid = true;
                Grid grid = result.gameObject.GetComponent<Grid>();
                Piece combinablePiece = GetCombinableCwDir();
                if (grid.GridPos == gridPos && extraSpinToCw == 0&& (gridPos.x % (GameManager.Instance.boardSize - 1) == 0 || 
                    gridPos.y % (GameManager.Instance.boardSize - 1) == 0|| combinablePiece != this && combinablePiece.isFixed))
                {
                    FixPieceInGrid(grid);
                    return true;
                }
                else//그리드판 안 but 자기 위치는 No /피스끼리 결합 체크
                {
                    CombinePiece(combinablePiece);
                }
            }
        }
        if (!isIngrid && !transform.parent.CompareTag("Combine"))
            FindObjectOfType<PiecesBox>().PutItInBox(this);
        return false;
    }

    private void CombinePiece(Piece combinablePiece)
    {
        if (combinablePiece != this && !combinablePiece.isFixed)
        {
            Transform combineTr = transform.parent;
            if (!transform.parent.CompareTag("Combine"))
            {
                if (combinablePiece.transform.parent.CompareTag("Combine"))
                    combineTr = combinablePiece.transform.parent;
                else
                {
                    GameObject combineObj = new GameObject();
                    combineObj.name = "combinePieces";
                    combineObj.transform.SetParent(transform.parent);
                    combineObj.tag = "Combine";
                    combineTr = combineObj.transform;
                }
            }
            bool isCombinePieces = false;
            if (combinablePiece.transform.parent.CompareTag("Combine") && combinablePiece.transform.parent != combineTr)
            {
                isCombinePieces = true;
            }
            SoundPlayer.Instance.PlayPieceSound();
            this.transform.SetParent(combineTr);
            combinablePiece.transform.SetParent(combineTr);
            rectTr.position = (Vector2)combinablePiece.rectTr.position +
                (Vector2)GridMaker.CwToVector(extraSpinToCw + GridMaker.VectorToCw(gridPos - combinablePiece.gridPos))
                * 1000f / (GameManager.Instance.boardSize*combineTr.localScale.x);
            if (isCombinePieces)
            {
                combinablePiece.FixOrCombineOrPutInBox();
            }
        }
    }

    public void FixPieceInGrid(Grid grid)
    {
        SoundPlayer.Instance.PlayPieceSound();
        grid.SetPiece(this);
        transform.SetParent(grid.transform);
        rectTr.anchoredPosition = Vector2.zero;
        rectTr.anchorMin = Vector2.one * 0.5f;
        rectTr.anchorMax = Vector2.one * 0.5f;
        isFixed = true;
        ChansePieceSize(1f);
        if (extraSpinToCw>0)
        {
            while(true)
            {
                transform.Rotate(0, 0, -90);
                extraSpinToCw++;
                extraSpinToCw %= 4;
                if (extraSpinToCw == 0)
                    break;
            }
        }
        fixCount--;
        int size = GameManager.Instance.boardSize * GameManager.Instance.boardSize;
        DataManager.Instance.FixPiece(GridPos,((size-fixCount)*100)/size);
        //저장
        if (fixCount == 0)
        {
            Debug.Log("끝");
            CompletePuzzle();
        }
    }

    public void CompletePuzzle()
    {
        FindObjectOfType<rewardUI>().RewardUION();
    }

    private Piece GetCombinableCwDir()
    {
        Piece result = this;
        m_PointerEventData = new PointerEventData(GameManager.Instance.EventSystem);
        
        for(int i=0;i<4;i++)
        {
            m_PointerEventData.position= (Vector2)rectTr.position +(Vector2) GridMaker.CwToVector(i+extraSpinToCw)*1000f/ (float)GameManager.Instance.boardSize*0.5f;
            //Ray ray = Camera.main.ScreenPointToRay(m_PointerEventData.position);
            //Debug.DrawRay(m_PointerEventData.position, ray.direction * 1000f, Color.green, 10f);
            List<RaycastResult> hits = new List<RaycastResult>();
            GameManager.Instance.Raycaster.Raycast(m_PointerEventData, hits);
            foreach (RaycastResult hit in hits)
            {
                if(hit.gameObject.CompareTag("Piece"))
                {
                    Piece piece = hit.gameObject.transform.parent.GetComponent<Piece>();
                    if (piece.extraSpinToCw == extraSpinToCw && piece.GridPos - gridPos == GridMaker.CwToVector(i))
                    {
                        if (transform.parent.CompareTag("Combine") && piece.transform.parent == transform.parent)//같은 Combine자식이면
                            break;
                        result = piece;
                        if (result.isFixed)
                            return result;
                    }
                    
                }
            }
        }
        return result;
    }

  

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isFixed)
            return;
        if(!isInBox)
            rectTr.SetAsLastSibling();
        if (transform.parent.CompareTag("Combine"))
        {

            Piece[] pieces = transform.parent.GetComponentsInChildren<Piece>();

            foreach (Piece piece in pieces)
            {
               
                int y = (piece.gridPos - gridPos).y;
                int x = (piece.gridPos - gridPos).x;
                Vector2 move;
                if (piece.extraSpinToCw % 2 == 0)
                {
                    move = y * new Vector2(1- piece.extraSpinToCw, piece.extraSpinToCw -1)
                   -x* new Vector2(1 - piece.extraSpinToCw, 1 - piece.extraSpinToCw);
                }
                else
                {
                    move = -y * new Vector2(2 - piece.extraSpinToCw, 2 - piece.extraSpinToCw)
                        - x * new Vector2(2 - piece.extraSpinToCw, piece.extraSpinToCw - 2);

                }
                piece.transform.Rotate(0, 0, -90);
                piece.extraSpinToCw++;
                piece.extraSpinToCw %= 4;
                piece.rectTr.anchoredPosition += move * 1000f / (GameManager.Instance.boardSize*transform.parent.localScale.x);    
                piece.FixOrCombineOrPutInBox();
            }
        }
        else
        {
            transform.Rotate(0, 0, -90);
            extraSpinToCw++;
            extraSpinToCw %= 4;
            FixOrCombineOrPutInBox();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canvasgroup.blocksRaycasts = true;
    }
}
