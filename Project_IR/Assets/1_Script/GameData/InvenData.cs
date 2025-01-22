using BackEnd;
using CodeStage.AntiCheat.ObscuredTypes;
using LitJson;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class SkillData {
    public ObscuredInt Key;
}
public class SkillDataReq {
    public int Key;
}

public class InvenData : GameData {
    private Dictionary<ObscuredInt, SkillData> _skill_Inven = new();
    public Dictionary<ObscuredInt, SkillData> Skill_Inven => _skill_Inven;

    public override string GetColumnName() {
        return null;
    }

    public override Param GetParam() {
        // 뒤끝은 안티치트를 지원하지 않아, ObscuredInt를 int로 변환하여 저장
        Param param = new Param();
        Dictionary<int, SkillDataReq> Skill_InvenReq = new Dictionary<int, SkillDataReq>();

        foreach (var item in _skill_Inven) {
            SkillDataReq skillDataReq = new SkillDataReq {
                Key = item.Key
            };
            Skill_InvenReq[skillDataReq.Key] = skillDataReq;
        }

        param.Add("Skill_Inven", JsonMapper.ToJson(Skill_InvenReq));
        return param;
    }

    public override string GetTableName() {
        return "InvenData";
    }

    public override void InitializeData() { // 최초 생성
        IsChangedData = true;
        _skill_Inven = new Dictionary<ObscuredInt, SkillData> {
            [1000] = new SkillData()
        };
    }

    public override void LocalLoad() {

    }

    public override void LocalSave() {

    }

    protected override void SetServerDataToLocal(JsonData gameDataJson) {
        IsChangedData = false;
        SetSkillDataToLocal(gameDataJson);
    }

    private void SetSkillDataToLocal(JsonData gameDataJson) {
        Dictionary<int, SkillDataReq> Skill_InvenAns = JsonConvert.DeserializeObject<Dictionary<int, SkillDataReq>>(gameDataJson["Skill_Inven"].ToString());
        // 기존 _skill_Inven에서 Skill_InvenAns에 없는 키를 저장 (서버정보와 로컬정보를 비교하여 삭제할 키를 저장)
        var keysToRemove = new List<ObscuredInt>();
        foreach (var key in _skill_Inven.Keys) {
            if (!Skill_InvenAns.ContainsKey(key)) {
                keysToRemove.Add(key);
            }
        }
        // _skill_Inven에서 Skill_InvenAns에 없는 키를 삭제
        foreach (var key in keysToRemove) {
            _skill_Inven.Remove(key);
        }
        // Skill_InvenAns의 데이터를 _skill_Inven에 추가
        foreach (var item in Skill_InvenAns) {
            _skill_Inven[item.Key] = new SkillData { Key = item.Key };
        }
        foreach (var item in _skill_Inven) {
            Debug.Log($"Skill_Inven Key : {item.Key}");
        }
    }
}
