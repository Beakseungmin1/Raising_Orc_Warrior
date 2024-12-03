using System.Collections.Generic;

public class SkillInventory : IInventory<SkillDataSO>
{
    private Dictionary<SkillDataSO, int> skills = new Dictionary<SkillDataSO, int>();

    public void AddItem(SkillDataSO item)
    {
        if (skills.ContainsKey(item))
        {
            skills[item]++;
        }
        else
        {
            skills[item] = 1;
        }
    }

    public void RemoveItem(SkillDataSO item)
    {
        if (skills.ContainsKey(item))
        {
            skills[item]--;
            if (skills[item] <= 0)
            {
                skills.Remove(item);
            }
        }
    }

    public SkillDataSO GetItem(string itemName)
    {
        foreach (var skill in skills.Keys)
        {
            if (skill.itemName == itemName)
                return skill;
        }
        return null;
    }

    public int GetItemStackCount(SkillDataSO item)
    {
        return skills.TryGetValue(item, out int count) ? count : 0;
    }

    public List<SkillDataSO> GetAllItems()
    {
        return new List<SkillDataSO>(skills.Keys);
    }

    public int GetTotalItemCount()
    {
        return skills.Count;
    }
}