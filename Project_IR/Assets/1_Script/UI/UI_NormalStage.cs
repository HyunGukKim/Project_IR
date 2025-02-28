using System.Collections.Generic;
using UnityEngine;

public class UI_NormalStage : MonoBehaviour
{
    // 적유닛 정보
    [SerializeField] private List<UI_CharInfo> _enemyInfoList = null;
    // 플레이어 유닛 정보
    [SerializeField] private List<UI_CharInfo> _playerInfoList = null;

    private void Awake() {
        Global_UIEventSystem.RegisterUIEvent<ActionLog>(eUIEventType.NormalTurnStartData, TurnStartData);
    }

    private void TurnStartData(ActionLog actionLog) {
        switch (actionLog.ActiveObj) {
            case ActiveObj.Player:
                _playerInfoList[actionLog.ObjIndex].SetTurnStartData(actionLog);
                break;
            case ActiveObj.Enemy:
                _enemyInfoList[actionLog.ObjIndex].SetTurnStartData(actionLog);
                break;
        }
    }
}
