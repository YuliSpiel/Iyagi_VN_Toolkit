using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// CSV 한 행을 헤더명→값으로 보존하는 데이터 컨테이너.
/// - 모든 컬럼 문자열 그대로 유지(파서가 양끝 큰따옴표 제거/""→" 복원)
/// - 대소문자 무시 조회 인덱스 제공
/// - 다국어 본문/선택지/화자명 헬퍼 제공
/// </summary>
[Serializable]
public class DialogueRecord
{
    public Dictionary<string, string> Fields = new Dictionary<string, string>();
    private Dictionary<string, string> _ci = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>대소문자 무시 인덱스 재구축</summary>
    public void FinalizeIndex()
    {
        _ci.Clear();
        foreach (var kv in Fields)
        {
            var key = kv.Key?.Trim() ?? string.Empty;
            if (key.Length == 0) continue;
            if (!_ci.ContainsKey(key)) _ci[key] = kv.Value ?? string.Empty;
        }
    }

    public string GetRaw(string key)
        => (key != null && Fields.TryGetValue(key, out var v)) ? (v ?? string.Empty) : string.Empty;

    public string Get(string key)
        => (key != null && _ci.TryGetValue(key, out var v)) ? (v ?? string.Empty) : string.Empty;

    /// <summary>여러 후보 키를 순서대로 조회해 처음 나오는 비어있지 않은 값을 반환</summary>
    public string GetFirst(params string[] keys)
    {
        if (keys == null) return string.Empty;
        foreach (var k in keys)
        {
            var v = Get(k);
            if (!string.IsNullOrEmpty(v)) return v;
        }
        return string.Empty;
    }

    public bool Has(string key) => key != null && _ci.ContainsKey(key);

    public bool TryGetInt(string key, out int value) => int.TryParse(Get(key), out value);
    public bool TryGetFloat(string key, out float v) => float.TryParse(Get(key), out v);
    public bool TryGetBool(string key, out bool v)
    {
        var s = Get(key)?.Trim().ToLowerInvariant();
        if (s == "true" || s == "1" || s == "yes" || s == "y") { v = true; return true; }
        if (s == "false"|| s == "0" || s == "no"  || s == "n") { v = false; return true; }
        v = false; return false;
    }

    // ───────────────────────────── 텍스트/선택지/화자명 ─────────────────────────────

    /// <summary>본문 텍스트(언어별) 폴백 포함</summary>
    public string GetTextByLang(GameLanguage lang)
    {
        switch (lang)
        {
            case GameLanguage.English: return FirstNonEmpty(Get("ENG"), Get("EN"), Get("Text_EN"), Get("Text_ENG"));
            case GameLanguage.Japanese: return FirstNonEmpty(Get("JPN"), Get("JP"), Get("Text_JP"), Get("Text_JPN"));
            default:   return FirstNonEmpty(Get("KR"),  Get("KO"), Get("Text_KR"));
        }
    }

    /// <summary>선택지 텍스트(언어별) 폴백 포함</summary>
    public string GetChoiceText(int idx, GameLanguage lang)
    {
        string baseKey = $"Choice{idx}";
        switch (lang)
        {
            case GameLanguage.English: return FirstNonEmpty(Get($"{baseKey}_ENG"), Get($"{baseKey}_EN"), Get(baseKey));
            case GameLanguage.Japanese: return FirstNonEmpty(Get($"{baseKey}_JPN"), Get($"{baseKey}_JP"), Get(baseKey));
            default:   return FirstNonEmpty(Get($"{baseKey}_KR"),  Get($"{baseKey}_KO"), Get(baseKey));
        }
    }

    /// <summary>
    /// 화자명(네임태그) 조회. 언어별 컬럼 우선 → 범용 컬럼 → 캐릭터명 폴백.
    /// 시트가 Speaker/Name/Nametag 등 어떤 표기를 써도 대응.
    /// </summary>
    public string GetSpeakerByLang()
    {
        // Speaker 컬럼 값(예: "sasha", "narrator")
        string key = GetFirst("NameTag").Trim().ToLowerInvariant();

        if (string.IsNullOrEmpty(key))
            return "";
        
        // 현재 게임 언어 가져오기
        var lang = GameManager.Instance.CurrentLanguage;

        // 하드코딩된 스피커 이름 매핑
        switch (key)
        {
            case "sasha":
                return lang switch
                {
                    GameLanguage.Korean  => "사샤",
                    GameLanguage.English => "Sasha",
                    GameLanguage.Japanese=> "サーシャ",
                    _ => "Sasha"
                };
            
            case "edan":
                return lang switch
                {
                    GameLanguage.Korean  => "에단",
                    GameLanguage.English => "Edan",
                    GameLanguage.Japanese=> "エダン",
                    _ => "Edan"
                };
            
            case "sion":
                return lang switch
                {
                    GameLanguage.Korean  => "시온",
                    GameLanguage.English => "Sion",
                    GameLanguage.Japanese=> "シオン",
                    _ => "Sion"
                };

            case "narrator":
            case "system":
                return lang switch
                {
                    GameLanguage.Korean  => "나레이터",
                    GameLanguage.English => "Narrator",
                    GameLanguage.Japanese=> "ナレーター",
                    _ => "Narrator"
                };

            default:
                // 매핑 안 된 스피커는 그대로 키 반환
                return key;
        }
    }

    // ───────────────────────────── 유틸 ─────────────────────────────

    private string FirstNonEmpty(params string[] arr)
        => arr?.FirstOrDefault(s => !string.IsNullOrEmpty(s)) ?? string.Empty;

    // /// <summary>입력 언어코드를 표준화(KR/EN/JP/DE)</summary>
    // private string NormLang(string lang)
    // {
    //     var L = (lang ?? "KR").Trim().ToUpperInvariant();
    //     if (L is "KO") return "KR";
    //     if (L is "EN" or "ENG") return "EN";
    //     if (L is "JP" or "JPN") return "JP";
    //     if (L is "DE" or "GER") return "DE";
    //     return (L.Length == 0) ? "KR" : L;
    // }

    /// <summary>표준 언어코드에 대한 별칭 목록 반환. 우선순위 높은 순.</summary>
    private IReadOnlyList<string> LangAliases(string std)
    {
        switch (std)
        {
            case "EN": return _en;
            case "JP": return _jp;
            case "DE": return _de;
            default:   return _kr;
        }
    }

    private static readonly string[] _kr = { "KR", "KO" };
    private static readonly string[] _en = { "ENG", "EN" };
    private static readonly string[] _jp = { "JPN", "JP" };
    private static readonly string[] _de = { "GER", "DE" };

    public IReadOnlyDictionary<string, string> All => Fields;
}
