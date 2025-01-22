using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillGroupData {
    public ObscuredInt Key;
    public List<SkillFuncBase> Skill = new List<SkillFuncBase>();

    public void Create(int key) // 스킬 그룹 생성
    {
        Skill.Clear();
        Key = key;

        // 스킬과 확률을 배열로 관리
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

        // 배열을 통해 스킬을 생성하고 추가
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
        // 스킬 사용
        float percent = 0;
        for (int i = 0; i < Skill.Count; i++) {
            percent = Random.Range(0, 100);
            if (Skill[i].ActivePercent > percent) {
                // 확률 맞으면 스킬 사용
                Skill[i].Use(my, myIndex, units, actionLogs);
            } else {
                // 한번 확률 안 맞으면 끝 (다음 스킬 사용 안 됨)
                break;
            }
        }
    }
}
