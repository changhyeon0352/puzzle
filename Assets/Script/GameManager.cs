using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    GraphicRaycaster m_Raycaster;
    [SerializeField]
    private int puzzleId;
    public int PuzzleId { get { return puzzleId; } }
    public int ongoingStep=0;
    public GraphicRaycaster Raycaster { get 
        { if (m_Raycaster == null)
            m_Raycaster=FindObjectOfType<GraphicRaycaster>();
            
            return m_Raycaster; } }
    EventSystem m_EventSystem;
    public EventSystem EventSystem { get { return m_EventSystem; } }    

    [SerializeField]
    Transform puzzleBoardTr;
    [SerializeField]
    GridMaker gridMaker;
    //public GridMaker GridMaker { get => gridMaker; }
    //public Transform PuzzleBoardTr { get { return puzzleBoardTr; } }
    public int boardSize;
    protected override void Awake()
    {
        base.Awake();
        
    }
    private void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();

        //Fetch the Event System from the Scene
        m_EventSystem = FindObjectOfType<EventSystem>();
    }
    public void StartPuzzleGame()
    {
        FindObjectOfType<PiecesBox>().PutAllPiecesInBox();
    }
    public void SetPuzzleId(int id)
    {
        puzzleId = id;
    }

    public void MoveScene(int iScene)
    {
        SceneManager.LoadScene(iScene);
    }
    public void ResetBoardSize()
    {
        boardSize = 6;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
