using System.Numerics;

public static class CustomMath
{
    /// <summary>
    /// �� ���� ���
    /// </summary>
    /// <param name="stat">���� ����</param>
    /// <param name="a">���ذ�</param>
    /// <param name="b">����</param>
    /// <returns></returns>
    public static BigInteger EnemyStat(BigInteger stat, float a, int b) {
        BigInteger returnA = 1;
        BigInteger tempA = (BigInteger)(a * 100);
        for (int i = b; i > 0; i--) {
            returnA = returnA * tempA;
        }
        returnA = stat * returnA; 
        returnA = returnA / BigInteger.Pow(100, b);
        return returnA;
    }
}
