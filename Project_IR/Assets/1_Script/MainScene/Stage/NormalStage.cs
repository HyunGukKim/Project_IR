
using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class NormalStage : MonoBehaviour {
    private const int MaxTurns = 30;
    private const int MaxWave = 100;
    private ObscuredInt _curStage = 0;
    private ObscuredInt _curWave = 0;

    private List<UnitData> _turnList = new List<UnitData>(); // ��Ʋ�� �� ����Ʈ �Ͻ��۽� ������ �̸� �־��ش�.
    private List<UnitData> _userUnitList = new List<UnitData>(); // ���� ���� ����Ʈ
    private List<UnitData> _enemyUnitList = new List<UnitData>(); // ���� ���� ����Ʈ
    private List<List<ActionLog>> _turnLogs = new List<List<ActionLog>>(); // �� �α� ����Ʈ

    private ObscuredBool _isWaveEnd = true; // ���̺� ���� ����
    private ObscuredInt _turnListCount = 0;
    private ObscuredInt _turnIndex = 0;
    private ActiveObj _winner = ActiveObj.Player; // �¸���

    // �ڷ�ƾ
    private Coroutine _coNextWave = null;
    private Coroutine _coDisplayTurnLogs = null;

    private void Start() {
        SkillDataCreate.Inst.DicInit();
        _curStage = StaticManager.Backend.GameData.UserData.StageData["stage"];
        _curWave = StaticManager.Backend.GameData.UserData.StageData["wave"];
        EnterStage(_curStage, _curWave);
    }

    public void EnterStage(int stage, int wave) {
        Debug.Log("�������� : " + stage + " ���̺� : " + wave);
        WaveStart(stage, wave);
        if (_coDisplayTurnLogs != null) {
            StopCoroutine(_coDisplayTurnLogs);
            _coDisplayTurnLogs = null;
        }
        _coDisplayTurnLogs = StartCoroutine(DisplayTurnLogs());
    }

    private void WaveStart(int stage, int wave) {
        _isWaveEnd = false;
        _turnIndex = 0;
        _turnLogs.Clear();

        LoadUserUnitData();
        LoadEnemyUnitData(stage, wave);

        while (!_isWaveEnd) {
            StartTurn();
        }
    }

    private void LoadUserUnitData() {
        _userUnitList.Clear();

        UnitData user = new UnitData();
        user.Init();
        user.ActiveObj = ActiveObj.Player;
        user.BaseAtk = StaticManager.Backend.GameData.UserData.UserStat.Str; // �ϴ� ���ݷ� ��ȹ�� �ϼ��Ǳ� ������ STR�� ���ݷ����� ���
        user.BaseHp = StaticManager.Backend.GameData.UserData.UserStat.Vit * 10; // HP�� VIT * 10
        user.UpdateFinalAll(); // �⺻���� �־������� �������� ������Ʈ

        user.CurHP = user.FinalHp;
        user.ObjIndex = 0; // ���Ŀ� ���ᰡ �߰��� ��츦 ����Ͽ� �ε����� �ο�

        int skillCount = StaticManager.Backend.GameData.UserData.EqSkills.Length;
        for (int i = 0; i < skillCount; i++) {
            if (StaticManager.Backend.GameData.UserData.EqSkills[i] != 0) { // ��ų�� �����Ǿ� ���� ��� ( Ű���� 0�� �ƴҰ��)
                SkillGroupData skillg = new SkillGroupData();
                skillg.Create(StaticManager.Backend.GameData.UserData.EqSkills[i]);
                user.EquipSkill[i] = skillg;
            }
        }

        user.AddNormalAtk(); // �Ϲݰ��� �߰�
        _userUnitList.Add(user);
    }

    private int DetermineEnemyType(int stage, int ran) {
        int enemy0Per = t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_0_per;
        int enemy1Per = t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_1_per;
        int enemy2Per = t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_2_per;

        if (ran < enemy0Per) {
            // �� ���� 0
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_0;
        } else if (ran < enemy0Per + enemy1Per) {
            // �� ���� 1
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_1;
        } else if (ran < enemy0Per + enemy1Per + enemy2Per) {
            // �� ���� 2
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_2;
        } else {
            // �� ���� 3
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_3;
        }
    }

    private void LoadEnemyUnitData(int stage, int wave) {
        _enemyUnitList.Clear();

        BigInteger baseHp = CustomMath.EnemyStat(BigInteger.Parse(t_StageData.Enemy_Stat.Enemy_StatMap[stage].wave1HP), t_StageData.Enemy_Stat.Enemy_StatMap[stage].upPer, (wave - 1));
        BigInteger baseAtk = CustomMath.EnemyStat(BigInteger.Parse(t_StageData.Enemy_Stat.Enemy_StatMap[stage].wave1Atk), t_StageData.Enemy_Stat.Enemy_StatMap[stage].upPer, (wave - 1));

        for (int i = 0; i < t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_Count; i++) {
            int ran = Random.Range(0, 100);
            int enemykey = DetermineEnemyType(stage, ran);
            EnemyData enemy = new EnemyData();
            enemy.Init();
            enemy.Key = enemykey;
            enemy.BaseHp = baseHp;
            enemy.BaseAtk = baseAtk;
            enemy.BaseAbsorb = t_StageData.Enemy_Stat.Enemy_StatMap[stage].absorb;
            enemy.BaseDef = BigInteger.Parse(t_StageData.Enemy_Stat.Enemy_StatMap[stage].def);
            enemy.BaseCri = t_StageData.Enemy_Stat.Enemy_StatMap[stage].cri;
            enemy.BaseCriDmg = t_StageData.Enemy_Stat.Enemy_StatMap[stage].criDmg;
            enemy.UpdateFinalAll(); // �⺻���� �־������� �������� ������Ʈ

            enemy.CurHP = enemy.FinalHp;

            enemy.ObjIndex = i;

            int[] skillGroups = {
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup0_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup1_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup2_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup3_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup4_key,
            };

            // �迭�� ���� ��ų�׷��� �����ϰ� �߰�
            for (int groupInedx = 0; groupInedx < skillGroups.Length; groupInedx++) {
                if (skillGroups[groupInedx] != 0) {
                    SkillGroupData skillg = new SkillGroupData();
                    skillg.Create(skillGroups[groupInedx]);
                    enemy.EquipSkill[groupInedx] = skillg;
                }
            }
            enemy.AddNormalAtk(); // �Ϲݰ��� �߰�

            _enemyUnitList.Add(enemy);
        }
    }

    private void StartTurn() {
        _turnList.Clear();

        List<ActionLog> actionLogs = new List<ActionLog>();
        _turnListCount = Mathf.Max(_enemyUnitList.Count, _userUnitList.Count);

        for (int i = 0; i < _turnListCount; i++) {
            if (i < _userUnitList.Count) {
                if (_userUnitList[i].CurHP > 0) {
                    AddUnitToTurnList(_userUnitList[i], ActiveObj.Player, actionLogs);
                }
            }
            if (i < _enemyUnitList.Count) {
                if (_enemyUnitList[i].CurHP > 0) {
                    AddUnitToTurnList(_enemyUnitList[i], ActiveObj.Enemy, actionLogs);
                }
            }
        }

        for (int i = 0; i < _turnList.Count; i++) {
            if (_turnList[i].ActiveObj == ActiveObj.Player) {
                if (_turnList[i].CurHP <= 0) {
                    continue;
                }
                _turnList[i].SkillUse(_userUnitList, _turnList[i].ObjIndex, _enemyUnitList, actionLogs);
            } else {
                if (_turnList[i].CurHP <= 0) {
                    continue;
                }
                _turnList[i].SkillUse(_enemyUnitList, _turnList[i].ObjIndex, _userUnitList, actionLogs);
            }

            if (WaveEndChk(actionLogs)) {
                break;
            }
        }
        _turnLogs.Add(actionLogs);
        _turnIndex++;
    }

    private void AddUnitToTurnList(UnitData unit, ActiveObj activeObj, List<ActionLog> actionLogs) {
        _turnList.Add(unit);
        ActionLog log = new ActionLog();
        log.ActiveObj = activeObj;
        log.ActionType = Game.ActionType.None;
        log.ObjIndex = unit.ObjIndex;
        log.ObjCurHp = unit.CurHP;
        log.ObjHP = unit.FinalHp;

        // ����(����,�����) ����Ʈ �߰�
        foreach (var condition in unit.ConditionDic) {
            log.CondtionList.Add(condition.Key);
        }
        actionLogs.Add(log);
    }

    private IEnumerator DisplayTurnLogs() {
        for (int a = 0; a < _turnLogs.Count; a++) {
            Debug.Log("============= �� : " + (a + 1) + " ==============");
            for (int i = 0; i < _turnLogs[a].Count; i++) {
                if (_turnLogs[a][i].Winner != ActiveObj.None) {
                    Debug.Log("�¸��� : " + _turnLogs[a][i].Winner);
                    break;
                }

                // �� �����ϰ�� �⺻ ����
                if (_turnLogs[a][i].ActionType == Game.ActionType.None) {
                    Global_UIEventSystem.CallUIEvent<ActionLog>(eUIEventType.NormalTurnStartData, _turnLogs[a][i]);
                    continue;
                }

                if (_turnLogs[a][i].ActiveObj == ActiveObj.Enemy) {
                    Debug.Log(_turnLogs[a][i].ActiveObj + " : " + _enemyUnitList[_turnLogs[a][i].ObjIndex].Key);
                } else {
                    Debug.Log(_turnLogs[a][i].ActiveObj);
                }
                
                Debug.Log(_turnLogs[a][i].ActionType);
                if (_turnLogs[a][i].ActionType == Game.ActionType.Hit) {
                    Debug.Log("DMG : " + _turnLogs[a][i].HitDmg);
                }
                Debug.Log("HP : " + _turnLogs[a][i].ObjCurHp + " / " + _turnLogs[a][i].ObjHP);
                Debug.Log("------------------------------------");
            }
            Debug.Log("=================================");
            yield return new WaitForSeconds(1.0f);
        }

        if (_winner == ActiveObj.Player) { // �¸��ڰ� �÷��̾��� ��� ���� �� ���� ���̺� ����
            Debug.Log("�÷��̾� �¸�");
            _curWave++;
            if (_coNextWave != null) {
                StopCoroutine(_coNextWave);
                _coNextWave = null;
            }
            _coNextWave = StartCoroutine(NextWave());
        } else { // �¸��ڰ� ���� ��� ���̺� �ʱ�ȭ 1�ܰ�� ���ư�
            Debug.Log("�� �¸�");
            if (_curWave == 1) { // 1���̺� ���� ���� ��� �������� �Ѵܰ� ����
                _curStage--;
            }
            _curWave = 1;
            if (_coNextWave != null) {
                StopCoroutine(_coNextWave);
                _coNextWave = null;
            }
            _coNextWave = StartCoroutine(NextWave());
        } 

        yield return null;
    }

    private IEnumerator NextWave() {
        yield return new WaitForSeconds(1.0f);
        if (_curWave > MaxWave) {
            _curWave = 1;
            if (_curStage < t_StageData.EnemyData.EnemyDataMap.Count) {
                _curStage++;
            }
            EnterStage(_curStage, _curWave);
        } else {
            EnterStage(_curStage, _curWave);
        }
    }
    
    private bool WaveEndChk(List<ActionLog> actionLogs) {
        bool userWin = true;
        bool enemyWin = true;
        _isWaveEnd = false;

        if (_turnIndex == MaxTurns - 1) {
            _winner = ActiveObj.Enemy;
            _isWaveEnd = true;
            actionLogs.Add(new ActionLog { Winner = _winner });
            return _isWaveEnd;
        }

        foreach (var enemy in _enemyUnitList) {
            if (enemy.CurHP > 0) {
                userWin = false;
            }
        }

        foreach (var user in _userUnitList) {
            if (user.CurHP > 0) {
                enemyWin = false;
            }
        }

        if (userWin) {
            _winner = ActiveObj.Player;
            _isWaveEnd = true;
            actionLogs.Add(new ActionLog { Winner = _winner });
        } else if (enemyWin) {
            _winner = ActiveObj.Enemy;
            _isWaveEnd = true;
            actionLogs.Add(new ActionLog { Winner = _winner });
        }
        return _isWaveEnd;
    }
}
