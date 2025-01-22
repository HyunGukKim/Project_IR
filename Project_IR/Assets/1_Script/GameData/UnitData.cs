using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitData 
{
    public ObscuredInt Key; // ���� Ű

    public ActiveObj ActiveObj; // �ൿ�� (�÷��̾�, ��)
    public ObscuredInt ObjIndex; // �ൿ�� �ε���

    // ���� ���� (���� ���Խ� ������ ����) �Һ�
    public ObscuredBigInteger BaseAtk; // ���ݷ�
    public ObscuredBigInteger BaseDef; // ����
    public ObscuredBigInteger BaseHp; // ü��
    public ObscuredFloat BaseAbsorb; // ���
    public ObscuredFloat BaseCri; // ũ��Ƽ�� Ȯ��
    public ObscuredFloat BaseCriDmg; // ũ��Ƽ�� ������

    // ���� ����
    public ObscuredBool IsCounter; // ī���� ���� (�ݰݹ��� ������ true)

    //  Ư�� (���� �������� ����)
    public ObscuredFloat AddAtkPer; // ���ݷ� ����
    public ObscuredFloat AddDefPer; // ���� ����
    public ObscuredFloat AddHpPer; // ü�� ����
    public ObscuredFloat AddCriPer; // ũ��Ƽ�� Ȯ�� ����
    public ObscuredFloat AddCriDmgPer; // ũ��Ƽ�� ������ ����
    public ObscuredFloat AddFinalDmgPer; // ���� ������ ���ʽ� ����

    // ���� ���� (��� ������ ���� ����) ������ ����, ��������� ���⿡ ����ȵ�
    public ObscuredBigInteger FinalAtk; // ���� ���ݷ� (���ݷ� * ���ݷ� ����) * ���� ������ ���ʽ� ����
    public ObscuredBigInteger FinalDef; // ���� ����
    public ObscuredBigInteger FinalHp; // ���� ü��
    public ObscuredFloat FinalAbsorb; // ���� ���
    public ObscuredFloat FinalCri; // ���� ũ��Ƽ�� Ȯ��
    public ObscuredFloat FinalCriDmg; // ���� ũ��Ƽ�� ������

    // HP ���� 
    public ObscuredBigInteger CurHP; // ���� ü�� (����ü���� �ǵ帱�� ������)

    // ����, ����� �� �ɸ� ��� ��ų
    public Dictionary<int,SkillFuncBase> ConditionDic = new Dictionary<int, SkillFuncBase>(); // ���ֿ��� �ɸ� ��� ��ų

    // �� ���۽� ���Ǵ� ��ų
    public Dictionary<int, SkillFuncBase> TurnStartDic = new Dictionary<int, SkillFuncBase>(); // �� ���۽� ���Ǵ� ��ų
    // �� ����� ���Ǵ� ��ų
    public Dictionary<int, SkillFuncBase> TurnEndDic = new Dictionary<int, SkillFuncBase>(); // �� ����� ���Ǵ� ��ų
    // ���ݽ� ���Ǵ� ��ų
    public Dictionary<int, SkillFuncBase> AttackDic = new Dictionary<int, SkillFuncBase>(); // ���ݽ� ���Ǵ� ��ų
    // ������ ���Ǵ� ��ų
    public Dictionary<int, SkillFuncBase> AttackEndDic = new Dictionary<int, SkillFuncBase>(); // ������ ���Ǵ� ��ų
    // �ǰݽ� ���Ǵ� ��ų
    public Dictionary<int, SkillFuncBase> HitDic = new Dictionary<int, SkillFuncBase>(); // �ǰݽ� ���Ǵ� ��ų

    protected ObscuredInt _skillIndex = 0;

    // ���� ��ų ���� (�ִ�5�� ���� �����ϰ� ������ 2���� �Ϲݰ����� ��) ��ų�� �����°� ����. �׷��� ����� �� ������ ���� 2�� (���� ��,�� ����) <-- �ߵ��� �𸣰����� �ȵǸ� ���� 
    // 1�� ��ų ������ �� 3ĭ ���� (��ų 1, �Ϲݰ���, �Ϲݰ���)
    // 2�� ��ų ������ �� 4ĭ ���� (��ų 1,��ų2, �Ϲݰ���, �Ϲݰ���)
    public SkillGroupData[] EquipSkill = new SkillGroupData[7]; // ���� ��ų 

    // �Ϲݰ��� �߰�
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
            EquipSkill[i].Create(1000); // �Ϲݰ���
        }
    }

    // ��ų ���
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
        BaseCriDmg = 2; // 2��
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

    // ���� ����� ����
    public void UpdateFinalAtk() {
        FinalAtk = BaseAtk * (AddAtkPer * 100); // ���ݷ� * ���ݷ� ���� (100�� ����� ���� �Ҽ��� ���ֱ�����)
        FinalAtk = FinalAtk / 100; // ���� ������� 100 �ٽ� ������ ���� ����
        FinalAtk = FinalAtk * (AddFinalDmgPer * 100); // ���� ������ ���ʽ� ���� (100�� ����� ���� �Ҽ��� ���ֱ�����)
        FinalAtk = FinalAtk / 100; // ���� ������� 100 �ٽ� ������ ���� ����
    }

    public void UpdateFinalDef() {
        FinalDef = BaseDef * (AddDefPer * 100); // ���� * ���� ���� (100�� ����� ���� �Ҽ��� ���ֱ�����)
        FinalDef = FinalDef / 100; // ���� ������� 100 �ٽ� ������ ���� ����
    }

    public void UpdateFinalHp() {
        FinalHp = BaseHp * (AddHpPer * 100); // ü�� * ü�� ���� (100�� ����� ���� �Ҽ��� ���ֱ�����)
        FinalHp = FinalHp / 100; // ���� ������� 100 �ٽ� ������ ���� ����
    }

    public void UpdateFinalCri() {
        FinalCri = BaseCri * AddCriPer; // ũ��Ƽ�� Ȯ�� * ũ��Ƽ�� Ȯ�� ����
    }

    public void UpdateFinalCriDmg() {
        FinalCriDmg = BaseCriDmg * AddCriDmgPer; // ũ��Ƽ�� ������ * ũ��Ƽ�� ������ ����
    }

    public void UpdateFinalAbsorb() {
        FinalAbsorb = BaseAbsorb; // ����� �������� �����Ƿ� �״�� ���
    }

    // ��ü ����
    public void UpdateFinalAll() {
        UpdateFinalAtk();
        UpdateFinalDef();
        UpdateFinalHp();
        UpdateFinalCri();
        UpdateFinalCriDmg();
        UpdateFinalAbsorb();
    }
}
