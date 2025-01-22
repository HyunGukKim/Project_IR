using System.Collections.Generic;
using Game;
using UnityEngine;
/// <summary>
/// �⺻����
/// </summary>
public class SkillD_10000 : SkillFuncBase
{
    public override void Use(List<UnitData> attackers, int atkIndex, List<UnitData> defenders, List<ActionLog> actionLogs) {
        for (int i = 0; i < defenders.Count; i++) {
            if (defenders[i].CurHP > 0) {
                attackers[atkIndex].UpdateFinalAtk(); // ���� ����� ����
                // ���� ���� ȣ��
                BattleData_Etc etc;
                etc.isAtkLog = true;
                BattleCalculate.Inst.Battle(etc, ActionType, attackers[atkIndex].FinalAtk, attackers[atkIndex], defenders[i], actionLogs);
                break;
            }
        }
    }
}
