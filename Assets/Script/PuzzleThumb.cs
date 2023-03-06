using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleThumb : MonoBehaviour, IPointerClickHandler
{
    public
    int puzzleId = 0;
    Sprite puzzleSprite;
    [SerializeField]
    Image image;
    [SerializeField]
    Text text;
    [SerializeField]
    GameObject progressingObj;
    int progress;
    bool isProgressing=false;
    
    public Sprite PuzzleSprite { set { puzzleSprite = value; image.sprite = value; } get { return puzzleSprite; } }


    public void SetProgress(int progressNum)
    {
        progressingObj.SetActive(true);
        progress =progressNum;
        isProgressing=true;
        text.text = $"{progress}%";
        if(progress==100)
        {
            isProgressing = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SetPuzzleId(puzzleId);
        if (isProgressing)
        {
            GameManager.Instance.boardSize = DataManager.Instance.GetPuzzleData().boardSize;
            Transform tr = transform.root.Find("ChooseContinue");
            tr.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            GameManager.Instance.ResetBoardSize();
            Transform tr = transform.root.Find("puzzleSettingUI");
            tr.GetChild(0).gameObject.SetActive(true);
           
        }
    }
}
