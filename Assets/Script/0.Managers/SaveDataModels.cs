using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// 데이터 저장과 관련된 클래스들입니다

[System.Serializable]
// 주인공의 능력치 6종류. 초기 값은 각각 2~4이나, 경험치를 이용해 최대 8까지 스텟을 올릴 수 있다.
public class PlayerStats
{
    // private int _yeom;
    // public int Yeom
    // {
    //     get { return _yeom; }
    //     set
    //     {
    //         if (value > 8)
    //         {
    //             throw new ArgumentOutOfRangeException("value", "value must be between 0 and 8");
    //             value = 8;
    //         }
    //
    //         else
    //         {
    //             _yeom = value;
    //         }
    //     }
    // }
    public int Yeom;
    public int Sinche;
    public int Jineung;
    public int Insung;
    public int Jikkam;
    public int Luck;
    
    public int Sanity;
    public int EXP;
}

[System.Serializable]
// ToDo: 챕터관리 어떻게 할지 상의 필요함. 먼저, 챕터와 타이틀을 딕셔너리로 매핑할건지, enum을 사용할건지 결정이 필요
public class ChapterProgress
{
    public int CurChapter; // 현재 챕터 (1~11)
    public int CurIndex; // 현재 장(?)
}

[CreateAssetMenu(fileName = "ChapterThumbnails", menuName = "GameData/ChapterThumbnails")]
public class ChapterThumbnails : ScriptableObject
{
    public List<Sprite> thumbnailList;
}

// [System.Serializable]
// public static class ChapterInfo
// {
//     public static Dictionary<int, string> Chapters = new Dictionary<int, string>()
//     {
//         { 1, "빨간 딸기" },
//         { 2, "노란 꽃" },
//         { 3, "주황 당근" },
//         { 4, "보라 하트" },
//         { 5, "파란 고래" },
//         { 6, "초록 개구리"},
//         { 7, "빨간 사과" },
//         { 8, "노란 병아리" },
//         { 9, "보라 버섯" },
//         { 10, "주황 사탕" },
//         { 11, "초록 잎" },
//         { 12, "노란 별"},
//     };
//     
//     public static Dictionary<int, Sprite> ChapterImgs = new Dictionary<int, Sprite>();
//
//     public static void ImgsInit(ChapterThumbnails thumbnails)
//     {
//         for (int i = 0; i<thumbnails.thumbnailList.Count; i++)
//         {
//             ChapterImgs[i]  = thumbnails.thumbnailList[i];
//         }
//     }
// }

[System.Serializable]
// 저장할 데이터 블록입니다.
// 구조 변경시 GameManager의 Initialize(), ToSaveDataBlock()도 함께 수정 바랍니다.
public class SaveDataBlock 
{
    public PlayerStats Stats = new PlayerStats(); // 에러 방지용 초기화
    public ChapterProgress Chapter = new ChapterProgress(); // 에러 방지용 초기화
    // public int index; // 현재 대사 인덱스
    public string SaveTime; // "yyyy.MM.dd HH:mm"
}

[System.Serializable]
// 세이브 데이터 블록의 리스트
// JsonUtility.ToJson()은 클래스 하나만 JSON의 루트 객체로 직렬화할 수 있기 때문에 별개의 클래스로관리
public class SaveData
{
    public List<SaveDataBlock> Blocks = new List<SaveDataBlock>();
}