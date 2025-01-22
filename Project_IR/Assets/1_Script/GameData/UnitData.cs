using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitData 
{
    public ObscuredInt Key; // 유닛 키

    public ActiveObj ActiveObj; // 행동자 (플레이어, 적)
    public ObscuredInt ObjIndex; // 행동자 인덱스

    // 기초 스텟 (최초 진입시 가지는 스텟) 불변
    public ObscuredBigInteger BaseAtk; // 공격력
    public ObscuredBigInteger BaseDef; // 방어력
    public ObscuredBigInteger BaseHp; // 체력
    public ObscuredFloat BaseAbsorb; // 흡수
    public ObscuredFloat BaseCri; // 크리티컬 확률
    public ObscuredFloat BaseCriDmg; // 크리티컬 데미지

    // 전투 관련
    public ObscuredBool IsCounter; // 카운터 여부 (반격버프 받으면 true)

    //  특수 (버프 관련으로 증가)
    public ObscuredFloat AddAtkPer; // 공격력 증가
    public ObscuredFloat AddDefPer; // 방어력 증가
    public ObscuredFloat AddHpPer; // 체력 증가
    public ObscuredFloat AddCriPer; // 크리티컬 확률 증가
    public ObscuredFloat AddCriDmgPer; // 크리티컬 데미지 증가
    public ObscuredFloat AddFinalDmgPer; // 최종 데미지 보너스 증가

    // 최종 스텟 (장비 장착시 계산된 스텟) 전투시 버프, 디버프등은 여기에 적용안됨
    public ObscuredBigInteger FinalAtk; // 최종 공격력 (공격력 * 공격력 증가) * 최종 데미지 보너스 증가
    public ObscuredBigInteger FinalDef; // 최종 방어력
    public ObscuredBigInteger FinalHp; // 최종 체력
    public ObscuredFloat FinalAbsorb; // 최종 흡수
    public ObscuredFloat FinalCri; // 최종 크리티컬 확률
    public ObscuredFloat FinalCriDmg; // 최종 크리티컬 데미지

    // HP 관련 
    public ObscuredBigInteger CurHP; // 현재 체력 (최종체력을 건드릴순 없으니)

    // 버프, 디버프 및 걸린 모든 스킬
    public Dictionary<int,SkillFuncBase> ConditionDic = new Dictionary<int, SkillFuncBase>(); // 유닛에게 걸린 모든 스킬

    // 턴 시작시 사용되는 스킬
    public Dictionary<int, SkillFuncBase> TurnStartDic = new Dictionary<int, SkillFuncBase>(); // 턴 시작시 사용되는 스킬
    // 턴 종료시 사용되는 스킬
    public Dictionary<int, SkillFuncBase> TurnEndDic = new Dictionary<int, SkillFuncBase>(); // 턴 종료시 사용되는 스킬
    // 공격시 사용되는 스킬
    public Dictionary<int, SkillFuncBase> AttackDic = new Dictionary<int, SkillFuncBase>(); // 공격시 사용되는 스킬
    // 공격후 사용되는 스킬
    public Dictionary<int, SkillFuncBase> AttackEndDic = new Dictionary<int, SkillFuncBase>(); // 공격후 사용되는 스킬
    // 피격시 사용되는 스킬
    public Dictionary<int, SkillFuncBase> HitDic = new Dictionary<int, SkillFuncBase>(); // 피격시 사용되는 스킬

    protected ObscuredInt _skillIndex = 0;

    // 장착 스킬 정보 (최대5개 장착 가능하고 나머지 2개엔 일반공격이 들어감) 스킬만 나가는걸 방지. 그래서 디버프 및 버프는 길어야 2턴 (무한 벞,디벞 방지) <-- 잘될지 모르겠지만 안되면 말고 
    // 1개 스킬 장착시 총 3칸 차지 (스킬 1, 일반공격, 일반공격)
    // 2개 스킬 장착시 총 4칸 차지 (스킬 1,스킬2, 일반공격, 일반공격)
    public SkillGroupData[] EquipSkill = new SkillGroupData[7]; // 장착 스킬 

    // 일반공격 추가
    public virtual void AddNormalAtk() {
        int index = 0;
        for (int i = 0; i < EquipSkill.Length; i++) {
            if (EquipSkill[i] == null) {
                index = i;
                break;
            }
        }
        for (int i = index; i < index + 2; i++) {          
            EquipSkill[i] = new SkillGroupData();
            EquipSkill[i].Create(1000); // 일반공격
        }
    }

    // 스킬 사용
    public void SkillUse(List<UnitData> my, int myIndex, List<UnitData> units, List<ActionLog> actionLogs) {
        if (EquipSkill[_skillIndex] == null) {
            _skillIndex = 0;
        }
        EquipSkill[_skillIndex].Use(my, myIndex, units, actionLogs);
        _skillIndex++;
    }

    public virtual void Init()
    {
        ActiveObj = ActiveObj.Player;
        BaseAtk = 0;
        BaseDef = 0;
        BaseHp = 0;
        BaseAbsorb = 0;
        BaseCri = 10; // 10%
        BaseCriDmg = 2; // 2배
        IsCounter = false;
        AddAtkPer = 1;
        AddDefPer = 1;
        AddHpPer = 1;
        AddCriPer = 1;
        AddCriDmgPer = 1;
        AddFinalDmgPer = 1;

        FinalAtk = 0;
        FinalDef = 0;
        FinalHp = 0;
        FinalAbsorb = 0;
        FinalCri = 0;
        FinalCriDmg = 0;

        _skillIndex = 0;
        //_skillIndexMax = 0;

        for (int i = 0; i < EquipSkill.Length; i++) {
            EquipSkill[i] = null;
        }

        //NoramlBnDDic.Clear();
        TurnStartDic.Clear();
        TurnEndDic.Clear();
        AttackDic.Clear();
        AttackEndDic.Clear();
        HitDic.Clear();
        ConditionDic.Clear();
    }

    // 최종 대미지 갱신
    public void UpdateFinalAtk() {
        FinalAtk = BaseAtk * (AddAtkPer * 100); // 공격력 * 공격력 증가 (100은 계산을 위해 소수점 없애기위함)
        FinalAtk = FinalAtk / 100; // 계산시 곱해줬던 100 다시 나눠서 정상값 만듦
        FinalAtk = FinalAtk * (AddFinalDmgPer * 100); // 최종 데미지 보너스 증가 (100은 계산을 위해 소수점 없애기위함)
        FinalAtk = FinalAtk / 100; // 계산시 곱해줬던 100 다시 나눠서 정상값 만듦
    }

    public void UpdateFinalDef() {
        FinalDef = BaseDef * (AddDefPer * 100); // 방어력 * 방어력 증가 (100은 계산을 위해 소수점 없애기위함)
        FinalDef = FinalDef / 100; // 계산시 곱해줬던 100 다시 나눠서 정상값 만듦
    }

    public void UpdateFinalHp() {
        FinalHp = BaseHp * (AddHpPer * 100); // 체력 * 체력 증가 (100은 계산을 위해 소수점 없애기위함)
        FinalHp = FinalHp / 100; // 계산시 곱해줬던 100 다시 나눠서 정상값 만듦
    }

    public void UpdateFinalCri() {
        FinalCri = BaseCri * AddCriPer; // 크리티컬 확률 * 크리티컬 확률 증가
    }

    public void UpdateFinalCriDmg() {
        FinalCriDmg = BaseCriDmg * AddCriDmgPer; // 크리티컬 데미지 * 크리티컬 데미지 증가
    }

    public void UpdateFinalAbsorb() {
        FinalAbsorb = BaseAbsorb; // 흡수는 증가율이 없으므로 그대로 사용
    }

    // 전체 갱신
    public void UpdateFinalAll() {
        UpdateFinalAtk();
        UpdateFinalDef();
        UpdateFinalHp();
        UpdateFinalCri();
        UpdateFinalCriDmg();
        UpdateFinalAbsorb();
    }
}
