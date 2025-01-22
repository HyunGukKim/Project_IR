using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
// ���� �ǰ� ���
public class BattleCalculate {
    private static readonly BattleCalculate _inst = new BattleCalculate();
    public static BattleCalculate Inst => _inst;

    // ���� ����� �����ͼ� �߰� ����� ����� ����
    public void Battle(BattleData_Etc etc, ActionType actionType ,ObscuredBigInteger atk ,UnitData attacker, UnitData defender, List<ActionLog> actionLogs) {
        //  �α� ���
        if (etc.isAtkLog == true) {
            // - ������
            ActionLog log = new ActionLog();
            log.ActionType = actionType;
            log.ObjIndex = attacker.ObjIndex;
            log.ActiveObj = attacker.ActiveObj;
            log.ObjHP = attacker.FinalHp;
            log.ObjCurHp = attacker.CurHP;
            actionLogs.Add(log);
        }
        ObscuredBigInteger FinalDmg = atk;

        // ���� ����� �߰� ���(���¿� ���� ����� ���� ��)
        //------���� �߰� �ؾ���-----//
        // �߰� ��� ��

        defender.CurHP -= FinalDmg;
        // - �ǰ���
        ActionLog log2 = new ActionLog();
        log2.ActionType = ActionType.Hit;
        log2.ActiveObj = defender.ActiveObj;
        log2.ObjIndex = defender.ObjIndex;
        log2.HitDmg = FinalDmg;
        log2.ObjHP = defender.FinalHp;
        log2.ObjCurHp = defender.CurHP;
        // �α� ����
        actionLogs.Add(log2);
    }
}

public struct BattleData_Etc{
    public bool isAtkLog; // ������ �α� ��뿩��
    // �����...
    // �ǰݽ� ��ų �ߵ����� �̰� ���� �������ɼ��ִµ�... �ݰ� ��ų�̶�  �ǰݽ� ��ų�� ���� �����ؾ��ҵ�
    // �ǰݽ� �ߵ� ��ų���� �ϴ� ȸ�� ������ �־��ҵ�. ��������... �ϱ�� �Լ��� �������̵� �ؾ���
}