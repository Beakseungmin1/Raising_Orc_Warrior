using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private static Dictionary<string, BaseItemDataSO> database = new Dictionary<string, BaseItemDataSO>();

    public static BaseItemDataSO GetItem(string itemName)
    {
        if (!database.ContainsKey(itemName))
        {
            var item = Resources.Load<BaseItemDataSO>($"Items/{itemName}");
            if (item != null)
            {
                database[itemName] = item;
            }
        }
        return database.TryGetValue(itemName, out var result) ? result : null;
    }
}