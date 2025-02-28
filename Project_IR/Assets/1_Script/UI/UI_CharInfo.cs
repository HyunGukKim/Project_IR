
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharInfo : MonoBehaviour
{
    [SerializeField] private Image _charIcon = default;
    [SerializeField] private TMP_Text _txtHp = default;
    [SerializeField] private Image _hpFill = default;

    public void SetTurnStartData(ActionLog actionLog) { // 턴 시작시 데이터를 가져옴 (기본 정보 세팅용, 이벤트x)
        _txtHp.text = actionLog.ObjCurHp.ToString() +" / " + actionLog.ObjHP.ToString();
        float fillAmount = (float)((actionLog.ObjCurHp *100) / actionLog.ObjHP);
        _hpFill.fillAmount = fillAmount / 100;
    }

    public void SetData(ActionLog actionLog) { // 로그 데이터를 가져옴

    }

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
