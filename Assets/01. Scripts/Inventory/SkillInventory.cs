using System.Collections.Generic;

public class SkillInventory : IInventory<Skill>
{
    private Dictionary<Skill, int> skills = new Dictionary<Skill, int>();
    private int maxInventorySize = 100;

    public void AddItem(Skill skill)
    {
        if (CanAddItem(skill))
        {
            if (skills.ContainsKey(skill))
            {
                skills[skill]++;
            }
            else
            {
                skills[skill] = 1;
            }
        }
    }

    public void RemoveItem(Skill skill)
    {
        if (skills.ContainsKey(skill))
        {
            skills[skill]--;
            if (skills[skill] <= 0)
            {
                skills.Remove(skill);
            }
        }
    }

    public Skill GetItem(string itemName)
    {
        foreach (var skill in skills.Keys)
        {
            if (skill.BaseData.itemName == itemName)
                return skill;
        }
        return null;
    }

    public int GetItemStackCount(Skill skill)
    {
        return skills.TryGetValue(skill, out int count) ? count : 0;
    }

    public List<Skill> GetAllItems()
    {
        return new List<Skill>(skills.Keys);
    }

    public int GetTotalItemCount()
    {
        return skills.Count;
    }

    public bool CanAddItem(Skill skill)
    {
        if (skills.ContainsKey(skill))
        {
            return true;
        }
        return skills.Count < maxInventorySize;
    }
}