using System.Collections.Generic;

public class SkillInventory : IInventory<SkillDataSO>
{
    private List<SkillDataSO> skills = new List<SkillDataSO>();

    public void AddItem(SkillDataSO item)
    {
        skills.Add(item);
    }

    public void RemoveItem(SkillDataSO item)
    {
        skills.Remove(item);
    }

    public SkillDataSO GetItem(string itemName)
    {
        return skills.Find(skill => skill.itemName == itemName);
    }

    public List<SkillDataSO> GetAllItems()
    {
        return new List<SkillDataSO>(skills);
    }

    public int GetItemCount()
    {
        return skills.Count;
    }
}