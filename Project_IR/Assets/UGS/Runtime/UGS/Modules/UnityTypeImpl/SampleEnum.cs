
using GoogleSheet.Core.Type;


namespace Game{
    [UGS(typeof(ActionType))]
    public enum ActionType
    {
        None, // ���� (�Ͻ��۽� �⺻���� �ҷ����� �뵵)
        NormalAtk, // �Ϲ� ����
        SkillAtk, // ��ų ����
        Buff, // ����
        Debuff, // �����
        Heal, // ȸ��
        Hit, // �ǰ�
        CounterAtk, // �ݰ�
    }
}
public class SampleEnum
{

}
