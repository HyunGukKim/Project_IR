using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using System.Collections.Generic;
using UnityEngine;

public class SkillFuncBase {
    public ActionType ActionType;
    public ObscuredInt Key;
    public ObscuredFloat Value0;
    public ObscuredFloat Value1;
    public ObscuredFloat Value2;
    public ObscuredFloat Value3;
    public ObscuredFloat Value4;
    public ObscuredFloat Value5;

    public ObscuredInt UseTurn; // 사용 가능 턴 (0이면 사용가능)
    public ObscuredInt ConditionTurn; // 상태 이상 지속 턴 (0이면 상태이상 없음)

    public SkillUseType eSkillUseType = SkillUseType.Use; // 스킬 사용 타입 (타입에 따라 해당하는 턴 감소)

    public ObscuredFloat ActivePercent;

    public virtual void Create(int key) // 스킬 생성
    {
        Key = key;
        ActionType = t_SkillData.Data.DataMap[key].ActionType;
        Value0 = t_SkillData.Data.DataMap[key].value0;
        Value1 = t_SkillData.Data.DataMap[key].value1;
        Value2 = t_SkillData.Data.DataMap[key].value2;
        Value3 = t_SkillData.Data.DataMap[key].value3;
        Value4 = t_SkillData.Data.DataMap[key].value4;
        Value5 = t_SkillData.Data.DataMap[key].value5;

    }

    public virtual void Remove() { } // 스킬 제거
    public virtual void Use(List<UnitData> attackers,int atkIndex , List<UnitData> defenders, List<ActionLog> actionLogs) { } // 스킬 사용
    public virtual void Turn() {
        if (eSkillUseType == SkillUseType.Use) {
            // 턴차감
            if (UseTurn > 0) {
                UseTurn--;
            }
            return;
        }
        if (ConditionTurn > 0) {
            ConditionTurn--;
            if (ConditionTurn == 0) {
                Remove();
            }
        }

    } // 턴 차감

    #region 스킬이 사용된 대상에게 버프및 디버프를 적용할 경우 사용
    public virtual void Buff(List<UnitData> my, int myIndex, List<UnitData> units, List<ActionLog> actionLogs) { } // 버프 사용
    public virtual void DeBuff(List<UnitData> my, int myIndex, List<UnitData> units, List<ActionLog> actionLogs) { } // 디버프 사용
    #endregion

}
// 스킬 사용 타입
public enum SkillUseType {
    Use = 0,
    Buff = 1,
    DeBuff = 2,
}
