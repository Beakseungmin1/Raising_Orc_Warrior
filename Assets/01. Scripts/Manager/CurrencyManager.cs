using System;
using System.Collections.Generic;
using System.Numerics; // BigInteger 사용

public class CurrencyManager : Singleton<CurrencyManager>
{
    private Dictionary<CurrencyType, object> currencies;

    private void Awake()
    {
        // 딕셔너리 초기화
        currencies = new Dictionary<CurrencyType, object>
        {
            { CurrencyType.Gold, new BigInteger(100000000f) },
            { CurrencyType.Emerald, 100000000f }, // float
            { CurrencyType.Cube, 100000000f },    // float
            { CurrencyType.Diamond, 100000000f } // float
        };
    }

    public void AddCurrency<T>(CurrencyType type, T amount) where T : struct
    {
        if (currencies.ContainsKey(type) && currencies[type] is T currentAmount)
        {
            currencies[type] = AddValues(currentAmount, amount);
        }
    }

    public void SubtractCurrency<T>(CurrencyType type, T amount) where T : struct
    {
        if (currencies.ContainsKey(type) && currencies[type] is T currentAmount)
        {
            currencies[type] = SubtractValues(currentAmount, amount);

            // 0 이하로 내려가지 않도록 보정
            if (Comparer<T>.Default.Compare((T)currencies[type], default(T)) < 0)
            {
                currencies[type] = default(T);
            }
        }
    }

    public T GetCurrency<T>(CurrencyType type) where T : struct
    {
        if (currencies.ContainsKey(type) && currencies[type] is T value)
        {
            return value;
        }
        throw new System.InvalidCastException($"{type} 타입을 {typeof(T)}타입으로 변환할 수 없습니다.");
    }

    private T AddValues<T>(T a, T b) where T : struct
    {
        if (typeof(T) == typeof(float))
        {
            return (T)(object)((float)(object)a + (float)(object)b);
        }
        if (typeof(T) == typeof(BigInteger))
        {
            return (T)(object)((BigInteger)(object)a + (BigInteger)(object)b);
        }

        throw new InvalidOperationException($"AddValues does not support type {typeof(T)}");
    }

    private T SubtractValues<T>(T a, T b) where T : struct
    {
        if (typeof(T) == typeof(float))
        {
            return (T)(object)((float)(object)a - (float)(object)b);
        }
        if (typeof(T) == typeof(BigInteger))
        {
            return (T)(object)((BigInteger)(object)a - (BigInteger)(object)b);
        }

        throw new InvalidOperationException($"SubtractValues does not support type {typeof(T)}");
    }
}