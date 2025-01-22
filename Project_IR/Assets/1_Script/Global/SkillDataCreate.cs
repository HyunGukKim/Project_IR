using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// 스킬 데이터를 생성하는 팩토리 클래스
/// </summary>
public class SkillDataCreate
{
    private static readonly SkillDataCreate _inst = new SkillDataCreate();
    public static SkillDataCreate Inst => _inst;

    private Dictionary<int, CreateSkillFunc> SkillCreateDic = new Dictionary<int, CreateSkillFunc>();

    private delegate SkillFuncBase CreateSkillFunc(int key);

    public void DicInit()
    {
        SkillCreateDic[10000] = Craete_10000;
        SkillCreateDic[10001] = Craete_10001;
        SkillCreateDic[10002] = Craete_10002;
        SkillCreateDic[10003] = Craete_10003;
    }

    /// <summary>
    /// 스킬 만들기
    /// </summary>
    /// <param name="key"></param>
    public SkillFuncBase CreateSkill(int key) {
        if (SkillCreateDic.ContainsKey(key) == false) { return null; } // 에러 시스템 종료 때려야 할듯
   
        return SkillCreateDic[key](key); 
    }

    public SkillFuncBase Craete_10000(int key)
    {
        SkillFuncBase skill = null;
        skill = new SkillD_10000();
        skill.Create(key);
        return skill;
    }

    public SkillFuncBase Craete_10001(int key) {
        SkillFuncBase skill = null;
        skill = new SkillD_10001();
        skill.Create(key);
        return skill;
    }

    public SkillFuncBase Craete_10002(int key) {
        SkillFuncBase skill = null;
        skill = new SkillD_10002();
        skill.Create(key);
        return skill;
    }

    public SkillFuncBase Craete_10003(int key) {
        SkillFuncBase skill = null;
        skill = new SkillD_10003();
        skill.Create(key);
        return skill;
    }
}
