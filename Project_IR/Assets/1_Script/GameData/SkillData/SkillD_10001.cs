using System.Collections.Generic;
using Game;
using UnityEngine;
/// <summary>
///  ������ 2�� ��� ����
/// </summary>
public class SkillD_10001 : SkillFuncBase {
    public override void Use(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        for (int i = 0; i < defenders.Count; i++) {
            if (defenders[i].CurHP > 0) {
                //  �α� ��� 
                attackers[atkIndex].UpdateFinalAtk(); // ���� ����� ����
                BattleData_Etc etc;
                etc.isAtkLog = true;
                BattleCalculate.Inst.Battle(etc, ActionType, attackers[atkIndex].FinalAtk *2, attackers[atkIndex], defenders[i], actionLogs);
                break;
            }
        }
    }
}
