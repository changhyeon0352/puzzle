using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GridData
{
    public int[] shapeInfo;
    public bool isFixed;
}
public class PropertyData
{
    public int gemNum=200;
}
public enum DataType { puzzleData=0,property}

[Serializable]
public class PuzzleData
{
    public int puzzleId;
    public int boardSize;
    public int ongoingStep;
    public int progressRate;
    public GridData[] gridDatas;
    public PuzzleData(int progressRate, GridData[] gridDatas)
    {
        this.puzzleId = GameManager.Instance.PuzzleId;
        this.boardSize = GameManager.Instance.boardSize;
        this.ongoingStep = GameManager.Instance.ongoingStep;
        this.progressRate = progressRate;
        this.gridDatas = gridDatas;
        
    }
    public PuzzleData()
    {
        this.puzzleId = -1;
        this.boardSize = 0;
        this.ongoingStep = 0;
        this.progressRate = 0;
        this.gridDatas = gridDatas;
    }

}


[Serializable]
public class PuzzleDatas
{
    public List<PuzzleData> m_List=new List<PuzzleData>();
}
public class DataManager : Singleton<DataManager>
{
    public int GemNum { get => propertyData.gemNum; }
    int[] rewardByStep = { 50, 110, 180, 260, 350,450 };
    public int[] RewadByStep { get => rewardByStep; }
    string dataPath;
    PuzzleDatas puzzledatas;
    PropertyData propertyData;
    public List<PuzzleData> PuzzleDataList { get => puzzledatas.m_List; }
    override protected void Awake()
    {
        base.Awake();
        dataPath = Path.Combine(Application.streamingAssetsPath, "Data");
        //dataPath = Application.persistentDataPath;
        //LoadPuzzleDataFromJson();
        LoadFromJson(DataType.puzzleData);
       
        LoadFromJson(DataType.property);
    }

    public void SaveToJson(DataType dataType)
    {
        object data = null;
        string fileName = "";
        switch (dataType) {
            case DataType.puzzleData:
                data = puzzledatas;
                fileName = "puzzleDataList.json";
                break;
            case DataType.property:
                data = propertyData;
                fileName = "propertyData.json";
                break;
        }
        string jsonData = JsonUtility.ToJson(data, true);
        string path = Path.Combine(dataPath, fileName);
        File.WriteAllText(path, jsonData);
    }
    
    public void AddPuzzleData(PuzzleData puzzleData)
    {
        puzzledatas.m_List.Add(puzzleData);
        SaveToJson(DataType.puzzleData);
    }
    public void RemovePuzzleData()
    {
        PuzzleData data = GetPuzzleData();
        puzzledatas.m_List.Remove(data);
        SaveToJson(DataType.puzzleData);
    }
    
    public void LoadFromJson(DataType dataType)
    {
        object data = null;
        string fileName = "";
        switch (dataType)
        {
            case DataType.puzzleData:
                data = new PuzzleDatas();
                fileName = "puzzleDataList.json";
                break;
            case DataType.property:
                data = new PropertyData();
                fileName = "propertyData.json";
                break;
        }
        string path = Path.Combine(dataPath, fileName);
        try
        {
            string jsonData = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(jsonData, data);
        }
        catch
        {
            switch (dataType)
            {
                case DataType.puzzleData:
                    data=new PuzzleDatas();
                    break;
                case DataType.property:
                    data=new PropertyData();
                    break;
            }
        }
        
        switch (dataType)
        {
            case DataType.puzzleData:
                puzzledatas = (PuzzleDatas)data;
                break;
            case DataType.property:
                propertyData = (PropertyData)data;
                break;
        }
    }
    public void LoadPuzzleDataFromJson()
    {
        string fileName = "puzzleDataList.json";
        string path = Path.Combine(dataPath, fileName);
        string jsonData = File.ReadAllText(path);
        puzzledatas = JsonUtility.FromJson<PuzzleDatas>(jsonData);
    }
    public PuzzleData GetPuzzleData()
    {
        LoadFromJson(DataType.puzzleData);
        if(puzzledatas == null)
        {
            puzzledatas = new PuzzleDatas();
            puzzledatas.m_List = new List<PuzzleData>();
            
        }
        PuzzleData data = new PuzzleData();
        for (int i=0;i<puzzledatas.m_List.Count;i++)
        {
            if(puzzledatas.m_List[i].puzzleId==GameManager.Instance.PuzzleId)
            {
                data=puzzledatas.m_List[i];
                break;
            }
        }
        return data;

    }
    public void FixPiece(Vector2Int gridPos,int progress)
    {
        PuzzleData data = GetPuzzleData();
        data.gridDatas[gridPos.x+gridPos.y * GameManager.Instance.boardSize].isFixed=true;
        data.progressRate = progress;
        SaveToJson(DataType.puzzleData);
    }
    public void GetGem(int gemNum)
    {
        propertyData.gemNum += gemNum;
        SaveToJson(DataType.property);
    }
    public void UseGem(int gemUsing)
    {
        propertyData.gemNum-=gemUsing;
        SaveToJson(DataType.property);
    }
}
