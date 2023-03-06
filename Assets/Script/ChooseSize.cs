using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class ChooseSize : MonoBehaviour
{
    [SerializeField] HorizontalScrollSnap scrollsnap;
    
    [SerializeField] Color[] colors;
    PiecesNum[] piecesNums;
    bool isChange=false;
    void Awake()
    {
        //scrollsnap.OnSelectionChangeStartEvent.AddListener(OnPageChangeStart);
        //scrollsnap.OnSelectionPageChangedEvent.AddListener(OnPageChanged);
        scrollsnap.OnSelectionChangeEndEvent.AddListener(OnPageChangeEnd);
        
        piecesNums = new PiecesNum[transform.childCount];
        for(int i=0;i< piecesNums.Length; i++)
        {
            piecesNums[i] = transform.GetChild(i).GetComponent<PiecesNum>();
        }
        
    }

    void OnPageChangeStart()
    {
        isChange = true;
    }
    void OnPageChanged(int index)
    {
        Debug.Log("CHanged");
        for (int i = 0; i < piecesNums.Length; i++)
        {
            if (i == index)
                piecesNums[i].image.color = colors[1];
            else
                piecesNums[i].image.color = colors[0];
        }
        //isChange=false;
    }

    void OnPageChangeEnd(int index)
    {
        Debug.Log("End");
        
        
        for (int i = 0; i < piecesNums.Length; i++)
        {
            if (i == index)
            {
                piecesNums[i].image.color = colors[1];
                GameManager.Instance.boardSize = (int)Mathf.Sqrt(piecesNums[i].iPiecesNum);
                GameManager.Instance.ongoingStep = i;
            }
                
            else
                piecesNums[i].image.color = colors[0];
            
        }
        
    }
}
