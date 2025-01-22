using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public struct StatData
{
    public ObscuredBigInteger Str;       // 힘
    public ObscuredBigInteger Dex;      // 민
    public ObscuredBigInteger Int;       // 지
    public ObscuredBigInteger Vit;       // 체
}

public struct  StatDataReq // 스텟 서버 저장용
{
    public string Str;
    public string Dex;
    public string Int;
    public string Vit;
}

public class UserData : GameData
{
    // 유저 레벨
    private ObscuredInt _userLevel = 1;
    public ObscuredInt UserLevel => _userLevel;

    // 유저 경험치
    private ObscuredBigInteger _userExp = 0;
    public ObscuredBigInteger UserExp => _userExp;

    // 유저 스텟
    private StatData _userStat;
    public StatData UserStat => _userStat;

    // 스킬 장착 정보 5칸 스킬 키 값만 저장
    private ObscuredInt[] _eqSkills = new ObscuredInt[5];
    public ObscuredInt[] EqSkills => _eqSkills;

    private Dictionary<string,int> _stageData = new Dictionary<string, int>();
    public Dictionary<string, int> StageData => _stageData;

    public override string GetColumnName()
    {
        return null;
    }

    public override Param GetParam()
    {
        Param param = new Param();

        param.Add("StageData", JsonMapper.ToJson(_stageData)); // 스테이지 정보

        param.Add("UserLevel", (int)_userLevel); // 레벨
        param.Add("UserExp", _userExp.ToString()); // 경험치
        
        StatDataReq StatDataReq; // 스텟 서버 저장용
        StatDataReq.Str = _userStat.Str.ToString();
        StatDataReq.Dex = _userStat.Dex.ToString();
        StatDataReq.Int = _userStat.Int.ToString();
        StatDataReq.Vit = _userStat.Vit.ToString();
        param.Add("UserStat", StatDataReq);

        // 스킬 장착정보
        int[] EqSkillReq = {
            _eqSkills[0], _eqSkills[1], _eqSkills[2], _eqSkills[3], _eqSkills[4] 
        };
        param.Add("EqSkills", JsonMapper.ToJson(EqSkillReq));

        return param;
    }

    public override string GetTableName()
    {
        return "UserData";
    }

    // 최초 생성
    public override void InitializeData()
    {
        IsChangedData = true;
        // 초기 스테이지 정보
        _stageData["stage"] = 1;
        _stageData["wave"] = 1;
        _stageData["maxStage"] = 1;

        _userLevel = 1; 
        _userExp = 0;
        _userStat = new StatData
        {
            Str = 5,
            Dex = 5,
            Int = 5,
            Vit = 5
        };

        _eqSkills = new ObscuredInt[] { // 스킬 장착 기본공격 키값 1000
            1000, 0, 0, 0, 0
        };
    }

    public override void LocalLoad()
    {
        
    }

    public override void LocalSave()
    {
       
    }

    protected override void SetServerDataToLocal(JsonData gameDataJson)
    {
        //스테이지 정보 가져오기
        _stageData = JsonMapper.ToObject<Dictionary<string, int>>(gameDataJson["StageData"].ToString());

        // 유저 정보 가져오기
        _userLevel = int.Parse(gameDataJson["UserLevel"].ToString());
        _userExp = BigInteger.Parse(gameDataJson["UserExp"].ToString());
        _userStat = new StatData
        {
            Str = BigInteger.Parse(gameDataJson["UserStat"]["Str"].ToString()),
            Dex = BigInteger.Parse(gameDataJson["UserStat"]["Dex"].ToString()),
            Int = BigInteger.Parse(gameDataJson["UserStat"]["Int"].ToString()),
            Vit = BigInteger.Parse(gameDataJson["UserStat"]["Vit"].ToString())
        };
        // 스킬 장착정보 가져오기
        int[] eqSkills = JsonMapper.ToObject<int[]>(gameDataJson["EqSkills"].ToString());
        for (int i = 0; i < _eqSkills.Length; i++) {
            _eqSkills[i] = eqSkills[i];
        }
    }
}
