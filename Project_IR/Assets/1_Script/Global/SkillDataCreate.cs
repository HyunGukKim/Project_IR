using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// ��ų �����͸� �����ϴ� ���丮 Ŭ����
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
    /// ��ų �����
    /// </summary>
    /// <param name="key"></param>
    public SkillFuncBase CreateSkill(int key) {
        if (SkillCreateDic.ContainsKey(key) == false) { return null; } // ���� �ý��� ���� ������ �ҵ�
   
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
