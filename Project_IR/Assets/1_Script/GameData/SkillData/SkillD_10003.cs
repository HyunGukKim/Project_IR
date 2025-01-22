using System;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// 공격력 강화 : 최종 대미지 1.2배 상승
/// </summary>
public class SkillD_10003 : SkillFuncBase {
    List<UnitData> _attackers;
    int _atkIndex;
    List<UnitData> _defenders;

    public override void Use(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        // 버프 생성
        if (attackers[atkIndex].ConditionDic.ContainsKey(10003) == true) { // 버프가 이미 걸려있으면 턴만 갱신
            attackers[atkIndex].ConditionDic[10003].ConditionTurn = 2;
            ActionLog log = new ActionLog();
            log.ActionType = ActionType.Buff;
            log.ObjIndex = atkIndex;
            log.ActiveObj = attackers[atkIndex].ActiveObj;
            log.ObjHP = attackers[atkIndex].FinalHp;
            log.ObjCurHp = attackers[atkIndex].CurHP;
            actionLogs.Add(log);
            return;
        }

        // 버프 안걸려있으면, 버프 생성
        SkillFuncBase buff_10003 = new SkillD_10003();
        buff_10003.Create(10003);
        buff_10003.eSkillUseType = SkillUseType.Buff;
        buff_10003.ConditionTurn = 2;
        // 즉발성이므로 바로 버프 사용
        buff_10003.Buff(attackers, atkIndex, defenders, actionLogs);

        attackers[atkIndex].ConditionDic[10003] = buff_10003; // 버프 딕셔너리에 추가
    }

    public override void Buff(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        _attackers = attackers;
        _atkIndex = atkIndex;

        // 버프 걸어줌
        _attackers[atkIndex].AddFinalDmgPer *= t_SkillData.Data.DataMap[10003].value0;
        ActionLog log = new ActionLog();
        log.ActionType = ActionType.Buff;
        log.ObjIndex = atkIndex;
        log.ActiveObj = _attackers[atkIndex].ActiveObj;
        log.ObjHP = _attackers[atkIndex].FinalHp;
        log.ObjCurHp = _attackers[atkIndex].CurHP;
        actionLogs.Add(log);
    }

    // 버프 해제
    public override void Remove() {
        _attackers[_atkIndex].AddFinalDmgPer /= t_SkillData.Data.DataMap[10003].value0;
        _attackers[_atkIndex].ConditionDic[10003] = null;
        _attackers[_atkIndex].ConditionDic.Remove(10003);
    }
}
