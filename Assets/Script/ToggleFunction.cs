using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFunction : MonoBehaviour
{
    [SerializeField]
    GameObject sideObj;
    [SerializeField]
    GameObject onlyPaintingObj;
    [SerializeField]
    GameObject cheatPaintingObj;
    [SerializeField]
    GameObject piecesBoxObj;
    bool[] toggleBools =new bool[3];
    [SerializeField]
    Color[] colors;
    [SerializeField]
    Image[] toggleImage;
    [SerializeField]
    Button button;
    [SerializeField]
    float cheatTime = 30;
    [SerializeField]
    int cheatCost = 30;
    [SerializeField]
    TextMeshProUGUI gemText; 
    [SerializeField]
    TextMeshProUGUI textEffect;


    private void Start()
    {
        gemText.text = DataManager.Instance.GemNum.ToString();
    }
    private void ToggleInt(int i)
    {
        toggleBools[i] = !toggleBools[i];
        toggleImage[i].color = toggleBools[i] ? colors[1] : colors[0];
    }
    public void ToggleSide()
    {
        ToggleInt(0);
    }
    public void ToggleOnlyPainting()
    {
        ToggleInt(1);
        onlyPaintingObj.SetActive(toggleBools[1]);
        piecesBoxObj.SetActive(!toggleBools[1]);

    }
    public void ToggleCheat()
    {
        StartCoroutine(CheatOn());
        //StartCoroutine(UseGemCor(DataManager.Instance.GemNum, cheatCost));
        StartCoroutine(TextDisapearEffect());
        UseGem();
    }
    IEnumerator CheatOn()
    {
        toggleImage[2].fillAmount = 1;
        cheatPaintingObj.SetActive(true);
        float sec = cheatTime;
        float a = 1f / cheatTime;
        button.enabled = false;
        while(true)
        {
            yield return null;
            sec -= Time.deltaTime;
            toggleImage[2].fillAmount = sec * a;
            if (sec < 0)
            {
                break;
            }
        }
        button.enabled = true;
        cheatPaintingObj.SetActive(false);
    }
    private void UseGem()
    {
        DataManager.Instance.UseGem(cheatCost);
        gemText.text = (DataManager.Instance.GemNum).ToString();
    }
    IEnumerator UseGemCor(int iNum,int cost)
    {
        DataManager.Instance.UseGem(cost);
        int gemNum = iNum;
        float delTime = 2 / cost;
        while(true)
        {
            yield return new WaitForSeconds(delTime);
            gemNum--;
            gemText.text = (gemNum).ToString();
            if (gemNum == iNum - cost)
            {
                break;
            }
        }
    }
    IEnumerator TextDisapearEffect()
    {
        textEffect.color = new Color(0, 0, 0, 1);
        float time = 1f;
        float delTime = 0.05f;
        int i = 0;
        while (true)
        {
            i++;
            yield return new WaitForSeconds(delTime);
            time-=delTime;
            textEffect.color = new Color(0, 0, 0, 1 - delTime * i);
            if (time<0)
            {
                break;
            }
        }
        textEffect.color = new Color(0, 0, 0, 0);
    }
}
