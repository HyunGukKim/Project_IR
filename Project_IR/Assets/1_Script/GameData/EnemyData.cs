using UnityEngine;

public class EnemyData : UnitData {
    public override void Init() {
        ActiveObj = ActiveObj.Enemy;
        BaseAtk = 0;
        BaseDef = 0;
        BaseHp = 0;
        BaseAbsorb = 0;
        BaseCri = 0; //  ���ʹ̴� �⺻ ũ��Ƽ�� ����
        BaseCriDmg = 0; //  ���ʹ̴� �⺻ ũ��Ƽ�� ���� 
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
        for (int i = 0; i < EquipSkill.Length; i++) {
            EquipSkill[i] = null;
        }

        TurnStartDic.Clear();
        TurnEndDic.Clear();
        AttackDic.Clear();
        AttackEndDic.Clear();
        HitDic.Clear();
        ConditionDic.Clear();
    }
}
