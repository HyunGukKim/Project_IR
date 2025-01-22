using System.Collections.Generic;
using Game;
using UnityEngine;
/// <summary>
///  강공격 2배 쎄게 공격
/// </summary>
public class SkillD_10001 : SkillFuncBase {
    public override void Use(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        for (int i = 0; i < defenders.Count; i++) {
            if (defenders[i].CurHP > 0) {
                //  로그 기록 
                attackers[atkIndex].UpdateFinalAtk(); // 최종 대미지 갱신
                BattleData_Etc etc;
                etc.isAtkLog = true;
                BattleCalculate.Inst.Battle(etc, ActionType, attackers[atkIndex].FinalAtk *2, attackers[atkIndex], defenders[i], actionLogs);
                break;
            }
        }
    }
}
