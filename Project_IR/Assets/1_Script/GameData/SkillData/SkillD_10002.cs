using System.Collections.Generic;
using Game;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
/// <summary>
/// 더블어택 : 2번 공격한다
/// </summary>
public class SkillD_10002 : SkillFuncBase {
    public override void Use(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        for (int i = 0; i < defenders.Count; i++) {
            if (defenders[i].CurHP > 0) {
                ActionLog log = new ActionLog();
                log.ActionType = ActionType;
                log.ObjIndex = atkIndex;
                log.ActiveObj = attackers[atkIndex].ActiveObj;
                log.ObjHP = attackers[atkIndex].FinalHp;
                log.ObjCurHp = attackers[atkIndex].CurHP;
                actionLogs.Add(log);

                attackers[atkIndex].UpdateFinalAtk(); // 최종 대미지 갱신
                for (int n = 0; n < t_SkillData.Data.DataMap[10002].value0 ; n++ ) { // n 번 공격
                    BattleData_Etc etc;
                    etc.isAtkLog = false;
                    BattleCalculate.Inst.Battle(etc,ActionType,attackers[atkIndex].FinalAtk, attackers[atkIndex], defenders[i], actionLogs);
                }
                break;
            }
        }
    }
}
