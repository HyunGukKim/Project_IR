using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using System.Collections.Generic;
using UnityEngine;

public class ActionLog
{
    public ActiveObj Winner = ActiveObj.None; // 승리자 None 면 아직 안끝남
    public ActionType ActionType; // 행동 타입 (일반공격, 스킬, 카운터, 피격)
    public ActiveObj ActiveObj; // 행동자 (플레이어, 적)
    public ObscuredInt ObjIndex; // 행동자 인덱스
    public ObscuredBigInteger HitDmg; // 피격 데미지 (피격시)
    public ObscuredBigInteger ObjHP; // 행동자 체력
    public ObscuredBigInteger ObjCurHp; // 행동자 현재 체력
    public List<ObscuredInt> CondtionList = new List<ObscuredInt>(); // 상태(버프,디버프) 리스트
}

public enum ActiveObj {
    Player,
    Enemy,
    None,
}
