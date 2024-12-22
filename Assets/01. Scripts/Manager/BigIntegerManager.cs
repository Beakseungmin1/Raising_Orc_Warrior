using System.Numerics;

public class BigIntegerManager : Singleton<BigIntegerManager>
{
    public string FormatBigInteger(BigInteger amount)
    {
        if (amount < 1000)
            return amount.ToString();
        string[] units = { "", "K", "M", "B", "T", "A", "B", "C", "D", "E", "F", "G" };
        int unitIndex = 0;
        BigInteger divisor = new BigInteger(1000);

        while (amount >= divisor && unitIndex < units.Length - 1)
        {
            amount /= 1000;
            unitIndex++;
        }

        return string.Format("{0:F1}{1}", (double)amount, units[unitIndex]);
    }
}
