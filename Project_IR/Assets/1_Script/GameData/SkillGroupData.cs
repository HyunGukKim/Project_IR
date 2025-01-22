using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillGroupData {
    public ObscuredInt Key;
    public List<SkillFuncBase> Skill = new List<SkillFuncBase>();

    public void Create(int key) // ��ų �׷� ����
    {
        Skill.Clear();
        Key = key;

        // ��ų�� Ȯ���� �迭�� ����
        int[] skills = {
            t_SkillGroup.Data.DataMap[key].skill0,
            t_SkillGroup.Data.DataMap[key].skill1,
            t_SkillGroup.Data.DataMap[key].skill2,
            t_SkillGroup.Data.DataMap[key].skill3,
            t_SkillGroup.Data.DataMap[key].skill4,
            t_SkillGroup.Data.DataMap[key].skill5
        };

        float[] skillPercents = {
            t_SkillGroup.Data.DataMap[key].skill0Per,
            t_SkillGroup.Data.DataMap[key].skill1Per,
            t_SkillGroup.Data.DataMap[key].skill2Per,
            t_SkillGroup.Data.DataMap[key].skill3Per,
            t_SkillGroup.Data.DataMap[key].skill4Per,
            t_SkillGroup.Data.DataMap[key].skill5Per
        };

        // �迭�� ���� ��ų�� �����ϰ� �߰�
        for (int i = 0; i < skills.Length; i++) {
            if (skills[i] != 0) {
                SkillFuncBase skill = SkillDataCreate.Inst.CreateSkill(skills[i]);
                skill.ActivePercent = skillPercents[i];
                Skill.Add(skill);
            }
        }

        //Debug.Log("Create Key : " + key);
        //Debug.Log("Skill Count : " + Skill.Count);
    }

    public void Use(List<UnitData> my, int myIndex, List<UnitData> units, List<ActionLog> actionLogs) {
        // ��ų ���
        float percent = 0;
        for (int i = 0; i < Skill.Count; i++) {
            percent = Random.Range(0, 100);
            if (Skill[i].ActivePercent > percent) {
                // Ȯ�� ������ ��ų ���
                Skill[i].Use(my, myIndex, units, actionLogs);
            } else {
                // �ѹ� Ȯ�� �� ������ �� (���� ��ų ��� �� ��)
                break;
            }
        }
    }
}
