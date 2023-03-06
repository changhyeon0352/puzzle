using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PiecesNum : MonoBehaviour
{
    public Image image;

    [SerializeField]
    int piecesNum;
    [SerializeField]
    GameObject rewardInfoObj;
    [SerializeField]
    GameObject clearMessageObj;
    public int iPiecesNum { get { return piecesNum; } }
    
    private void Start()
    {
        GetComponentInChildren<Text>().text = piecesNum.ToString();
        
    }
    public void IsClear(bool isClear)
    {
        clearMessageObj.SetActive(isClear);
        rewardInfoObj.SetActive(!isClear);
    }
}
