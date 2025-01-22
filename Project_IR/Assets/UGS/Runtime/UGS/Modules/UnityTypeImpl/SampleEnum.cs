
using GoogleSheet.Core.Type;


namespace Game{
    [UGS(typeof(ActionType))]
    public enum ActionType
    {
        None, // 없음 (턴시작시 기본정보 불러오는 용도)
        NormalAtk, // 일반 공격
        SkillAtk, // 스킬 공격
        Buff, // 버프
        Debuff, // 디버프
        Heal, // 회복
        Hit, // 피격
        CounterAtk, // 반격
    }
}
public class SampleEnum
{

}
