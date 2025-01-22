using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public struct StatData
{
    public ObscuredBigInteger Str;       // ��
    public ObscuredBigInteger Dex;      // ��
    public ObscuredBigInteger Int;       // ��
    public ObscuredBigInteger Vit;       // ü
}

public struct  StatDataReq // ���� ���� �����
{
    public string Str;
    public string Dex;
    public string Int;
    public string Vit;
}

public class UserData : GameData
{
    // ���� ����
    private ObscuredInt _userLevel = 1;
    public ObscuredInt UserLevel => _userLevel;

    // ���� ����ġ
    private ObscuredBigInteger _userExp = 0;
    public ObscuredBigInteger UserExp => _userExp;

    // ���� ����
    private StatData _userStat;
    public StatData UserStat => _userStat;

    // ��ų ���� ���� 5ĭ ��ų Ű ���� ����
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

        param.Add("StageData", JsonMapper.ToJson(_stageData)); // �������� ����

        param.Add("UserLevel", (int)_userLevel); // ����
        param.Add("UserExp", _userExp.ToString()); // ����ġ
        
        StatDataReq StatDataReq; // ���� ���� �����
        StatDataReq.Str = _userStat.Str.ToString();
        StatDataReq.Dex = _userStat.Dex.ToString();
        StatDataReq.Int = _userStat.Int.ToString();
        StatDataReq.Vit = _userStat.Vit.ToString();
        param.Add("UserStat", StatDataReq);

        // ��ų ��������
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

    // ���� ����
    public override void InitializeData()
    {
        IsChangedData = true;
        // �ʱ� �������� ����
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

        _eqSkills = new ObscuredInt[] { // ��ų ���� �⺻���� Ű�� 1000
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
        //�������� ���� ��������
        _stageData = JsonMapper.ToObject<Dictionary<string, int>>(gameDataJson["StageData"].ToString());

        // ���� ���� ��������
        _userLevel = int.Parse(gameDataJson["UserLevel"].ToString());
        _userExp = BigInteger.Parse(gameDataJson["UserExp"].ToString());
        _userStat = new StatData
        {
            Str = BigInteger.Parse(gameDataJson["UserStat"]["Str"].ToString()),
            Dex = BigInteger.Parse(gameDataJson["UserStat"]["Dex"].ToString()),
            Int = BigInteger.Parse(gameDataJson["UserStat"]["Int"].ToString()),
            Vit = BigInteger.Parse(gameDataJson["UserStat"]["Vit"].ToString())
        };
        // ��ų �������� ��������
        int[] eqSkills = JsonMapper.ToObject<int[]>(gameDataJson["EqSkills"].ToString());
        for (int i = 0; i < _eqSkills.Length; i++) {
            _eqSkills[i] = eqSkills[i];
        }
    }
}
