using CodeStage.AntiCheat.ObscuredTypes;
using Game;
using UnityEngine;

public class ActionLog
{
    public ActiveObj Winner = ActiveObj.None; // �¸��� None �� ���� �ȳ���
    public ActionType ActionType; // �ൿ Ÿ�� (�Ϲݰ���, ��ų, ī����, �ǰ�)
    public ActiveObj ActiveObj; // �ൿ�� (�÷��̾�, ��)
    public ObscuredInt ObjIndex; // �ൿ�� �ε���
    public ObscuredBigInteger HitDmg; // �ǰ� ������ (�ǰݽ�)
    public ObscuredBigInteger ObjHP; // �ൿ�� ü��
    public ObscuredBigInteger ObjCurHp; // �ൿ�� ���� ü��
}

public enum ActiveObj {
    Player,
    Enemy,
    None,
}
