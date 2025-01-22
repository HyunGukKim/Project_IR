using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using System.Collections.Generic;
using UnityEngine;

public class SkillFuncBase {
    public ActionType ActionType;
    public ObscuredInt Key;
    public ObscuredFloat Value0;
    public ObscuredFloat Value1;
    public ObscuredFloat Value2;
    public ObscuredFloat Value3;
    public ObscuredFloat Value4;
    public ObscuredFloat Value5;

    public ObscuredInt UseTurn; // ��� ���� �� (0�̸� ��밡��)
    public ObscuredInt ConditionTurn; // ���� �̻� ���� �� (0�̸� �����̻� ����)

    public SkillUseType eSkillUseType = SkillUseType.Use; // ��ų ��� Ÿ�� (Ÿ�Կ� ���� �ش��ϴ� �� ����)

    public ObscuredFloat ActivePercent;

    public virtual void Create(int key) // ��ų ����
    {
        Key = key;
        ActionType = t_SkillData.Data.DataMap[key].ActionType;
        Value0 = t_SkillData.Data.DataMap[key].value0;
        Value1 = t_SkillData.Data.DataMap[key].value1;
        Value2 = t_SkillData.Data.DataMap[key].value2;
        Value3 = t_SkillData.Data.DataMap[key].value3;
        Value4 = t_SkillData.Data.DataMap[key].value4;
        Value5 = t_SkillData.Data.DataMap[key].value5;

    }

    public virtual void Remove() { } // ��ų ����
    public virtual void Use(List<UnitData> attackers,int atkIndex , List<UnitData> defenders, List<ActionLog> actionLogs) { } // ��ų ���
    public virtual void Turn() {
        if (eSkillUseType == SkillUseType.Use) {
            // ������
            if (UseTurn > 0) {
                UseTurn--;
            }
            return;
        }
        if (ConditionTurn > 0) {
            ConditionTurn--;
            if (ConditionTurn == 0) {
                Remove();
            }
        }

    } // �� ����

    #region ��ų�� ���� ��󿡰� ������ ������� ������ ��� ���
    public virtual void Buff(List<UnitData> my, int myIndex, List<UnitData> units, List<ActionLog> actionLogs) { } // ���� ���
    public virtual void DeBuff(List<UnitData> my, int myIndex, List<UnitData> units, List<ActionLog> actionLogs) { } // ����� ���
    #endregion

}
// ��ų ��� Ÿ��
public enum SkillUseType {
    Use = 0,
    Buff = 1,
    DeBuff = 2,
}
