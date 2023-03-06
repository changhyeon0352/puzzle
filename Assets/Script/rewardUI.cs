using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class rewardUI : MonoBehaviour
{
    [SerializeField]
    Button button;
    [SerializeField]
    TextMeshProUGUI gemText;

    private void Start()
    {
        
    }
    public void RewardUION()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        PuzzleData data = DataManager.Instance.GetPuzzleData();
        int rewardGem = DataManager.Instance.RewadByStep[GameManager.Instance.ongoingStep];
        button.onClick.AddListener(() => DataManager.Instance.GetGem(rewardGem));
        button.onClick.AddListener(() => GameManager.Instance.MoveScene(0));
        gemText.text = $"+{rewardGem}";

    }
}
