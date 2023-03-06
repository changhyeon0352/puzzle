using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingPieces : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pieces;
    int a;
    private void Start()
    {
         a= 0;

    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SetPieceToGrid(a, 0, pieces[0]);
            a++;
        }
    }
    public void SetPieceToGrid(Vector2Int gridPos,GameObject pieceObj)
    {
        Vector2 v2=GridUtils.FromGrid(gridPos); 
        GameObject obj=Instantiate(pieceObj, transform);
        obj.GetComponent<RectTransform>().localPosition = v2;
    }
    public void SetPieceToGrid(int x,int y, GameObject pieceObj)
    {
        SetPieceToGrid(new Vector2Int(x, y), pieceObj);
    }
}
