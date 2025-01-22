using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
// 공격 피격 계산
public class BattleCalculate {
    private static readonly BattleCalculate _inst = new BattleCalculate();
    public static BattleCalculate Inst => _inst;

    // 최종 대미지 가져와서 추가 계산후 대미지 정산
    public void Battle(BattleData_Etc etc, ActionType actionType ,ObscuredBigInteger atk ,UnitData attacker, UnitData defender, List<ActionLog> actionLogs) {
        //  로그 기록
        if (etc.isAtkLog == true) {
            // - 공격자
            ActionLog log = new ActionLog();
            log.ActionType = actionType;
            log.ObjIndex = attacker.ObjIndex;
            log.ActiveObj = attacker.ActiveObj;
            log.ObjHP = attacker.FinalHp;
            log.ObjCurHp = attacker.CurHP;
            actionLogs.Add(log);
        }
        ObscuredBigInteger FinalDmg = atk;

        // 최종 대미지 추가 계산(방어력에 따른 대미지 감소 등)
        //------내용 추가 해야함-----//
        // 추가 계산 끝

        defender.CurHP -= FinalDmg;
        // - 피격자
        ActionLog log2 = new ActionLog();
        log2.ActionType = ActionType.Hit;
        log2.ActiveObj = defender.ActiveObj;
        log2.ObjIndex = defender.ObjIndex;
        log2.HitDmg = FinalDmg;
        log2.ObjHP = defender.FinalHp;
        log2.ObjCurHp = defender.CurHP;
        // 로그 종료
        actionLogs.Add(log2);
    }
}

public struct BattleData_Etc{
    public bool isAtkLog; // 공격자 로그 사용여부
    // 어디보자...
    // 피격시 스킬 발동여부 이게 무한 루프가될수있는데... 반격 스킬이랑  피격시 스킬을 따로 구분해야할듯
    // 피격시 발동 스킬들은 턴당 회수 제한을 둬야할듯. 구현은뭐... 턴깎는 함수에 오버라이드 해야지
}