
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

    private List<UnitData> _turnList = new List<UnitData>(); // 배틀의 턴 리스트 턴시작시 순서를 미리 넣어준다.
    private List<UnitData> _userUnitList = new List<UnitData>(); // 유저 유닛 리스트
    private List<UnitData> _enemyUnitList = new List<UnitData>(); // 몬스터 유닛 리스트
    private List<List<ActionLog>> _turnLogs = new List<List<ActionLog>>(); // 턴 로그 리스트

    private ObscuredBool _isWaveEnd = true; // 웨이브 종료 여부
    private ObscuredInt _turnListCount = 0;
    private ObscuredInt _turnIndex = 0;
    private ActiveObj _winner = ActiveObj.Player; // 승리자

    // 코루틴
    private Coroutine _coNextWave = null;
    private Coroutine _coDisplayTurnLogs = null;

    private void Start() {
        SkillDataCreate.Inst.DicInit();
        _curStage = StaticManager.Backend.GameData.UserData.StageData["stage"];
        _curWave = StaticManager.Backend.GameData.UserData.StageData["wave"];
        EnterStage(_curStage, _curWave);
    }

    public void EnterStage(int stage, int wave) {
        Debug.Log("스테이지 : " + stage + " 웨이브 : " + wave);
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
        user.BaseAtk = StaticManager.Backend.GameData.UserData.UserStat.Str; // 일단 공격력 기획이 완성되기 전까진 STR을 공격력으로 사용
        user.BaseHp = StaticManager.Backend.GameData.UserData.UserStat.Vit * 10; // HP는 VIT * 10
        user.UpdateFinalAll(); // 기본값은 넣어줬으니 최종값을 업데이트

        user.CurHP = user.FinalHp;
        user.ObjIndex = 0; // 추후에 동료가 추가될 경우를 대비하여 인덱스를 부여

        int skillCount = StaticManager.Backend.GameData.UserData.EqSkills.Length;
        for (int i = 0; i < skillCount; i++) {
            if (StaticManager.Backend.GameData.UserData.EqSkills[i] != 0) { // 스킬이 장착되어 있을 경우 ( 키값이 0이 아닐경우)
                SkillGroupData skillg = new SkillGroupData();
                skillg.Create(StaticManager.Backend.GameData.UserData.EqSkills[i]);
                user.EquipSkill[i] = skillg;
            }
        }

        user.AddNormalAtk(); // 일반공격 추가
        _userUnitList.Add(user);
    }

    private int DetermineEnemyType(int stage, int ran) {
        int enemy0Per = t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_0_per;
        int enemy1Per = t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_1_per;
        int enemy2Per = t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_2_per;

        if (ran < enemy0Per) {
            // 적 유형 0
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_0;
        } else if (ran < enemy0Per + enemy1Per) {
            // 적 유형 1
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_1;
        } else if (ran < enemy0Per + enemy1Per + enemy2Per) {
            // 적 유형 2
            return t_StageData.Enemy_Group.Enemy_GroupMap[stage].Enemy_2;
        } else {
            // 적 유형 3
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
            enemy.UpdateFinalAll(); // 기본값은 넣어줬으니 최종값을 업데이트

            enemy.CurHP = enemy.FinalHp;

            enemy.ObjIndex = i;

            int[] skillGroups = {
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup0_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup1_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup2_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup3_key,
                t_StageData.EnemyData.EnemyDataMap[enemykey].skillGroup4_key,
            };

            // 배열을 통해 스킬그룹을 생성하고 추가
            for (int groupInedx = 0; groupInedx < skillGroups.Length; groupInedx++) {
                if (skillGroups[groupInedx] != 0) {
                    SkillGroupData skillg = new SkillGroupData();
                    skillg.Create(skillGroups[groupInedx]);
                    enemy.EquipSkill[groupInedx] = skillg;
                }
            }
            enemy.AddNormalAtk(); // 일반공격 추가

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

        // 상태(버프,디버프) 리스트 추가
        foreach (var condition in unit.ConditionDic) {
            log.CondtionList.Add(condition.Key);
        }
        actionLogs.Add(log);
    }

    private IEnumerator DisplayTurnLogs() {
        for (int a = 0; a < _turnLogs.Count; a++) {
            Debug.Log("============= 턴 : " + (a + 1) + " ==============");
            for (int i = 0; i < _turnLogs[a].Count; i++) {
                if (_turnLogs[a][i].Winner != ActiveObj.None) {
                    Debug.Log("승리자 : " + _turnLogs[a][i].Winner);
                    break;
                }

                // 턴 시작일경우 기본 세팅
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

        if (_winner == ActiveObj.Player) { // 승리자가 플레이어일 경우 보상 후 다음 웨이브 진행
            Debug.Log("플레이어 승리");
            _curWave++;
            if (_coNextWave != null) {
                StopCoroutine(_coNextWave);
                _coNextWave = null;
            }
            _coNextWave = StartCoroutine(NextWave());
        } else { // 승리자가 적일 경우 웨이브 초기화 1단계로 돌아감
            Debug.Log("적 승리");
            if (_curWave == 1) { // 1웨이브 에서 죽은 경우 스테이지 한단계 낮춤
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
