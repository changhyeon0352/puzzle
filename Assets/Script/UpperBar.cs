using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpperBar : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI gemText;
    private void Start()
    {
        gemText.text = DataManager.Instance.GemNum.ToString();

        
    }
}
