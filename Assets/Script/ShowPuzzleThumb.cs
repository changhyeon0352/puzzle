using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ShowPuzzleThumb : MonoBehaviour
{
    [SerializeField]
    GameObject thumbPrefab;
    
    private void Start()
    {
        PuzzleThumb[] puzzleThumbs = new PuzzleThumb[SpriteStorage.Instance.Sprites.Length];
        for (int i = 0; i < SpriteStorage.Instance.Sprites.Length; i++)
        {
            puzzleThumbs[i] = Instantiate(thumbPrefab, transform).GetComponent<PuzzleThumb>();
            puzzleThumbs[i].PuzzleSprite= SpriteStorage.Instance.Sprites[i];
            puzzleThumbs[i].puzzleId = i;
        }
        for(int i=0;i<DataManager.Instance.PuzzleDataList.Count;i++)
        {
            puzzleThumbs[DataManager.Instance.PuzzleDataList[i].puzzleId].SetProgress(DataManager.Instance.PuzzleDataList[i].progressRate);

        }
    }
}
