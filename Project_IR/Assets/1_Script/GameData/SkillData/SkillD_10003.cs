using System;
using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// ���ݷ� ��ȭ : ���� ����� 1.2�� ���
/// </summary>
public class SkillD_10003 : SkillFuncBase {
    List<UnitData> _attackers;
    int _atkIndex;
    List<UnitData> _defenders;

    public override void Use(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        // ���� ����
        if (attackers[atkIndex].ConditionDic.ContainsKey(10003) == true) { // ������ �̹� �ɷ������� �ϸ� ����
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

        // ���� �Ȱɷ�������, ���� ����
        SkillFuncBase buff_10003 = new SkillD_10003();
        buff_10003.Create(10003);
        buff_10003.eSkillUseType = SkillUseType.Buff;
        buff_10003.ConditionTurn = 2;
        // ��߼��̹Ƿ� �ٷ� ���� ���
        buff_10003.Buff(attackers, atkIndex, defenders, actionLogs);

        attackers[atkIndex].ConditionDic[10003] = buff_10003; // ���� ��ųʸ��� �߰�
    }

    public override void Buff(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        _attackers = attackers;
        _atkIndex = atkIndex;

        // ���� �ɾ���
        _attackers[atkIndex].AddFinalDmgPer *= t_SkillData.Data.DataMap[10003].value0;
        ActionLog log = new ActionLog();
        log.ActionType = ActionType.Buff;
        log.ObjIndex = atkIndex;
        log.ActiveObj = _attackers[atkIndex].ActiveObj;
        log.ObjHP = _attackers[atkIndex].FinalHp;
        log.ObjCurHp = _attackers[atkIndex].CurHP;
        actionLogs.Add(log);
    }

    // ���� ����
    public override void Remove() {
        _attackers[_atkIndex].AddFinalDmgPer /= t_SkillData.Data.DataMap[10003].value0;
        _attackers[_atkIndex].ConditionDic[10003] = null;
        _attackers[_atkIndex].ConditionDic.Remove(10003);
    }
}
